namespace Shane32.PostGrid.Contacts;

/// <summary>
/// Represents the response from the PostGrid API when creating a contact.
/// </summary>
public class ContactResponse : ContactBaseResponse
{
    /// <summary>
    /// true if this is a live mode contact else false
    /// </summary>
    public required bool Live { get; set; }

    /// <summary>
    /// Skip address correction and verification
    /// </summary>
    public required bool SkipVerification { get; set; }

    /// <summary>
    /// Override the given address verification status
    /// </summary>
    public required bool ForceVerifiedStatus { get; set; }

    /// <summary>
    /// The date and time when the contact was created
    /// </summary>
    public required DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The date and time when the contact was last updated
    /// </summary>
    public required DateTimeOffset UpdatedAt { get; set; }
}
