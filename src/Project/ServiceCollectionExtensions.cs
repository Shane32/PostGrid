using Shane32.PostGrid;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up PostGrid services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds PostGrid services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configureOptions">A callback to configure the <see cref="PostGridOptions"/>. This is optional.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddPostGrid(this IServiceCollection services, Action<PostGridOptions>? configureOptions = null)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        
        // Add options
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }
        else
        {
            // Add empty options to ensure IOptions<PostGridOptions> is available
            services.Configure<PostGridOptions>(_ => { });
        }
        
        // Add HttpClient
        services.AddHttpClient<IPostGridConnection, PostGridConnection>();
        
        // Add PostGrid service
        services.AddTransient<PostGrid>();
        
        return services;
    }
}
