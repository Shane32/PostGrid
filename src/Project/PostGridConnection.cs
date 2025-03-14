using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Options;

namespace Shane32.PostGrid;

/// <summary>
/// Implementation of <see cref="IPostGridConnection"/> for connecting to the PostGrid API.
/// </summary>
public partial class PostGridConnection : IPostGridConnection
{
    private readonly HttpClient _httpClient;
    private readonly PostGridOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostGridConnection"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client to use for API requests.</param>
    /// <param name="options">The options for configuring the PostGrid API client.</param>
    public PostGridConnection(HttpClient httpClient, IOptions<PostGridOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    /// <inheritdoc />
    public virtual ValueTask<string> GetApiKey(CancellationToken cancellationToken = default)
    {
        var apiKey = _options.ApiKey;
        if (apiKey == null || string.IsNullOrWhiteSpace(apiKey)) {
            throw new InvalidOperationException("The API key is not configured.");
        }
        return new ValueTask<string>(apiKey);
    }

    /// <summary>
    /// Sends an HTTP request to the PostGrid API and processes the response.
    /// The API key is added to the request headers.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response content to.</typeparam>
    /// <param name="requestFactory">A function that creates the HTTP request message to send.</param>
    /// <param name="deserializeFunc">A function to deserialize the response content stream to type T.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The deserialized response object.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    protected virtual async Task<T> SendRequestAsync<T>(Func<HttpRequestMessage> requestFactory, Func<Stream, CancellationToken, ValueTask<T>> deserializeFunc, CancellationToken cancellationToken = default)
    {
        int retryCount = 0;
        HttpResponseMessage? response = null;

        // Get the API key
        var apiKey = await GetApiKey(cancellationToken);

        while (true) {
            // Create a new request for each attempt
            using var request = requestFactory();

            // Add the API key header to the request
            request.Headers.Add("x-api-key", apiKey);

            // Send the request
            response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            // If we got a 429 status code (rate limit) and retries are enabled
            if ((int)response.StatusCode == 429 &&
                _options.MaxRetryAttempts > 0 &&
                retryCount < _options.MaxRetryAttempts) {
                // Increment retry count
                retryCount++;

                // Determine if we should retry and how long to wait
                TimeSpan? retryDelay;

                if (_options.ShouldRetryAsync != null) {
                    // Use the custom retry strategy
                    retryDelay = await _options.ShouldRetryAsync(retryCount);
                } else {
                    // Use default exponential backoff strategy
                    retryDelay = TimeSpan.FromMilliseconds(_options.DefaultRetryDelayMs * Math.Pow(2, retryCount - 1));
                }

                // If retryDelay is null, stop retrying
                if (retryDelay == null) {
                    break;
                }

                // If retryDelay is positive, wait before retrying
                if (retryDelay.Value > TimeSpan.Zero) {
                    await Task.Delay(retryDelay.Value, cancellationToken);
                }

                // Dispose the current response before retrying
                response.Dispose();

                // Continue to the next iteration (retry)
                continue;
            }

            // Break the loop if we're not retrying
            break;
        }

        // Check for success status code
        if (!response.IsSuccessStatusCode) {
            // Check if the content type is JSON
            var isJson = response.Content.Headers.ContentType?.MediaType?.Equals("application/json", StringComparison.OrdinalIgnoreCase) ?? false;

            if (isJson) {
                try {
                    // Try to deserialize the error response
                    var errorStream = await response.Content.ReadAsStreamAsync();
                    var errorResponse = await JsonSerializer.DeserializeAsync(errorStream, PostGridJsonSerializerContext.Default.ErrorResponse, cancellationToken);

                    if (errorResponse != null && errorResponse.Type != null) {
                        throw new PostGridException(
                            errorResponse.Type,
                            errorResponse.Message ?? "Unknown error",
                            response.StatusCode,
                            null);
                    }
                } catch (JsonException) {
                    // If deserialization fails, we'll fall back to the default HttpRequestException
                }
            }

            // If we couldn't deserialize the error response, throw a standard HttpRequestException
            throw new HttpRequestException(
                $"The request failed with status code {response.StatusCode} and was unable to be parsed.");
        }

        // Get the response content as a stream
        var contentStream = await response.Content.ReadAsStreamAsync();

        // Deserialize the response using the provided function
        return await deserializeFunc(contentStream, cancellationToken);
    }

    /// <summary>
    /// Sends an HTTP request to the PostGrid API and processes the response using JSON source generation.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response content to.</typeparam>
    /// <param name="requestFactory">A function that creates the HTTP request message to send.</param>
    /// <param name="jsonTypeInfo">The JSON type information for deserialization.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The deserialized response object.</returns>
    /// <exception cref="PostGridException">Thrown when the request fails and the error response can be deserialized.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails and the error response cannot be deserialized.</exception>
    protected virtual Task<T> SendRequestAsync<T>(Func<HttpRequestMessage> requestFactory, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        return SendRequestAsync(
            requestFactory,
            async (stream, token) => await JsonSerializer.DeserializeAsync(stream, jsonTypeInfo, token)
                ?? throw new JsonException("The response returned a null object."),
            cancellationToken);
    }
}
