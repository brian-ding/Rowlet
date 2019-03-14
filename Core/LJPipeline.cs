using System;
using System.Collections.Generic;
using DotnetSpider.Core;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Extraction.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace Rowlet.Core
{
    internal class LJPipeline : IPipeline
    {
        public ILogger Logger { get; set; }

        public void Dispose()
        {

        }

        public void Process(IList<ResultItems> resultItems, dynamic sender = null)
        {
            if (resultItems == null)
            {
                return;
            }

            foreach (var data in resultItems)
            {
                if (data.ContainsKey(typeof(DealIndexEntity).FullName))
                {
                    var list = (data[typeof(DealIndexEntity).FullName] as List<Object>).Cast<DealIndexEntity>().ToList();
                    foreach (var entity in list)
                    {
                        SaveDealIndex(entity);
                    }
                }
                else if (data.ContainsKey(typeof(DealInfoEntity).FullName))
                {
                    var list = (data[typeof(DealInfoEntity).FullName] as List<Object>).Cast<DealInfoEntity>().ToList();
                    foreach (var entity in list)
                    {
                        SaveDealInfo(entity);
                    }
                }
            }
        }

        private void SaveDealIndex(DealIndexEntity entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Server=CNPC0Z76R8\SQLEXPRESS;Database=LJDT;Trusted_Connection=True;ConnectRetryCount=0"))
                {
                    connection.Open();

                    string cmdText = $"insert into dbo.DealIndex values (@ID, @Title, @Scrapped)";

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
                Console.WriteLine("ERROR" + ex.Message);
            }
        }

        private void SaveDealInfo(DealInfoEntity entity)
        {
            Console.WriteLine(entity.DealPrice.ToString());
        }
    }
}