using System.Text.Json.Serialization;
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
internal partial class PostGridJsonSerializerContext : JsonSerializerContext
{
}