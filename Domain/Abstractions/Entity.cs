// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Entity.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Abstractions;

/// <summary>
///   Base class for all entities in the domain model.
/// </summary>
public abstract class Entity
{

	/// <summary>
	///   /// Gets the unique identifier for this entity.
	/// </summary>
	[Key]
	public Guid Id { get; protected init; } = Guid.NewGuid();

	/// <summary>
	///   Gets the date and time when this entity was created.
	/// </summary>
	[Required(ErrorMessage = "A Created On Date is required")]
	[BsonRepresentation(BsonType.DateTime)]
	[Display(Name = "Created On")]
	public DateTime CreatedOn { get; protected init; } = DateTime.UtcNow;

	/// <summary>
	///   Gets or sets the date and time when this entity was last modified.
	/// </summary>
	[BsonElement("modifiedOn")]
	[BsonRepresentation(BsonType.DateTime)]
	[Display(Name = "Modified On")]
	public DateTime? ModifiedOn { get; set; }

	/// <summary>
	///   Gets or sets the archived status of the entity.
	/// </summary>
	[BsonElement("archived")]
	[Display(Name = "Archived")]
	public bool Archived { get; set; }

}