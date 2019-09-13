using DotnetSpider.DataFlow;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowlet.Dataflows
{
    public class InfoDataFlow : IDataFlow
    {
        public ILogger Logger { get; set; }

        public string Name => nameof(InfoDataFlow);

        public void Dispose()
        {

        }

        public Task<DataFlowResult> HandleAsync(DataFlowContext context)
        {
            IEnumerator enumerator = context.GetParseItem(typeof(InfoEntity).FullName).GetEnumerator();
            while (enumerator.MoveNext())
            {
                SaveDealInfo((InfoEntity)enumerator.Current);
            }

            return Task.FromResult(DataFlowResult.Success);
        }

        public Task InitAsync()
        {
            return Task.CompletedTask;
        }

        private void SaveDealInfo(InfoEntity entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigManager.GetConfig("SQLServer").Replace("{your_username}", ConfigManager.GetConfig("Username")).Replace("{your_password}", ConfigManager.GetConfig("Password"))))
                {
                    connection.Open();

                    string cmdText = $"insert into dbo.LJDealInfo values ("
                    + $"@{nameof(InfoEntity.ID)}, "
                    + $"@{nameof(InfoEntity.DealPrice)}, "
                    + $"@{nameof(InfoEntity.InitPrice)}, "
                    + $"@{nameof(InfoEntity.UnitPrice)}, "
                    + $"@{nameof(InfoEntity.PriceChange)}, "
                    + $"@{nameof(InfoEntity.Visitor)}, "
                    + $"@{nameof(InfoEntity.Starer)}, "
                    + $"@{nameof(InfoEntity.WebViewer)}, "
                    + $"@{nameof(InfoEntity.BedroomNum)}, "
                    + $"@{nameof(InfoEntity.LivingroomNum)}, "
                    + $"@{nameof(InfoEntity.KitchenNum)}, "
                    + $"@{nameof(InfoEntity.RestroomNum)}, "
                    + $"@{nameof(InfoEntity.Floor)}, "
                    + $"@{nameof(InfoEntity.TotalFloor)}, "
                    + $"@{nameof(InfoEntity.OutArea)}, "
                    + $"@{nameof(InfoEntity.InArea)}, "
                    + $"@{nameof(InfoEntity.Direction)}, "
                    + $"@{nameof(InfoEntity.Year)}, "
                    + $"@{nameof(InfoEntity.Decoration)}, "
                    + $"@{nameof(InfoEntity.HasElevator)}, "
                    + $"@{nameof(InfoEntity.PublishDate)}, "
                    + $"@{nameof(InfoEntity.DealDate)}, "
                    + $"@{nameof(InfoEntity.Community)}"
                    + ")";

                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        command.Parameters.Add(nameof(InfoEntity.ID), SqlDbType.NVarChar).Value = entity.ID;
                        command.Parameters.Add(nameof(InfoEntity.DealPrice), SqlDbType.Int).Value = entity.DealPrice;
                        command.Parameters.Add(nameof(InfoEntity.InitPrice), SqlDbType.Int).Value = entity.InitPrice;
                        command.Parameters.Add(nameof(InfoEntity.UnitPrice), SqlDbType.Int).Value = entity.UnitPrice;
                        command.Parameters.Add(nameof(InfoEntity.PriceChange), SqlDbType.Int).Value = entity.PriceChange;
                        command.Parameters.Add(nameof(InfoEntity.Visitor), SqlDbType.Int).Value = entity.Visitor;
                        command.Parameters.Add(nameof(InfoEntity.Starer), SqlDbType.Int).Value = entity.Starer;
                        command.Parameters.Add(nameof(InfoEntity.WebViewer), SqlDbType.Int).Value = entity.WebViewer;
                        command.Parameters.Add(nameof(InfoEntity.BedroomNum), SqlDbType.Int).Value = entity.BedroomNum;
                        command.Parameters.Add(nameof(InfoEntity.LivingroomNum), SqlDbType.Int).Value = entity.LivingroomNum;
                        command.Parameters.Add(nameof(InfoEntity.KitchenNum), SqlDbType.Int).Value = entity.KitchenNum;
                        command.Parameters.Add(nameof(InfoEntity.RestroomNum), SqlDbType.Int).Value = entity.RestroomNum;
                        command.Parameters.Add(nameof(InfoEntity.Floor), SqlDbType.NVarChar).Value = entity.Floor;
                        command.Parameters.Add(nameof(InfoEntity.TotalFloor), SqlDbType.Int).Value = entity.TotalFloor;
                        command.Parameters.Add(nameof(InfoEntity.OutArea), SqlDbType.Real).Value = entity.OutArea;
                        command.Parameters.Add(nameof(InfoEntity.InArea), SqlDbType.Real).Value = entity.InArea;
                        command.Parameters.Add(nameof(InfoEntity.Direction), SqlDbType.NVarChar).Value = entity.Direction;
                        command.Parameters.Add(nameof(InfoEntity.Year), SqlDbType.Int).Value = entity.Year;
                        command.Parameters.Add(nameof(InfoEntity.Decoration), SqlDbType.NChar).Value = entity.Decoration;
                        command.Parameters.Add(nameof(InfoEntity.HasElevator), SqlDbType.Bit).Value = entity.HasElevator;
                        command.Parameters.Add(nameof(InfoEntity.PublishDate), SqlDbType.Date).Value = entity.PublishDate;
                        command.Parameters.Add(nameof(InfoEntity.DealDate), SqlDbType.Date).Value = entity.DealDate;
                        command.Parameters.Add(nameof(InfoEntity.Community), SqlDbType.NVarChar).Value = entity.Community;
                        command.ExecuteNonQuery();

                        string indexCmdText = $"update dbo.LJDealIndex set Scrapped = 1 where ID = {entity.ID}";
                        using (SqlCommand indexCommand = new SqlCommand(indexCmdText, connection))
                        {
                            indexCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }
    }
}
