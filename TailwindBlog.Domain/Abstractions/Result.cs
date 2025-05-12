// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Result.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Abstractions;

public class Result
{
	public bool Success { get; }
	public bool Failure => !Success;
	public string? Error { get; }

	protected Result(bool success, string? errorMessage = null)
	{
		Success = success;
		Error = errorMessage;
	}
	public static Result Ok() => new (true);
	public static Result Fail(string errorMessage) => new (false, errorMessage);
	
	public static Result<T> Ok<T>(T value) => new(value, true);
	public static Result<T> Fail<T>(string errorMessage) => new(default, false, errorMessage);
	public static Result<T> FromValue<T>(T? value) => value is not null ? Ok(value) : Fail<T>("Provided value is null.");

}

public sealed class Result<T> : Result
{
	public T? Value { get; }

	internal Result(T? value, bool success, string? errorMessage = null)
		: base(success, errorMessage)
	{
		Value = value;
	}

	public static Result<T> Ok(T value) => new(value, true);
	
	public new static Result<T> Fail(string errorMessage) => new(default, false, errorMessage);

	public static implicit operator T?(Result<T> result) => result.Value;
	
	public static implicit operator Result<T>(T value) => Ok(value);

}