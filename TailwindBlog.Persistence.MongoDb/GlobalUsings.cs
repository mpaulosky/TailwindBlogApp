// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     GlobalUsings.cs
// Project Name:  TailwindBlog.Persistence.MongoDb
// =======================================================

#region

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Linq.Expressions;
global using System.Threading.Tasks;

global using Mapster;

global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using MongoDB.Bson;
global using MongoDB.Driver;

global using TailwindBlog.Domain.Abstractions;
global using static TailwindBlog.Domain.Constants.ServiceNames;
global using TailwindBlog.Domain.Entities;
global using TailwindBlog.Domain.Helpers;
global using TailwindBlog.Domain.Interfaces;
global using TailwindBlog.Domain.Models;
global using TailwindBlog.Persistence.Repositories;

#endregion