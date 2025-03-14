namespace Shane32.PostGrid.Contacts;

/// <summary>
/// Represents a request to delete a contact from PostGrid.
/// </summary>
public class DeleteRequest
{
    /// <summary>
    /// The unique ID of the contact to delete.
    /// </summary>
    public required string Id { get; set; }
}
