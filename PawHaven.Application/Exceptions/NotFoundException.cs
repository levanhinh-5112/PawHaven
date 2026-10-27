namespace PawHaven.Application.Exceptions;

/// <summary>
/// Thrown when a requested resource is not found in the database.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Gets the resource type that was not found.
    /// </summary>
    public string? ResourceType { get; }

    /// <summary>
    /// Gets the resource identifier that was not found.
    /// </summary>
    public object? ResourceId { get; }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public NotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class with resource context.
    /// </summary>
    /// <param name="resourceType">The type of resource that was not found.</param>
    /// <param name="resourceId">The identifier of the resource that was not found.</param>
    public NotFoundException(string resourceType, object resourceId)
        : base($"{resourceType} with ID {resourceId} was not found.")
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
    }
}
