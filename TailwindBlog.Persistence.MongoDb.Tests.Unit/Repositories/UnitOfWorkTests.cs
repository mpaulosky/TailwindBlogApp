// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     UnitOfWorkTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

// [ExcludeFromCodeCoverage]
// [TestSubject(typeof(UnitOfWork))]
public class UnitOfWorkTests
{
	[Fact]
	public async Task SaveChangesAsync_Should_Call_DbContext_SaveChangesAsync()
	{
		// Arrange
		var dbContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
		dbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
		var unitOfWork = new UnitOfWork(dbContext);

		// Act
		var result = await unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

		// Assert
		result.Should().Be(1);
		await dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
	}
}
