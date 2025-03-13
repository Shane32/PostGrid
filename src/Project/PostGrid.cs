using Shane32.PostGrid.Helpers;

namespace Shane32.PostGrid;

/// <summary>
/// The main entry point for the PostGrid API.
/// </summary>
public class PostGrid
{
    private readonly IPostGridConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostGrid"/> class.
    /// </summary>
    /// <param name="connection">The connection to use for API requests.</param>
    public PostGrid(IPostGridConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    private PostGridContacts? _contacts;
    
    /// <summary>
    /// Gets the contacts API for creating and managing contacts.
    /// </summary>
    public PostGridContacts Contacts => _contacts ??= new PostGridContacts(_connection);
}
