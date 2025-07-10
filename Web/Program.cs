var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddOutputCache();

builder.Services.AddPersistence();

builder.Services.AddSyncfusionBlazor();

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();

builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", true);

	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
		.AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();