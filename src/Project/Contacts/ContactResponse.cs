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
    /// Hide contact details apart from final print
    /// </summary>
    public required bool Secret { get; set; }

    /// <summary>
    /// Skip address correction and verification
    /// </summary>
    public required bool SkipVerification { get; set; }

    /// <summary>
    /// Override the given address verification status
    /// </summary>
    public required bool ForceVerifiedStatus { get; set; }
}
