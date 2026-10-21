using Microsoft.AspNetCore.Mvc;

namespace PawHaven.API.Controllers;

/// <summary>
/// Health check endpoint for API readiness verification.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Returns the health status of the API.
    /// </summary>
    /// <returns>A health status response with current UTC timestamp.</returns>
    [HttpGet]
    public IActionResult GetHealth()
    {
        var response = new HealthResponse
        {
            Status = "Healthy",
            TimestampUtc = DateTime.UtcNow
        };

        return Ok(response);
    }

    /// <summary>
    /// Health check response model.
    /// </summary>
    public class HealthResponse
    {
        /// <summary>
        /// Health status indicator.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// UTC timestamp of the health check.
        /// </summary>
        public required DateTime TimestampUtc { get; set; }
    }
}
