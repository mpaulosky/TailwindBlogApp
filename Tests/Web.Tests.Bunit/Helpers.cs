// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Helpers.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Web.Tests.Bunit
// =======================================================

namespace Web;

public static class Helpers
{

	/// <summary>
	///   Sets up the authorization context for testing components
	/// </summary>
	/// <param name="context">A BunitContext</param>
	/// <param name="isAuthorized">If true, authorizes the test user; if false, sets unauthorized state</param>
	/// <param name="hasRoles">If true, adds Admin and User roles to the claims</param>
	/// <remarks>
	///   This helper method configures the authentication state for component testing:
	///   - When authorized, sets up a "Test User" identity
	///   - When roles are enabled, adds Admin and User role claims
	///   - When unauthorized, explicitly sets not authorized state
	/// </remarks>
	public static void SetAuthorization(BunitContext context, bool isAuthorized = true, bool hasRoles = false)
	{

		var authContext = context.AddAuthorization();

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