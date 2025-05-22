// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleTestDataBuilder.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

namespace TailwindBlog.Persistence.Repositories.TestData;

/// <summary>
/// Builder class for creating test Article data.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ArticleTestDataBuilder
{
    private string _title = "Test Article";
    private string _introduction = "Test Content";
    private string _coverImageUrl = "test-cover.jpg";
    private string _urlSlug = "test-article";
    private AppUserModel _author = new() { Id = "test-user" };
    private bool _isPublished;
    private DateTime? _publishedOn;
    private bool _skipValidation = true;

    /// <summary>
    /// Sets the title of the article.
    /// </summary>
    public ArticleTestDataBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    /// <summary>
    /// Sets the introduction of the article.
    /// </summary>
    public ArticleTestDataBuilder WithIntroduction(string introduction)
    {
        _introduction = introduction;
        return this;
    }

    /// <summary>
    /// Sets the cover image URL of the article.
    /// </summary>
    public ArticleTestDataBuilder WithCoverImageUrl(string coverImageUrl)
    {
        _coverImageUrl = coverImageUrl;
        return this;
    }

    /// <summary>
    /// Sets the URL slug of the article.
    /// </summary>
    public ArticleTestDataBuilder WithUrlSlug(string urlSlug)
    {
        _urlSlug = urlSlug;
        return this;
    }

    /// <summary>
    /// Sets the author of the article.
    /// </summary>
    public ArticleTestDataBuilder WithAuthor(AppUserModel author)
    {
        _author = author;
        return this;
    }

    /// <summary>
    /// Sets the published state of the article.
    /// </summary>
    public ArticleTestDataBuilder WithPublishedState(bool isPublished, DateTime? publishedOn = null)
    {
        _isPublished = isPublished;
        _publishedOn = publishedOn;
        return this;
    }

    /// <summary>
    /// Sets whether to skip validation when creating the article.
    /// </summary>
    public ArticleTestDataBuilder WithSkipValidation(bool skipValidation)
    {
        _skipValidation = skipValidation;
        return this;
    }

    /// <summary>
    /// Builds a single article with the configured properties.
    /// </summary>
    public Article Build()
    {
        return new Article(
            _title,
            _introduction,
            _coverImageUrl,
            _urlSlug,
            _author,
            _isPublished,
            _publishedOn,
            _skipValidation);
    }

    /// <summary>
    /// Builds a list of articles with the configured properties.
    /// </summary>
    /// <param name="count">The number of articles to create.</param>
    public List<Article> BuildMany(int count)
    {
        var articles = new List<Article>();
        for (var i = 0; i < count; i++)
        {
            articles.Add(new Article(
                $"{_title} {i + 1}",
                $"{_introduction} {i + 1}",
                $"{_coverImageUrl}-{i + 1}",
                $"{_urlSlug}-{i + 1}",
                _author,
                _isPublished,
                _publishedOn,
                _skipValidation));
        }
        return articles;
    }

    /// <summary>
    /// Creates a default test article.
    /// </summary>
    public static Article CreateDefault()
    {
        return new ArticleTestDataBuilder().Build();
    }

    /// <summary>
    /// Creates a list of default test articles.
    /// </summary>
    /// <param name="count">The number of articles to create.</param>
    public static List<Article> CreateDefaultMany(int count)
    {
        return new ArticleTestDataBuilder().BuildMany(count);
    }
} 