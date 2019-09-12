using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow.Storage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowlet
{
    public class MyDataParser<T> : DataParser<T> where T : EntityBase<T>, new()
    {
        protected override Task<DataFlowResult> Parse(DataFlowContext context)
        {
            return base.Parse(context);
        }

        protected override T ConfigureDataObject(T t)
        {
            return base.ConfigureDataObject(t);
        }
    }
}
