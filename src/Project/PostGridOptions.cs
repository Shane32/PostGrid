namespace Shane32.PostGrid;

/// <summary>
/// Options for configuring the PostGrid API client.
/// </summary>
public class PostGridOptions
{
    /// <summary>
    /// The API key for authenticating with the PostGrid API.
    /// </summary>
    public string ApiKey { get; set; } = null!;
    
    /// <summary>
    /// The base URL for the PostGrid API. Defaults to "https://api.postgrid.com/print-mail/v1".
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.postgrid.com/print-mail/v1";
    
    /// <summary>
    /// The maximum number of retry attempts for rate-limited requests (429 status code).
    /// Default is disabled.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 0;
    
    /// <summary>
    /// The default delay between retry attempts in milliseconds.
    /// Default is 1000ms (1 second).
    /// </summary>
    public int DefaultRetryDelayMs { get; set; } = 1000;
    
    /// <summary>
    /// A function that determines whether to retry a rate-limited request and how long to wait.
    /// The function receives the current retry count (starting from 1) and returns a ValueTask with:
    /// <list type="bullet">
    /// <item>A positive TimeSpan value to indicate the wait time before retrying</item>
    /// <item>TimeSpan.Zero to retry immediately</item>
    /// <item>A null value to stop retrying</item>
    /// </list>
    /// If null, a default exponential backoff strategy will be used.
    /// </summary>
    public Func<int, ValueTask<TimeSpan?>>? ShouldRetryAsync { get; set; }
}