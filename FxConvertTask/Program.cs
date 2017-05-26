using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

using System;

namespace FxConvertTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup DI provider
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            //configure logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug().WriteTo.RollingFile("Log/FxConvert-{Date}.txt").CreateLogger();

            try
            {
                serviceProvider.GetService<App>().Run();
            }
            catch(Exception e)
            {
                var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Serilog.ILogger>();
                logger.LogError(e.Message);
            }
        }

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            //add logging
            serviceCollection.AddSingleton(new LoggerFactory().AddSerilog());
            serviceCollection.AddLogging();

            //add services
            serviceCollection.AddTransient<IConvertService, ConvertService>();
            serviceCollection.AddTransient<IRatesRepository, RatesRepository>();
            serviceCollection.AddTransient<IFixerClient, FixerClient>();
            serviceCollection.AddSingleton<IOptions, Options>();

            //add app
            serviceCollection.AddTransient<App>();
        }
    }
}