using Shane32.PostGrid;

namespace ConsoleApp;

public class App
{
    private readonly PostGrid _postGrid;

    public App(PostGrid postGrid)
    {
        _postGrid = postGrid;
    }

    public async Task RunAsync()
    {
        // Create a new contact
        var contactRequest = new Shane32.PostGrid.Contacts.CreateRequest {
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

        Console.WriteLine("Creating contact...");
        var contactResponse = await _postGrid.Contacts.CreateAsync(contactRequest);

        // Display the response
        Console.WriteLine($"Contact created with ID: {contactResponse.Id}");
        Console.WriteLine($"Status: {contactResponse.AddressStatus}");

        // Get the contact by ID and display formatted JSON
        Console.WriteLine("Getting contact by ID...");
        var retrievedContact = await _postGrid.Contacts.GetAsync(contactResponse.Id);
        var contactJson = System.Text.Json.JsonSerializer.Serialize(retrievedContact, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(contactJson);
        Console.WriteLine();

        // Create a bank account
        var bankAccountRequest = new Shane32.PostGrid.BankAccounts.CreateRequest {
            BankName = "Example Bank",
            AccountNumber = "123456789",
            RoutingNumber = "021000021", // Example routing number
            BankCountryCode = "US",
            SignatureText = "Kevin Smith",
            Description = "Example bank account"
        };

        Console.WriteLine("Creating bank account...");
        var bankAccountResponse = await _postGrid.BankAccounts.CreateAsync(bankAccountRequest);

        // Display the response
        Console.WriteLine($"Bank account created with ID: {bankAccountResponse.Id}");

        // Get the bank account by ID and display formatted JSON
        Console.WriteLine("Getting bank account by ID...");
        var retrievedBankAccount = await _postGrid.BankAccounts.GetAsync(bankAccountResponse.Id);
        var bankAccountJson = System.Text.Json.JsonSerializer.Serialize(retrievedBankAccount, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(bankAccountJson);
        Console.WriteLine();

        // Create a check
        var checkRequest = new Shane32.PostGrid.Checks.CreateRequest {
            To = contactResponse.Id!, // Use the contact we just created as the recipient
            From = contactResponse.Id!, // Use the same contact as the sender for this example
            BankAccount = bankAccountResponse.Id!, // Use the bank account we just created
            Amount = 10000, // $100.00 (amount in cents)
            Number = 1001, // Check number
            Memo = "Example payment",
            Message = "<p>Thank you for your business!</p>",
            IdempotencyKey = Guid.NewGuid().ToString() // Add a unique idempotency key
        };

        Console.WriteLine("Creating check...");
        var checkResponse = await _postGrid.Checks.CreateAsync(checkRequest);
        var checkResponse2 = await _postGrid.Checks.CreateAsync(checkRequest); // This will return the same data because the idempotency key is the same

        // Display the response
        Console.WriteLine($"Check created with ID: {checkResponse.Id}");
        Console.WriteLine($"Similar check created with ID: {checkResponse2.Id}");
        Console.WriteLine($"Status: {checkResponse.Status}");
        Console.WriteLine($"Preview URL: {checkResponse.Url}");
        Console.WriteLine();
        var pretty = System.Text.Json.JsonSerializer.Serialize(checkResponse, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine("Check response:");
        Console.WriteLine(pretty);
        Console.WriteLine();
        Console.ReadLine(); // Pause before attempting to read the check

        // Get the check by ID and display formatted JSON
        Console.WriteLine("Getting check by ID...");
        var retrievedCheck = await _postGrid.Checks.GetAsync(checkResponse.Id);
        var checkJson = System.Text.Json.JsonSerializer.Serialize(retrievedCheck, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(checkJson);
    }
}
