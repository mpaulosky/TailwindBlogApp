// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ServiceNames.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Constants;

public static class ServiceNames
{

	public static string ServerName { get; } = "posts-server";

	public static string DatabaseName { get; } = "articlesdb";

	public static string OutputCache { get; } = "output-cache";

	public static string ApiService { get; } = "blog-api";

	public static string WebApp { get; } = "web-frontend";

	public static string CategoryCacheName { get; } = "CategoryData";

	public static string BlogPostCacheName { get; } = "BlogPostData";

}