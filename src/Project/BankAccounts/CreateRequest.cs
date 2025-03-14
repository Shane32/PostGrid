namespace Shane32.PostGrid.BankAccounts;

/// <summary>
/// Represents a request to create a bank account in PostGrid.
/// </summary>
public class CreateRequest
{
    /// <summary>
    /// The name of the bank.
    /// </summary>
    public required string BankName { get; set; }
    
    /// <summary>
    /// The account number, only used when creating the account.
    /// </summary>
    public required string AccountNumber { get; set; }
    
    /// <summary>
    /// The bank account's routing number.
    /// </summary>
    public required string RoutingNumber { get; set; }
    
    /// <summary>
    /// The corresponding country code for the bank. Must be either CA or US.
    /// </summary>
    public required string BankCountryCode { get; set; }
    
    /// <summary>
    /// An image containing a signature.
    /// This field is required if SignatureText is not provided.
    /// </summary>
    public byte[]? SignatureImage { get; set; }
    
    /// <summary>
    /// The MIME type of the signature image (e.g., "image/png", "image/jpeg").
    /// Defaults to "image/png" if not specified.
    /// </summary>
    public string? SignatureImageContentType { get; set; }
    
    /// <summary>
    /// A string which will be used to create a signature for documents.
    /// This field is required if SignatureImage is not provided.
    /// </summary>
    public string? SignatureText { get; set; }
    
    /// <summary>
    /// First line of the bank's address.
    /// </summary>
    public string? BankPrimaryLine { get; set; }
    
    /// <summary>
    /// Second line of the bank's address.
    /// </summary>
    public string? BankSecondaryLine { get; set; }
    
    /// <summary>
    /// A description for the bank account.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Metadata for the bank account.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }
}