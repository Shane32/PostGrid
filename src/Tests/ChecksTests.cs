using System.Net;
using System.Net.Http;
using System.Text;
using Shane32.PostGrid.Checks;

namespace Tests;

public class ChecksTests : PostGridTestBase
{
    [Fact]
    public async Task CreateCheck_Successful()
    {
        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Create the check request
        var checkRequest = new CreateRequest {
            To = "contact_123456789",
            From = "contact_123456789",
            BankAccount = "bank_123456789",
            Amount = 10000, // $100.00 (amount in cents)
            Number = 1001, // Check number
            Memo = "Example payment",
            Message = "<p>Thank you for your business!</p>",
            IdempotencyKey = "test-idempotency-key"
        };

        // Act
        var result = await PostGrid.Checks.CreateAsync(checkRequest);

        // Assert
        // Verify the response
        result.ShouldNotBeNull();
        result.Id.ShouldBe("cheque_123456789");
        result.Live.ShouldBeFalse();
        result.Amount.ShouldBe(10000);
        result.BankAccount.ShouldBe("bank_123456789");
        result.CurrencyCode.ShouldBe("USD");
        result.Envelope.ShouldBe("standard");
        result.MailingClass.ShouldBe("first_class");
        result.Memo.ShouldBe("Example payment");
        result.Message.ShouldBe("<p>Thank you for your business!</p>");
        result.Number.ShouldBe(1001);
        result.SendDate.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.983Z"));
        result.Size.ShouldBe("us_letter");
        result.Status.ShouldBe("ready");
        result.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.986Z"));
        result.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.986Z"));

        // Verify the contact information in the check
        result.To.ShouldNotBeNull();
        result.To.Id.ShouldBe("contact_123456789");
        result.To.AddressLine1.ShouldBe("20-20 BAY ST");
        result.To.AddressLine2.ShouldBe("FLOOR 11");
        result.To.AddressStatus.ShouldBe("verified");
        result.To.City.ShouldBe("TORONTO");
        result.To.CompanyName.ShouldBe("PostGrid");
        result.To.Country.ShouldBe("CANADA");
        result.To.CountryCode.ShouldBe("CA");
        result.To.Description.ShouldBe("Kevin Smith's contact information");
        result.To.Email.ShouldBe("kevinsmith@postgrid.com");
        result.To.FirstName.ShouldBe("Kevin");
        result.To.JobTitle.ShouldBe("Manager");
        result.To.LastName.ShouldBe("Smith");
        result.To.PhoneNumber.ShouldBe("8885550100");
        result.To.PostalOrZip.ShouldBe("M5J 2N8");
        result.To.ProvinceOrState.ShouldBe("ON");
        result.To.Secret.ShouldBeFalse();
        result.To.Metadata.ShouldNotBeNull();
        result.To.Metadata["friend"].ShouldBe("no");

        result.From.ShouldNotBeNull();
        result.From.Id.ShouldBe("contact_123456789");
        result.From.AddressLine1.ShouldBe("20-20 BAY ST");
        result.From.AddressLine2.ShouldBe("FLOOR 11");
        result.From.AddressStatus.ShouldBe("verified");
        result.From.City.ShouldBe("TORONTO");
        result.From.CompanyName.ShouldBe("PostGrid");
        result.From.Country.ShouldBe("CANADA");
        result.From.CountryCode.ShouldBe("CA");
        result.From.Description.ShouldBe("Kevin Smith's contact information");
        result.From.Email.ShouldBe("kevinsmith@postgrid.com");
        result.From.FirstName.ShouldBe("Kevin");
        result.From.JobTitle.ShouldBe("Manager");
        result.From.LastName.ShouldBe("Smith");
        result.From.PhoneNumber.ShouldBe("8885550100");
        result.From.PostalOrZip.ShouldBe("M5J 2N8");
        result.From.ProvinceOrState.ShouldBe("ON");
        result.From.Secret.ShouldBeFalse();
        result.From.Metadata.ShouldNotBeNull();
        result.From.Metadata["friend"].ShouldBe("no");

        // Local function to verify the request and return a response
        async Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Post);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe("https://api.postgrid.com/print-mail/v1/cheques");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });
            request.Headers.Contains("Idempotency-Key").ShouldBeTrue();
            request.Headers.GetValues("Idempotency-Key").ShouldBe(new string[] { "test-idempotency-key" });

            // Verify form data
            request.Content.ShouldBeOfType<FormUrlEncodedContent>();
            var formData = await request.Content.ReadAsStringAsync();
            formData.ShouldBe("""
                to=contact_123456789&from=contact_123456789&bankAccount=bank_123456789&amount=10000&number=1001&memo=Example+payment&message=%3Cp%3EThank+you+for+your+business%21%3C%2Fp%3E
                """);

            // Set up the response
            var response = """
                {"id":"cheque_123456789","object":"cheque","live":false,"amount":10000,"bankAccount":"bank_123456789","carrierTracking":null,"currencyCode":"USD","envelope":"standard","from":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"mailingClass":"first_class","memo":"Example payment","message":"<p>Thank you for your business!</p>","number":1001,"sendDate":"2025-03-16T17:07:19.983Z","size":"us_letter","status":"ready","to":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"createdAt":"2025-03-16T17:07:19.986Z","updatedAt":"2025-03-16T17:07:19.986Z"}
                """;

            // Return the response
            return new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            };
        }
    }

    [Fact]
    public async Task GetCheck_Successful()
    {
        // Check ID to retrieve
        var checkId = "cheque_123456789";

        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Act
        var result = await PostGrid.Checks.GetAsync(checkId);

        // Assert
        // Verify the response
        result.ShouldNotBeNull();
        result.Id.ShouldBe("cheque_123456789");
        result.Live.ShouldBeFalse();
        result.Amount.ShouldBe(10000);
        result.BankAccount.ShouldBe("bank_123456789");
        result.CurrencyCode.ShouldBe("USD");
        result.Envelope.ShouldBe("standard");
        result.MailingClass.ShouldBe("first_class");
        result.Memo.ShouldBe("Example payment");
        result.Message.ShouldBe("<p>Thank you for your business!</p>");
        result.Number.ShouldBe(1001);
        result.PageCount.ShouldBe(2);
        result.SendDate.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.983Z"));
        result.Size.ShouldBe("us_letter");
        result.Status.ShouldBe("ready");
        result.Url.ShouldBe("https://example.com/test/cheque_123456789.pdf");
        result.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.986Z"));
        result.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:22.775Z"));

        // Verify the contact information in the check
        result.To.ShouldNotBeNull();
        result.To.Id.ShouldBe("contact_123456789");
        result.To.AddressLine1.ShouldBe("20-20 BAY ST");
        result.To.AddressLine2.ShouldBe("FLOOR 11");
        result.To.AddressStatus.ShouldBe("verified");
        result.To.City.ShouldBe("TORONTO");
        result.To.CompanyName.ShouldBe("PostGrid");
        result.To.Country.ShouldBe("CANADA");
        result.To.CountryCode.ShouldBe("CA");
        result.To.Description.ShouldBe("Kevin Smith's contact information");
        result.To.Email.ShouldBe("kevinsmith@postgrid.com");
        result.To.FirstName.ShouldBe("Kevin");
        result.To.JobTitle.ShouldBe("Manager");
        result.To.LastName.ShouldBe("Smith");
        result.To.PhoneNumber.ShouldBe("8885550100");
        result.To.PostalOrZip.ShouldBe("M5J 2N8");
        result.To.ProvinceOrState.ShouldBe("ON");
        result.To.Secret.ShouldBeFalse();
        result.To.Metadata.ShouldNotBeNull();
        result.To.Metadata["friend"].ShouldBe("no");

        result.From.ShouldNotBeNull();
        result.From.Id.ShouldBe("contact_123456789");
        result.From.AddressLine1.ShouldBe("20-20 BAY ST");
        result.From.AddressLine2.ShouldBe("FLOOR 11");
        result.From.AddressStatus.ShouldBe("verified");
        result.From.City.ShouldBe("TORONTO");
        result.From.CompanyName.ShouldBe("PostGrid");
        result.From.Country.ShouldBe("CANADA");
        result.From.CountryCode.ShouldBe("CA");
        result.From.Description.ShouldBe("Kevin Smith's contact information");
        result.From.Email.ShouldBe("kevinsmith@postgrid.com");
        result.From.FirstName.ShouldBe("Kevin");
        result.From.JobTitle.ShouldBe("Manager");
        result.From.LastName.ShouldBe("Smith");
        result.From.PhoneNumber.ShouldBe("8885550100");
        result.From.PostalOrZip.ShouldBe("M5J 2N8");
        result.From.ProvinceOrState.ShouldBe("ON");
        result.From.Secret.ShouldBeFalse();
        result.From.Metadata.ShouldNotBeNull();
        result.From.Metadata["friend"].ShouldBe("no");

        // Local function to verify the request and return a response
        Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Get);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe($"https://api.postgrid.com/print-mail/v1/cheques/{checkId}");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Set up the response
            var response = """
                {"id":"cheque_123456789","object":"cheque","live":false,"amount":10000,"bankAccount":"bank_123456789","carrierTracking":null,"currencyCode":"USD","envelope":"standard","from":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"mailingClass":"first_class","memo":"Example payment","message":"<p>Thank you for your business!</p>","number":1001,"pageCount":2,"sendDate":"2025-03-16T17:07:19.983Z","size":"us_letter","status":"ready","to":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"url":"https://example.com/test/cheque_123456789.pdf","createdAt":"2025-03-16T17:07:19.986Z","updatedAt":"2025-03-16T17:07:22.775Z"}
                """;

            // Return the response
            return Task.FromResult(new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task ListChecks_Successful()
    {
        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Create the list request with pagination parameters
        var listRequest = new ListRequest {
            Skip = 0,
            Limit = 10
        };

        // Act
        var result = await PostGrid.Checks.ListAsync(listRequest);

        // Assert
        // Verify the response
        result.ShouldNotBeNull();
        result.Skip.ShouldBe(0);
        result.Limit.ShouldBe(10);
        result.TotalCount.ShouldBe(10);
        result.Data.ShouldNotBeNull();
        result.Data.Length.ShouldBe(1);

        // Verify the check in the list
        var check = result.Data[0];
        check.ShouldNotBeNull();
        check.Id.ShouldBe("cheque_123456789");
        check.Live.ShouldBeFalse();
        check.Amount.ShouldBe(10000);
        check.BankAccount.ShouldBe("bank_123456789");
        check.CurrencyCode.ShouldBe("USD");
        check.Envelope.ShouldBe("standard");
        check.MailingClass.ShouldBe("first_class");
        check.Memo.ShouldBe("Example payment");
        check.Message.ShouldBe("<p>Thank you for your business!</p>");
        check.Number.ShouldBe(1001);
        check.PageCount.ShouldBe(2);
        check.SendDate.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.983Z"));
        check.Size.ShouldBe("us_letter");
        check.Status.ShouldBe("ready");
        check.Url.ShouldBe("https://example.com/test/cheque_123456789.pdf");
        check.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.986Z"));
        check.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:22.775Z"));

        // Verify the contact information in the check
        check.To.ShouldNotBeNull();
        check.To.Id.ShouldBe("contact_123456789");
        check.To.AddressLine1.ShouldBe("20-20 BAY ST");
        check.To.AddressLine2.ShouldBe("FLOOR 11");
        check.To.AddressStatus.ShouldBe("verified");
        check.To.City.ShouldBe("TORONTO");
        check.To.CompanyName.ShouldBe("PostGrid");
        check.To.Country.ShouldBe("CANADA");
        check.To.CountryCode.ShouldBe("CA");
        check.To.Description.ShouldBe("Kevin Smith's contact information");
        check.To.Email.ShouldBe("kevinsmith@postgrid.com");
        check.To.FirstName.ShouldBe("Kevin");
        check.To.JobTitle.ShouldBe("Manager");
        check.To.LastName.ShouldBe("Smith");
        check.To.PhoneNumber.ShouldBe("8885550100");
        check.To.PostalOrZip.ShouldBe("M5J 2N8");
        check.To.ProvinceOrState.ShouldBe("ON");
        check.To.Secret.ShouldBeFalse();
        check.To.Metadata.ShouldNotBeNull();
        check.To.Metadata["friend"].ShouldBe("no");

        check.From.ShouldNotBeNull();
        check.From.Id.ShouldBe("contact_123456789");
        check.From.AddressLine1.ShouldBe("20-20 BAY ST");
        check.From.AddressLine2.ShouldBe("FLOOR 11");
        check.From.AddressStatus.ShouldBe("verified");
        check.From.City.ShouldBe("TORONTO");
        check.From.CompanyName.ShouldBe("PostGrid");
        check.From.Country.ShouldBe("CANADA");
        check.From.CountryCode.ShouldBe("CA");
        check.From.Description.ShouldBe("Kevin Smith's contact information");
        check.From.Email.ShouldBe("kevinsmith@postgrid.com");
        check.From.FirstName.ShouldBe("Kevin");
        check.From.JobTitle.ShouldBe("Manager");
        check.From.LastName.ShouldBe("Smith");
        check.From.PhoneNumber.ShouldBe("8885550100");
        check.From.PostalOrZip.ShouldBe("M5J 2N8");
        check.From.ProvinceOrState.ShouldBe("ON");
        check.From.Secret.ShouldBeFalse();
        check.From.Metadata.ShouldNotBeNull();
        check.From.Metadata["friend"].ShouldBe("no");

        // Local function to verify the request and return a response
        Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Get);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe("https://api.postgrid.com/print-mail/v1/cheques?skip=0&limit=10");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Set up the response
            var response = """
                {"object":"list","limit":10,"skip":0,"totalCount":10,"data":[{"id":"cheque_123456789","object":"cheque","live":false,"amount":10000,"bankAccount":"bank_123456789","carrierTracking":null,"currencyCode":"USD","envelope":"standard","from":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"mailingClass":"first_class","memo":"Example payment","message":"<p>Thank you for your business!</p>","number":1001,"pageCount":2,"sendDate":"2025-03-16T17:07:19.983Z","size":"us_letter","status":"ready","to":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"url":"https://example.com/test/cheque_123456789.pdf","createdAt":"2025-03-16T17:07:19.986Z","updatedAt":"2025-03-16T17:07:22.775Z"}]}
                """;

            // Return the response
            return Task.FromResult(new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task DeleteCheck_Successful()
    {
        // Check ID to delete
        var checkId = "cheque_123456789";

        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Act
        var result = await PostGrid.Checks.DeleteAsync(checkId);

        // Assert
        // Verify the response
        result.ShouldNotBeNull();
        result.Id.ShouldBe("cheque_123456789");
        result.Live.ShouldBeFalse();
        result.Amount.ShouldBe(10000);
        result.BankAccount.ShouldBe("bank_123456789");
        result.CurrencyCode.ShouldBe("USD");
        result.Envelope.ShouldBe("standard");
        result.MailingClass.ShouldBe("first_class");
        result.Memo.ShouldBe("Example payment");
        result.Message.ShouldBe("<p>Thank you for your business!</p>");
        result.Number.ShouldBe(1001);
        result.PageCount.ShouldBe(2);
        result.SendDate.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.983Z"));
        result.Size.ShouldBe("us_letter");
        result.Status.ShouldBe("cancelled");
        result.Url.ShouldBe("https://example.com/test/cheque_123456789.pdf");
        result.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.986Z"));
        result.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:09:47.707Z"));

        // Verify cancellation details
        result.Cancellation.ShouldNotBeNull();
        result.Cancellation.CancelledByUser.ShouldBe("user_123456789");
        result.Cancellation.Reason.ShouldBe("user_initiated");

        // Local function to verify the request and return a response
        Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Delete);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe($"https://api.postgrid.com/print-mail/v1/cheques/{checkId}");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Set up the response
            var response = """
                {"id":"cheque_123456789","object":"cheque","live":false,"amount":10000,"bankAccount":"bank_123456789","cancellation":{"cancelledByUser":"user_123456789","reason":"user_initiated"},"carrierTracking":null,"currencyCode":"USD","envelope":"standard","from":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"mailingClass":"first_class","memo":"Example payment","message":"<p>Thank you for your business!</p>","number":1001,"pageCount":2,"sendDate":"2025-03-16T17:07:19.983Z","size":"us_letter","status":"cancelled","to":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"url":"https://example.com/test/cheque_123456789.pdf","createdAt":"2025-03-16T17:07:19.986Z","updatedAt":"2025-03-16T17:09:47.707Z"}
                """;

            // Return the response
            return Task.FromResult(new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task GetCheck_AfterDelete_Successful()
    {
        // Check ID to retrieve
        var checkId = "cheque_123456789";

        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Act
        var result = await PostGrid.Checks.GetAsync(checkId);

        // Assert
        // Verify the response
        result.ShouldNotBeNull();
        result.Id.ShouldBe("cheque_123456789");
        result.Live.ShouldBeFalse();
        result.Amount.ShouldBe(10000);
        result.BankAccount.ShouldBe("bank_123456789");
        result.CurrencyCode.ShouldBe("USD");
        result.Envelope.ShouldBe("standard");
        result.MailingClass.ShouldBe("first_class");
        result.Memo.ShouldBe("Example payment");
        result.Message.ShouldBe("<p>Thank you for your business!</p>");
        result.Number.ShouldBe(1001);
        result.PageCount.ShouldBe(2);
        result.SendDate.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.983Z"));
        result.Size.ShouldBe("us_letter");
        result.Status.ShouldBe("cancelled");
        result.Url.ShouldBe("https://example.com/test/cheque_123456789.pdf");
        result.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:07:19.986Z"));
        result.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T17:09:47.707Z"));

        // Verify cancellation details
        result.Cancellation.ShouldNotBeNull();
        result.Cancellation.CancelledByUser.ShouldBe("user_123456789");
        result.Cancellation.Reason.ShouldBe("user_initiated");

        // Local function to verify the request and return a response
        Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Get);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe($"https://api.postgrid.com/print-mail/v1/cheques/{checkId}");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Set up the response
            var response = """
                {"id":"cheque_123456789","object":"cheque","live":false,"amount":10000,"bankAccount":"bank_123456789","cancellation":{"cancelledByUser":"user_123456789","reason":"user_initiated"},"carrierTracking":null,"currencyCode":"USD","envelope":"standard","from":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"mailingClass":"first_class","memo":"Example payment","message":"<p>Thank you for your business!</p>","number":1001,"pageCount":2,"sendDate":"2025-03-16T17:07:19.983Z","size":"us_letter","status":"cancelled","to":{"id":"contact_123456789","object":"contact","addressLine1":"20-20 BAY ST","addressLine2":"FLOOR 11","addressStatus":"verified","city":"TORONTO","companyName":"PostGrid","country":"CANADA","countryCode":"CA","description":"Kevin Smith's contact information","email":"kevinsmith@postgrid.com","firstName":"Kevin","jobTitle":"Manager","lastName":"Smith","metadata":{"friend":"no"},"phoneNumber":"8885550100","postalOrZip":"M5J 2N8","provinceOrState":"ON","secret":false},"url":"https://example.com/test/cheque_123456789.pdf","createdAt":"2025-03-16T17:07:19.986Z","updatedAt":"2025-03-16T17:09:47.707Z"}
                """;

            // Return the response
            return Task.FromResult(new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            });
        }
    }
}
