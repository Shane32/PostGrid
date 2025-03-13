using Microsoft.Extensions.DependencyInjection;
using Shane32.PostGrid;
using Shane32.PostGrid.Contacts;

// Example of how to use the PostGrid API client
await RunPostGridExampleAsync();

async Task RunPostGridExampleAsync()
{
    // Set up dependency injection
    var services = new ServiceCollection();
    
    // Add PostGrid services
    services.AddPostGrid(options =>
    {
        options.ApiKey = "your_api_key_here"; // Replace with your actual API key
    });
    
    // Build the service provider
    var serviceProvider = services.BuildServiceProvider();
    
    // Get the PostGrid service
    var postGrid = serviceProvider.GetRequiredService<PostGrid>();
    
    try
    {
        // Create a new contact
        var request = new CreateRequest
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
        request.Metadata = new Dictionary<string, string>
        {
            { "friend", "no" }
        };
        
        Console.WriteLine("Creating contact...");
        var response = await postGrid.Contacts.CreateAsync(request);
        
        // Display the response
        Console.WriteLine($"Contact created with ID: {response.Id}");
        Console.WriteLine($"Status: {response.AddressStatus}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
