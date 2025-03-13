namespace Shane32.PostGrid.Contacts;

/// <summary>
/// Represents the response from the PostGrid API when creating a contact.
/// </summary>
public class ContactResponse
{
    /// <summary>
    /// A unique ID prefixed with contact_
    /// </summary>
    public string? Id { get; set; }
    
    /// <summary>
    /// Always contact
    /// </summary>
    public string? Object { get; set; }
    
    /// <summary>
    /// true if this is a live mode contact else false
    /// </summary>
    public bool Live { get; set; }
    
    /// <summary>
    /// Optional line describing the contact
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// First line of address
    /// </summary>
    public string? AddressLine1 { get; set; }
    
    /// <summary>
    /// Second line of address
    /// </summary>
    public string? AddressLine2 { get; set; }
    
    /// <summary>
    /// Province or state of address
    /// </summary>
    public string? ProvinceOrState { get; set; }
    
    /// <summary>
    /// Postal or ZIP code of address
    /// </summary>
    public string? PostalOrZip { get; set; }
    
    /// <summary>
    /// ISO 3611-1 country code of address
    /// </summary>
    public string? CountryCode { get; set; }
    
    /// <summary>
    /// One of verified, corrected, or failed
    /// </summary>
    public string? AddressStatus { get; set; }
    
    /// <summary>
    /// First name or full name of contact
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Last name of contact
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Email address
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Company name of contact
    /// </summary>
    public string? CompanyName { get; set; }
    
    /// <summary>
    /// Job title of contact
    /// </summary>
    public string? JobTitle { get; set; }
    
    /// <summary>
    /// See Metadata
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }
    
    /// <summary>
    /// Skip address correction and verification
    /// </summary>
    public bool SkipVerification { get; set; }
    
    /// <summary>
    /// Override the given address verification status
    /// </summary>
    public bool ForceVerifiedStatus { get; set; }
    
    /// <summary>
    /// Hide contact details apart from final print
    /// </summary>
    public bool Secret { get; set; }
}
