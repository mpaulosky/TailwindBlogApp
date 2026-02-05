// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GlobalUsings.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Web;

global using Bogus;

global using MongoDB.Bson;
global using MongoDB.Bson.Serialization.Attributes;
global using MongoDB.Driver;

global using Domain.Abstractions;
global using Domain.Entities;
global using Domain.Models;

global using FluentValidation;

global using Domain.Enums;
global using Domain.Helpers;
global using Domain.Interfaces;
global using Domain.Validators;

global using Microsoft.EntityFrameworkCore;

global using ValidationException = FluentValidation.ValidationException;