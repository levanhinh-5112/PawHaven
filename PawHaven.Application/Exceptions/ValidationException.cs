namespace PawHaven.Application.Exceptions;

/// <summary>
/// Thrown when validation of a command or query fails.
/// Contains a dictionary of field names to validation error messages.
/// Field names should use camelCase to match JSON API contract.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Gets the validation errors mapped by field name (camelCase).
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with a single error.
    /// </summary>
    /// <param name="fieldName">The name of the field that failed validation (camelCase).</param>
    /// <param name="errorMessage">The validation error message.</param>
    public ValidationException(string fieldName, string errorMessage)
        : this(new Dictionary<string, string[]> { { fieldName, [errorMessage] } })
    {
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with multiple errors.
    /// </summary>
    /// <param name="errors">A dictionary mapping field names (camelCase) to arrays of error messages.</param>
    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors.AsReadOnly();
    }

    /// <summary>
    /// Creates a ValidationException from an ArgumentException.
    /// Extracts the parameter name as the field name and converts to camelCase.
    /// </summary>
    /// <param name="exception">The ArgumentException to convert.</param>
    /// <returns>A ValidationException with the error mapped to the camelCase field name.</returns>
    public static ValidationException FromArgumentException(ArgumentException exception)
    {
        var fieldName = exception.ParamName ?? "unknown";
        // Convert PascalCase to camelCase (e.g., "Name" -> "name", "PhotoUrl" -> "photoUrl")
        var camelCaseFieldName = ToCamelCase(fieldName);
        // ArgumentException.Message appends " (Parameter 'paramName')" when a paramName is provided.
        // Strip that suffix so the API contract surfaces only the clean validation message.
        var suffix = $" (Parameter '{exception.ParamName}')";
        var cleanMessage = exception.Message.EndsWith(suffix)
            ? exception.Message[..^suffix.Length]
            : exception.Message;
        return new ValidationException(camelCaseFieldName, cleanMessage);
    }

    /// <summary>
    /// Converts a PascalCase or regular casing string to camelCase.
    /// </summary>
    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
            return str;

        return char.ToLowerInvariant(str[0]) + str[1..];
    }
}
