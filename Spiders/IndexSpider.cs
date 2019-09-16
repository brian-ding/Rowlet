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
using System.Text.RegularExpressions;

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

            Speed = 1;
            // Depth = 3;

            // requests
            AddRequests(new Request("https://nj.lianjia.com/chengjiao/pg1/"));

            //AddDataFlow()
            AddDataFlow(new DataParser<IndexEntity>()
            {
                FollowRequestQuerier = (context) =>
                {
                    List<Request> requests = new List<Request>();
                    int pageNo = int.Parse(Regex.Match(context.Response.Request.Url, @"pg(\d+)/").Groups[1].Value) + 1;
                    requests.Add(new Request("https://nj.lianjia.com/chengjiao/pg" + pageNo.ToString() + "/") { OwnerId = context.Response.Request.OwnerId });

                    //string href = new Selectable(context.Response.RawText).XPath(".//div[contains(@class, 'house-lst-page-box')]/a[last()]").Links().GetValue();

                    return requests;
                }
            })
                .AddDataFlow(new IndexDataFlow());

            base.Initialize();
        }
    }
}
