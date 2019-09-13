using DotnetSpider;
using DotnetSpider.Common;
using DotnetSpider.DownloadAgent;
using DotnetSpider.Statistics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rowlet.Core;
using Rowlet.Spiders;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rowlet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            StartWithHost(args);



            //using (var spider = new LJSpider()
            //{
            //    ThreadNum = 1,
            //    CycleRetryTimes = 1,
            //    SleepTime = 1000,

            //})
            //{
            //    spider.AddRequest(new Request("https://nj.lianjia.com/chengjiao/pg1/"));
            //    spider.Run();
            //}

            //string[] deals = GetDeals().ToArray();

            //for (int i = 0; i < deals.Length; i++)
            //{
            //    using (var spider = new LJSpider().AddRequest(new Request($"https://nj.lianjia.com/chengjiao/{deals[i]}.html")))
            //    {
            //        spider.Run();
            //    }

            //    Thread.Sleep(200);
            //    Console.WriteLine(deals[i] + " finished!");
            //    Console.WriteLine(deals.Length - i - 1 + " to go!");
            //    Console.WriteLine();
            //}

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
            hostBuilder.Register<EntitySpider>();
            hostBuilder.Register<IndexSpider>();
            var host = hostBuilder.Build();

            host.Start();

            var spider1 = host.Create<IndexSpider>();
            Task task = spider1.RunAsync();
            task.ContinueWith((t) =>
            {
                var spider2 = host.Create<EntitySpider>();
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

            Startup.Execute<EntitySpider>(args);
        }

        public static SpiderHostBuilder CreateHostBuilder()
        {
            return new SpiderHostBuilder()
                .Register<LJSpider>();
        }

        //private static List<string> GetDeals()
        //{
        //    List<string> deals = new List<string>();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConfigManager.GetConfig("SQLServer").Replace("{your_username}", ConfigManager.GetConfig("Username")).Replace("{your_password}", ConfigManager.GetConfig("Password"))))
        //        {
        //            connection.Open();

        //            string cmdText = $"select id from dbo.LJDealIndex where Scrapped = 0 AND NOT TITLE LIKE '%车位%'";

        //            using (SqlCommand command = new SqlCommand(cmdText, connection))
        //            {
        //                SqlDataReader reader = command.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    deals.Add(reader.GetString(reader.GetOrdinal(nameof(DealIndexEntity.ID))));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("ERROR\t" + ex.Message);
        //    }

        //    return deals;
        //}
    }
}
