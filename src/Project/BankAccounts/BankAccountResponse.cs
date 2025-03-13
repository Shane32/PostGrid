namespace Shane32.PostGrid.BankAccounts;

/// <summary>
/// Represents the response from the PostGrid API when working with a bank account.
/// </summary>
public class BankAccountResponse
{
    /// <summary>
    /// A unique ID prefixed with bank_
    /// </summary>
    public string? Id { get; set; }
    
    /// <summary>
    /// true if this is a live mode bank account else false
    /// </summary>
    public bool Live { get; set; }
    
    /// <summary>
    /// Optional line describing this bank account
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// The name of the bank which provides this account
    /// </summary>
    public string? BankName { get; set; }
    
    /// <summary>
    /// First line of the bank's address, e.g. 100 Garden Street
    /// </summary>
    public string? BankPrimaryLine { get; set; }
    
    /// <summary>
    /// Second line of the bank's address, e.g. Toronto, ON M5V 4G9
    /// </summary>
    public string? BankSecondaryLine { get; set; }
    
    /// <summary>
    /// Either CA or US
    /// </summary>
    public string? BankCountryCode { get; set; }
    
    /// <summary>
    /// Transit number for cheques associated with this account (CA only)
    /// </summary>
    public string? TransitNumber { get; set; }
    
    /// <summary>
    /// Also referred to as the Financial Institution Number (CA only)
    /// </summary>
    public string? RouteNumber { get; set; }
    
    /// <summary>
    /// The bank account routing number (US only)
    /// </summary>
    public string? RoutingNumber { get; set; }
    
    /// <summary>
    /// The account number, only used when creating the account
    /// </summary>
    public string? AccountNumber { get; set; }
    
    /// <summary>
    /// Returned in GET requests in place of accountNumber
    /// </summary>
    public string? AccountNumberLast4 { get; set; }
    
    /// <summary>
    /// A base64-encoded SHA256 hash of accountNumber + bank account ID
    /// </summary>
    public string? AccountNumberAndIDSHA256 { get; set; }
    
    /// <summary>
    /// To be supplied when account is created (instead of signatureText)
    /// </summary>
    public object? SignatureImage { get; set; }
    
    /// <summary>
    /// To be supplied when account is created (instead of signatureImage)
    /// </summary>
    public string? SignatureText { get; set; }
    
    /// <summary>
    /// See Metadata
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }
}
