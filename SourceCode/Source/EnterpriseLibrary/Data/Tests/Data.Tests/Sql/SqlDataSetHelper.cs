/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    internal sealed class SqlDataSetHelper
    {
		public static void CreateDataAdapterCommandsDynamically(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
		{
			insertCommand = db.GetStoredProcCommandWithSourceColumns("RegionInsert", "RegionID", "RegionDescription");
			updateCommand = db.GetStoredProcCommandWithSourceColumns("RegionUpdate", "RegionID", "RegionDescription");
			deleteCommand = db.GetStoredProcCommandWithSourceColumns("RegionDelete", "RegionID");
		}
        public static void CreateDataAdapterCommands(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
        {
            insertCommand = db.GetStoredProcCommand("RegionInsert");
            updateCommand = db.GetStoredProcCommand("RegionUpdate");
            deleteCommand = db.GetStoredProcCommand("RegionDelete");
            db.AddInParameter(insertCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(insertCommand, "@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);
            db.AddInParameter(updateCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(updateCommand, "@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);
            db.AddInParameter(deleteCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
        }
        public static void CreateStoredProcedures(Database db)
        {
            string sql = "create procedure RegionSelect as " +
                "select * from Region Order by RegionId";
            DbCommand command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
            sql = "create procedure RegionInsert (@RegionID int, @RegionDescription varchar(100) ) as " +
                "insert into Region values(@RegionID, @RegionDescription)";
            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
            sql = "create procedure RegionUpdate (@RegionID int, @RegionDescription varchar(100) ) as " +
                "update Region set RegionDescription = @RegionDescription where RegionID = @RegionID";
            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
            sql = "create procedure RegionDelete (@RegionID int) as " +
                "delete from Region where RegionID = @RegionID";
            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
		}
        public static void DeleteStoredProcedures(Database db)
        {
            DbCommand command;
			string sql = "drop procedure RegionSelect; " +
				"drop procedure RegionInsert; " +
				"drop procedure RegionDelete; " +
				"drop procedure RegionUpdate; ";
            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
        }
        public static void AddTestData(Database db)
        {
            string sql =
                "insert into Region values (99, 'Midwest');" +
                    "insert into Region values (100, 'Central Europe');" +
                    "insert into Region values (101, 'Middle East');" +
                    "insert into Region values (102, 'Australia')";
            DbCommand testDataInsertion = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(testDataInsertion);
        }
    }
}
