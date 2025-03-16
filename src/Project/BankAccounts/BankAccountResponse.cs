using System.Text.Json.Serialization;

namespace Shane32.PostGrid.BankAccounts;

/// <summary>
/// Represents the response from the PostGrid API when working with a bank account.
/// </summary>
public class BankAccountResponse
{
    /// <summary>
    /// A unique ID prefixed with bank_
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// true if this is a live mode bank account else false
    /// </summary>
    public required bool Live { get; set; }

    /// <summary>
    /// Optional line describing this bank account
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The name of the bank which provides this account
    /// </summary>
    public required string BankName { get; set; }

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
    public required string BankCountryCode { get; set; }

    /*
    /// <summary>
    /// Transit number for checks associated with this account (CA only)
    /// </summary>
    public required string TransitNumber { get; set; }

    /// <summary>
    /// Also referred to as the Financial Institution Number (CA only)
    /// </summary>
    public required string RouteNumber { get; set; }
    */

    /// <summary>
    /// The bank account routing number (US only)
    /// </summary>
    public required string RoutingNumber { get; set; }

    /// <summary>
    /// Returned in GET requests in place of accountNumber
    /// </summary>
    public required string AccountNumberLast4 { get; set; }

    /// <summary>
    /// A base64-encoded SHA256 hash of accountNumber + bank account ID
    /// </summary>
    [JsonPropertyName("accountNumberAndIDSHA256")]
    public required string AccountNumberAndIDSHA256 { get; set; }

    /// <summary>
    /// See Metadata
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    /// The date and time when the bank account was created
    /// </summary>
    public required DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The date and time when the bank account was last updated
    /// </summary>
    public required DateTimeOffset UpdatedAt { get; set; }
}
