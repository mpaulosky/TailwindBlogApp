// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ServiceCollectionExtensions.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.ApiService.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddMyMediator(this IServiceCollection services, params Assembly[] assemblies)
	{
		services.AddScoped<ISender, Sender>();
		// Register all request handlers
		foreach (var assembly in assemblies)
		{
			var handlerTypes = assembly.GetTypes()
				.Where(t => !t.IsAbstract && !t.IsInterface)
				.Where(t => t.GetInterfaces()
					.Any(i => i.IsGenericType &&
						i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

			foreach (var handlerType in handlerTypes)
			{
				var handlerInterface = handlerType.GetInterfaces()
					.First(i => i.IsGenericType &&
						i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

				services.AddScoped(handlerInterface, handlerType);
			}
		}

		return services;
	}
}
