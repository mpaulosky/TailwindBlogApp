// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     TestSubjectAttribute.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

using System;

namespace TailwindBlog.Persistence.Repositories;

/// <summary>
/// Attribute to mark the subject under test in a test class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TestSubjectAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestSubjectAttribute"/> class.
    /// </summary>
    /// <param name="testSubjectType">The type of the class being tested.</param>
    public TestSubjectAttribute(Type testSubjectType)
    {
        TestSubjectType = testSubjectType;
    }

    /// <summary>
    /// Gets the type of the class being tested.
    /// </summary>
    public Type TestSubjectType { get; }
}