namespace Shane32.PostGrid.Contacts;

/// <summary>
/// Represents a request to get a contact from PostGrid.
/// </summary>
public class GetRequest
{
    /// <summary>
    /// The unique ID of the contact to retrieve.
    /// </summary>
    public
#if NET7_0_OR_GREATER
        required
#endif
        string Id { get; set; } = null!;
}
