using System.Text.Json.Serialization;

namespace Shane32.PostGrid;

/// <summary>
/// Represents a paginated list response from the PostGrid API.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
public class ListResponse<T>
{
    /// <summary>
    /// Gets or sets the maximum number of items to return.
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// Gets or sets the number of items to skip.
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    /// Gets or sets the total count of items available.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the list of items.
    /// </summary>
    public T[]? Data { get; set; }
}