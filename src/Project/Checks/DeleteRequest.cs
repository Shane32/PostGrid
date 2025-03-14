namespace Shane32.PostGrid.Checks;

/// <summary>
/// Represents a request to delete a check from PostGrid.
/// </summary>
public class DeleteRequest
{
    /// <summary>
    /// The unique ID of the check to delete.
    /// </summary>
    public required string Id { get; set; }
}