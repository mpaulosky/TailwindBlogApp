// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AssemblyReference.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Architecture.Tests
// =======================================================

namespace Architecture;

[ExcludeFromCodeCoverage]
public static class AssemblyReference
{

	public static Assembly Domain => typeof(Result).Assembly;

	public static Assembly Web => Assembly.Load("Web");

	public static Assembly Persistence => typeof(PgContext).Assembly;

	public static Assembly MyMediator => typeof(ISender).Assembly;

	public static Assembly Migrations => typeof(Worker).Assembly;

}