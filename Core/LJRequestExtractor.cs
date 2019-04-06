using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
            if (page.TargetUrl.Contains("pg") && page.ResultItems.ContainsKey(typeof(DealIndexEntity).FullName))
            {
                var list = (page.ResultItems[typeof(DealIndexEntity).FullName] as List<Object>).Cast<DealIndexEntity>().ToList();
                if (!Exist(list.Last()))
                {
                    int pageNo = int.Parse(Regex.Match(page.TargetUrl, @"pg(\d+)/").Groups[1].Value) + 1;
                    results.Add(new Request("https://nj.lianjia.com/chengjiao/pg" + pageNo.ToString() + "/"));
                }
            }

            return results;
        }

        private bool Exist(DealIndexEntity entity)
        {
            object result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Server=" + ConfigManager.GetConfig("SQLServer") + ";Database=LJDT;Trusted_Connection=True;ConnectRetryCount=0"))
                {
                    connection.Open();

                    string cmdText = $"select id from [dbo].DealIndex where id = @id";

                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        command.Parameters.Add("ID", SqlDbType.NVarChar).Value = entity.ID;
                        result = command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR" + ex.Message);
            }

            return result != null;
        }
    }
}