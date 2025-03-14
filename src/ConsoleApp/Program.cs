using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shane32.ConsoleDI;

namespace ConsoleApp;

public class Program
{
    public static async Task Main(string[] args)
        => await ConsoleHost.RunAsync<App>(args, CreateHostBuilder, app => app.RunAsync());

    // this function is necessary for Entity Framework Core tools to perform migrations, etc
    // do not change signature!!
    public static IHostBuilder CreateHostBuilder(string[] args)
        => ConsoleHost.CreateHostBuilder(args, ConfigureServices);

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // Add PostGrid services using configuration
        services.AddPostGrid(context.Configuration.GetSection("PostGrid"));

        // Add the App as a service
        services.AddTransient<App>();
    }
}
