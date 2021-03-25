using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GetItemCQRSTester
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Warning)
                 .MinimumLevel.Warning()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            try
            {
                MainAsync(args);
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        static void MainAsync(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                Log.Information("Starting app, to set number of test requests and url, check appsettings.json in current directory...");
                serviceProvider.GetService<App>().Run();
                Log.Information("Stopping app");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error starting application.");
                throw ex;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder
                    .AddSerilog(dispose: true);
            }));

            serviceCollection.AddLogging();

            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton(configuration);

            serviceCollection.AddHttpClient("CQRS", c =>
            {
                c.BaseAddress = new Uri(configuration.GetValue<string>("url"));
            });

            serviceCollection.AddTransient<App>();
        }
    }
}
