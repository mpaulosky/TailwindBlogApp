// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AssemblyReference.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Architecture.Tests
// =======================================================

using Persistence;

namespace Architecture.Tests;

[ExcludeFromCodeCoverage]
public static class AssemblyReference
{
	public static Assembly Domain => typeof(Result).Assembly;
	public static Assembly Web => Assembly.Load("Web");
	public static Assembly MongoDb => typeof(MongoDbRepository<>).Assembly;
}