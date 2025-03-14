namespace Shane32.PostGrid.Checks;

/// <summary>
/// Represents a request to get a check from PostGrid.
/// </summary>
public class GetRequest
{
    /// <summary>
    /// The unique ID of the check to retrieve.
    /// </summary>
    public required string Id { get; set; }
}