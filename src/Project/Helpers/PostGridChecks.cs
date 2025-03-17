using Shane32.PostGrid.Checks;

namespace Shane32.PostGrid.Helpers;

/// <summary>
/// Provides methods for working with checks in the PostGrid API.
/// </summary>
public class PostGridChecks
{
    private readonly IPostGridConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostGridChecks"/> class.
    /// </summary>
    /// <param name="connection">The connection to use for API requests.</param>
    public PostGridChecks(IPostGridConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Creates a new check in PostGrid.
    /// </summary>
    /// <param name="request">The check creation request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the API.</returns>
    public async Task<CheckResponse> CreateAsync(CreateRequest request, CancellationToken cancellationToken = default)
    {
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Gets a check from PostGrid by ID.
    /// </summary>
    /// <param name="id">The ID of the check to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the API.</returns>
    public async Task<CheckResponse> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var request = new GetRequest { Id = id };
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Cancels a check from PostGrid by ID.
    /// </summary>
    /// <param name="id">The ID of the check to cancel.</param>
    /// <param name="note">An optional note explaining the reason for cancellation.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the API.</returns>
    public async Task<CheckResponse> CancelAsync(string id, string? note, CancellationToken cancellationToken = default)
    {
        var request = new CancelRequest { Id = id, Note = note };
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Cancels a check from PostGrid by ID.
    /// </summary>
    /// <param name="id">The ID of the check to cancel.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response from the API.</returns>
    public Task<CheckResponse> CancelAsync(string id, CancellationToken cancellationToken = default)
        => CancelAsync(id, null, cancellationToken);

    /// <summary>
    /// Lists checks from PostGrid with pagination and search options.
    /// </summary>
    /// <param name="skip">The number of items to skip for pagination.</param>
    /// <param name="limit">The maximum number of items to return.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list response from the API.</returns>
    public async Task<ListResponse<CheckResponse>> ListAsync(int? skip = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var request = new ListRequest {
            Skip = skip,
            Limit = limit,
        };

        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Lists checks from PostGrid with the specified request parameters.
    /// </summary>
    /// <param name="request">The list request parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list response from the API.</returns>
    public async Task<ListResponse<CheckResponse>> ListAsync(ListRequest request, CancellationToken cancellationToken = default)
    {
        return await _connection.ExecuteAsync(request, cancellationToken);
    }

    /// <summary>
    /// Lists all checks from PostGrid by making multiple API calls as needed.
    /// </summary>
    /// <param name="pageLimit">The maximum number of items to return per page (default: 100).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable containing all checks from the API.</returns>
    public async IAsyncEnumerable<CheckResponse> ListAllAsync(int pageLimit = 100, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        int skip = 0;
        bool hasMore = true;

        while (hasMore) {
            var response = await ListAsync(skip, pageLimit, cancellationToken);

            if (response.Data == null || response.Data.Length == 0)
                break;

            foreach (var check in response.Data) {
                yield return check;
            }

            skip += response.Data.Length;
            hasMore = skip < response.TotalCount;
        }
    }
}
