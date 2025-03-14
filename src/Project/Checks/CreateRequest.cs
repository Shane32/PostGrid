namespace Shane32.PostGrid.Checks;

/// <summary>
/// Represents a request to create a check in PostGrid.
/// </summary>
public class CreateRequest
{
    /// <summary>
    /// The id or contact object of the receiver. You can either pass a contact object or a contact's id.
    /// </summary>
    public required string To { get; set; }
    
    /// <summary>
    /// The id or contact object of the sender. You can either pass a contact object or a contact's id.
    /// </summary>
    public required string From { get; set; }
    
    /// <summary>
    /// The id for which bank account will be used for the check.
    /// </summary>
    public required string BankAccount { get; set; }
    
    /// <summary>
    /// The amount for which the check is issued for. Must be specified in cents.
    /// </summary>
    public required int Amount { get; set; }
    
    /// <summary>
    /// The check number.
    /// </summary>
    public required int Number { get; set; }
    
    /// <summary>
    /// An optional parameter that adds a blank page insert to the check in order to redirect its destination.
    /// </summary>
    public string? RedirectTo { get; set; }
    
    /// <summary>
    /// A memo to be included on the check.
    /// </summary>
    public string? Memo { get; set; }
    
    /// <summary>
    /// An HTML message to print on the same page as the check.
    /// </summary>
    public string? Message { get; set; }
    
    /// <summary>
    /// A publicly accessible image URL to be printed on the check. We recommend this image is at least 280px x 280px to be 300 DPI.
    /// </summary>
    public string? Logo { get; set; }
    
    /// <summary>
    /// Indicates extra services for the letter. See certified and registered mail.
    /// </summary>
    public string? ExtraService { get; set; }
    
    /// <summary>
    /// A 3-letter code representing the currency of amount.
    /// </summary>
    public string? CurrencyCode { get; set; }
    
    /// <summary>
    /// See express shipping.
    /// </summary>
    public bool? Express { get; set; }
    
    /// <summary>
    /// The desired date for the letter to be sent out.
    /// </summary>
    public DateTimeOffset? SendDate { get; set; }
    
    /// <summary>
    /// Defaults to first_class. See mailing class.
    /// </summary>
    public string? MailingClass { get; set; }
    
    /// <summary>
    /// A description for the check.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// See merge variables.
    /// </summary>
    public Dictionary<string, string>? MergeVariables { get; set; }
    
    /// <summary>
    /// See metadata.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }
    
    /// <summary>
    /// Default size will be chosen based on the destination country, if not provided. Indicates the check size for the check being created. See check size.
    /// </summary>
    public string? Size { get; set; }
    
    /// <summary>
    /// The HTML content for the attached letter. If provided, this will be sent using form-url-encoding.
    /// This field is mutually exclusive with LetterPDF.
    /// </summary>
    public string? LetterHTML { get; set; }
    
    /// <summary>
    /// The PDF content for the attached letter. If provided, this will be sent using multipart/form-data.
    /// This field is mutually exclusive with LetterHTML.
    /// </summary>
    public byte[]? LetterPDF { get; set; }
    
    /// <summary>
    /// The MIME type of the letter PDF (e.g., "application/pdf").
    /// Defaults to "application/pdf" if not specified.
    /// </summary>
    public string? LetterPDFContentType { get; set; }
}