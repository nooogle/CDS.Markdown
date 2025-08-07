# GitHub Copilot C# Code Generation Guidelines

Please generate C# code that follows these practices:

1. **Formatting**
   - Use .NET conventions:
     - `PascalCase` for public/protected members, methods, properties, and constants.
     - `camelCase` for private fields, parameters, and locals.
     - No underscores in identifiers.
     - Braces on a new line; always use curly braces for control flow.
   - Use `var` when the type is obvious; otherwise, use explicit types.
   - Prefer expression-bodied members for simple methods/properties.
   - Use single-line `using` statements.
   - Use the file-scoped namespace (`namespace Foo.Bar;`).

2. **Comments**
   - Add XML documentation (`///`) to all public classes, methods, and properties.
   - Keep comments meaningful; avoid boilerplate.
   - Use inline comments for complex logic.

3. **Naming**
   - Use descriptive, meaningful names.
   - Avoid abbreviations unless standard (e.g., `Http`, `Xml`).
   - Prefix interfaces with `I` (e.g., `IRepository`).

4. **Exception Handling**
   - Use `try-catch` for external interactions (file I/O, HTTP, etc.).
   - Provide detailed exception messages; rethrow only when necessary.

5. **Async**
   - Use `async`/`await` for asynchronous operations.
   - Name async methods with the `Async` suffix.
   - Only use `async void` for event handlers.

6. **Code Generation**
   - When updating a method, show only the method, not the whole class.

7. **Unit Testing**
   - Use MSTest for public methods.
   - Use FluentAssertions for assertions (`actual.Should().Be(...)`).
   - Follow Arrange-Act-Assert in tests.

8. **LINQ**
   - Use LINQ for collection operations, prioritizing readability.

9. **Best Practices**
   - Use `using` for disposables.
   - Implement `IDisposable` when managing unmanaged resources.
   - Prefer dependency injection over static classes.
   - Use method overloads instead of default parameter values.

10. **File Structure**
    - One class per file.
    - Match namespaces to folder structure.

Ensure all generated code follows these guidelines and demonstrates good practices.
