using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Threading;
using DotnetSpider.Core;
using DotnetSpider.Downloader;
using Rowlet.Core;

namespace Rowlet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var spider = new LJSpider()
            {
                ThreadNum = 1,
                CycleRetryTimes = 1,
                SleepTime = 1000,

            })
            {
                spider.AddRequest(new Request("https://nj.lianjia.com/chengjiao/pg1/"));
                spider.Run();
            }

            string[] deals = GetDeals().ToArray();

            for (int i = 0; i < deals.Length; i++)
            {
                using (var spider = new LJSpider().AddRequest(new Request($"https://nj.lianjia.com/chengjiao/{deals[i]}.html")))
                {
                    spider.Run();
                }

                Thread.Sleep(200);
                Console.WriteLine(deals[i] + " finished!");
                Console.WriteLine(deals.Length - i - 1 + " to go!");
                Console.WriteLine();
            }

            // Console.Read();
        }

        private static List<string> GetDeals()
        {
            List<string> deals = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigManager.GetConfig("SQLServer").Replace("{your_username}", ConfigManager.GetConfig("Username")).Replace("{your_password}", ConfigManager.GetConfig("Password"))))
                {
                    connection.Open();

                    string cmdText = $"select id from dbo.LJDealIndex where Scrapped = 0";

                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            deals.Add(reader.GetString(reader.GetOrdinal(nameof(DealIndexEntity.ID))));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR\t" + ex.Message);
            }

            return deals;
        }
    }
}
