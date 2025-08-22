using Auth0.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Register Postgres services
var pg = new RegisterPostgresServices();
pg.RegisterServices(builder);

// Add Output Cache
builder.Services.AddOutputCache();

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();

builder.Services.AddHealthChecks();

builder.Services.AddAuthentication( /* options */)
		.AddAuth0WebAppAuthentication(options =>
		{
			options.Domain = configuration["Auth0:Domain"] ?? string.Empty;
			options.ClientId = configuration["Auth0:ClientId"] ?? string.Empty;
		});

builder.Services.AddAuthorization(options =>
{
	// Define policies
	options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy => policy
			.WithOrigins("https://localhost:7219")
			.AllowAnyHeader()
			.AllowAnyMethod());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", true);

	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapGet("/Account/LoginComponent", async (HttpContext httpContext, string returnUrl = "/") =>
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