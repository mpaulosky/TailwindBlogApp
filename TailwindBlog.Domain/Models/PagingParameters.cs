namespace TailwindBlog.Domain.Models;

public class PagingParameters
{
	private const int MaxPageSize = 50;
	private int _pageSize = 10;

	/// <summary>
	/// Gets or sets the page number (1-based).
	/// </summary>
	public int PageNumber { get; set; } = 1;

	/// <summary>
	/// Gets or sets the page size, with a maximum limit.
	/// </summary>
	public int PageSize
	{
		get => _pageSize;
		set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
	}

	/// <summary>
	/// TryParse method for ASP.NET model binding (required for minimal APIs in .NET 8+)
	/// </summary>
	public static bool TryParse(string? value, IFormatProvider? provider, out PagingParameters result)
	{
		result = new PagingParameters();
		if (string.IsNullOrEmpty(value))
		{
			return true;
		}
		var dict = System.Web.HttpUtility.ParseQueryString(value);
		if (int.TryParse(dict["PageNumber"], out var pageNumber))
		{
			result.PageNumber = pageNumber;
		}
		if (int.TryParse(dict["PageSize"], out var pageSize))
		{
			result.PageSize = pageSize;
		}
		return true;
	}
}
