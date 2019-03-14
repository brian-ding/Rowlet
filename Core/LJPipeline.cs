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
                Console.WriteLine("ERROR\t" + ex.Message);
            }
        }

        private void SaveDealInfo(DealInfoEntity entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Server=CNPC0Z76R8\SQLEXPRESS;Database=LJDT;Trusted_Connection=True;ConnectRetryCount=0"))
                {
                    connection.Open();

                    string cmdText = $"insert into dbo.DealInfo values ("
                    + $"@{nameof(DealInfoEntity.ID)}, "
                    + $"@{nameof(DealInfoEntity.DealPrice)}, "
                    + $"@{nameof(DealInfoEntity.InitPrice)}, "
                    + $"@{nameof(DealInfoEntity.UnitPrice)}, "
                    + $"@{nameof(DealInfoEntity.PriceChange)}, "
                    + $"@{nameof(DealInfoEntity.Visitor)}, "
                    + $"@{nameof(DealInfoEntity.Starer)}, "
                    + $"@{nameof(DealInfoEntity.WebViewer)}, "
                    + $"@{nameof(DealInfoEntity.BedroomNum)}, "
                    + $"@{nameof(DealInfoEntity.LivingroomNum)}, "
                    + $"@{nameof(DealInfoEntity.KitchenNum)}, "
                    + $"@{nameof(DealInfoEntity.RestroomNum)}, "
                    + $"@{nameof(DealInfoEntity.Floor)}, "
                    + $"@{nameof(DealInfoEntity.TotalFloor)}, "
                    + $"@{nameof(DealInfoEntity.OutArea)}, "
                    + $"@{nameof(DealInfoEntity.InArea)}, "
                    + $"@{nameof(DealInfoEntity.Direction)}, "
                    + $"@{nameof(DealInfoEntity.Year)}, "
                    + $"@{nameof(DealInfoEntity.Decoration)}, "
                    + $"@{nameof(DealInfoEntity.HasElevator)}, "
                    + $"@{nameof(DealInfoEntity.PublishDate)}, "
                    + $"@{nameof(DealInfoEntity.DealDate)}, "
                    + $"@{nameof(DealInfoEntity.Community)}"
                    + ")";

                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        command.Parameters.Add(nameof(DealInfoEntity.ID), SqlDbType.NVarChar).Value = entity.ID;
                        command.Parameters.Add(nameof(DealInfoEntity.DealPrice), SqlDbType.Int).Value = entity.DealPrice;
                        command.Parameters.Add(nameof(DealInfoEntity.InitPrice), SqlDbType.Int).Value = entity.InitPrice;
                        command.Parameters.Add(nameof(DealInfoEntity.UnitPrice), SqlDbType.Int).Value = entity.UnitPrice;
                        command.Parameters.Add(nameof(DealInfoEntity.PriceChange), SqlDbType.Int).Value = entity.PriceChange;
                        command.Parameters.Add(nameof(DealInfoEntity.Visitor), SqlDbType.Int).Value = entity.Visitor;
                        command.Parameters.Add(nameof(DealInfoEntity.Starer), SqlDbType.Int).Value = entity.Starer;
                        command.Parameters.Add(nameof(DealInfoEntity.WebViewer), SqlDbType.Int).Value = entity.WebViewer;
                        command.Parameters.Add(nameof(DealInfoEntity.BedroomNum), SqlDbType.Int).Value = entity.BedroomNum;
                        command.Parameters.Add(nameof(DealInfoEntity.LivingroomNum), SqlDbType.Int).Value = entity.LivingroomNum;
                        command.Parameters.Add(nameof(DealInfoEntity.KitchenNum), SqlDbType.Int).Value = entity.KitchenNum;
                        command.Parameters.Add(nameof(DealInfoEntity.RestroomNum), SqlDbType.Int).Value = entity.RestroomNum;
                        command.Parameters.Add(nameof(DealInfoEntity.Floor), SqlDbType.NVarChar).Value = entity.Floor;
                        command.Parameters.Add(nameof(DealInfoEntity.TotalFloor), SqlDbType.Int).Value = entity.TotalFloor;
                        command.Parameters.Add(nameof(DealInfoEntity.OutArea), SqlDbType.Real).Value = entity.OutArea;
                        command.Parameters.Add(nameof(DealInfoEntity.InArea), SqlDbType.Real).Value = entity.InArea;
                        command.Parameters.Add(nameof(DealInfoEntity.Direction), SqlDbType.NVarChar).Value = entity.Direction;
                        command.Parameters.Add(nameof(DealInfoEntity.Year), SqlDbType.Int).Value = entity.Year;
                        command.Parameters.Add(nameof(DealInfoEntity.Decoration), SqlDbType.NChar).Value = entity.Decoration;
                        command.Parameters.Add(nameof(DealInfoEntity.HasElevator), SqlDbType.Bit).Value = entity.HasElevator;
                        command.Parameters.Add(nameof(DealInfoEntity.PublishDate), SqlDbType.Date).Value = entity.PublishDate;
                        command.Parameters.Add(nameof(DealInfoEntity.DealDate), SqlDbType.Date).Value = entity.DealDate;
                        command.Parameters.Add(nameof(DealInfoEntity.Community), SqlDbType.NVarChar).Value = entity.Community;
                        command.ExecuteNonQuery();

                        using (SqlConnection indexConn = new SqlConnection(@"Server=CNPC0Z76R8\SQLEXPRESS;Database=LJDT;Trusted_Connection=True;ConnectRetryCount=0"))
                        {
                            indexConn.Open();

                            string indexCmdText = $"update dbo.DealIndex set Scrapped = 1 where ID = {entity.ID}";

                            using (SqlCommand indexCommand = new SqlCommand(indexCmdText, indexConn))
                            {
                                indexCommand.ExecuteNonQuery();
                            }
                        }
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