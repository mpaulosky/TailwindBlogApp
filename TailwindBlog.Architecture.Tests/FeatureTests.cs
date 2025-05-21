// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FeatureTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

using Mono.Cecil;

namespace TailwindBlog.Architecture.Tests;

// All tests removed because they referenced the deleted ApiService.

internal class CommandValidationRule : NetArchTest.Rules.ICustomRule
{
	public bool MeetsRule(Type type)
	{
		// Example: Ensure type implements IRequest or IValidatable
		// Replace with actual interfaces as appropriate for your codebase!
		var implementsIRequest = type.GetInterfaces().Any(i => i.Name == "IRequest");
		var implementsIValidatable = type.GetInterfaces().Any(i => i.Name == "IValidatable");
		return implementsIRequest || implementsIValidatable;
	}

	public bool MeetsRule(TypeDefinition type)
	{
		// Check if the type implements either IRequest or IValidatable
		var implementsIRequest = type.Interfaces.Any(i => i.InterfaceType.Name == "IRequest");
		var implementsIValidatable = type.Interfaces.Any(i => i.InterfaceType.Name == "IValidatable");
		return implementsIRequest || implementsIValidatable;
	}
}