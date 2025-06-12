// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AppUser.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Entities;

/// <summary>
///   Domain entity representing an application user.
/// </summary>
public class AppUser
{

	/// <summary>
	/// Parameterless constructor for serialization and test data generation.
	/// </summary>
	public AppUser() : this(string.Empty, string.Empty, string.Empty, [], true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="AppUser" /> class.
	/// </summary>
	/// <param name="id">The users Id</param>
	/// <param name="userName">The username of the user.</param>
	/// <param name="email">The email address of the user.</param>
	/// <param name="roles">The list of roles assigned to the user.</param>
	/// <param name="skipValidation">If true, skips validation on construction.</param>
	public AppUser(string id, string userName, string email, List<string> roles, bool skipValidation = false)
	{
		Id = id;
		UserName = userName;
		Email = email;
		Roles = roles;

		if (!skipValidation)
		{
			ValidateState();
		}
	}

	/// <summary>
	///   Gets or sets the id of the user.
	/// </summary>
	public string Id { get; set; }

	/// <summary>
	///   Gets or sets the username of the user.
	/// </summary>
	[Display(Name = "User Name")]
	public string UserName { get; set; }

	/// <summary>
	///   Gets or sets the email address of the user.
	/// </summary>
	[Display(Name = "Email Address")]
	public string Email { get; set; }

	/// <summary>
	///   Gets or sets the list of roles assigned to the user.
	/// </summary>
	[Display(Name = "User Roles")]
	public List<string> Roles { get; set; }

	/// <summary>
	///   Gets an empty instance of AppUser with default values.
	/// </summary>
	public static AppUser Empty => new(string.Empty, string.Empty, string.Empty, [], true);

	/// <summary>
	///   Updates the user's information.
	/// </summary>
	/// <param name="userName">The new username.</param>
	/// <param name="email">The new email address.</param>
	public void Update(string userName, string email)
	{
		UserName = userName;
		Email = email;
		ValidateState();
	}

	/// <summary>
	///   Updates the user's roles.
	/// </summary>
	/// <param name="roles">The new list of roles.</param>
	public void UpdateRoles(List<string> roles)
	{
		Roles = roles;
		ValidateState();
	}

	/// <summary>
	///   Validates the state of the entity using FluentValidation.
	/// </summary>
	/// <exception cref="ValidationException">Thrown when validation fails.</exception>
	private void ValidateState()
	{
		var validator = new AppUserValidator();
		validator.ValidateAndThrow(this);
	}
}