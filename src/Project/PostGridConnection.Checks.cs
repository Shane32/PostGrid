using System.Net.Http.Headers;
using System.Web;
using Shane32.PostGrid.Checks;

namespace Shane32.PostGrid;

public partial class PostGridConnection
{
    /// <inheritdoc />
    public async Task<CheckResponse> ExecuteAsync(CreateRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.LetterPDF != null && request.LetterHTML != null)
            throw new ArgumentException("Only one of LetterPDF or LetterHTML can be provided", nameof(request));

        // Handle the request based on whether we're using LetterPDF or not
        if (request.LetterPDF != null) {
            // Use multipart/form-data for LetterPDF
            var content = new MultipartFormDataContent {
                // Required fields
                { new StringContent(request.To), "to" },
                { new StringContent(request.From), "from" },
                { new StringContent(request.BankAccount), "bankAccount" },
                { new StringContent(request.Amount.ToString()), "amount" },
                { new StringContent(request.Number.ToString()), "number" }
            };

            // Optional fields
            if (request.RedirectTo != null)
                content.Add(new StringContent(request.RedirectTo), "redirectTo");

            if (request.Memo != null)
                content.Add(new StringContent(request.Memo), "memo");

            if (request.Message != null)
                content.Add(new StringContent(request.Message), "message");

            if (request.Logo != null)
                content.Add(new StringContent(request.Logo), "logo");

            if (request.ExtraService != null)
                content.Add(new StringContent(request.ExtraService), "extraService");

            if (request.CurrencyCode != null)
                content.Add(new StringContent(request.CurrencyCode), "currencyCode");

            if (request.Express.HasValue)
                content.Add(new StringContent(request.Express.Value.ToString().ToLowerInvariant()), "express");

            if (request.SendDate.HasValue)
                content.Add(new StringContent(request.SendDate.Value.ToString("o")), "sendDate");

            if (request.MailingClass != null)
                content.Add(new StringContent(request.MailingClass), "mailingClass");

            if (request.Description != null)
                content.Add(new StringContent(request.Description), "description");

            if (request.Size != null)
                content.Add(new StringContent(request.Size), "size");

            // Add the PDF content
            var pdfContent = new ByteArrayContent(request.LetterPDF);
            pdfContent.Headers.ContentType = new MediaTypeHeaderValue(request.LetterPDFContentType ?? "application/pdf");
            content.Add(pdfContent, "letterPDF");

            // Handle merge variables
            if (request.MergeVariables != null) {
                foreach (var item in request.MergeVariables) {
                    content.Add(new StringContent(item.Value), $"mergeVariables[{item.Key}]");
                }
            }

            // Handle metadata
            if (request.Metadata != null) {
                foreach (var item in request.Metadata) {
                    content.Add(new StringContent(item.Value), $"metadata[{item.Key}]");
                }
            }

            // Create a request factory function
            Func<HttpRequestMessage> requestFactory = () => {
                var newRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/cheques");
                newRequest.Content = content;
                return newRequest;
            };

            // Use the generic SendRequestAsync method with JsonTypeInfo
            return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.CheckResponse, cancellationToken);
        } else {
            // Use form-urlencoded for regular requests or LetterHTML
            var formData = new List<KeyValuePair<string, string>> {
                // Required fields
                new KeyValuePair<string, string>("to", request.To),
                new KeyValuePair<string, string>("from", request.From),
                new KeyValuePair<string, string>("bankAccount", request.BankAccount),
                new KeyValuePair<string, string>("amount", request.Amount.ToString()),
                new KeyValuePair<string, string>("number", request.Number.ToString())
            };

            // Optional fields
            if (request.RedirectTo != null)
                formData.Add(new KeyValuePair<string, string>("redirectTo", request.RedirectTo));

            if (request.Memo != null)
                formData.Add(new KeyValuePair<string, string>("memo", request.Memo));

            if (request.Message != null)
                formData.Add(new KeyValuePair<string, string>("message", request.Message));

            if (request.Logo != null)
                formData.Add(new KeyValuePair<string, string>("logo", request.Logo));

            if (request.ExtraService != null)
                formData.Add(new KeyValuePair<string, string>("extraService", request.ExtraService));

            if (request.CurrencyCode != null)
                formData.Add(new KeyValuePair<string, string>("currencyCode", request.CurrencyCode));

            if (request.Express.HasValue)
                formData.Add(new KeyValuePair<string, string>("express", request.Express.Value.ToString().ToLowerInvariant()));

            if (request.SendDate.HasValue)
                formData.Add(new KeyValuePair<string, string>("sendDate", request.SendDate.Value.ToString("o")));

            if (request.MailingClass != null)
                formData.Add(new KeyValuePair<string, string>("mailingClass", request.MailingClass));

            if (request.Description != null)
                formData.Add(new KeyValuePair<string, string>("description", request.Description));

            if (request.Size != null)
                formData.Add(new KeyValuePair<string, string>("size", request.Size));

            // Add letterHTML if provided
            if (request.LetterHTML != null)
                formData.Add(new KeyValuePair<string, string>("letterHTML", request.LetterHTML));

            // Handle merge variables
            if (request.MergeVariables != null) {
                foreach (var item in request.MergeVariables) {
                    formData.Add(new KeyValuePair<string, string>($"mergeVariables[{item.Key}]", item.Value));
                }
            }

            // Handle metadata
            if (request.Metadata != null) {
                foreach (var item in request.Metadata) {
                    formData.Add(new KeyValuePair<string, string>($"metadata[{item.Key}]", item.Value));
                }
            }

            // Create a request factory function
            Func<HttpRequestMessage> requestFactory = () => {
                var newRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/cheques");
                newRequest.Content = new FormUrlEncodedContent(formData);
                return newRequest;
            };

            // Use the generic SendRequestAsync method with JsonTypeInfo
            return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.CheckResponse, cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task<CheckResponse> ExecuteAsync(GetRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrEmpty(request.Id))
            throw new ArgumentException("Cheque ID cannot be null or empty", nameof(request));

        // Create a request factory function
        Func<HttpRequestMessage> requestFactory =
            () => new HttpRequestMessage(HttpMethod.Get, $"{_options.BaseUrl}/cheques/{request.Id}");

        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.CheckResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CheckResponse> ExecuteAsync(DeleteRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrEmpty(request.Id))
            throw new ArgumentException("Cheque ID cannot be null or empty", nameof(request));

        // Create a request factory function
        Func<HttpRequestMessage> requestFactory =
            () => new HttpRequestMessage(HttpMethod.Delete, $"{_options.BaseUrl}/cheques/{request.Id}");

        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.CheckResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CheckResponse> ExecuteAsync(CancelRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrEmpty(request.Id))
            throw new ArgumentException("Cheque ID cannot be null or empty", nameof(request));

        // Convert the request object to form data
        var formData = new List<KeyValuePair<string, string>>();

        // Add the note if provided
        if (request.Note != null)
            formData.Add(new KeyValuePair<string, string>("note", request.Note));

        // Create a request factory function
        Func<HttpRequestMessage> requestFactory = () => {
            var newRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/cheques/{request.Id}/cancellation");
            newRequest.Content = new FormUrlEncodedContent(formData);
            return newRequest;
        };

        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.CheckResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ListResponse<CheckResponse>> ExecuteAsync(ListRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        // Build the query string
        var queryParams = new List<string>();

        if (request.Skip.HasValue)
            queryParams.Add($"skip={request.Skip.Value}");

        if (request.Limit.HasValue)
            queryParams.Add($"limit={request.Limit.Value}");

        var queryString = queryParams.Count > 0 ? $"?{string.Join("&", queryParams)}" : "";

        // Create a request factory function
        Func<HttpRequestMessage> requestFactory =
            () => new HttpRequestMessage(HttpMethod.Get, $"{_options.BaseUrl}/cheques{queryString}");

        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.ListResponseCheckResponse, cancellationToken);
    }
}
