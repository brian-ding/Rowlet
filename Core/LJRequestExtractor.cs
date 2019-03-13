using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using DotnetSpider.Downloader;
using DotnetSpider.Extraction;

namespace Rowlet.Core
{
    internal class LJRequestExtractor : IRequestExtractor
    {
        public IEnumerable<Request> Extract(Page page)
        {
            List<Request> results = new List<Request>();

            // list page
            if (page.TargetUrl.Contains("pg"))
            {
                List<string> urlList = page.Selectable().XPath(".//div[@class='title']/a/@href").GetValues() as List<string>;
                foreach (var url in urlList)
                {
                    results.Add(new Request(url));
                }

                int pageNo = int.Parse(Regex.Match(page.TargetUrl, @"pg(\d+)/").Groups[1].Value) + 1;
                results.Add(new Request("https://nj.lianjia.com/chengjiao/pg" + pageNo.ToString() + "/"));
            }

            return results;
        }
    }
}