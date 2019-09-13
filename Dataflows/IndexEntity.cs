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
    [Schema("SoData", "dbo.LJDealIndex")]
    [EntitySelector(Expression = ".//ul[@class='listContent']/li")]
    public class IndexEntity : EntityBase<IndexEntity>
    {
        protected override void Configure()
        {
            HasKey(x => x.ID);
        }

        [ValueSelector(Expression = ".//div[@class='title']/a/@href")]
        [ReplaceFormatter(NewValue = "", OldValue = "https://nj.lianjia.com/chengjiao/")]
        [ReplaceFormatter(NewValue = "", OldValue = ".html")]
        public string ID { get; set; }

        [ValueSelector(Expression = ".//div[@class='title']/a")]
        public string Title { get; set; }

        public bool Scrapped { get; set; } = false;
    }
}
