// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     ApiServiceProgram.cs
// Project Name:  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.ApiService;

public static partial class ApiServiceProgram
{
	public static WebApplication CreateApp(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add service defaults & Aspire client integrations.
		builder.AddServiceDefaults();

		// Add services to the container.
		builder.Services.AddProblemDetails();
		builder.Services.AddOpenApi();

		// Register MongoDB and Repository Services
		builder.Services.AddDbContext<AppDbContext>(options =>
		{
			var connectionString = builder.Configuration.GetConnectionString(DatabaseName) ??
									throw new InvalidOperationException("Connection string 'MongoDb' not found.");
			options.UseMongoDB(connectionString, DatabaseName);
		});

		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
		builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
		builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

		// Register MyMediator and handlers
		builder.Services.AddMyMediator(Assembly.GetExecutingAssembly());

		return builder.Build();
	}

	public static WebApplication ConfigureApp(WebApplication app)
	{
		// Configure the HTTP request pipeline.
		app.UseExceptionHandler();

		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();

			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
			});

			app.MapScalarApiReference(options =>
			{
				options.Title = "The TailwindBlog API Service";
				options.Theme = ScalarTheme.Saturn;
				options.HideClientButton = true;
			});
		}

		// Map API Endpoints
		app.MapArticleEndpoints();
		app.MapCategoryEndpoints();
		app.MapDefaultEndpoints();

		return app;
	}

	public static void Main(string[] args)
	{
		var app = CreateApp(args);
		app = ConfigureApp(app);
		app.Run();
	}
}