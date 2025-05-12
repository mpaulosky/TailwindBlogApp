// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GlobalUsings.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Linq;
global using System.Linq.Expressions;
global using System.Threading;
global using System.Threading.Tasks;

global using Bogus;

global using FluentAssertions;

global using JetBrains.Annotations;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Query;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Configuration.Memory;
global using Microsoft.Extensions.DependencyInjection;

global using NSubstitute;
global using NSubstitute.Extensions;

global using TailwindBlog.Domain.Abstractions;
global using TailwindBlog.Domain.Entities;
global using TailwindBlog.Domain.Models;
global using TailwindBlog.Persistence;
global using TailwindBlog.Persistence.Repositories;

global using Xunit;
