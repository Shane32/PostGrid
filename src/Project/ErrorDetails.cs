namespace Shane32.PostGrid;

/// <summary>
/// Represents the error details in an error response.
/// </summary>
internal class ErrorDetails
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
