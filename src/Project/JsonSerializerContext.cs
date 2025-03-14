using System.Text.Json.Serialization;
using Shane32.PostGrid.BankAccounts;
using Shane32.PostGrid.Checks;
using Shane32.PostGrid.Contacts;

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
[JsonSerializable(typeof(CheckResponse))]
[JsonSerializable(typeof(ListResponse<CheckResponse>))]
internal partial class PostGridJsonSerializerContext : JsonSerializerContext
{
}
