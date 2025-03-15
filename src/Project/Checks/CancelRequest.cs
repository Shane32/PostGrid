namespace Shane32.PostGrid.Checks;

/// <summary>
/// Represents a request to cancel a check from PostGrid.
/// </summary>
public class CancelRequest
{
    /// <summary>
    /// The unique ID of the check to cancel.
    /// </summary>
    public
#if NET7_0_OR_GREATER
        required
#endif
        string Id { get; set; } = null!;

    /// <summary>
    /// A note explaining the reason for cancellation.
    /// </summary>
    public string? Note { get; set; }
}
