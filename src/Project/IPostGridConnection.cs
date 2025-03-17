namespace Shane32.PostGrid;

/// <summary>
/// Represents a connection to the PostGrid API.
/// </summary>
public interface IPostGridConnection
{
    /// <summary>
    /// Gets the API key for authenticating with the PostGrid API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the API key.</returns>
    public ValueTask<string> GetApiKey(CancellationToken cancellationToken = default);

    #region Contacts

    /// <summary>
    /// Executes a create contact request against the PostGrid API.
    /// </summary>
    /// <param name="request">The contact creation request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the contact response from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<Contacts.ContactResponse> ExecuteAsync(Contacts.CreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a get contact request against the PostGrid API.
    /// </summary>
    /// <param name="request">The contact get request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the contact response from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<Contacts.ContactResponse> ExecuteAsync(Contacts.GetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a delete contact request against the PostGrid API.
    /// </summary>
    /// <param name="request">The contact delete request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task ExecuteAsync(Contacts.DeleteRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a list contacts request against the PostGrid API.
    /// </summary>
    /// <param name="request">The contacts list request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list of contacts from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<ListResponse<Contacts.ContactResponse>> ExecuteAsync(Contacts.ListRequest request, CancellationToken cancellationToken = default);

    #endregion

    #region Bank Accounts

    /// <summary>
    /// Executes a create bank account request against the PostGrid API.
    /// </summary>
    /// <param name="request">The bank account creation request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the bank account response from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<BankAccounts.BankAccountResponse> ExecuteAsync(BankAccounts.CreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a get bank account request against the PostGrid API.
    /// </summary>
    /// <param name="request">The bank account get request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the bank account response from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<BankAccounts.BankAccountResponse> ExecuteAsync(BankAccounts.GetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a delete bank account request against the PostGrid API.
    /// </summary>
    /// <param name="request">The bank account delete request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task ExecuteAsync(BankAccounts.DeleteRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a list bank accounts request against the PostGrid API.
    /// </summary>
    /// <param name="request">The bank accounts list request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list of bank accounts from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<ListResponse<BankAccounts.BankAccountResponse>> ExecuteAsync(BankAccounts.ListRequest request, CancellationToken cancellationToken = default);

    #endregion

    #region Checks

    /// <summary>
    /// Executes a create check request against the PostGrid API.
    /// </summary>
    /// <param name="request">The check creation request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the check response from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<Checks.CheckResponse> ExecuteAsync(Checks.CreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a get check request against the PostGrid API.
    /// </summary>
    /// <param name="request">The check get request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the check response from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<Checks.CheckResponse> ExecuteAsync(Checks.GetRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a cancel check request against the PostGrid API.
    /// </summary>
    /// <param name="request">The check cancellation request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the check response from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<Checks.CheckResponse> ExecuteAsync(Checks.CancelRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a list checks request against the PostGrid API.
    /// </summary>
    /// <param name="request">The checks list request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list of checks from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<ListResponse<Checks.CheckResponse>> ExecuteAsync(Checks.ListRequest request, CancellationToken cancellationToken = default);

    #endregion
}
