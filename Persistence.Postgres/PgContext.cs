// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PgContext.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.Postgres
// =======================================================

namespace Persistence.Postgres;

public class PgContext : DbContext
{

	public PgContext(DbContextOptions<PgContext> options) : base(options) { }

	public DbSet<Article> Articles => Set<Article>();

	public DbSet<Category> Categories => Set<Category>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		modelBuilder
				.Entity<Article>()
				.Property(e => e.CreatedOn);

		modelBuilder
				.Entity<Article>()
				.Property(e => e.ModifiedOn);

		modelBuilder
				.Entity<Article>()
				.Property(e => e.PublishedOn);

		modelBuilder
				.Entity<Category>()
				.Property(e => e.CreatedOn);

		modelBuilder
				.Entity<Category>()
				.Property(e => e.ModifiedOn);

	}

}

public class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTime>
{

	public DateTimeOffsetConverter() : base(
			v => v.UtcDateTime, // Store as UTC DateTime
			v => new DateTimeOffset(v, TimeSpan.Zero)) // Retrieve as DateTimeOffset (UTC)
	{ }

}