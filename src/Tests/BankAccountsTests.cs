using System.Net;
using System.Net.Http;
using System.Text;
using Shane32.PostGrid.BankAccounts;

namespace Tests;

public class BankAccountsTests : PostGridTestBase
{
    [Theory]
    [InlineData(true)]  // Test with signature image
    [InlineData(false)] // Test with signature text
    public async Task CreateBankAccount_Successful(bool useSignatureImage)
    {
        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Create the bank account request
        var bankAccountRequest = new CreateRequest {
            BankName = "Test Bank",
            AccountNumber = "123456789",
            RoutingNumber = "021000021",
            BankCountryCode = "US",
            BankPrimaryLine = "123 Main St",
            BankSecondaryLine = "New York, NY 10001",
            Description = "Test bank account",
            Metadata = new Dictionary<string, string>
            {
                { "type", "checking" }
            }
        };

        // Set either signature image or text based on the test case
        if (useSignatureImage) {
            // Use a sample image for testing
            bankAccountRequest.SignatureImage = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // PNG header bytes
            bankAccountRequest.SignatureImageContentType = "image/png";
        } else {
            bankAccountRequest.SignatureText = "John Doe";
        }

        // Act
        var result = await PostGrid.BankAccounts.CreateAsync(bankAccountRequest);

        // Assert
        // Verify the response
        result.ShouldNotBeNull();
        result.Id.ShouldBe("bank_123456789");
        result.BankName.ShouldBe("Test Bank");
        result.RoutingNumber.ShouldBe("021000021");
        result.BankCountryCode.ShouldBe("US");
        result.BankPrimaryLine.ShouldBe("123 Main St");
        result.BankSecondaryLine.ShouldBe("New York, NY 10001");
        result.Description.ShouldBe("Test bank account");
        result.AccountNumberLast4.ShouldBe("6789");
        result.AccountNumberAndIDSHA256.ShouldBe("abc123hash456def789");
        result.Live.ShouldBeFalse();
        result.Metadata.ShouldNotBeNull();
        result.Metadata["type"].ShouldBe("checking");
        result.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T15:22:33.726Z"));
        result.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T15:22:33.726Z"));

        // Local function to verify the request and return a response
        async Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Post);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe("https://api.postgrid.com/print-mail/v1/bank_accounts");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Verify form data
            if (request.Content is MultipartFormDataContent multipartContent && useSignatureImage) {
                // Verify the multipart form data contains all expected parts with correct values
                multipartContent.ShouldContainPart("signatureImage", "image/png");
                multipartContent.ShouldContainPartWithValue("bankName", "Test Bank");
                multipartContent.ShouldContainPartWithValue("accountNumber", "123456789");
                multipartContent.ShouldContainPartWithValue("routingNumber", "021000021");
                multipartContent.ShouldContainPartWithValue("bankCountryCode", "US");
                multipartContent.ShouldContainPartWithValue("bankPrimaryLine", "123 Main St");
                multipartContent.ShouldContainPartWithValue("bankSecondaryLine", "New York, NY 10001");
                multipartContent.ShouldContainPartWithValue("description", "Test bank account");
                multipartContent.ShouldContainPartWithValue("metadata[type]", "checking");
            } else {
                // For signature text, we expect form URL encoded content
                request.Content.ShouldBeOfType<FormUrlEncodedContent>();
                var formData = await request.Content.ReadAsStringAsync();
                formData.ShouldBe("""
                    bankName=Test+Bank&accountNumber=123456789&routingNumber=021000021&bankCountryCode=US&signatureText=John+Doe&bankPrimaryLine=123+Main+St&bankSecondaryLine=New+York%2C+NY+10001&description=Test+bank+account&metadata%5Btype%5D=checking
                    """);
            }

            // Set up the response - same for both cases
            var response = """
                {"id":"bank_123456789","object":"bank_account","live":false,"accountNumberAndIDSHA256":"abc123hash456def789","accountNumberLast4":"6789","bankCountryCode":"US","bankName":"Test Bank","bankPrimaryLine":"123 Main St","bankSecondaryLine":"New York, NY 10001","description":"Test bank account","metadata":{"type":"checking"},"routingNumber":"021000021","signatureText":"John Doe","createdAt":"2025-03-16T15:22:33.726Z","updatedAt":"2025-03-16T15:22:33.726Z"}
                """;

