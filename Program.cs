using DotnetSpider;
using DotnetSpider.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rowlet.Spiders;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Rowlet
{
    class Program
    {
        static void Main(string[] args)
        {
            StartWithHost(args);

            Console.Read();
        }

        public static void StartWithHost(string[] args)
        {
            var configure = new LoggerConfiguration()
#if DEBUG
                            .MinimumLevel.Verbose()
#else
                            .MinimumLevel.Information()
#endif
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console().WriteTo
    .RollingFile("dotnet-spider.log");
            Log.Logger = configure.CreateLogger();

            var hostBuilder = new SpiderHostBuilder()
                .ConfigureAppConfiguration(x =>
                {
                    if (File.Exists("appsettings.json"))
                    {
                        x.AddJsonFile("appsettings.json");
                    }

                    x.AddCommandLine(args);
                    //x.AddEnvironmentVariables();
                })
                .ConfigureLogging(x => { x.AddSerilog(); })
                .ConfigureServices((services) =>
                {

                    services.AddLocalEventBus();
                    services.AddSingleton<IScheduler>(new MyScheduler());
                    services.AddLocalDownloadCenter();
                    services.AddDownloaderAgent((x) =>
                    {
                        x.UseFileLocker();
                        x.UseDefaultAdslRedialer();
                        x.UseDefaultInternetDetector();
                    });
                    services.AddStatisticsCenter((x) =>
                    {
                        x.UseMemory();
                    });
                });
            hostBuilder.Register<IndexSpider>();
            hostBuilder.Register<InfoSpider>();
            var host = hostBuilder.Build();

            host.Start();

            var spider1 = host.Create<IndexSpider>();
            Task task = spider1.RunAsync();
            task.ContinueWith((t) =>
            {
                var spider2 = host.Create<InfoSpider>();
                spider2.RunAsync(args);
            });

        }

        public static void StartWithoutHost(string[] args)
        {
            var configure = new LoggerConfiguration()
#if DEBUG
                            .MinimumLevel.Verbose()
#else
                            .MinimumLevel.Information()
#endif
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console().WriteTo
    .RollingFile("dotnet-spider.log");
            Log.Logger = configure.CreateLogger();

            Startup.Execute<IndexSpider>(args);
        }
    }
}
