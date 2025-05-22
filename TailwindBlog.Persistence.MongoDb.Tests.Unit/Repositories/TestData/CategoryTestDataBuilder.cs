// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryTestDataBuilder.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

namespace TailwindBlog.Persistence.Repositories.TestData;

/// <summary>
/// Builder class for creating test Category data.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class CategoryTestDataBuilder
{
    private string _name = "Test Category";
    private string _urlSlug = "test-category";
    private bool _skipValidation = true;

    /// <summary>
    /// Sets the name of the category.
    /// </summary>
    public CategoryTestDataBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    /// <summary>
    /// Sets the URL slug of the category.
    /// </summary>
    public CategoryTestDataBuilder WithUrlSlug(string urlSlug)
    {
        _urlSlug = urlSlug;
        return this;
    }

    /// <summary>
    /// Sets whether to skip validation when creating the category.
    /// </summary>
    public CategoryTestDataBuilder WithSkipValidation(bool skipValidation)
    {
        _skipValidation = skipValidation;
        return this;
    }

    /// <summary>
    /// Builds a single category with the configured properties.
    /// </summary>
    public Category Build()
    {
        return new Category(_name, _urlSlug, _skipValidation);
    }

    /// <summary>
    /// Builds a list of categories with the configured properties.
    /// </summary>
    /// <param name="count">The number of categories to create.</param>
    public List<Category> BuildMany(int count)
    {
        var categories = new List<Category>();
        for (var i = 0; i < count; i++)
        {
            categories.Add(new Category(
                $"{_name} {i + 1}",
                $"{_urlSlug}-{i + 1}",
                _skipValidation));
        }
        return categories;
    }

    /// <summary>
    /// Creates a default test category.
    /// </summary>
    public static Category CreateDefault()
    {
        return new CategoryTestDataBuilder().Build();
    }

    /// <summary>
    /// Creates a list of default test categories.
    /// </summary>
    /// <param name="count">The number of categories to create.</param>
    public static List<Category> CreateDefaultMany(int count)
    {
        return new CategoryTestDataBuilder().BuildMany(count);
    }
} 