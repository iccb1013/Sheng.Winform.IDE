/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class UpdateDataSetWithTransactionsAndParameterDiscovery : UpdateDataSetStoredProcedureBase
    {
        protected DbTransaction transaction;
        protected override DataSet GetDataSetFromTable()
        {
            DbCommand selectCommand = db.GetStoredProcCommand("RegionSelect");
            return db.ExecuteDataSet(selectCommand, transaction);
        }
        public void ModifyRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";
            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, transaction);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }
        protected override void PrepareDatabaseSetup()
        {
            DbConnection connection = db.CreateConnection();
            connection.Open();
            transaction = connection.BeginTransaction();
            Database.ClearParameterCache();
        }
        protected override void ResetDatabase()
        {
            transaction.Rollback();
            RestoreRegionTable();
        }
        void RestoreRegionTable()
        {
            string sql = "delete from Region where RegionID >= 99";
            DbCommand cleanupCommand = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(cleanupCommand);
        }
    }
}
