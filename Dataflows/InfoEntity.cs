using DotnetSpider.DataFlow.Parser.Attribute;
using DotnetSpider.DataFlow.Parser.Formatter;
using DotnetSpider.DataFlow.Storage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowlet.Dataflows
{
    [Schema("SoData", "dbo.LJDealInfo")]
    [EntitySelector(Expression = "/*")]
    public class InfoEntity : EntityBase<InfoEntity>
    {
        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='transaction']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"\d+")]
        public string ID { get; set; }

        [ValueSelector(Expression = ".//span[@class='dealTotalPrice']/i")]
        public double DealPrice { get; set; }

        [ValueSelector(Expression = ".//div[@class='info fr']/div[@class='msg']/span[1]/label")]
        public double InitPrice { get; set; }

        [ValueSelector(Expression = ".//div[@class='info fr']/div[@class='price']/b")]
        public double UnitPrice { get; set; }

        [ValueSelector(Expression = ".//div[@class='info fr']/div[@class='msg']/span[2]/label")]
        [ReplaceFormatter(NewValue = "0", OldValue = "暂无数据")]
        public int DealPeriod { get; set; }

        [ValueSelector(Expression = ".//div[@class='info fr']/div[@class='msg']/span[3]/label")]
        [ReplaceFormatter(NewValue = "0", OldValue = "暂无数据")]
        public int PriceChange { get; set; }

        [ValueSelector(Expression = ".//div[@class='info fr']/div[@class='msg']/span[4]/label")]
        [ReplaceFormatter(NewValue = "0", OldValue = "暂无数据")]
        public int Visitor { get; set; }

        [ValueSelector(Expression = ".//div[@class='info fr']/div[@class='msg']/span[5]/label")]
        [ReplaceFormatter(NewValue = "0", OldValue = "暂无数据")]
        public int Starer { get; set; }

        [ValueSelector(Expression = ".//div[@class='info fr']/div[@class='msg']/span[6]/label")]
        [ReplaceFormatter(NewValue = "0", OldValue = "暂无数据")]
        public int WebViewer { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"(\d)室", Group = 1)]
        public int BedroomNum { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"(\d)厅", Group = 1)]
        public int LivingroomNum { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"(\d)厨", Group = 1)]
        public int KitchenNum { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[1]/text()")]
        [RegexFormatter(Pattern = @"(\d)卫", Group = 1)]
        public int RestroomNum { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[2]/text()")]
        [RegexFormatter(Pattern = "(.楼层)[(]", Group = 1)]
        public string Floor { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[2]/text()")]
        [RegexFormatter(Pattern = @"共(\d+)层", Group = 1)]
        public int TotalFloor { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[3]/text()")]
        [ReplaceFormatter(NewValue = "", OldValue = "㎡")]
        public double OutArea { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[5]/text()")]
        [ReplaceFormatter(NewValue = "0", OldValue = "暂无数据")]
        [ReplaceFormatter(NewValue = "", OldValue = "㎡")]
        public double InArea { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[7]/text()")]
        [ReplaceFormatter(NewValue = "", OldValue = " ")]
        public string Direction { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[8]/text()")]
        [ReplaceFormatter(NewValue = "0", OldValue = "未知")]
        public int Year { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[9]/text()")]
        public string Decoration { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='base']/div[@class='content']/ul/li[14]/text()")]
        [ReplaceFormatter(NewValue = "false", OldValue = "暂无数据")]
        [ReplaceFormatter(NewValue = "true", OldValue = "有")]
        [ReplaceFormatter(NewValue = "false", OldValue = "无")]
        public bool HasElevator { get; set; }

        [ValueSelector(Expression = ".//div[@class='introContent']/div[@class='transaction']/div[@class='content']/ul/li[3]/text()")]
        public DateTime PublishDate { get; set; }

        [ValueSelector(Expression = ".//body/div[contains(@class, 'house-title')]/div[@class='wrapper']/span")]
        [ReplaceFormatter(NewValue = "", OldValue = "成交")]
        [ReplaceFormatter(NewValue = "", OldValue = " ")]
        public DateTime DealDate { get; set; }

        [ValueSelector(Expression = ".//body/div[contains(@class, 'house-title')]/div[@class='wrapper']/text()")]
        [SplitFormatter(Separator = new[] { " " }, ElementAt = 0)]
        public string Community { get; set; }

        public double? Latitude { get; set; }

        public double? Longtitude { get; set; }
    }
}
