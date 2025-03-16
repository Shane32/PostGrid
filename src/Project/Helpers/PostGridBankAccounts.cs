using Shane32.PostGrid.BankAccounts;

namespace Shane32.PostGrid.Helpers;

/// <summary>
/// Provides methods for working with bank accounts in the PostGrid API.
/// </summary>
public class PostGridBankAccounts
{
    private readonly IPostGridConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostGridBankAccounts"/> class.
    /// </summary>
    /// <param name="connection">The connection to use for API requests.</param>
    public PostGridBankAccounts(IPostGridConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Creates a new bank account in PostGrid.
    /// </summary>
    /// <param name="request">The bank account creation request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the API.</returns>
    public async Task<BankAccountResponse> CreateAsync(CreateRequest request, CancellationToken cancellationToken = default)
    {
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Gets a bank account from PostGrid by ID.
    /// </summary>
    /// <param name="id">The ID of the bank account to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the API.</returns>
    public async Task<BankAccountResponse> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var request = new GetRequest { Id = id };
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Deletes a bank account from PostGrid by ID.
    /// </summary>
    /// <param name="id">The ID of the bank account to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var request = new DeleteRequest { Id = id };
        await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Lists bank accounts from PostGrid with pagination and search options.
    /// </summary>
    /// <param name="skip">The number of items to skip for pagination.</param>
    /// <param name="limit">The maximum number of items to return.</param>
    /// <param name="search">The search term to filter results.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list response from the API.</returns>
    public async Task<ListResponse<BankAccountResponse>> ListAsync(int? skip = null, int? limit = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var request = new ListRequest {
            Skip = skip,
            Limit = limit,
            Search = search
        };

        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Lists bank accounts from PostGrid with the specified request parameters.
    /// </summary>
    /// <param name="request">The list request parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list response from the API.</returns>
    public async Task<ListResponse<BankAccountResponse>> ListAsync(ListRequest request, CancellationToken cancellationToken = default)
    {
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Lists all bank accounts from PostGrid by making multiple API calls as needed.
    /// </summary>
    /// <param name="pageLimit">The maximum number of items to return per page (default: 100).</param>
    /// <param name="search">The search term to filter results.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable containing all bank accounts from the API.</returns>
    public async IAsyncEnumerable<BankAccountResponse> ListAllAsync(int pageLimit = 100, string? search = null, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        int skip = 0;
        bool hasMore = true;

        while (hasMore) {
            var response = await ListAsync(skip, pageLimit, search, cancellationToken);

            if (response.Data == null || response.Data.Length == 0)
                break;

            foreach (var bankAccount in response.Data) {
                yield return bankAccount;
            }

            skip += response.Data.Length;
            hasMore = skip < response.TotalCount;
        }
    }
}
