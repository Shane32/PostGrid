using System.Text.Json.Serialization;

namespace Shane32.PostGrid;

/// <summary>
/// Represents an error response from the PostGrid API.
/// </summary>
internal class ErrorResponse
{
    /// <summary>
    /// Gets or sets the type of the error.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string? Message { get; set; }
}