            // Return the response
            return new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            };
        }
    }

    [Fact]
    public async Task GetBankAccount_Successful()
    {
        // Bank account ID to retrieve
        var bankAccountId = "bank_123456789";

        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Act
        var result = await PostGrid.BankAccounts.GetAsync(bankAccountId);

        // Assert
        // Verify the response
        result.ShouldNotBeNull();
        result.Id.ShouldBe("bank_123456789");
        result.BankName.ShouldBe("Test Bank");
        result.RoutingNumber.ShouldBe("021000021");
        result.BankCountryCode.ShouldBe("US");
        result.BankPrimaryLine.ShouldBe("123 Main St");
        result.BankSecondaryLine.ShouldBe("New York, NY 10001");
        result.Description.ShouldBe("Test bank account");
        result.AccountNumberLast4.ShouldBe("6789");
        result.AccountNumberAndIDSHA256.ShouldBe("abc123hash456def789");
        result.Live.ShouldBeFalse();
        result.Metadata.ShouldNotBeNull();
        result.Metadata["type"].ShouldBe("checking");
        result.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T15:22:33.726Z"));
        result.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T15:22:33.726Z"));

        // Local function to verify the request and return a response
        Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Get);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe($"https://api.postgrid.com/print-mail/v1/bank_accounts/{bankAccountId}");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Set up the response
            var response = """
                {"id":"bank_123456789","object":"bank_account","live":false,"accountNumberAndIDSHA256":"abc123hash456def789","accountNumberLast4":"6789","bankCountryCode":"US","bankName":"Test Bank","bankPrimaryLine":"123 Main St","bankSecondaryLine":"New York, NY 10001","description":"Test bank account","metadata":{"type":"checking"},"routingNumber":"021000021","signatureText":"John Doe","createdAt":"2025-03-16T15:22:33.726Z","updatedAt":"2025-03-16T15:22:33.726Z"}
                """;

            // Return the response
            return Task.FromResult(new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task DeleteBankAccount_Successful()
    {
        // Bank account ID to delete
        var bankAccountId = "bank_123456789";

        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Act - this should not throw an exception if successful
        await PostGrid.BankAccounts.DeleteAsync(bankAccountId);

        // Assert - the verification happens in the VerifyRequestAndCreateResponse method
        // No need to assert on the result since it's void

        // Local function to verify the request and return a response
        Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Delete);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe($"https://api.postgrid.com/print-mail/v1/bank_accounts/{bankAccountId}");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Set up the response
            var response = """
                {"id":"bank_123456789","object":"bank_account","deleted":true}
                """;

            // Return the response
            return Task.FromResult(new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task DeleteBankAccount_NotFound()
    {
        // Bank account ID to delete
        var bankAccountId = "bank_123456789";

        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Act & Assert
        // The delete operation should throw a PostGridException
        var exception = await Should.ThrowAsync<PostGridException>(async () =>
            await PostGrid.BankAccounts.DeleteAsync(bankAccountId));

        // Verify the exception details
        exception.ShouldNotBeNull();
        exception.Message.ShouldBe("Could not find bank account with id bank_account_id");

        // Local function to verify the request and return a response
        Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Delete);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe($"https://api.postgrid.com/print-mail/v1/bank_accounts/{bankAccountId}");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Set up the error response
            var response = """
                {"object":"error","error":{"type":"bank_account_not_found_error","message":"Could not find bank account with id bank_account_id"}}
                """;

            // Return the response with a 404 Not Found status code
            return Task.FromResult(new HttpResponseMessage {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task ListBankAccounts_Successful()
    {
        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Create the list request with pagination and search parameters
        var listRequest = new ListRequest {
            Skip = 0,
            Limit = 10,
            Search = "Test"
        };

        // Act
        var result = await PostGrid.BankAccounts.ListAsync(listRequest);

        // Assert
        // Verify the response
        result.ShouldNotBeNull();
        result.Skip.ShouldBe(0);
        result.Limit.ShouldBe(10);
        result.TotalCount.ShouldBe(1);
        result.Data.ShouldNotBeNull();
        result.Data.Length.ShouldBe(1);

        // Verify the bank account in the list
        var bankAccount = result.Data[0];
        bankAccount.ShouldNotBeNull();
        bankAccount.Id.ShouldBe("bank_123456789");
        bankAccount.BankName.ShouldBe("Test Bank");
        bankAccount.RoutingNumber.ShouldBe("021000021");
        bankAccount.BankCountryCode.ShouldBe("US");
        bankAccount.BankPrimaryLine.ShouldBe("123 Main St");
        bankAccount.BankSecondaryLine.ShouldBe("New York, NY 10001");
        bankAccount.Description.ShouldBe("Test bank account");
        bankAccount.AccountNumberLast4.ShouldBe("6789");
        bankAccount.AccountNumberAndIDSHA256.ShouldBe("abc123hash456def789");
        bankAccount.Live.ShouldBeFalse();
        bankAccount.Metadata.ShouldNotBeNull();
        bankAccount.Metadata["type"].ShouldBe("checking");
        bankAccount.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T15:22:33.726Z"));
        bankAccount.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T15:22:33.726Z"));

        // Local function to verify the request and return a response
        Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Get);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe("https://api.postgrid.com/print-mail/v1/bank_accounts?skip=0&limit=10&search=Test");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Set up the response
            var response = """
                {"object":"list","limit":10,"skip":0,"totalCount":1,"data":[{"id":"bank_123456789","object":"bank_account","live":false,"accountNumberAndIDSHA256":"abc123hash456def789","accountNumberLast4":"6789","bankCountryCode":"US","bankName":"Test Bank","bankPrimaryLine":"123 Main St","bankSecondaryLine":"New York, NY 10001","description":"Test bank account","metadata":{"type":"checking"},"routingNumber":"021000021","signatureText":"John Doe","createdAt":"2025-03-16T15:22:33.726Z","updatedAt":"2025-03-16T15:22:33.726Z"}]}
                """;

            // Return the response
            return Task.FromResult(new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            });
        }
    }

    [Fact]
    public async Task ListAllBankAccounts_Successful()
    {
        // Set up the response handler
        CreateResponse = VerifyRequestAndCreateResponse;

        // Act
        var results = new List<BankAccountResponse>();
        await foreach (var bankAccountEntry in PostGrid.BankAccounts.ListAllAsync(search: "Test")) {
            results.Add(bankAccountEntry);
        }

        // Assert
        // Verify the response
        results.ShouldNotBeNull();
        results.Count.ShouldBe(1);

        // Verify the bank account in the list
        var bankAccount = results[0];
        bankAccount.ShouldNotBeNull();
        bankAccount.Id.ShouldBe("bank_123456789");
        bankAccount.BankName.ShouldBe("Test Bank");
        bankAccount.RoutingNumber.ShouldBe("021000021");
        bankAccount.BankCountryCode.ShouldBe("US");
        bankAccount.BankPrimaryLine.ShouldBe("123 Main St");
        bankAccount.BankSecondaryLine.ShouldBe("New York, NY 10001");
        bankAccount.Description.ShouldBe("Test bank account");
        bankAccount.AccountNumberLast4.ShouldBe("6789");
        bankAccount.AccountNumberAndIDSHA256.ShouldBe("abc123hash456def789");
        bankAccount.Live.ShouldBeFalse();
        bankAccount.Metadata.ShouldNotBeNull();
        bankAccount.Metadata["type"].ShouldBe("checking");
        bankAccount.CreatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T15:22:33.726Z"));
        bankAccount.UpdatedAt.ShouldBe(DateTimeOffset.Parse("2025-03-16T15:22:33.726Z"));

        // Local function to verify the request and return a response
        Task<HttpResponseMessage> VerifyRequestAndCreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Verify the request
            request.ShouldNotBeNull();
            request.Method.ShouldBe(HttpMethod.Get);
            request.RequestUri.ShouldNotBeNull().ToString().ShouldBe("https://api.postgrid.com/print-mail/v1/bank_accounts?skip=0&limit=100&search=Test");

            // Verify headers
            request.Headers.Contains("x-api-key").ShouldBeTrue();
            request.Headers.GetValues("x-api-key").ShouldBe(new string[] { "test_api_key_123" });

            // Set up the response
            var response = """
                {"object":"list","limit":100,"skip":0,"totalCount":1,"data":[{"id":"bank_123456789","object":"bank_account","live":false,"accountNumberAndIDSHA256":"abc123hash456def789","accountNumberLast4":"6789","bankCountryCode":"US","bankName":"Test Bank","bankPrimaryLine":"123 Main St","bankSecondaryLine":"New York, NY 10001","description":"Test bank account","metadata":{"type":"checking"},"routingNumber":"021000021","signatureText":"John Doe","createdAt":"2025-03-16T15:22:33.726Z","updatedAt":"2025-03-16T15:22:33.726Z"}]}
                """;

            // Return the response
            return Task.FromResult(new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json")
            });
        }
    }
}
