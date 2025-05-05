// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AppDbContext.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web
// =======================================================

using Microsoft.EntityFrameworkCore;

using TailwindBlog.Domain.Entities;

namespace TailwindBlog.ApiService.Context;

public class AppDbContext : DbContext
{

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Article> Articles { get; init; }
	public DbSet<Category> Categories { get; init; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Article>();
		modelBuilder.Entity<Category>();

	}

}
