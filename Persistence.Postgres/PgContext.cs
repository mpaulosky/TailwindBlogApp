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
				.Property(e => e.CreatedOn)
				.HasConversion(new DateTimeOffsetConverter());

		modelBuilder
				.Entity<Article>()
				.Property(e => e.ModifiedOn)
				.HasConversion(new DateTimeOffsetConverter());

		modelBuilder
				.Entity<Article>()
				.Property(e => e.PublishedOn)
				.HasConversion(new DateTimeOffsetConverter());

		modelBuilder
				.Entity<Category>()
				.HasIndex(p => p.Slug)
				.IsUnique();

		modelBuilder
				.Entity<Category>()
				.Property(e => e.CreatedOn)
				.HasConversion(new DateTimeOffsetConverter());

		modelBuilder
				.Entity<Category>()
				.Property(e => e.ModifiedOn)
				.HasConversion(new DateTimeOffsetConverter());

	}

}

public class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{

	public DateTimeOffsetConverter() : base(
			v => v.UtcDateTime,
			v => v) { }

}