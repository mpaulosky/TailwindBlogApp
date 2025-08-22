// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     LoginComponentTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web.Components.Shared;

/// <summary>
///   Unit tests for the LoginComponent component.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(LoginComponent))]
public class LoginComponentTests : BunitContext
{

	[Theory]
	[InlineData(true, false, "Log out")]
	[InlineData(false, false, "Log in")]
	public void LoginComponent_RendersCorrectly(bool isAuthorized, bool hasRoles, string expectedText)
	{
		// Arrange
		SetAuthorization(isAuthorized, hasRoles);

		// Act
		var cut = Render<LoginComponent>();

		// Assert
		cut.MarkupMatches(
				$"""<a href="Account/{(isAuthorized ? "Logout" : "Login")}">{expectedText}</a>"""); // Update selector as needed

	}

	private  void SetAuthorization(bool isAuthorized = true, bool hasRoles = false)
	{

		var authContext = AddAuthorization();

		// Set up the authentication state for the component
		if (isAuthorized)
		{

			// If authorized, set the context to authorize with a test user
			authContext.SetAuthorized("Test User");

		}
		else
		{

			// If not authorized, set the context to not authorized
			authContext.SetNotAuthorized();

		}

		// Optionally set roles if required
		if (hasRoles)
		{

			authContext.SetClaims(new Claim(ClaimTypes.Role, "Admin"), new Claim(ClaimTypes.Role, "User"));

		}

	}

}