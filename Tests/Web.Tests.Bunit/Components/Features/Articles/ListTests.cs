namespace Web.Components.Features.Articles;

/// <summary>
///   Unit tests for <see cref="List" /> (Articles List).
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(List))]
public class ListTests : BunitContext
{

	private readonly IArticleService _articleServiceSub = Substitute.For<IArticleService>();

	public ListTests()
	{
		Services.AddSingleton(_articleServiceSub);
		Services.AddCascadingAuthenticationState();
		Services.AddAuthorization();

		// Setup JS module import required by QuickGrid in bUnit
		var quickGridModule =
				JSInterop.SetupModule("./_content/Microsoft.AspNetCore.Components.QuickGrid/QuickGrid.razor.js");

		// QuickGrid calls module.invoke("init", element)
		// Configure to return a dummy JS object reference from init
		quickGridModule.SetupModule("init", _ => true);
	}

	[Fact]
	public void Renders_Articles_List()
	{
		// Act
		var cut = Render<List>();

		// Assert
		cut.Markup.Should().Contain("article");
	}

	[Fact]
	public void Renders_Loading_State()
	{
		// Arrange
		var cut = Render<List>();

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, true);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Loading");
	}

	[Fact]
	public void Renders_Empty_State_When_No_Articles()
	{
		// Arrange
		var cut = Render<List>();

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_articles", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, null);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("No articles available");
	}

	[Fact]
	public void Renders_Articles_List_With_Data()
	{
		// Arrange
		var articles = new List<ArticleDto>
				{ FakeArticleDto.GetNewArticleDto(true), FakeArticleDto.GetNewArticleDto(true) };

		var cut = Render<List>();

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_articles", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, articles.AsQueryable());

		// Act
		cut.Render();

		// Assert
		foreach (var article in articles)
		{
			cut.Markup.Should().Contain(article.Title);
			cut.Markup.Should().Contain(article.Author.UserName);
		}
	}

	[Fact]
	public void Navigates_To_Create_New_Article()
	{
		// Arrange
		var nav = Services.GetRequiredService<BunitNavigationManager>();
		var cut = Render<List>();

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_articles", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, new List<ArticleDto>().AsQueryable());

		cut.Render();

		// Act
		cut.Find("button.btn-success").Click();

		// Assert
		nav.Uri.Should().EndWith("/articles/create");
	}

	[Fact]
	public void Renders_NotFound_When_Article_Is_Null()
	{
		// Arrange
		var id = Guid.NewGuid();
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, null);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain("Article not found");
	}

	[Fact]
	public void Renders_Form_With_Article_Data()
	{
		// Arrange
		var article = FakeArticleDto.GetNewArticleDto(true);
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, article.Id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, article);

		// Act
		cut.Render();

		// Assert
		cut.Markup.Should().Contain(article.Title);
		cut.Markup.Should().Contain(article.Introduction);
	}

	[Fact]
	public async Task Submits_Valid_Form_And_Navigates_On_Success()
	{
		// Arrange
		var article = FakeArticleDto.GetNewArticleDto(true);
		_articleServiceSub.UpdateAsync(article).Returns(Result.Ok(article));
		var nav = Services.GetRequiredService<BunitNavigationManager>();
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, article.Id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, article);

		cut.Render();

		// Act
		await cut.Find("form").SubmitAsync();

		// Assert
		nav.Uri.Should().EndWith("/categories");
	}

	[Fact]
	public async Task Displays_Error_On_Failed_Submit()
	{
		// Arrange
		var article = FakeArticleDto.GetNewArticleDto(true);
		_articleServiceSub.UpdateAsync(article).Returns(Result.Fail<ArticleDto>("Update failed"));
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, article.Id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, article);

		cut.Render();

		// Act
		await cut.Find("form").SubmitAsync();

		// Assert
		cut.Markup.Should().Contain("Update failed");
	}

	[Fact]
	public void Cancel_Button_Navigates_To_List()
	{
		// Arrange
		var article = FakeArticleDto.GetNewArticleDto(true);
		var nav = Services.GetRequiredService<BunitNavigationManager>();
		var cut = Render<Edit>(parameters => parameters.Add(p => p.Id, article.Id));

		cut.Instance.GetType().GetField("_isLoading", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, false);

		cut.Instance.GetType().GetField("_article", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(cut.Instance, article);

		cut.Render();

		// Act
		cut.Find("button.btn-light").Click();

		// Assert
		nav.Uri.Should().EndWith("/articles");
	}

}