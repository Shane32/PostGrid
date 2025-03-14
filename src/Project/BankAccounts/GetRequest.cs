namespace Shane32.PostGrid.BankAccounts;

/// <summary>
/// Represents a request to get a bank account from PostGrid.
/// </summary>
public class GetRequest
{
    /// <summary>
    /// The unique ID of the bank account to retrieve.
    /// </summary>
    public required string Id { get; set; }
}
