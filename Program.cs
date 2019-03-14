using System;
using System.Security.Policy;
using DotnetSpider.Core;
using DotnetSpider.Downloader;
using Rowlet.Core;

namespace Rowlet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // using (var spider = new LJSpider()
            // {
            //     ThreadNum = 1,
            //     CycleRetryTimes = 1,
            //     SleepTime = 2000,

            // })
            // {
            //     spider.AddRequest(new Request("https://nj.lianjia.com/chengjiao/pg1/"));
            //     spider.Run();
            // }

            using (var spider = new LJSpider().AddRequest(new Request("https://nj.lianjia.com/chengjiao/103102849249.html")))
            {
                spider.Run();
            }

            // Console.Read();
        }
    }
}
