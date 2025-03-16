# Shane32.PostGrid

[![NuGet](https://img.shields.io/nuget/v/Shane32.PostGrid.svg)](https://www.nuget.org/packages/Shane32.PostGrid) [![Coverage Status](https://coveralls.io/repos/github/Shane32/PostGrid/badge.svg?branch=master)](https://coveralls.io/github/Shane32/PostGrid?branch=master)

This is a SDK for the PostGrid API. It is not endorsed or maintained by PostGrid. This implementation contains code to access the following parts of the PostGrid API:

- Contacts
- BankAccounts (for US banks)
- Checks

It also has an optional retry mechanism for rate-limited requests and supports NativeAOT.

## Supported APIs

- [x] Contacts
- [x] BankAccounts
- [x] Checks
- [ ] Templates
- [ ] Letters
- [ ] Postcards
- [ ] Letters with plastic cards
- [ ] Self mailers
- [ ] Webhooks
- [ ] Return envelopes
- [ ] Return envelope orders
- [ ] Events
- [ ] Template editor sessions
- [ ] Trackers

See https://docs.postgrid.com/

## Installation

Install the package via NuGet:

```
dotnet add package Shane32.PostGrid
```

## Setup and Usage

### Dependency Injection Setup

Add PostGrid to your services in `Program.cs` or `Startup.cs`:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Shane32.PostGrid;

// Add PostGrid services
services.AddPostGrid(options =>
{
    options.ApiKey = "your_postgrid_api_key";
    options.MaxRetryAttempts = 3; // Optional: Configure retry attempts for rate-limited requests
});
```

### Using PostGrid in a Service

Inject and use PostGrid in your service classes:

```csharp
using Shane32.PostGrid;
using Shane32.PostGrid.Contacts;
using Shane32.PostGrid.BankAccounts;
using Shane32.PostGrid.Checks;

public class CheckProcessingService
{
    private readonly PostGrid _postGrid;

    public CheckProcessingService(PostGrid postGrid)
    {
        _postGrid = postGrid;
    }

    public async Task<string> ProcessCheckAsync(decimal amount, string memo)
    {
        // 1. Create a bank account
        var bankAccountResponse = await _postGrid.BankAccounts.CreateAsync(new()
        {
            BankName = "Test Bank",
            AccountNumber = "123456789",
            RoutingNumber = "021000021",
            BankCountryCode = "US",
            SignatureText = "John Doe"
        });

        // 2. Create contacts for from/to
        var senderContact = await _postGrid.Contacts.CreateAsync(new()
        {
            FirstName = "John",
            LastName = "Doe",
            AddressLine1 = "123 Main St",
            City = "New York",
            ProvinceOrState = "NY",
            PostalOrZip = "10001",
            CountryCode = "US"
        });

        var recipientContact = await _postGrid.Contacts.CreateAsync(new()
        {
            FirstName = "Jane",
            LastName = "Smith",
            AddressLine1 = "456 Park Ave",
            City = "New York",
            ProvinceOrState = "NY",
            PostalOrZip = "10002",
            CountryCode = "US"
        });

        // 3. Create a check
        var checkResponse = await _postGrid.Checks.CreateAsync(new()
        {
            To = recipientContact.Id,
            From = senderContact.Id,
            BankAccount = bankAccountResponse.Id,
            Amount = (int)(amount * 100), // Convert dollars to cents
            Number = 1001,
            Memo = memo,
            Message = "<p>Thank you for your business!</p>",
            IdempotencyKey = Guid.NewGuid().ToString()
        });

        return checkResponse.Id;
    }
}
```

## Credits

Glory to Jehovah, Lord of Lords and King of Kings, creator of Heaven and Earth, who through his Son Jesus Christ,
has reedemed me to become a child of God. -Shane32
