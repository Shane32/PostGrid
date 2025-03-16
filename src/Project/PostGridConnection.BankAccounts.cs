using System.Net.Http.Headers;
using System.Web;
using Shane32.PostGrid.BankAccounts;

namespace Shane32.PostGrid;

public partial class PostGridConnection
{
    /// <inheritdoc />
    public async Task<BankAccountResponse> ExecuteAsync(CreateRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.SignatureImage == null && request.SignatureText == null)
            throw new ArgumentException("Either SignatureImage or SignatureText must be provided", nameof(request));

        // Handle the request based on whether we're using SignatureImage or SignatureText
        if (request.SignatureImage != null) {
            // Use multipart/form-data for SignatureImage
            var content = new MultipartFormDataContent {
                // Add all the form fields
                { new StringContent(request.BankName), "bankName" },
                { new StringContent(request.AccountNumber), "accountNumber" },
                { new StringContent(request.RoutingNumber), "routingNumber" },
                { new StringContent(request.BankCountryCode), "bankCountryCode" }
            };

            // Add the signature image
            var imageContent = new ByteArrayContent(request.SignatureImage);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(request.SignatureImageContentType ?? "image/png");
            content.Add(imageContent, "signatureImage");

            // Add optional fields
            if (request.BankPrimaryLine != null)
                content.Add(new StringContent(request.BankPrimaryLine), "bankPrimaryLine");

            if (request.BankSecondaryLine != null)
                content.Add(new StringContent(request.BankSecondaryLine), "bankSecondaryLine");

            if (request.Description != null)
                content.Add(new StringContent(request.Description), "description");

            // Handle metadata
            if (request.Metadata != null) {
                foreach (var item in request.Metadata) {
                    content.Add(new StringContent(item.Value), $"metadata[{item.Key}]");
                }
            }

            // Create a request factory function
            Func<HttpRequestMessage> requestFactory = () => {
                var newRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/bank_accounts");
                newRequest.Content = content;
                return newRequest;
            };

            // Use the generic SendRequestAsync method with JsonTypeInfo
            return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.BankAccountResponse, cancellationToken);
        } else {
            // Use form-urlencoded for SignatureText
            var formData = new List<KeyValuePair<string, string>> {
                // Add all the form fields
                new KeyValuePair<string, string>("bankName", request.BankName),
                new KeyValuePair<string, string>("accountNumber", request.AccountNumber),
                new KeyValuePair<string, string>("routingNumber", request.RoutingNumber),
                new KeyValuePair<string, string>("bankCountryCode", request.BankCountryCode),
                new KeyValuePair<string, string>("signatureText", request.SignatureText!)
            };

            // Add optional fields
            if (request.BankPrimaryLine != null)
                formData.Add(new KeyValuePair<string, string>("bankPrimaryLine", request.BankPrimaryLine));

            if (request.BankSecondaryLine != null)
                formData.Add(new KeyValuePair<string, string>("bankSecondaryLine", request.BankSecondaryLine));

            if (request.Description != null)
                formData.Add(new KeyValuePair<string, string>("description", request.Description));

            // Handle metadata
            if (request.Metadata != null) {
                foreach (var item in request.Metadata) {
                    formData.Add(new KeyValuePair<string, string>($"metadata[{item.Key}]", item.Value));
                }
            }

            // Create a request factory function
            Func<HttpRequestMessage> requestFactory = () => {
                var newRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/bank_accounts");
                newRequest.Content = new FormUrlEncodedContent(formData);
                return newRequest;
            };

            // Use the generic SendRequestAsync method with JsonTypeInfo
            return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.BankAccountResponse, cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task<BankAccountResponse> ExecuteAsync(GetRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrEmpty(request.Id))
            throw new ArgumentException("Bank account ID cannot be null or empty", nameof(request));

        // Create a request factory function
        Func<HttpRequestMessage> requestFactory =
            () => new HttpRequestMessage(HttpMethod.Get, $"{_options.BaseUrl}/bank_accounts/{request.Id}");

        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.BankAccountResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(DeleteRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrEmpty(request.Id))
            throw new ArgumentException("Bank account ID cannot be null or empty", nameof(request));

        // Create a request factory function
        Func<HttpRequestMessage> requestFactory =
            () => new HttpRequestMessage(HttpMethod.Delete, $"{_options.BaseUrl}/bank_accounts/{request.Id}");

        // Use the generic SendRequestAsync method with JsonTypeInfo
        await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.DeleteResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ListResponse<BankAccountResponse>> ExecuteAsync(ListRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        // Build the query string
        var queryParams = new List<string>();

        if (request.Skip.HasValue)
            queryParams.Add($"skip={request.Skip.Value}");

        if (request.Limit.HasValue)
            queryParams.Add($"limit={request.Limit.Value}");

        if (!string.IsNullOrEmpty(request.Search))
            queryParams.Add($"search={HttpUtility.UrlEncode(request.Search)}");

        var queryString = queryParams.Count > 0 ? $"?{string.Join("&", queryParams)}" : "";

        // Create a request factory function
        Func<HttpRequestMessage> requestFactory =
            () => new HttpRequestMessage(HttpMethod.Get, $"{_options.BaseUrl}/bank_accounts{queryString}");

        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.ListResponseBankAccountResponse, cancellationToken);
    }
}
