using Shane32.PostGrid.Contacts;

namespace Shane32.PostGrid.Helpers;

// In the future, other API endpoints can be added here as properties
// For example:
// public Letters Letters => _letters ??= new Letters(_connection);
// public Postcards Postcards => _postcards ??= new Postcards(_connection);


/// <summary>
/// Provides methods for working with contacts in the PostGrid API.
/// </summary>
public class PostGridContacts
{
    private readonly IPostGridConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="Contacts"/> class.
    /// </summary>
    /// <param name="connection">The connection to use for API requests.</param>
    public PostGridContacts(IPostGridConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Creates a new contact in PostGrid.
    /// </summary>
    /// <param name="request">The contact creation request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the API.</returns>
    public async Task<ContactResponse> CreateAsync(CreateRequest request, CancellationToken cancellationToken = default)
    {
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Gets a contact from PostGrid by ID.
    /// </summary>
    /// <param name="id">The ID of the contact to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the API.</returns>
    public async Task<ContactResponse> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var request = new GetRequest { Id = id };
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Deletes a contact from PostGrid by ID.
    /// </summary>
    /// <param name="id">The ID of the contact to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the API.</returns>
    public async Task<ContactResponse> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var request = new DeleteRequest { Id = id };
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Lists contacts from PostGrid with pagination and search options.
    /// </summary>
    /// <param name="skip">The number of items to skip for pagination.</param>
    /// <param name="limit">The maximum number of items to return.</param>
    /// <param name="search">The search term to filter results.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list response from the API.</returns>
    public async Task<ListResponse<ContactResponse>> ListAsync(int? skip = null, int? limit = null, string? search = null, CancellationToken cancellationToken = default)
    {
        var request = new ListRequest {
            Skip = skip,
            Limit = limit,
            Search = search
        };

        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Lists contacts from PostGrid with the specified request parameters.
    /// </summary>
    /// <param name="request">The list request parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list response from the API.</returns>
    public async Task<ListResponse<ContactResponse>> ListAsync(ListRequest request, CancellationToken cancellationToken = default)
    {
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Lists all contacts from PostGrid by making multiple API calls as needed.
    /// </summary>
    /// <param name="pageLimit">The maximum number of items to return per page (default: 100).</param>
    /// <param name="search">The search term to filter results.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable containing all contacts from the API.</returns>
    public async IAsyncEnumerable<ContactResponse> ListAllAsync(int pageLimit = 100, string? search = null, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        int skip = 0;
        bool hasMore = true;

        while (hasMore) {
            var response = await ListAsync(skip, pageLimit, search, cancellationToken);

            if (response.Data == null || response.Data.Length == 0)
                break;

            foreach (var contact in response.Data) {
                yield return contact;
            }

            skip += response.Data.Length;
            hasMore = skip < response.TotalCount;
        }
    }
}
