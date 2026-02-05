// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IRegisterServices.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

using Microsoft.Extensions.Hosting;

namespace Domain.Interfaces;

/// <summary>
///   Interface for services that need to register services with the web application.
/// </summary>
public interface IRegisterServices
{

	IHostApplicationBuilder RegisterServices(IHostApplicationBuilder services, bool disableRetry = false);

}