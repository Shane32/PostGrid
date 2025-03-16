using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;

namespace Tests;

/// <summary>
/// Base class for PostGrid API tests that provides common setup functionality.
/// </summary>
public class PostGridTestBase : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

    /// <summary>
    /// Gets the service provider.
    /// </summary>
    protected ServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the PostGrid client.
    /// </summary>
    protected PostGrid PostGrid { get; }

    /// <summary>
    /// Gets or sets the function that creates HTTP responses based on requests.
    /// Each test can assign a different lambda to this property.
    /// </summary>
    protected Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> CreateResponse { get; set; } =
        (_, _) => throw new InvalidOperationException("CreateResponse has not been implemented for this test.");

    protected PostGridTestBase()
    {
        // Create the mock HTTP message handler
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        // Set up the mock HTTP message handler with the response factory
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns((HttpRequestMessage request, CancellationToken cancellationToken) =>
                CreateResponse(request, cancellationToken));

        // Set up DI
        var services = new ServiceCollection();

        // Configure with test API key
        services.AddPostGrid(options => {
            options.ApiKey = "test_api_key_123";
            options.BaseUrl = "https://api.postgrid.com/print-mail/v1";
        });

        // Replace the HttpClient with our mocked one
        services.AddHttpClient<IPostGridConnection, PostGridConnection>()
            .ConfigurePrimaryHttpMessageHandler(() => _mockHttpMessageHandler.Object);

        // Build the service provider
        ServiceProvider = services.BuildServiceProvider();

        // Get the PostGrid client from DI
        PostGrid = ServiceProvider.GetRequiredService<PostGrid>();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}
