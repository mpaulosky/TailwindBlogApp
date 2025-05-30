// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IDatabaseSettings.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =============================================

namespace TailwindBlog.Domain.Interfaces;

public interface IDatabaseSettings
{

	string ConnectionStrings { get; set; }

	string DatabaseName { get; set; }

}