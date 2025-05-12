// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArchitectureTestExtensions.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

namespace TailwindBlog.Architecture.Tests;

/// <summary>
///   Extensions for architecture tests to help with common test patterns
/// </summary>
internal static class ArchitectureTestExtensions
{

	public static Assembly GetAssembly<T>()
	{
		return typeof(T).Assembly;
	}

	public static bool HasDependencyOn(this string dependency, Assembly assembly)
	{
		return Types.InAssembly(assembly)
				.Should()
				.NotHaveDependencyOn(dependency)
				.GetResult()
				.IsSuccessful;
	}

	public static bool HasDependencyOnAny(this IEnumerable<string> dependencies, Assembly assembly)
	{
		var deps = dependencies.ToArray();

		return Types.InAssembly(assembly)
				.Should()
				.NotHaveDependencyOnAny(deps)
				.GetResult()
				.IsSuccessful;
	}

	public static bool HasDependencyOnAll(this IEnumerable<string> dependencies, Assembly assembly)
	{
		var deps = dependencies.ToArray();

		return Types.InAssembly(assembly)
				.Should()
				.NotHaveDependencyOnAll(deps)
				.GetResult()
				.IsSuccessful;
	}

	public static bool ResideInNamespace(this Assembly assembly, string @namespace)
	{
		return Types.InAssembly(assembly)
				.Should()
				.ResideInNamespace(@namespace)
				.GetResult()
				.IsSuccessful;
	}

	public static bool HaveNameEndingWith(this Assembly assembly, string suffix)
	{
		return Types.InAssembly(assembly)
				.Should()
				.HaveNameEndingWith(suffix)
				.GetResult()
				.IsSuccessful;
	}

	public static bool ImplementInterface(this Assembly assembly, Type interfaceType)
	{
		return Types.InAssembly(assembly)
				.Should()
				.ImplementInterface(interfaceType)
				.GetResult()
				.IsSuccessful;
	}

	public static bool BeImmutable(this Assembly assembly)
	{
		return Types.InAssembly(assembly)
				.Should()
				.BeImmutable()
				.GetResult()
				.IsSuccessful;
	}

	public static bool HaveCustomAttribute(this Assembly assembly, Type attributeType)
	{
		return Types.InAssembly(assembly)
				.Should()
				.HaveCustomAttribute(attributeType)
				.GetResult()
				.IsSuccessful;
	}

	public static bool MeetAllConditions(this Assembly assembly, params Func<Types, PredicateList>[] conditions)
	{
		var types = Types.InAssembly(assembly);

		return conditions.All(condition =>
				condition(types)
						.GetResult()
						.IsSuccessful);
	}

}