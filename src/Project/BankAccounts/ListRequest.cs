namespace Shane32.PostGrid.BankAccounts;

/// <summary>
/// Represents a request to list bank accounts from the PostGrid API.
/// </summary>
public class ListRequest
{
    /// <summary>
    /// Gets or sets the number of items to skip for pagination.
    /// </summary>
    /// <remarks>
    /// Used for traversing pages. See lists and pagination.
    /// </remarks>
    public int? Skip { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of resources to return.
    /// </summary>
    /// <remarks>
    /// Limits the number of resources returned. See lists and pagination.
    /// </remarks>
    public int? Limit { get; set; }

    /// <summary>
    /// Gets or sets the search term to filter results.
    /// </summary>
    /// <remarks>
    /// Searches the data for the given input. See search.
    /// </remarks>
    public string? Search { get; set; }
}
