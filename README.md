# Shane32.PostGrid

[![NuGet](https://img.shields.io/nuget/v/Shane32.PostGrid.svg)](https://www.nuget.org/packages/Shane32.PostGrid) [![Coverage Status](https://coveralls.io/repos/github/Shane32/PostGrid/badge.svg?branch=master)](https://coveralls.io/github/Shane32/PostGrid?branch=master)

This is a SDK for the PostGrid API. It is not endorsed or maintained by PostGrid. This implementation contains code to access the following parts of the PostGrid API:

- Contacts
- BankAccounts (for US banks)
- Checks

It also has an optional retry mechanism for rate-limited requests and supports NativeAOT.

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

public class ContactService
{
    private readonly PostGrid _postGrid;

    public ContactService(PostGrid postGrid)
    {
        _postGrid = postGrid;
    }

    public async Task<ContactResponse> CreateContactAsync(string firstName, string lastName, string address)
    {
        try
        {
            return await _postGrid.Contacts.CreateAsync(new()
            {
                FirstName = firstName,
                LastName = lastName,
                AddressLine1 = address,
                CountryCode = "US"
            });
        }
        catch (PostGridException ex)
        {
            // Handle API errors
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ContactResponse>> GetAllContactsAsync()
    {
        var contacts = new List<ContactResponse>();
        
        await foreach (var contact in _postGrid.Contacts.ListAllAsync())
        {
            contacts.Add(contact);
        }
        
        return contacts;
    }
}
```

## Credits

Glory to Jehovah, Lord of Lords and King of Kings, creator of Heaven and Earth, who through his Son Jesus Christ,
has reedemed me to become a child of God. -Shane32
