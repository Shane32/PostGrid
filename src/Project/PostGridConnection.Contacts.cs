using System.Web;
using Shane32.PostGrid.Contacts;

namespace Shane32.PostGrid;

public partial class PostGridConnection
{
    /// <inheritdoc />
    public async Task<ContactResponse> ExecuteAsync(CreateRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        // Convert the request object to form data
        var formData = new List<KeyValuePair<string, string>>();
        
        // Manually add each property to the form data
        if (request.FirstName != null)
            formData.Add(new KeyValuePair<string, string>("firstName", request.FirstName));
            
        if (request.LastName != null)
            formData.Add(new KeyValuePair<string, string>("lastName", request.LastName));
            
        if (request.CompanyName != null)
            formData.Add(new KeyValuePair<string, string>("companyName", request.CompanyName));
            
        formData.Add(new KeyValuePair<string, string>("addressLine1", request.AddressLine1));
            
        if (request.AddressLine2 != null)
            formData.Add(new KeyValuePair<string, string>("addressLine2", request.AddressLine2));
            
        if (request.City != null)
            formData.Add(new KeyValuePair<string, string>("city", request.City));
            
        if (request.ProvinceOrState != null)
            formData.Add(new KeyValuePair<string, string>("provinceOrState", request.ProvinceOrState));
            
        if (request.Email != null)
            formData.Add(new KeyValuePair<string, string>("email", request.Email));
            
        if (request.PhoneNumber != null)
            formData.Add(new KeyValuePair<string, string>("phoneNumber", request.PhoneNumber));
            
        if (request.JobTitle != null)
            formData.Add(new KeyValuePair<string, string>("jobTitle", request.JobTitle));
            
        if (request.PostalOrZip != null)
            formData.Add(new KeyValuePair<string, string>("postalOrZip", request.PostalOrZip));
            
        formData.Add(new KeyValuePair<string, string>("countryCode", request.CountryCode));
            
        if (request.Description != null)
            formData.Add(new KeyValuePair<string, string>("description", request.Description));
            
        // Boolean values
        formData.Add(new KeyValuePair<string, string>("skipVerification", request.SkipVerification.ToString().ToLowerInvariant()));
        formData.Add(new KeyValuePair<string, string>("forceVerifiedStatus", request.ForceVerifiedStatus.ToString().ToLowerInvariant()));
        formData.Add(new KeyValuePair<string, string>("secret", request.Secret.ToString().ToLowerInvariant()));
        
        // Handle metadata
        if (request.Metadata != null)
        {
            foreach (var item in request.Metadata)
            {
                formData.Add(new KeyValuePair<string, string>($"metadata[{item.Key}]", item.Value));
            }
        }
        
        // Create a request factory function
        Func<HttpRequestMessage> requestFactory = () =>
        {
            var newRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/contacts");
            newRequest.Content = new FormUrlEncodedContent(formData);
            return newRequest;
        };
        
        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.ContactResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ContactResponse> ExecuteAsync(GetRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrEmpty(request.Id))
            throw new ArgumentException("Contact ID cannot be null or empty", nameof(request));

        // Create a request factory function
        Func<HttpRequestMessage> requestFactory = 
            () => new HttpRequestMessage(HttpMethod.Get, $"{_options.BaseUrl}/contacts/{request.Id}");

        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.ContactResponse, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<ContactResponse> ExecuteAsync(DeleteRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrEmpty(request.Id))
            throw new ArgumentException("Contact ID cannot be null or empty", nameof(request));

        // Create a request factory function
        Func<HttpRequestMessage> requestFactory =
            () => new HttpRequestMessage(HttpMethod.Delete, $"{_options.BaseUrl}/contacts/{request.Id}");

        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.ContactResponse, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<ListResponse<ContactResponse>> ExecuteAsync(ListRequest request, CancellationToken cancellationToken = default)
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
            () => new HttpRequestMessage(HttpMethod.Get, $"{_options.BaseUrl}/contacts{queryString}");

        // Use the generic SendRequestAsync method with JsonTypeInfo
        return await SendRequestAsync(requestFactory, PostGridJsonSerializerContext.Default.ListResponseContactResponse, cancellationToken);
    }
}
