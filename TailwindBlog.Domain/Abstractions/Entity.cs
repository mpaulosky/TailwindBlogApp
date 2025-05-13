// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Entity.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web
// =======================================================

namespace TailwindBlog.Domain.Abstractions;

public abstract class Entity
{

	[Key, BsonId, BsonRepresentation(BsonType.ObjectId)]
	public ObjectId Id { get; init; } = ObjectId.Empty;

	[Required(ErrorMessage = "A Created On Date is required")]
	[BsonRepresentation(BsonType.DateTime)]
	[Display(Name = "Created On")]
	public DateTime CreatedOn { get; init; } = DateTime.Now;

	[BsonElement("modifiedOn")]
	[BsonRepresentation(BsonType.DateTime)]
	[Display(Name = "Modified On")]
	public DateTime? ModifiedOn { get; set; }

}