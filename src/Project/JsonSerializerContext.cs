using System.Text.Json.Serialization;
using Shane32.PostGrid.Contacts;
using Shane32.PostGrid.BankAccounts;

namespace Shane32.PostGrid;

/// <summary>
/// JSON serializer context for source generation.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    WriteIndented = false)]
[JsonSerializable(typeof(ContactResponse))]
[JsonSerializable(typeof(ErrorResponse))]
[JsonSerializable(typeof(ListResponse<ContactResponse>))]
[JsonSerializable(typeof(BankAccountResponse))]
[JsonSerializable(typeof(ListResponse<BankAccountResponse>))]
internal partial class PostGridJsonSerializerContext : JsonSerializerContext
{
}