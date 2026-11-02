namespace PawHaven.API.ExceptionHandling;

/// <summary>
/// Extension methods for registering exception handling services and middleware.
/// </summary>
public static class ExceptionHandlingExtensions
{
    /// <summary>
    /// Adds exception handling middleware to the application pipeline.
    /// This should be called early in the middleware pipeline to catch exceptions from all subsequent middleware.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
