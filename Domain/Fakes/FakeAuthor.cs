// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FakeAuthor.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Fakes;

/// <summary>
///   Provides fake data generation methods for the <see cref="Author" /> entity.
/// </summary>
public static class FakeAuthor
{

	/// <summary>
	///   Generates a new fake <see cref="Author" /> object.
	/// </summary>
	/// <param name="useSeed">Indicates whether to apply a fixed seed for deterministic results.</param>
	/// <returns>A single fake <see cref="Author" /> object.</returns>
	public static Author GetNewAuthor(bool useSeed = false)
	{
		return GenerateFake(useSeed).Generate();
	}

	/// <summary>
	///   Generates a list of fake <see cref="Author" /> objects.
	/// </summary>
	/// <param name="numberRequested">The number of <see cref="Author" /> objects to generate.</param>
	/// <param name="useSeed">Indicates whether to apply a fixed seed for deterministic results.</param>
	/// <returns>A list of fake <see cref="Author" /> objects.</returns>
	public static List<Author> GetAuthors(int numberRequested, bool useSeed = false)
	{
		return GenerateFake(useSeed).Generate(numberRequested);
	}

	/// <summary>
	///   Generates a configured <see cref="Faker{T}" /> for creating fake <see cref="Author" /> objects.
	/// </summary>
	/// <param name="useSeed">Indicates whether to apply a fixed seed for deterministic results.</param>
	/// <returns>A configured <see cref="Faker{Author}" /> instance.</returns>
	internal static Faker<Author> GenerateFake(bool useSeed = false)
	{
		const int seed = 621;

		var fake = new Faker<Author>()
				.RuleFor(x => x.Id, Guid.CreateVersion7().ToString())
				.RuleFor(x => x.UserName, f => f.Name.FullName())
				.RuleFor(x => x.Email, (f, u) => f.Internet.Email(u.UserName));

		return useSeed ? fake.UseSeed(seed) : fake;
	}

}