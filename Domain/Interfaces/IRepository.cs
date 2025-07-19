// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IRepository.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Interfaces;

/// <summary>
///   Base interface for repository pattern implementation
/// </summary>
/// <typeparam name="T">The type of entity</typeparam>
public interface IRepository<T> where T : class
{

	Task<Result> ArchiveAsync(T entity);

	Task<Result> CreateAsync(T entity);

	Task<Result<T>> GetAsync(Guid entityId);

	Task<Result<IEnumerable<T>>> GetAllAsync();

	Task<Result> UpdateAsync(Guid entityId, T entity);

}