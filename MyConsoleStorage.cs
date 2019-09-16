using DotnetSpider.Common;
using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Storage;
using DotnetSpider.DownloadAgent;
using DotnetSpider.EventBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rowlet
{
    public class MyConsoleStorage : StorageBase
    {
        public static MyConsoleStorage CreateFromOptions(SpiderOptions options)
        {
            return new MyConsoleStorage();
        }

        protected override Task<DataFlowResult> Store(DataFlowContext context)
        {
            return Task.FromResult(DataFlowResult.Success);
        }
    }

    public class DataFlow1 : IDataFlow
    {
        public ILogger Logger { get; set; }

        public string Name => "DataFlow1";

        public void Dispose()
        {

        }

        public Task<DataFlowResult> HandleAsync(DataFlowContext context)
        {
            IEventBus bus = (IEventBus)context.Services.GetService(typeof(IEventBus));
            bus.Publish(context.Response.Request.OwnerId, new Event() { Type = Framework.ExitCommand });

            return Task.FromResult(DataFlowResult.Terminated);
        }

        public Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }

    public class DataFlow2 : IDataFlow
    {
        public ILogger Logger { get; set; }

        public string Name => "DataFlow2";

        public void Dispose()
        {

        }

        public Task<DataFlowResult> HandleAsync(DataFlowContext context)
        {
            return Task.FromResult(DataFlowResult.Success);
        }

        public Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
