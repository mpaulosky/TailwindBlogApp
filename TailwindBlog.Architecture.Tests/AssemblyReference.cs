// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AssemblyReference.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

using TailwindBlog.Persistence;

namespace TailwindBlog.Architecture.Tests;

public static class AssemblyReference
{
	public static Assembly Domain => typeof(Result).Assembly;
	public static Assembly ApiService => typeof(ApiServiceProgram).Assembly;
	public static Assembly Web => Assembly.Load("TailwindBlog.Web");
	public static Assembly MongoDb => typeof(MongoDbRepository<>).Assembly;
}