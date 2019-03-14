using System;
using System.Security.Policy;
using DotnetSpider.Core;
using Rowlet.Core;

namespace Rowlet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var spider = new LJSpider()
            {
                ThreadNum = 1,
                CycleRetryTimes = 1,
                SleepTime = 2000,

            })
            {
                spider.Run();
            }

            // Console.Read();
        }
    }
}
