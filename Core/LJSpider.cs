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
    internal class LJSpider : DotnetSpider.Extension.EntitySpider
    {
        protected override void OnInit(params string[] arguments)
        {
            AddEntityType<DealIndexEntity>().Filter = new PatternFilter(@"https://nj.lianjia.com/chengjiao/pg\d+/");
            AddEntityType<DealInfoEntity>().Filter = new PatternFilter(@"https://nj.lianjia.com/chengjiao/\d+.html");
            AddPipeline(new LJPipeline());

            var processor = new DefaultPageProcessor() { CleanPound = false };
            processor.RequestExtractor = new LJRequestExtractor();
            AddPageProcessor(processor);
        }
    }
}