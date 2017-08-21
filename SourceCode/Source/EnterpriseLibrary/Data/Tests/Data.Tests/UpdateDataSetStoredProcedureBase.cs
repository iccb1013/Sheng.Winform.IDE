/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class UpdateDataSetStoredProcedureBase
    {
        protected Database db;
        protected DataSet startingData;
        protected DbCommand insertCommand;
        protected DbCommand updateCommand;
        protected DbCommand deleteCommand;
        public void SetUp()
        {
            PrepareDatabaseSetup();
            AddTestData();
            startingData = GetDataSetFromTable();
            CreateDataAdapterCommands();
        }
        public void TearDown()
        {
            ResetDatabase();
        }
        protected abstract void ResetDatabase();
        protected abstract void AddTestData();
        protected abstract DataSet GetDataSetFromTable();
        protected abstract void CreateStoredProcedures();
        protected abstract void DeleteStoredProcedures();
		protected abstract void CreateDataAdapterCommands();
        protected abstract void PrepareDatabaseSetup();
        protected virtual DataRow AddRowsWithErrorsToDataTable(DataTable table)
        {
            AddRowsToDataTable(table);
            DataRow errRow = AddRow(table, 502, "Washington"); 
            AddRow(table, 504, "Canada");
            AddRow(table, 505, "Mexico");
            return errRow; 
        }
        protected virtual void AddRowsToDataTable(DataTable table)
        {
            AddRow(table, 500, "California");
            AddRow(table, 501, "Arizona");
            AddRow(table, 502, "Washington");
            AddRow(table, 503, "Texas");
        }
        private DataRow AddRow(DataTable table, int regionID, string description)
        {
            DataRow newRow = table.NewRow();
            newRow["RegionID"] = regionID;
            newRow["RegionDescription"] = description;
            table.Rows.Add(newRow);
            return newRow;
        }
    }
}
