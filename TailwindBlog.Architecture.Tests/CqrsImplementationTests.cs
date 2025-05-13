// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CqrsImplementationTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

namespace TailwindBlog.Architecture.Tests;

public class CqrsImplementationTests
{
    [Fact(DisplayName = "CQRS Test: Commands should modify state")]
    public void Commands_Should_Modify_State()
    {
        var assembly = AssemblyReference.ApiService;

        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespaceContaining("Commands")
            .And()
            .HaveNameEndingWith("Command")
            .Should()
            .HaveDependencyOn("TailwindBlog.Persistence.MongoDb")
            .Or()
            .HaveDependencyOn("TailwindBlog.Domain.Interfaces")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Commands should interact with persistence or domain interfaces to modify state");
    }

    [Fact(DisplayName = "CQRS Test: Commands should return Result")]
    public void Commands_Should_Return_Result()
    {
        var assembly = AssemblyReference.ApiService;

        // get all types in assembly that implement IRequestHandler<,>
        var commandHandlers = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespaceContaining("Commands")
            .GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition().Name == "IRequestHandler`2"))
            .ToList();

        var handlerMethods = commandHandlers
            .SelectMany(t => t.GetMethods())
            .Where(m => m.Name == "Handle")
            .ToList();

        handlerMethods.Should().AllSatisfy(method =>
        {
            var returnType = method.ReturnType;
            returnType.IsGenericType.Should().BeTrue("Return type should be generic");
            returnType.GetGenericTypeDefinition().Should().Be(typeof(Task<>));
            var resultType = returnType.GetGenericArguments()[0];
            resultType.Should().Match(t =>
                t == typeof(Result) ||
                (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Result<>)),
                "Command handlers should return Result or Result<T>");
        }, "Command handlers should return Result or Result<T>");
    }

    // [Fact(DisplayName = "CQRS Test: Queries should be read-only")]
    // public void Queries_Should_Be_ReadOnly()
    // {
    //     var assembly = AssemblyReference.ApiService;
    //
    //     var result = Types
    //         .InAssembly(assembly)
    //         .That()
    //         .ResideInNamespaceContaining("Queries")
    //         .Should()
    //         .NotHaveDependencyOn("TailwindBlog.ApiService.Features.Commands")
    //         .GetResult();
    //
    //     result.IsSuccessful.Should().BeTrue(
    //         "Queries should not depend on Commands");
    //
    //     // Check for repository mutating methods.
    //     var queryHandlers = Types
    //         .InAssembly(assembly)
    //         .That()
    //         .ResideInNamespaceContaining("Queries")
    //         .GetTypes()
    //         .Where(t => t.GetInterfaces().Any(i =>
    //             i.IsGenericType &&
    //             i.GetGenericTypeDefinition().Name == "IRequestHandler`2"))
    //         .ToList();
    //
    //     foreach (var handler in queryHandlers)
    //     {
    //         var handlerTestResult = Types
    //             .InAssembly(assembly)
    //             .That(t => t.FullName == handler.FullName)
    //             .Should()
    //             .NotCallAny(
    //                 "*.Add",
    //                 "*.Remove",
    //                 "*.Update",
    //                 "*.Delete",
    //                 "*.AddRange*",
    //                 "*.RemoveRange*")
    //             .GetResult();
    //
    //         handlerTestResult.IsSuccessful.Should().BeTrue($"Query handler {handler.Name} should not call methods that modify state");
    //     }
    // }

    [Fact(DisplayName = "CQRS Test: Queries should return domain objects")]
    public void Queries_Should_Return_Domain_Objects()
    {
        var assembly = AssemblyReference.ApiService;

        var queryHandlers = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespaceContaining("Queries")
            .GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition().Name == "IRequestHandler`2"))
            .ToList();

        var handlerMethods = queryHandlers
            .SelectMany(t => t.GetMethods())
            .Where(m => m.Name == "Handle")
            .ToList();

        handlerMethods.Should().AllSatisfy(method =>
        {
            var returnType = method.ReturnType;
            returnType.IsGenericType.Should().BeTrue("Return type should be generic");
            returnType.GetGenericTypeDefinition().Should().Be(typeof(Task<>));
            var resultType = returnType.GetGenericArguments()[0];
            resultType.Should().Match(t =>
                typeof(IEnumerable).IsAssignableFrom(t) ||
                t == typeof(Result) ||
                (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Result<>)),
                "Query handlers should return collections or Result types");
        }, "Query handlers should return collections or Result types");
    }
}