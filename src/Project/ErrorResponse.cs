namespace Shane32.PostGrid;

/// <summary>
/// Represents an error response from the PostGrid API.
/// </summary>
internal class ErrorResponse
{
    /// <summary>
    /// Gets or sets the error details.
    /// </summary>
    public ErrorDetails? Error { get; set; }
}
