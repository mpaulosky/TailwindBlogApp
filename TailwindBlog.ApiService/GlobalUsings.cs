// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GlobalUsings.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

#region

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Reflection;
global using System.Threading;
global using System.Threading.Tasks;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using MongoDB.Bson;

global using MyMediator;

global using Scalar.AspNetCore;

global using TailwindBlog.ApiService.Extensions;
global using TailwindBlog.Domain.Abstractions;
global using static TailwindBlog.Domain.Constants.ServiceNames;
global using TailwindBlog.Domain.Entities;
global using TailwindBlog.Domain.Interfaces;
global using TailwindBlog.Domain.Models;
global using TailwindBlog.Persistence;
global using TailwindBlog.Persistence.Repositories;

#endregion