// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AuthorDto.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Models;

/// <summary>
///   Data Transfer Object (DTO) representing an author user.
/// </summary>
public class AuthorDto
{

	/// <summary>
	///   Parameterless constructor for serialization and test data generation.
	/// </summary>
	public AuthorDto() : this(string.Empty, string.Empty, string.Empty, [], true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="AuthorDto" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the user.</param>
	/// <param name="userName">The username of the user.</param>
	/// <param name="email">The email address of the user.</param>
	/// <param name="roles">The list of roles assigned to the user.</param>
	/// <param name="skipValidation">If true, skips validation on construction.</param>
	public AuthorDto(string id, string userName, string email, List<string> roles, bool skipValidation = false)
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
	///   Gets or sets the unique identifier for the user.
	/// </summary>
	public string Id { get; init; }

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
	///   Gets an empty instance of AuthorDto with default values.
	/// </summary>
	public static AuthorDto Empty => new(string.Empty, string.Empty, string.Empty, [], true);

	/// <summary>
	///   Updates the user's information.
	/// </summary>
	/// <param name="userName">The new username.</param>
	/// <param name="email">The new email address.</param>
	public void Update(string userName, string email)
	{

		if (string.IsNullOrWhiteSpace(userName))
		{
			throw new ValidationException("UserName is required");
		}

		if (string.IsNullOrWhiteSpace(email))
		{
			throw new ValidationException("Email is required");
		}

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
		if (roles == null)
		{
			throw new ValidationException("Roles cannot be null.");
		}

		if (roles.Count == 0)
		{
			throw new ValidationException("Roles cannot be an empty collection.");
		}

		Roles = roles;
		ValidateState();
	}

	/// <summary>
	///   Validates the current state of the user DTO.
	/// </summary>
	/// <exception cref="ValidationException">Thrown when validation fails.</exception>
	private void ValidateState()
	{

		var validator = new AuthorDtoValidator();

		var validationResult = validator.Validate(this);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}

	}

}