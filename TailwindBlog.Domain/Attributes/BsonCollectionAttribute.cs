// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     BsonCollectionAttribute.cs
// Project Name:  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
	public string CollectionName { get; }

	public BsonCollectionAttribute(string collectionName)
	{
		CollectionName = collectionName;
	}
}
