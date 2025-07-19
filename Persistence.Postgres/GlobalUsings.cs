// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GlobalUsings.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.Postgres
// =======================================================

global using Domain.Abstractions;
global using Domain.Entities;
global using Domain.Interfaces;

global using Mapster;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;

global using Persistence.Postgres;
global using Persistence.Postgres.Services;
global using Persistence.Services;

global using static Domain.Constants.ServiceNames;