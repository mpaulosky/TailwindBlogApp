using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace AppHost;

/// <summary>
///   Provides functionality to run end-to-end tests as a command in the application.
/// </summary>
public static class RunE2ETestsCommand
{

	private const string Name = "run-e2e-tests";

	/// <summary>
	///   Adds a command to run end-to-end tests to the specified resource builder.
	/// </summary>
	/// <param name="builder">The resource builder to add the command to.</param>
	/// <returns>The configured resource builder.</returns>
	public static IResourceBuilder<ProjectResource> WithRunE2eTestsCommand(
			this IResourceBuilder<ProjectResource> builder)
	{
		builder.WithCommand(
				Name,
				"Run end to end tests",
				context => RunTests(),
				OnUpdateResourceState,
				iconName: "BookGlobe",
				iconVariant: IconVariant.Filled);

		return builder;
	}

	/// <summary>
	///   Executes the end-to-end tests using the dotnet test command.
	/// </summary>
	/// <returns>A task representing the asynchronous operation with the execution result.</returns>
	private static async Task<ExecuteCommandResult> RunTests()
	{
		var processStartInfo = new ProcessStartInfo
		{
				FileName = "dotnet",
				Arguments = "test ../../e2e/SharpSite.E2E",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
		};

		var process = new Process { StartInfo = processStartInfo };
		process.Start();

		var output = await process.StandardOutput.ReadToEndAsync();
		var error = await process.StandardError.ReadToEndAsync();

		process.WaitForExit();
		Console.WriteLine("E2E Tests Output: " + output);

		if (process.ExitCode == 0)
		{
			return new ExecuteCommandResult { Success = true };
		}

		return new ExecuteCommandResult { Success = false, ErrorMessage = error };
	}

	/// <summary>
	///   Updates the command state based on the resource's health status.
	/// </summary>
	/// <param name="context">The context containing information about the resource state.</param>
	/// <returns>The updated command state.</returns>
	private static ResourceCommandState OnUpdateResourceState(
			UpdateCommandStateContext context)
	{
		var logger = context.ServiceProvider.GetRequiredService<ILogger<Program>>();

		//if (logger.IsEnabled(LogLevel.Information))
		//{
		//	logger.LogInformation(
		//			"Updating resource state: {ResourceSnapshot}",
		//			context.ResourceSnapshot);
		//}

		return context.ResourceSnapshot.HealthStatus is HealthStatus.Healthy
				? ResourceCommandState.Enabled
				: ResourceCommandState.Disabled;
	}

}