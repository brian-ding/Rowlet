using System;
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
    [Entity(Expression = "/*")]
    internal class DealInfoEntity : IBaseEntity
    {
        [Field(Expression = ".//div[@class='introContent']/div[@class='transaction']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"\d+")]
        public string ID { get; set; }

        [Field(Expression = ".//span[@class='dealTotalPrice']/i")]
        public int DealPrice { get; set; }

        [Field(Expression = ".//div[@class='info fr']/div[@class='msg']/span[1]/label")]
        public int InitPrice { get; set; }

        [Field(Expression = ".//div[@class='info fr']/div[@class='price']/b")]
        public int UnitPrice { get; set; }

        [Field(Expression = ".//div[@class='info fr']/div[@class='msg']/span[2]/label")]
        public int DealPeriod { get; set; }

        [Field(Expression = ".//div[@class='info fr']/div[@class='msg']/span[3]/label")]
        public int PriceChange { get; set; }

        [Field(Expression = ".//div[@class='info fr']/div[@class='msg']/span[4]/label")]
        public int Visitor { get; set; }

        [Field(Expression = ".//div[@class='info fr']/div[@class='msg']/span[5]/label")]
        public int Starer { get; set; }

        [Field(Expression = ".//div[@class='info fr']/div[@class='msg']/span[6]/label")]
        public int WebViewer { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"(\d)室", Group = 1)]
        public int BedroomNum { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"(\d)厅", Group = 1)]
        public int LivingroomNum { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"(\d)厨", Group = 1)]
        public int KitchenNum { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"(\d)卫", Group = 1)]
        public int RestroomNum { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[2]/text()")]
        [RegexFormatter(Pattern = "(.楼层)[(]", Group = 1)]
        public string Floor { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[2]/text()")]
        [RegexFormatter(Pattern = @"共(\d+)层", Group = 1)]
        public int TotalFloor { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[3]/text()")]
        [ReplaceFormatter(NewValue = "", OldValue = "㎡")]
        public double OutArea { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[5]/text()")]
        [ReplaceFormatter(NewValue = "", OldValue = "㎡")]
        [ReplaceFormatter(NewValue = "0", OldValue = "暂无数据")]
        public double InArea { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[7]/text()")]
        [ReplaceFormatter(NewValue = "", OldValue = " ")]
        public string Direction { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[8]/text()")]
        public int Year { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[9]/text()")]
        public string Decoration { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[14]/text()")]
        [ReplaceFormatter(NewValue = "true", OldValue = "有")]
        [ReplaceFormatter(NewValue = "false", OldValue = "无")]
        public bool HasElevator { get; set; }

        [Field(Expression = ".//div[@class='introContent']/div[@class='transaction']/div[@class='content']/ul/li[3]/text()")]
        public DateTime PublishDate { get; set; }

        [Field(Expression = ".//body/div[contains(@class, 'house-title')]/div[@class='wrapper']/span")]
        [ReplaceFormatter(NewValue = "", OldValue = "成交")]
        [ReplaceFormatter(NewValue = "", OldValue = " ")]
        public DateTime DealDate { get; set; }

        [Field(Expression = ".//body/div[contains(@class, 'house-title')]/div[@class='wrapper']/text()")]
        [SplitFormatter(Separator = new[] { " " }, ElementAt = 0)]
        public string Community { get; set; }
    }
}