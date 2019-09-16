using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetSpider.DataFlow;
using Microsoft.Extensions.Logging;
using DotnetSpider.DataFlow.Parser;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using DotnetSpider.EventBus;
using DotnetSpider.Common;

namespace Rowlet.Dataflows
{
    public class IndexDataFlow : IDataFlow
    {
        public ILogger Logger { get; set; }

        public string Name => nameof(IndexDataFlow);

        public void Dispose()
        {

        }

        public Task<DataFlowResult> HandleAsync(DataFlowContext context)
        {
            int result = 0;

            IEnumerator enumerator = context.GetParseItem(typeof(IndexEntity).FullName).GetEnumerator();
            while (enumerator.MoveNext())
            {
                result = SaveDealIndex((IndexEntity)enumerator.Current);
                if (result == -1)
                {
                    break;
                }
            }

            if (result == -1)
            {
                IEventBus bus = (IEventBus)context.Services.GetService(typeof(IEventBus));
                bus.Publish(context.Response.Request.OwnerId, new Event() { Type = Framework.ExitCommand });

                return Task.FromResult(DataFlowResult.Terminated);
            }
            else
            {
                return Task.FromResult(DataFlowResult.Success);
            }

        }

        public Task InitAsync()
        {
            return Task.CompletedTask;
        }

        private int SaveDealIndex(IndexEntity entity)
        {
            int result = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigManager.GetConfig("SQLServer").Replace("{your_username}", ConfigManager.GetConfig("Username")).Replace("{your_password}", ConfigManager.GetConfig("Password"))))
                {
                    connection.Open();

                    string cmdText = $"insert into dbo.LJDealIndex values (@ID, @Title, @Scrapped)";

                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        command.Parameters.Add("ID", SqlDbType.NVarChar).Value = entity.ID;
                        command.Parameters.Add("Title", SqlDbType.NVarChar).Value = entity.Title;
                        command.Parameters.Add("Scrapped", SqlDbType.Bit).Value = entity.Scrapped;
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                result = -1;
            }

            return result;
        }

    }
}
