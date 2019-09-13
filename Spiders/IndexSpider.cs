using DotnetSpider;
using DotnetSpider.Common;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.Downloader;
using DotnetSpider.EventBus;
using DotnetSpider.Scheduler;
using DotnetSpider.Statistics;
using Microsoft.Extensions.Logging;
using Rowlet.Dataflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowlet.Spiders
{
    public class IndexSpider : Spider
    {
        public IndexSpider(IEventBus eventBus, IStatisticsService statisticsService, SpiderOptions options, ILogger<Spider> logger, IServiceProvider services) : base(eventBus, statisticsService, options, logger, services)
        {
        }

        protected override void Initialize()
        {
            NewGuidId();
            Scheduler = new QueueDistinctBfsScheduler();
            Speed = 1;
            // Depth = 3;

            // requests
            AddRequests(new Request("https://nj.lianjia.com/chengjiao/pg1/"));

            //AddDataFlow()
            AddDataFlow(new DataParser<IndexEntity>())
                .AddDataFlow(new IndexDataFlow());

            base.Initialize();
        }
    }
}
