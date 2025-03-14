namespace Shane32.PostGrid.Checks;

/// <summary>
/// Represents a request to cancel a check from PostGrid.
/// </summary>
public class CancelRequest
{
    /// <summary>
    /// The unique ID of the check to cancel.
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// A note explaining the reason for cancellation.
    /// </summary>
    public string? Note { get; set; }
}
