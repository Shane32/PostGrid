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
        var contactRequest = new Shane32.PostGrid.Contacts.CreateRequest
        {
            FirstName = "Kevin",
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
            Description = "Kevin Smith's contact information"
        };

        // Add metadata
        contactRequest.Metadata = new Dictionary<string, string>
        {
            { "friend", "no" }
        };

        Console.WriteLine("Creating contact...");
        var contactResponse = await _postGrid.Contacts.CreateAsync(contactRequest);

        // Display the response
        Console.WriteLine($"Contact created with ID: {contactResponse.Id}");
        Console.WriteLine($"Status: {contactResponse.AddressStatus}");

        // Create a bank account
        var bankAccountRequest = new Shane32.PostGrid.BankAccounts.CreateRequest
        {
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

        // Create a check
        var checkRequest = new Shane32.PostGrid.Checks.CreateRequest
        {
            To = contactResponse.Id!, // Use the contact we just created as the recipient
            From = contactResponse.Id!, // Use the same contact as the sender for this example
            BankAccount = bankAccountResponse.Id!, // Use the bank account we just created
            Amount = 10000, // $100.00 (amount in cents)
            Number = 1001, // Check number
            Memo = "Example payment",
            Message = "<p>Thank you for your business!</p>"
        };

        Console.WriteLine("Creating check...");
        var checkResponse = await _postGrid.Checks.CreateAsync(checkRequest);

        // Display the response
        Console.WriteLine($"Check created with ID: {checkResponse.Id}");
        Console.WriteLine($"Status: {checkResponse.Status}");
        Console.WriteLine($"Preview URL: {checkResponse.Url}");
    }
}
