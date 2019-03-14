using System.Collections.Generic;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Processor.Filter;
using DotnetSpider.Core.Processor.RequestExtractor;
using DotnetSpider.Downloader;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Pipeline;
using DotnetSpider.Extraction;
using DotnetSpider.Extraction.Model;
using DotnetSpider.Extraction.Model.Attribute;
using DotnetSpider.Extraction.Model.Formatter;

namespace Rowlet.Core
{
    [Entity(Expression = ".//ul[@class='listContent']/li")]
    internal class DealIndexEntity : IBaseEntity
    {
        [Field(Expression = ".//div[@class='title']/a/@href")]
        [ReplaceFormatter(NewValue = "", OldValue = "https://nj.lianjia.com/chengjiao/")]
        [ReplaceFormatter(NewValue = "", OldValue = ".html")]
        public string ID { get; set; }

        [Field(Expression = ".//div[@class='title']/a")]
        public string Title { get; set; }

        public bool Scrapped { get; set; } = false;
    }
}