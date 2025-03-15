namespace Shane32.PostGrid.Checks;

/// <summary>
/// Represents a request to get a check from PostGrid.
/// </summary>
public class GetRequest
{
    /// <summary>
    /// The unique ID of the check to retrieve.
    /// </summary>
    public
#if NET7_0_OR_GREATER
        required
#endif
        string Id { get; set; } = null!;
}
