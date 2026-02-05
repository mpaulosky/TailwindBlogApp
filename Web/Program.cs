using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Register Postgres services
var postgresServices = new RegisterPostgresServices();
postgresServices.RegisterServices(builder);

// Add Output Cache
builder.Services.AddOutputCache();

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();

builder.Services.AddHealthChecks();

// Register HttpContextAccessor for diagnostics
builder.Services.AddHttpContextAccessor();

// Configure authentication: use cookies as the default authenticate/sign-in scheme
// and Auth0 as the default challenge scheme for external login flows.
// Note: Do not call AddCookie explicitly here because the Auth0 integration
// already registers the cookie scheme. Registering it twice throws a "Scheme already exists" error.
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = Auth0Constants.AuthenticationScheme;
})
.AddAuth0WebAppAuthentication(options =>
{
	options.Domain = configuration["Auth0:Domain"] ?? throw new InvalidOperationException("Auth0:Domain configuration is missing");
	options.ClientId = configuration["Auth0:ClientId"] ?? throw new InvalidOperationException("Auth0:ClientId configuration is missing");
	options.ClientSecret = configuration["Auth0:ClientSecret"] ?? throw new InvalidOperationException("Auth0:ClientSecret configuration is missing");
});

builder.Services.AddAuthorizationBuilder()
	.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy => policy
			.WithOrigins("https://localhost:7219")
			.AllowAnyHeader()
			.AllowAnyMethod());
});

// Enhanced OpenTelemetry tracing configuration
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? builder.Environment.ApplicationName ?? "web";
var serviceVersion = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0";

builder.Services.AddOpenTelemetry()
	.WithTracing(tracing =>
	{
		// Resource describes the service producing telemetry
		tracing.SetResourceBuilder(ResourceBuilder.CreateDefault()
			.AddService(serviceName: serviceName, serviceVersion: serviceVersion));

		// Common instrumentations
		tracing
			.AddAspNetCoreInstrumentation(options => options.RecordException = true)
			.AddHttpClientInstrumentation(options => options.RecordException = true);

		// Configure sampler (default to always sample in dev, or use OTEL_SAMPLER_PROBABILITY if provided)
		var samplerProbability = configuration.GetValue<double?>("OTEL_SAMPLER_PROBABILITY") 
			?? (builder.Environment.IsDevelopment() ? 1.0 : 0.1);
		tracing.SetSampler(new ParentBasedSampler(new TraceIdRatioBasedSampler(samplerProbability)));

		// Do not add a signal-specific AddOtlpExporter here because ServiceDefaults registers a cross-cutting UseOtlpExporter when configured.
	});

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", true);

	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
else
{
	// In development enable the developer exception page for richer diagnostics.
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

// Simple request/response logging middleware for runtime diagnostics
app.Use(async (context, next) =>
{
	var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
	var logger = loggerFactory.CreateLogger("RequestDiagnostics");

	logger.LogInformation("Incoming request {Method} {Path} from {RemoteIp} - Authenticated: {IsAuthenticated}",
		context.Request.Method,
		context.Request.Path,
		context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
		context.User?.Identity?.IsAuthenticated ?? false);

	await next();

	logger.LogInformation("Outgoing response {StatusCode} for {Path}", 
		context.Response.StatusCode, context.Request.Path);
});

app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
{
	var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
			.WithRedirectUri(returnUrl)
			.Build();

	await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/Account/Logout", async httpContext =>
{
	var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
			.WithRedirectUri("/")
			.Build();

	await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
	await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

// Diagnostics endpoint exposing environment, default auth schemes and registered schemes.
app.MapGet("/diagnostics", async (IConfiguration cfg, IAuthenticationSchemeProvider schemeProvider, IOptions<AuthenticationOptions> authOptions, IWebHostEnvironment env) =>
{
	var schemes = await schemeProvider.GetAllSchemesAsync();
	var schemeList = schemes.Select(s => new { s.Name, Handler = s.HandlerType?.FullName }).ToList();

	string Mask(string? value) => string.IsNullOrEmpty(value) 
		? string.Empty 
		: (value.Length <= 6 ? "******" : value[..3] + "..." + value[^3..]);

	return Results.Json(new
	{
		Environment = env.EnvironmentName,
		DefaultAuthenticateScheme = authOptions.Value.DefaultAuthenticateScheme,
		DefaultChallengeScheme = authOptions.Value.DefaultChallengeScheme,
		DefaultSignInScheme = authOptions.Value.DefaultSignInScheme,
		RegisteredSchemes = schemeList,
		Auth0 = new
		{
			Domain = Mask(cfg["Auth0:Domain"]),
			ClientId = Mask(cfg["Auth0:ClientId"])
		}
	});
});

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.UseOutputCache();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapDefaultEndpoints();

app.UseStatusCodePages(async context =>
{

	var response = context.HttpContext.Response;

	if (response.StatusCode == 404)
	{
		context.HttpContext.Response.Redirect("/error/404");
	}
	else if (response.StatusCode == 401)
	{
		context.HttpContext.Response.Redirect("/error/401");
	}
	else if (response.StatusCode == 403)
	{
		context.HttpContext.Response.Redirect("/error/403");
	}

	// ...other codes as needed
	await Task.CompletedTask;

});

app.Run();