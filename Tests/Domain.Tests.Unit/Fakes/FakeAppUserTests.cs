// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FakeAppUserTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Fakes;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(FakeAppUser))]
public class FakeAppUserTests
{

	[Fact]
	public void GetNewAppUser_Should_Return_AppUser()
	{

		// Arrange & Act
		var user = FakeAppUser.GetNewAppUser();

		// Assert
		user.Should().NotBeNull();

	}

	[Fact]
	public void GetAppUsers_Should_Return_Correct_Count()
	{

		// Arrange & Act
		var users = FakeAppUser.GetAppUsers(2);

		// Assert
		users.Should().HaveCount(2);

	}


}