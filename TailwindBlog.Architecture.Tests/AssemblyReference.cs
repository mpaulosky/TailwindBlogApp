// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AssemblyReference.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

namespace TailwindBlog.Architecture.Tests;

/// <summary>
///   Provides access to assemblies used in architecture tests
/// </summary>
public static class AssemblyReference
{

	private static Assembly GetAssemblyContaining<T>()
	{
		return typeof(T).Assembly;
	}

	// Core assemblies
	public static readonly Assembly Domain = GetAssemblyContaining<Result>();

	public static readonly Assembly ApiService = Assembly.Load("TailwindBlog.ApiService");

	public static readonly Assembly Web = Assembly.Load("TailwindBlog.Web");

	public static readonly Assembly Infrastructure = Assembly.Load("TailwindBlog.Persistence.MongoDb");

	// Entity/Model types for testing
	public static readonly Type ResultType = typeof(Result);

	public static readonly Type ArticleType = typeof(Article);

	public static readonly Type GenericRepositoryType = typeof(IRepository<>);

}