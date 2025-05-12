#region

using System.Reflection;
using NetArchTest.Rules;

#endregion

namespace TailwindBlog.Architecture.Tests;

internal static class ArchitectureTestExtensions
{
	public static Assembly GetAssembly<T>() => typeof(T).Assembly;

	public static bool HasDependencyOnAll(this IEnumerable<string> dependencies, Assembly assembly) =>
			Types.InAssembly(assembly)
					 .Should()
					 .NotHaveDependencyOnAll(dependencies)
					 .GetResult()
					 .IsSuccessful;

	public static bool HasDependencyOn(this string dependency, Assembly assembly) =>
			Types.InAssembly(assembly)
					 .Should()
					 .NotHaveDependencyOn(dependency)
					 .GetResult()
					 .IsSuccessful;

	public static bool ResideInNamespace(this Assembly assembly, string @namespace) =>
			Types.InAssembly(assembly)
					 .Should()
					 .ResideInNamespace(@namespace)
					 .GetResult()
					 .IsSuccessful;

	public static bool HaveNameEndingWith(this Assembly assembly, string suffix) =>
			Types.InAssembly(assembly)
					 .Should()
					 .HaveNameEndingWith(suffix)
					 .GetResult()
					 .IsSuccessful;

	public static bool ImplementInterface(this Assembly assembly, Type interfaceType) =>
			Types.InAssembly(assembly)
					 .Should()
					 .ImplementInterface(interfaceType)
					 .GetResult()
					 .IsSuccessful;
}