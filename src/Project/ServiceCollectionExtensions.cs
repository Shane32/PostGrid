using Microsoft.Extensions.Configuration;
using Shane32.PostGrid;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up PostGrid services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class PostGridExtensions
{
    /// <summary>
    /// Adds PostGrid services to the specified <see cref="IServiceCollection"/> using configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing the PostGrid configuration.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddPostGrid(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        // Configure options from the configuration and call the other overload
        return AddPostGrid(services, options => {
            options.ApiKey = configuration["ApiKey"]!;

            var baseUrl = configuration["BaseUrl"];
            if (baseUrl != null && !string.IsNullOrEmpty(baseUrl))
                options.BaseUrl = baseUrl;

            if (int.TryParse(configuration["MaxRetryAttempts"], out var maxRetryAttempts))
                options.MaxRetryAttempts = maxRetryAttempts;

            if (int.TryParse(configuration["DefaultRetryDelayMs"], out var defaultRetryDelayMs))
                options.DefaultRetryDelayMs = defaultRetryDelayMs;
        });
    }

    /// <summary>
    /// Adds PostGrid services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configureOptions">A callback to configure the <see cref="PostGridOptions"/>. This is optional.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddPostGrid(this IServiceCollection services, Action<PostGridOptions>? configureOptions)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // Add options
        if (configureOptions != null) {
            services.Configure(configureOptions);
        } else {
            // Add empty options to ensure IOptions<PostGridOptions> is available
            services.AddOptions<PostGridOptions>();
        }

        // Add HttpClient
        services.AddHttpClient<IPostGridConnection, PostGridConnection>();

        // Add PostGrid service
        services.AddTransient<PostGrid>();

        return services;
    }
}
