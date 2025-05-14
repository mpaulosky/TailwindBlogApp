// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     CategoryServiceExtensions.cs
// Company:       mpaulosky
// Author:        Matthew
// Solution Name: TailwindBlog
// Project Name:  TailwindBlog.ApiService
// =======================================================

using Microsoft.Extensions.DependencyInjection;
using TailwindBlog.ApiService.Features.Categories;
using TailwindBlog.Domain.Interfaces;
using TailwindBlog.Domain.Validators;

namespace TailwindBlog.ApiService.Extensions;

/// <summary>
/// Extension methods for configuring category-related services.
/// </summary>
public static class CategoryServiceExtensions
{
	/// <summary>
	/// Adds category services to the service collection.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <returns>The service collection for chaining.</returns>
	public static IServiceCollection AddCategoryServices(this IServiceCollection services)
	{
		services.AddDistributedMemoryCache();
		services.AddScoped<ICategoryCache, CategoryCache>();
		services.AddScoped<CategoryValidator>();

		return services;
	}
}
