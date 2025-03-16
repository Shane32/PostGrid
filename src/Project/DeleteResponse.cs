using System.Text.Json.Serialization;

namespace Shane32.PostGrid;

/// <summary>
/// Represents a response from the PostGrid API when deleting a resource.
/// </summary>
public class DeleteResponse
{
    /// <summary>
    /// Gets or sets the ID of the deleted resource.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the resource was successfully deleted.
    /// </summary>
    public required bool Deleted { get; set; }
}
