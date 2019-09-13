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
            IEnumerator enumerator = context.GetParseItem(typeof(IndexEntity).FullName).GetEnumerator();
            while (enumerator.MoveNext())
            {
                SaveDealIndex((IndexEntity)enumerator.Current);
            }

            return Task.FromResult(DataFlowResult.Success);
        }

        public Task InitAsync()
        {
            return Task.CompletedTask;
        }

        private void SaveDealIndex(IndexEntity entity)
        {
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
                Console.WriteLine("ERROR\t" + ex.Message);
            }
        }

    }
}
