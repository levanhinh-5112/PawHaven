namespace PawHaven.Application.Exceptions;

/// <summary>
/// Thrown when a requested operation conflicts with the current state of a resource.
/// For example, attempting to delete a pet that has active applications.
/// </summary>
public class ConflictException : Exception
{
    /// <summary>
    /// Gets the resource type that has a conflict.
    /// </summary>
    public string? ResourceType { get; }

    /// <summary>
    /// Gets the resource identifier that has a conflict.
    /// </summary>
    public object? ResourceId { get; }

    /// <summary>
    /// Initializes a new instance of the ConflictException class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ConflictException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ConflictException class with resource context.
    /// </summary>
    /// <param name="resourceType">The type of resource that has a conflict.</param>
    /// <param name="resourceId">The identifier of the resource that has a conflict.</param>
    /// <param name="message">The error message describing the conflict.</param>
    public ConflictException(string resourceType, object resourceId, string message)
        : base(message)
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
    }
}
