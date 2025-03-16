using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Shane32.PostGrid.Contacts;

namespace Tests;

public class ContactsTests
{
    [Fact]
    public async Task CreateContact_ShouldSendCorrectRequest_AndReturnResponse()
    {
        // Arrange
        // Mock HTTP message handler to capture the request and return a predefined response
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns(VerifyRequestAndCreateResponse);

        // Set up DI
        var services = new ServiceCollection();

        // Configure with test API key
        services.AddPostGrid(options => {
            options.ApiKey = "test_api_key_123";
            options.BaseUrl = "https://api.postgrid.com/print-mail/v1";
        });

        // Replace the HttpClient with our mocked one
        services.AddHttpClient<IPostGridConnection, PostGridConnection>()
            .ConfigurePrimaryHttpMessageHandler(() => mockHttpMessageHandler.Object);

        using var serviceProvider = services.BuildServiceProvider();

        // Get the PostGrid client from DI
        var postGrid = serviceProvider.GetRequiredService<PostGrid>();

        // Create the contact request (similar to App.cs)
        var contactRequest = new CreateRequest {
            FirstName = "Kevin",
            LastName = "Smith",
            CompanyName = "PostGrid",
            AddressLine1 = "20-20 bay st",
            AddressLine2 = "floor 11",
            City = "toronto",
            ProvinceOrState = "ON",
            PostalOrZip = "M5J 2N8",
            CountryCode = "CA",
            Email = "kevinsmith@postgrid.com",
            PhoneNumber = "8885550100",
            JobTitle = "Manager",
            Description = "Kevin Smith's contact information",
            Metadata = new Dictionary<string, string>
            {
                { "friend", "no" }
            },
            ForceVerifiedStatus = false,
            SkipVerification = false,
            Secret = false,
        };

        // Act
        var result = await postGrid.Contacts.CreateAsync(contactRequest);

        // Assert

        // Verify the response
        result.ShouldNotBeNull();
        result.Id.ShouldBe("contact_123456789");
        result.FirstName.ShouldBe("Kevin");
        result.LastName.ShouldBe("Smith");
        result.CompanyName.ShouldBe("PostGrid");
        result.AddressLine1.ShouldBe("20-20 BAY ST");
        result.AddressLine2.ShouldBe("FLOOR 11");
        result.City.ShouldBe("TORONTO");
        result.ProvinceOrState.ShouldBe("ON");
        result.PostalOrZip.ShouldBe("M5J 2N8");
        result.CountryCode.ShouldBe("CA");
        result.Email.ShouldBe("kevinsmith@postgrid.com");
        result.PhoneNumber.ShouldBe("8885550100");
        result.JobTitle.ShouldBe("Manager");
        result.Description.ShouldBe("Kevin Smith's contact information");
        result.AddressStatus.ShouldBe("verified");
        result.Live.ShouldBeFalse();
        result.Secret.ShouldBeFalse();
        result.SkipVerification.ShouldBeFalse();
        result.ForceVerifiedStatus.ShouldBeFalse();
        result.Metadata.ShouldNotBeNull();
        result.Metadata["friend"].ShouldBe("no");
        result.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T04:25:42.200Z"));
        result.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T04:25:42.200Z"));

        // Local function to verify the request and return a response
        async Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Post);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe("https://api.postgrid.com/print-mail/v1/contacts");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Verify form data
            request.Content.ShouldBeOfType<FormUrlEncodedContent>();
            var formData = await request.Content.ReadAsStringAsync();
            formData.ShouldBe("""
                firstName=Kevin&lastName=Smith&companyName=PostGrid&addressLine1=20-20+bay+st&addressLine2=floor+11&city=toronto&provinceOrState=ON&email=kevinsmith%40postgrid.com&phoneNumber=8885550100&jobTitle=Manager&postalOrZip=M5J+2N8&countryCode=CA&description=Kevin+Smith%27s+contact+information&skipVerification=false&forceVerifiedStatus=false&secret=false&metadata%5Bfriend%5D=no
                """);

            // Set up the response
            // Note: this is an actual response from the PostGrid API (a test account), except the id
            // Notice that the address has been validated and modified by the API
            var response = """
                {"id":"contact_123456789","object":"contact","live":false,"addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","forceVerifiedStatus":false,"jobTitle":"Manager","lastName":"Smith","mailingLists":[],"metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false,"skipVerification":false,"createdAt":"2025-03-16T04:25:42.200Z","updatedAt":"2025-03-16T04:25:42.200Z"}
                """;

            // Return the response
            return new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            };
        }
    }
}
