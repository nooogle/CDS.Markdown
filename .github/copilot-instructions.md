# Instructions for GitHub Copilot

Please generate C# code that adheres to the following practices:

1. **Code Formatting**:
   - Follow .NET conventions:
     - Use `PascalCase` for public and protected members, including methods, properties, and constants.
     - Use `camelCase` for private fields, parameters, and local variables.
     - Place braces on a new line, following the standard .NET formatting rules.
     - Don't use underscores in identifiers.
     - Always use curly braces for control flow statements, even when the body is empty or contains a single statement.
   - Use `var` when the type is obvious from the right side of the assignment, explicit type otherwise.
   - Use expression-bodied members for simple methods and properties to improve readability.
   - Use single-line using statements in preference to block using statements.
   - Declare the namespace for a file using the new semi-colon syntax instead of curly braces.

2. **Commenting**:
   - Use XML documentation comments (`///`) for all public methods, properties, and classes.
   - Keep comments meaningful and avoid boilerplate XML documentation.
   - Provide inline comments for complex logic or where clarity is needed.

3. **Naming**:
   - Prefer descriptive and meaningful names for variables, methods, and classes.
   - Avoid abbreviations unless they are commonly understood (e.g., `Http`, `Xml`).
   - Use prefixes like `I` for interfaces (e.g., `IRepository`).

4. **Exception Handling**:
   - Include `try-catch` blocks for any code that interacts with external systems (e.g., file I/O, HTTP requests).
   - Provide detailed exception messages and rethrow exceptions only when necessary.

5. **Async Programming**:
   - Use `async` and `await` for all asynchronous operations.
   - Ensure methods that return tasks follow the naming convention of ending with "Async" (e.g., `GetDataAsync`).
   - Avoid `async void` methods except for event handlers.

6. **Creating new code**:
   - When asked to work on a method just show the updated method, don't regenerate the entire class.

7. **Unit Testing**:
   - Write unit tests using MSTest for all public methods.
   - Use the FluentAssertions library for assertions, using the actual-should syntax.
   - Follow the Arrange-Act-Assert pattern in test methods.

8. **LINQ Usage**:
   - Use LINQ for collection filtering and transformation, prioritizing readability and maintainability.

9. **Patterns and Practices**:
   - Use the `using` statement for disposable resources.
   - Implement `IDisposable` correctly in classes that manage unmanaged resources.
   - Prefer dependency injection over static classes where practical.
   - Avoid default parameter values in favor of method overloads.

10. **File Structure**:
    - Place each class in its own file.
    - Organize namespaces to match the folder structure.

Please ensure the generated code follows these guidelines and includes examples of good practices whenever applicable.
