// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Entities.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web
// =======================================================

using System.ComponentModel.DataAnnotations;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TailwindBlog.Domain.Abstractions;

public abstract class Entity
{

	[Key, BsonId, BsonRepresentation(BsonType.ObjectId)]
	public ObjectId Id { get; set; } = ObjectId.Empty;

	[Required(ErrorMessage = "A Created On Date is required"),
	BsonRepresentation(BsonType.DateTime),
	Display(Name = "Created On")]
	public DateTime CreatedOn { get; set; } = DateTime.Now;

	[BsonElement("modifiedOn"),
	BsonRepresentation(BsonType.DateTime),
	Display(Name = "Modified On")]
	public DateTime? ModifiedOn { get; set; }

}
