namespace Shane32.PostGrid.Contacts;

/// <summary>
/// Represents a request to create a contact in PostGrid.
/// </summary>
public class CreateRequest
{
    /// <summary>
    /// The first name of the contact.
    /// This field is optional if a companyName is being supplied. If no companyName is supplied then this field is required.
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// The last name of the contact.
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// The contact's company name.
    /// This field is optional if a firstName is being supplied. If no firstName is supplied then this field is required.
    /// </summary>
    public string? CompanyName { get; set; }
    
    /// <summary>
    /// The contact's first address line.
    /// The full address can be provided here, which will be automatically parsed to the other address fields.
    /// </summary>
    public required string AddressLine1 { get; set; }
    
    /// <summary>
    /// The contact's second address line.
    /// </summary>
    public string? AddressLine2 { get; set; }
    
    /// <summary>
    /// The contact's city.
    /// </summary>
    public string? City { get; set; }
    
    /// <summary>
    /// The province or state of the contact.
    /// </summary>
    public string? ProvinceOrState { get; set; }
    
    /// <summary>
    /// The contact's email.
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// The contact's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// The contact's job title.
    /// </summary>
    public string? JobTitle { get; set; }
    
    /// <summary>
    /// The postal code or ZIP code of the contact.
    /// </summary>
    public string? PostalOrZip { get; set; }
    
    /// <summary>
    /// The ISO 3611-1 country code of the contact's address. Defaults to CA.
    /// </summary>
    public required string CountryCode { get; set; } = "CA";
    
    /// <summary>
    /// A description for the contact.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Metadata for the contact.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }
    
    /// <summary>
    /// If true, skip address verification and mark the address as failed.
    /// </summary>
    public bool SkipVerification { get; set; } = false;
    
    /// <summary>
    /// If true, force the status of the address to be verified, effectively whitelisting it even if our address verification system would otherwise fail it.
    /// </summary>
    public bool ForceVerifiedStatus { get; set; } = false;
    
    /// <summary>
    /// If true, allows hiding information from the dashboard and API.
    /// </summary>
    public bool Secret { get; set; } = false;
}
