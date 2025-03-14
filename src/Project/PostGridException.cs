using System.Net;

namespace Shane32.PostGrid;

/// <summary>
/// Represents an exception that occurs when a request to the PostGrid API fails.
/// </summary>
public class PostGridException : HttpRequestException
{
    /// <summary>
    /// Gets the type of the error as reported by the PostGrid API.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the error message as reported by the PostGrid API.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Gets the HTTP status code of the response.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostGridException"/> class.
    /// </summary>
    /// <param name="type">The type of the error as reported by the PostGrid API.</param>
    /// <param name="message">The error message as reported by the PostGrid API.</param>
    /// <param name="statusCode">The HTTP status code of the response.</param>
    public PostGridException(string type, string message, HttpStatusCode statusCode)
        : base(message)
    {
        Type = type;
        ErrorMessage = message;
        StatusCode = statusCode;
    }
}
