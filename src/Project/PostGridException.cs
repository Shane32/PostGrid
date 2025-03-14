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

#if NET6_0_OR_GREATER
    /// <summary>
    /// Gets the HTTP status code of the response.
    /// </summary>
    public new HttpStatusCode? StatusCode => base.StatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostGridException"/> class.
    /// </summary>
    /// <param name="type">The type of the error as reported by the PostGrid API.</param>
    /// <param name="message">The error message as reported by the PostGrid API.</param>
    /// <param name="statusCode">The HTTP status code of the response.</param>
    /// <param name="inner">The exception that caused this exception.</param>
    public PostGridException(string type, string message, HttpStatusCode? statusCode, Exception? inner)
        : base(message, inner, statusCode)
    {
        Type = type;
    }
#else
    /// <summary>
    /// Gets the HTTP status code of the response.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostGridException"/> class.
    /// </summary>
    /// <param name="type">The type of the error as reported by the PostGrid API.</param>
    /// <param name="message">The error message as reported by the PostGrid API.</param>
    /// <param name="statusCode">The HTTP status code of the response.</param>
    /// <param name="inner">The exception that caused this exception.</param>
    public PostGridException(string type, string message, HttpStatusCode? statusCode, Exception? inner)
        : base(message, inner)
    {
        Type = type;
        StatusCode = statusCode;
    }
#endif
}
