/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Data;
using System.Data.Common;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    internal sealed class OracleDataSetHelper
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
            db.AddInParameter(insertCommand, "vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(insertCommand, "vRegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);
            db.AddInParameter(updateCommand, "vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            db.AddInParameter(updateCommand, "vRegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);
            db.AddInParameter(deleteCommand, "vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
        }
        public static void CreateStoredProcedures(Database db)
        {
            DbCommand command;
            string sql;
            sql = "create procedure RegionSelect (cur_OUT OUT PKGENTLIB_ARCHITECTURE.CURENTLIB_ARCHITECTURE) as " +
                "BEGIN OPEN cur_OUT FOR select * from Region Order By RegionId; END;";
            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
            sql = "create procedure RegionInsert (vRegionID IN Region.RegionID%TYPE, vRegionDescription IN Region.RegionDescription%TYPE) as " +
                "BEGIN insert into Region values(vRegionID, vRegionDescription); END;";
            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
            sql = "create procedure RegionUpdate (vRegionID IN Region.RegionID%TYPE, vRegionDescription IN Region.RegionDescription%TYPE) as " +
                "BEGIN update Region set RegionDescription = vRegionDescription where RegionID = vRegionID; END;";
            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
            sql = "create procedure RegionDelete (vRegionID IN Region.RegionID%TYPE) as " +
                "BEGIN delete from Region where RegionID = vRegionID; END;";
            command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
		}
        public static void DeleteStoredProcedures(Database db)
        {
            DbCommand command;
            string sql = "drop procedure RegionSelect";
            command = db.GetSqlStringCommand(sql);
			try { db.ExecuteNonQuery(command); }
			catch { }
            sql = "drop procedure RegionInsert";
            command = db.GetSqlStringCommand(sql);
			try { db.ExecuteNonQuery(command); }
			catch { }
            sql = "drop procedure RegionDelete";
            command = db.GetSqlStringCommand(sql);
			try { db.ExecuteNonQuery(command); }
			catch { }
            sql = "drop procedure RegionUpdate";
            command = db.GetSqlStringCommand(sql);
			try { db.ExecuteNonQuery(command); }
			catch { }
		}
        public static void AddTestData(Database db)
        {
            string sql =
                "BEGIN " +
                    "insert into Region values (99, 'Midwest');" +
                    "insert into Region values (100, 'Central Europe');" +
                    "insert into Region values (101, 'Middle East');" +
                    "insert into Region values (102, 'Australia');" +
                    "END;";
            DbCommand testDataInsertion = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(testDataInsertion);
        }
    }
}
