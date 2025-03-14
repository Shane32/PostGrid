using System.Text.Json.Serialization;

namespace Shane32.PostGrid.Checks;

/// <summary>
/// Represents the response from the PostGrid API when working with a check.
/// </summary>
public class CheckResponse
{
    /// <summary>
    /// A unique ID prefixed with check_
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// See Tracking
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// The recipient
    /// </summary>
    public required Contacts.ContactBaseResponse To { get; set; }

    /// <summary>
    /// The sender
    /// </summary>
    public required Contacts.ContactBaseResponse From { get; set; }

    /// <summary>
    /// See Intelligent-Mail Tracking
    /// </summary>
    public string? ImbStatus { get; set; }

    /// <summary>
    /// true if this is a live mode check else false
    /// </summary>
    public required bool Live { get; set; }

    /// <summary>
    /// Signed link to a preview of this letter order
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Optional line describing this check
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date when the check will be sent
    /// </summary>
    public required DateTimeOffset SendDate { get; set; }

    /// <summary>
    /// A Bank Account ID, the drawee
    /// </summary>
    public required string BankAccount { get; set; }

    /// <summary>
    /// 3-letter code representing currency of amount
    /// </summary>
    public required string CurrencyCode { get; set; }

    /// <summary>
    /// The check amount as an integer representing cents
    /// </summary>
    public required int Amount { get; set; }

    /// <summary>
    /// An optional memo to be printed on the check (Max 40 characters long)
    /// </summary>
    public string? Memo { get; set; }

    /// <summary>
    /// An optional logo image URL which will be printed on the check
    /// </summary>
    public string? Logo { get; set; }

    /// <summary>
    /// The check number
    /// </summary>
    public required int Number { get; set; }

    /// <summary>
    /// An optional (HTML) message to print on the same page as the check
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// The raw HTML for the attached letter, if any
    /// </summary>
    [JsonPropertyName("letterHTML")]
    public string? LetterHTML { get; set; }

    /// <summary>
    /// A Template ID for the attached letter, if any
    /// </summary>
    public string? LetterTemplate { get; set; }

    /// <summary>
    /// A URL pointing to the original PDF of the attached letter
    /// </summary>
    [JsonPropertyName("letterUploadedPDF")]
    public string? LetterUploadedPDF { get; set; }

    /// <summary>
    /// Defaults to first_class. See mailing class.
    /// </summary>
    public string? MailingClass { get; set; }

    /// <summary>
    /// Merge Variables for the message/attached letter
    /// </summary>
    public Dictionary<string, string>? MergeVariables { get; set; }

    /// <summary>
    /// See Metadata
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }
}
