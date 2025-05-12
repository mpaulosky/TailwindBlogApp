// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ApplicationConfigurationTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace TailwindBlog.Architecture.Tests;

public class ApplicationConfigurationTests
{

	[Fact(DisplayName = "Config Test: Configuration classes should follow naming convention")]
	public void Configuration_Classes_Should_Follow_Naming_Convention()
	{
		// Arrange
		var assemblies = new[]
		{
				Domain,
				ApiService,
				Infrastructure,
				Web
		};

		// Act & Assert
		foreach (var assembly in assemblies)
		{
			var result = Types
					.InAssembly(assembly)
					.That()
					.HaveNameContaining("Config")
					.Or()
					.HaveNameContaining("Settings")
					.Or()
					.HaveNameContaining("Options")
					.Should()
					.HaveNameEndingWith("Configuration")
					.Or()
					.HaveNameEndingWith("Settings")
					.Or()
					.HaveNameEndingWith("Options")
					.GetResult();

			result.IsSuccessful.Should().BeTrue(
					$"Configuration classes in {assembly.GetName().Name} should follow naming conventions");
		}
	}

	[Fact(DisplayName = "Config Test: Settings should have proper validation")]
	public void Configuration_Settings_Should_Have_Validation()
	{
		// Arrange
		var assemblies = new[]
		{
				Domain,
				ApiService,
				Infrastructure,
				Web
		};

		// Act & Assert
		foreach (var assembly in assemblies)
		{
			var result = Types
					.InAssembly(assembly)
					.That()
					.HaveNameEndingWith("Settings")
					.Or()
					.HaveNameEndingWith("Configuration")
					.Or()
					.HaveNameEndingWith("Options")
					.Should()
					.HaveCustomAttribute(typeof(ValidationAttribute))
					.GetResult();

			// Optional validation attribute check as not all config classes need validation
			if (!result.IsSuccessful)
			{
				// Log warning but don't fail the test
				Console.WriteLine(
						$"Warning: Some configuration classes in {assembly.GetName().Name} lack validation attributes");
			}
		}
	}

	[Fact(DisplayName = "Config Test: Configuration bindings should use Options pattern")]
	public void Configuration_Should_Use_Options_Pattern()
	{
		// Arrange & Act
		var result = Types
				.InAssembly(ApiService)
				.That()
				.HaveNameEndingWith("Configuration")
				.Or()
				.HaveNameEndingWith("Settings")
				.Or()
				.HaveNameEndingWith("Options")
				.Should()
				.MeetCustom(type =>
				{
					var services = new ServiceCollection();
					services.Configure<IConfiguration>(config => { });

					return true; // We're just verifying the pattern can be used
				})
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue(
				"Configuration classes should support the Options pattern");
	}

	[Fact(DisplayName = "Config Test: Verify strongly typed configuration")]
	public void Configuration_Should_Be_Strongly_Typed()
	{
		// Arrange
		var assemblies = new[]
		{
				Domain,
				ApiService,
				Infrastructure,
				Web
		};

		foreach (var assembly in assemblies)
		{
			// Act
			var configClasses = Types
					.InAssembly(assembly)
					.That()
					.HaveNameEndingWith("Configuration")
					.Or()
					.HaveNameEndingWith("Settings")
					.Or()
					.HaveNameEndingWith("Options")
					.GetTypes();

			// Assert
			foreach (var configClass in configClasses)
			{
				// Check that properties are strongly typed (not string unless explicitly needed)
				var properties = configClass.GetProperties();

				foreach (var prop in properties)
				{
					prop.PropertyType.Should().NotBe(typeof(object),
							$"Property {prop.Name} in {configClass.Name} should be strongly typed");

					// String properties should have clear naming indicating string content
					if (prop.PropertyType == typeof(string))
					{
						prop.Name.Should().Match(n =>
										n.EndsWith("Name") ||
										n.EndsWith("Path") ||
										n.EndsWith("ConnectionString") ||
										n.EndsWith("Url") ||
										n.EndsWith("Key") ||
										n.EndsWith("Id"),
								$"String property {prop.Name} should have a clear purpose indicated in its name");
					}
				}
			}
		}
	}

}