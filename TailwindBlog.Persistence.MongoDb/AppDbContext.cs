// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AppDbContext.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

namespace TailwindBlog.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public virtual DbSet<Article> Articles { get; set; }

	public virtual DbSet<Category> Categories { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Article>();
		modelBuilder.Entity<Category>();

	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{

		var result = await base.SaveChangesAsync(cancellationToken);

		return result;

	}

}