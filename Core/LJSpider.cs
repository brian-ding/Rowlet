using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DotnetSpider;
using DotnetSpider.Common;
using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow.Parser.Attribute;
using DotnetSpider.DataFlow.Parser.Formatter;
using DotnetSpider.DataFlow.Storage.Model;
using DotnetSpider.Downloader;
using DotnetSpider.EventBus;
using DotnetSpider.Scheduler;
using DotnetSpider.Selector;
using DotnetSpider.Statistics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Rowlet.Core
{
    internal class LJSpider : Spider
    {
        public LJSpider(IEventBus eventBus, IStatisticsService statisticsService, SpiderOptions options, ILogger<Spider> logger, IServiceProvider services) : base(eventBus, statisticsService, options, logger, services)
        {
        }

        protected override void Initialize()
        {
            NewGuidId();
            Name = Id;
            AddDataFlow(new DataFlow1());
            AddRequests(new Request("https://nj.lianjia.com/chengjiao/pg1/", new Dictionary<string, string> { { "ÍøÕ¾", "²©¿ÍÔ°" } }) { DownloaderType = DownloaderType.HttpClient });
        }

        //protected override void OnInit(params string[] arguments)
        //{
        //    AddEntityType<DealIndexEntity>().Filter = new PatternFilter(@"https://nj.lianjia.com/chengjiao/pg\d+/");
        //    AddEntityType<DealInfoEntity>().Filter = new PatternFilter(@"https://nj.lianjia.com/chengjiao/\d+.html");
        //    AddPipeline(new LJPipeline());

        //    var processor = new DefaultPageProcessor() { CleanPound = false };
        //    processor.RequestExtractor = new LJRequestExtractor();
        //    AddPageProcessor(processor);
        //}
    }

    public class EntitySpider : Spider
    {
        public EntitySpider(IEventBus eventBus, IStatisticsService statisticsService, SpiderOptions options, ILogger<Spider> logger, IServiceProvider services, IConfiguration configuration) : base(eventBus, statisticsService, options, logger, services)
        {

        }

        protected override void Initialize()
        {
            NewGuidId();
            Name = Id;
            Scheduler = new MyScheduler();
            Speed = 1;
            //Depth = 3;
            //AddDataFlow(new CnblogsDataParser()).AddDataFlow(GetDefaultStorage());
            AddDataFlow(new MyDataParser<CnblogsEntry>())
                .AddDataFlow(new DataFlow1())
                //.AddDataFlow(GetDefaultStorage())
                .AddDataFlow(new DataFlow2());
            //AddRequests(new Request("https://www.cnblogs.com/sitehome/p/1", new Dictionary<string, string> { { "ÍøÕ¾", "²©¿ÍÔ°" } }) { DownloaderType = DownloaderType.Test });
            AddRequests(new Request("https://www.cnblogs.com/news/1", new Dictionary<string, string> { { "ÍøÕ¾", "²©¿ÍÔ°" } }) { DownloaderType = DownloaderType.Test });
        }

        class CnblogsDataParser : DataParser
        {
            public CnblogsDataParser()
            {
                //Required = DataParserHelper.CheckIfRequiredByRegex("cnblogs\\.com");
                FollowRequestQuerier = (context) =>
                {
                    List<Request> requests = new List<Request>();
                    string href = new Selectable(context.Response.RawText).XPath(".//a[contains(text(), 'Next &gt;')]").Links().GetValue();
                    requests.Add(new Request("https://www.cnblogs.com" + href) { OwnerId = context.Response.Request.OwnerId });

                    return requests;
                };
            }

            protected override Task<DataFlowResult> Parse(DataFlowContext context)
            {
                context.AddItem("URL", context.Response.Request.Url);
                context.AddItem("Title", new Selectable(context.Response.RawText).XPath(".//a[@class='titlelnk']").GetValues());
                return Task.FromResult(DataFlowResult.Success);
            }
        }


        [Schema("cnblogs", "news")]
        [EntitySelector(Expression = ".//div[@class='post_item']", Type = SelectorType.XPath)]
        //[GlobalValueSelector(Expression = ".//a[@class='current']", Name = "Àà±ð", Type = SelectorType.XPath)]
        [FollowSelector(XPaths = new[] { "//div[@class='pager']" })]
        public class CnblogsEntry : EntityBase<CnblogsEntry>
        {
            public CnblogsEntry()
            {

            }

            protected override void Configure()
            {
                HasIndex(x => x.Title);
                //HasIndex(x => new { x.WebSite, x.Guid }, true);
            }

            public int Id { get; set; }

            //[Required]
            //[StringLength(200)]
            //[ValueSelector(Expression = "Àà±ð", Type = SelectorType.Enviroment)]
            //public string Category { get; set; }

            //[Required]
            //[StringLength(200)]
            //[ValueSelector(Expression = "ÍøÕ¾", Type = SelectorType.Enviroment)]
            //public string WebSite { get; set; }

            [StringLength(200)]
            [ValueSelector(Expression = ".//div[@class='post_item_body']/h3/a", Type = SelectorType.XPath, ValueOption = ValueOption.InnerText)]
            [ReplaceFormatter(NewValue = "", OldValue = " - ²©¿ÍÔ°")]
            public string Title { get; set; }

            //[StringLength(40)]
            //[ValueSelector(Expression = "GUID", Type = SelectorType.Enviroment)]
            //public string Guid { get; set; }

            //[ValueSelector(Expression = ".//h2[@class='news_entry']/a")]
            //public string News { get; set; }

            //[ValueSelector(Expression = ".//h2[@class='news_entry']/a/@href")]
            //public string Url { get; set; }

            //[ValueSelector(Expression = ".//div[@class='entry_summary']", ValueOption = ValueOption.InnerText)]
            //public string PlainText { get; set; }

            //[ValueSelector(Expression = "DATETIME", Type = SelectorType.Enviroment)]
            //public DateTime CreationTime { get; set; }
        }
    }

}