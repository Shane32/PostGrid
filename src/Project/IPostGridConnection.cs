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
    /// <returns>A task representing the asynchronous operation, containing the contact response from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<Contacts.ContactResponse> ExecuteAsync(Contacts.DeleteRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Executes a list contacts request against the PostGrid API.
    /// </summary>
    /// <param name="request">The contacts list request to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the paginated list of contacts from the API.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    public Task<ListResponse<Contacts.ContactResponse>> ExecuteAsync(Contacts.ListRequest request, CancellationToken cancellationToken = default);
}
