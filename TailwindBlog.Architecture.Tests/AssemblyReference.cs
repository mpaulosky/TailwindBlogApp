#region

using System.Reflection;
using TailwindBlog.Domain.Interfaces;
using TailwindBlog.Domain.Constants;
using TailwindBlog.ApiService;
using TailwindBlog.Web;

#endregion

namespace TailwindBlog.Architecture.Tests;

public static class AssemblyReference
{
	public static readonly Assembly Domain = typeof(TailwindBlog.Domain.Abstractions.Result).Assembly;
	public static readonly Assembly ApiService = typeof(ApiServiceProgram).Assembly;
	public static readonly Assembly Web = typeof(WebProgram).Assembly;
	public static readonly Assembly Infrastructure = typeof(ServiceNames).Assembly;
}
