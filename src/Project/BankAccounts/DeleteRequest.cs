namespace Shane32.PostGrid.BankAccounts;

/// <summary>
/// Represents a request to delete a bank account from PostGrid.
/// </summary>
public class DeleteRequest
{
    /// <summary>
    /// The unique ID of the bank account to delete.
    /// </summary>
    public required string Id { get; set; }
}