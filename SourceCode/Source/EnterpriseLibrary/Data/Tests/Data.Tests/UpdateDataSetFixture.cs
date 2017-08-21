/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class UpdateDataSetFixture : UpdateDataSetStoredProcedureBase
    {
        public void DeleteRowWithMissingInsertAndUpdateCommands()
        {
            startingData.Tables[0].Rows[5].Delete();
            db.UpdateDataSet(startingData, "Table", null, null, deleteCommand,
                             UpdateBehavior.Standard);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.AreEqual(7, resultTable.Rows.Count);
        }
        public void DeleteRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[5].Delete();
            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.AreEqual(7, resultTable.Rows.Count);
        }
        protected override DataSet GetDataSetFromTable()
        {
            DbCommand selectCommand = db.GetStoredProcCommand("RegionSelect");
            return db.ExecuteDataSet(selectCommand);
        }
        public void InsertRowWithMissingUpdateAndDeleteCommands()
        {
            DataRow newRow = startingData.Tables[0].NewRow();
            newRow["RegionID"] = 1000;
            newRow["RegionDescription"] = "Moon Base Alpha";
            startingData.Tables[0].Rows.Add(newRow);
            db.UpdateDataSet(startingData, "Table", insertCommand, null, null,
                             UpdateBehavior.Standard);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.AreEqual(9, resultTable.Rows.Count);
            Assert.AreEqual(1000, Convert.ToInt32(resultTable.Rows[8]["RegionID"]));
            Assert.AreEqual("Moon Base Alpha", resultTable.Rows[8]["RegionDescription"].ToString().Trim());
        }
        public void InsertRowWithStoredProcedure()
        {
            DataRow newRow = startingData.Tables[0].NewRow();
            newRow["RegionID"] = 1000;
            newRow["RegionDescription"] = "Moon Base Alpha";
            startingData.Tables[0].Rows.Add(newRow);
            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.AreEqual(9, resultTable.Rows.Count);
            int result = Convert.ToInt32(resultTable.Rows[8]["RegionID"]);
            Assert.AreEqual(1000, result);
            Assert.AreEqual("Moon Base Alpha", resultTable.Rows[8]["RegionDescription"].ToString().Trim());
        }
        public void ModifyRowsWithStoredProcedureAndBatchUpdate()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";
            startingData.Tables[0].Rows[5]["RegionDescription"] = "Australia";
            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue, 5);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
            Assert.AreEqual("Australia", resultTable.Rows[5]["RegionDescription"].ToString().Trim());
        }
        public void ModifyRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";
            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }
        protected override void PrepareDatabaseSetup() {}
        protected override void ResetDatabase()
        {
            RestoreRegionTable();
        }
        void RestoreRegionTable()
        {
            string sql = "delete from Region where RegionID >= 99";
            DbCommand cleanupCommand = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(cleanupCommand);
        }
        public void UpdateDataSetWithAllCommandsMissing()
        {
            DataRow newRow = startingData.Tables[0].NewRow();
            newRow["RegionID"] = 1000;
            newRow["RegionDescription"] = "Moon Base Alpha";
            startingData.Tables[0].Rows.Add(newRow);
            db.UpdateDataSet(startingData, "Table", null, null, null,
                             UpdateBehavior.Standard);
        }
        public void UpdateDataSetWithNullTable()
        {
            db.UpdateDataSet(null, null, null, null, null, UpdateBehavior.Standard);
        }
        public void UpdateRowWithMissingInsertAndDeleteCommands()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";
            db.UpdateDataSet(startingData, "Table", null, updateCommand, null,
                             UpdateBehavior.Standard);
            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];
            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }
        public void UpdateSetWithNullDataSet()
        {
            db.UpdateDataSet(null, "Table", insertCommand, null, null,
                             UpdateBehavior.Standard);
        }
    }
}
