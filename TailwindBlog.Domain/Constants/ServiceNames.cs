// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ServiceNames.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : BlogApp
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Constants;

public static class ServiceNames
{

	public static string ServerName { get; } = "posts-server";

	public static string DatabaseName { get; } = "blog-app-database";

	public static string OutputCache { get; } = "output-cache";

	public static string Api { get; } = "blog-api";

	public static string WebApp { get; } = "web-frontend";

	public static string CategoryCacheName { get; } = "CategoryData";

	public static string BlogPostCacheName { get; } = "BlogPostData";

}
