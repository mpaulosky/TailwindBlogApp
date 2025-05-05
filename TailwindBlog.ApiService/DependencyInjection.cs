// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DependencyInjection.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using static TailwindBlog.Domain.Constants.ServiceNames;

using Microsoft.EntityFrameworkCore;

using TailwindBlog.ApiService.Context;

namespace TailwindBlog.ApiService;

public static class DependencyInjection
{

	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{

		var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__tailwind-blog") ??
													throw new InvalidOperationException(
															"Environment variable 'ConnectionStrings__tailwind-blog' is not set.");

		services.AddDbContext<AppDbContext>(options =>
				options.UseMongoDB(connectionString, DatabaseName));

		return services;

	}

}
