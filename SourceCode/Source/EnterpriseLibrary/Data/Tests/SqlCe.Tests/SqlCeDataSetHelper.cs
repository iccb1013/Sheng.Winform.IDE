/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.Tests.VSTS
{
	class SqlCeDataSetHelper
	{
		public static void CreateDataAdapterCommandsDynamically(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
		{
			CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
		}
		public static void CreateDataAdapterCommands(Database db, ref DbCommand insertCommand, ref DbCommand updateCommand, ref DbCommand deleteCommand)
		{
			insertCommand = db.GetSqlStringCommand("INSERT INTO Region VALUES(@RegionID, @RegionDescription)");
			updateCommand = db.GetSqlStringCommand("UPDATE region SET RegionDescription=@RegionDescription WHERE RegionID=@RegionId");
			deleteCommand = db.GetSqlStringCommand("DELETE FROM Region WHERE RegionID=@RegionID");
			db.AddInParameter(insertCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
			db.AddInParameter(insertCommand, "@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);
			db.AddInParameter(updateCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
			db.AddInParameter(updateCommand, "@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);
			db.AddInParameter(deleteCommand, "@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
		}
		public static void AddTestData(Database database)
		{
			SqlCeDatabase db = (SqlCeDatabase)database;
			db.ExecuteNonQuerySql("insert into Region values (99, 'Midwest');");
			db.ExecuteNonQuerySql("insert into Region values (100, 'Central Europe');");
			db.ExecuteNonQuerySql("insert into Region values (101, 'Middle East');");
			db.ExecuteNonQuerySql("insert into Region values (102, 'Australia')");
		}
	}
}
