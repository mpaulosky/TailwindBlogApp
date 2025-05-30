# Copilot Instructions

**Last updated:** May 5, 2025

These instructions define the required coding, architecture, and project rules for all .NET code in this repository. All contributors and AI assistants (such as GitHub Copilot) must follow these rules. For further details, see [CONTRIBUTING.md](docs/CONTRIBUTING.md).

---

## C# (Required)

### Style

- **Use .editorconfig:** `true`
- **Preferred Modifier Order:** `public`, `private`, `protected`, `internal`, `static`, `readonly`, `const`

  - _Example:_

  ```csharp
    public static readonly int MY_CONST = 42;
  ```

- **Use Explicit Type:** `true` (except where `var` improves clarity)
- **Use Var:** `true` (when the type is obvious)
- **Prefer Null Check:**
  - Use `is null`: `true`
  - Use `is not null`: `true`
- **Prefer Primary Constructors:** `false`
- **Prefer Records:** `true`
- **Prefer Minimal APIs:** `true`
- **Prefer File Scoped Namespaces:** `true`
- **Use Global Usings:** `true`
- **Use Nullable Reference Types:** `true`
- **Use Pattern Matching:** `true`

### Naming

- **Interface Prefix:** `I` (e.g., `IService`)
- **Async Suffix:** `Async` (e.g., `GetDataAsync`)
- **Private Field Prefix:** `_` (e.g., `_myField`)
- **Constant Case:** `UPPER_CASE` (e.g., `MAX_SIZE`)
- **Component Suffix:** `Component` (for Blazor components)
- **Blazor Page Suffix:** `Page` (for Blazor pages)

### Security (Required)

- **Require HTTPS:** `true`
- **Require Authentication:** `true`
- **Require Authorization:** `true`
- **Use Antiforgery Tokens:** `true`
- **Use CORS:** `true`
- **Use Secure Headers:** `true`

### Architecture (Required)

- **Enforce SOLID:** `true`
- **Enforce Dependency Injection:** `true`
- **Enforce Async/Await:** `true`
- **Enforce Strongly Typed Config:** `true`
- **Enforce CQRS:** `true`
- **Enforce Unit Tests:** `true`
- **Enforce Integration Tests:** `true`
- **Enforce Vertical Slice Architecture:** `true`
- **Enforce Aspire:** `true` (_.NET Aspire: use Aspire orchestration and best practices where applicable_)

### Blazor (Required)

- **Enforce State Management:** `true`
- **Use Interactive Server Rendering:** `true`
- **Use Stream Rendering:** `true`
- **Enforce Component Lifecycle:** `true`
- **Use Cascading Parameters:** `true`
- **Use Render Fragments:** `true`
- **Use Virtualization:** `true`
- **Use Error Boundaries:** `true`

### Documentation (Required)

- **Require XML Docs:** `true`
- **Require Swagger:** `true` (for REST APIs)
- **Require OpenAPI:** `true` (OpenAPI/Swagger must be provided for all APIs)
- **Require Component Documentation:** `true`
- **Require README:** `true`

#### Logging (Required)

- **Require Structured Logging:** `true`
- **Require Health Checks:** `true`
- **Use OpenTelemetry:** `true`
- **Use Application Insights:** `true`

#### Database (Required)

- **Use Entity Framework Core:** `true`
- **Use MongoDB:** `true`
- **Prefer Async Operations:** `true`
- **Use Migrations:** `false` (for MongoDB)
- **Use TestContainers:** `true` (for Integration testing)
- **Use Change Tracking:** `true`
- **Use DbContext Pooling:** `true`
- **Use In-Memory Database:** `false`

#### Versioning (Required)

- **Require API Versioning:** `true`
- **Use Semantic Versioning:** `true`

#### Caching (Required)

- **Require Caching Strategy:** `true`
- **Use Distributed Cache:** `true`
- **Use Output Caching:** `true`

#### Middleware (Required)

- **Require Cross-Cutting Concerns:** `true`
- **Use Exception Handling:** `true`
- **Use Request Logging:** `true`

#### Background Services (Required)

- **Require Background Service:** `true`

#### Environment (Required)

- **Require Environment Config:** `true`
- **Use User Secrets:** `true`
- **Use Key Vault:** `true`

#### Validation (Required)

- **Require Model Validation:** `true`
- **Use FluentValidation:** `true`

#### Testing (Required)

- **Require Unit Tests:** `true`
- **Require Integration Tests:** `true`
- **Require Architecture Tests:** `true`
- **Use xUnit:** `true`
- **Use FluentAssertions:** `true`
- **Use NSubstitute:** `true`
- **Use bUnit:** `true`

---

### General (Required)

- **Max Line Length:** `120`
- **Indent Style:** `tab`
- **Indent Size:** `2`
- **End of Line:** `lf`
- **Trim Trailing Whitespace:** `true`
- **Insert Final Newline:** `true`
- **Charset:** `utf-8`

---

**Note:** These rules are enforced via `.editorconfig`, StyleCop, and other tooling where possible. For questions or clarifications, see [CONTRIBUTING.md](docs/CONTRIBUTING.md).
