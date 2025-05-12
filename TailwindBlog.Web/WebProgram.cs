// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     WebProgram.cs
// Project Name:  TailwindBlog.Web
// =======================================================

using TailwindBlog.Web.Services;

namespace TailwindBlog.Web;

public static partial class WebProgram
{
	public static WebAssemblyHost CreateApp(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);

		builder.RootComponents.Add<App>("#app");

		// Add HttpClient for API communication
		builder.Services.AddScoped(sp => new HttpClient
		{
			BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
		});

		// Register services
		builder.Services.AddScoped<IArticleService, ArticleService>();
		builder.Services.AddScoped<ICategoryService, CategoryService>();

		return builder.Build();
	}

	public static void Main(string[] args)
	{
		var app = CreateApp(args);
		app.RunAsync().GetAwaiter().GetResult();
	}
}