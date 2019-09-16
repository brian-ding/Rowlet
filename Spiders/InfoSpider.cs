using DotnetSpider;
using DotnetSpider.Common;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.Downloader;
using DotnetSpider.EventBus;
using DotnetSpider.Statistics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rowlet.Dataflows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rowlet.Spiders
{
    public class InfoSpider : Spider
    {
        public InfoSpider(IEventBus eventBus, IStatisticsService statisticsService, SpiderOptions options, ILogger<Spider> logger, IServiceProvider services) : base(eventBus, statisticsService, options, logger, services)
        {
        }

        protected override void Initialize()
        {
            NewGuidId();

            Speed = 0.1;

            AddRequests(GetDeals().Select((id) =>
            {
                return new Request($"https://nj.lianjia.com/chengjiao/{id}.html");
            }).ToArray());
            AddDataFlow(new DataParser<InfoEntity>())
                .AddDataFlow(new InfoDataFlow());

            base.Initialize();
        }

        private List<string> GetDeals()
        {
            List<string> deals = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigManager.GetConfig("SQLServer").Replace("{your_username}", ConfigManager.GetConfig("Username")).Replace("{your_password}", ConfigManager.GetConfig("Password"))))
                {
                    connection.Open();

                    string cmdText = $"select id from dbo.LJDealIndex where Scrapped = 0 AND NOT TITLE LIKE '%车位%'";

                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            deals.Add(reader.GetString(reader.GetOrdinal(nameof(IndexEntity.ID))));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }

            return deals;
        }
    }
}
