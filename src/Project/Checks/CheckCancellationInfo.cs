namespace Shane32.PostGrid.Checks;

/// <summary>
/// Represents cancellation information for a check
/// </summary>
public class CheckCancellationInfo
{
    /// <summary>
    /// The ID of the user who cancelled the check
    /// </summary>
    public required string CancelledByUser { get; set; }

    /// <summary>
    /// The reason for cancellation
    /// </summary>
    public required string Reason { get; set; }
}
