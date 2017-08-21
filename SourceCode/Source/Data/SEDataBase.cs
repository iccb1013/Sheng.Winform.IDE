/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace Sheng.SailingEase.Data
{
    public class SEDataBase
    {
        private Database _database;
        public SEDataBase()
        {
            _database = DatabaseFactory.CreateDatabase();
        }
        public SEDataBase(string name)
        {
            _database = DatabaseFactory.CreateDatabase(name);
        }
        public DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(CommandType.Text, commandText, null);
        }
        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            return ExecuteDataSet(commandType, commandText, null);
        }
        public DataSet ExecuteDataSet(CommandType commandType, string commandText, SEDbCommandParameterCollection parameters)
        {
            DbCommand dbCmd;
            switch (commandType)
            {
                case CommandType.Text:
                    dbCmd = _database.GetSqlStringCommand(commandText);
                    break;
                case CommandType.StoredProcedure:
                    dbCmd = _database.GetStoredProcCommand(commandText);
                    break;
                default:
                    dbCmd = null;
                    break;
            }
            if (parameters != null)
            {
                foreach (SEDbCommandParameter parameter in parameters)
                {
                    SqlParameter par = new SqlParameter();
                    par.ParameterName = parameter.Name;
                    par.Value = parameter.Value;
                    dbCmd.Parameters.Add(par);
                }
            }
            return _database.ExecuteDataSet(dbCmd);
        }
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(CommandType.Text, commandText, null);
        }
        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            return ExecuteScalar(commandType, commandText, null);
        }
        public object ExecuteScalar(CommandType commandType, string commandText, SEDbCommandParameterCollection parameters)
        {
            DbCommand dbCmd;
            switch (commandType)
            {
                case CommandType.Text:
                    dbCmd = _database.GetSqlStringCommand(commandText);
                    break;
                case CommandType.StoredProcedure:
                    dbCmd = _database.GetStoredProcCommand(commandText);
                    break;
                default:
                    dbCmd = null;
                    break;
            }
            if (parameters != null)
            {
                foreach (SEDbCommandParameter parameter in parameters)
                {
                    SqlParameter par = new SqlParameter();
                    par.ParameterName = parameter.Name;
                    par.Value = parameter.Value;
                    dbCmd.Parameters.Add(par);
                }
            }
            return _database.ExecuteScalar(dbCmd);
        }
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(CommandType.Text, commandText, null);
        }
        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(commandType, commandText, null);
        }
        public int ExecuteNonQuery(CommandType commandType, string commandText, SEDbCommandParameterCollection parameters)
        {
            DbCommand dbCmd;
            switch (commandType)
            {
                case CommandType.Text:
                    dbCmd = _database.GetSqlStringCommand(commandText);
                    break;
                case CommandType.StoredProcedure:
                    dbCmd = _database.GetStoredProcCommand(commandText);
                    break;
                default:
                    dbCmd = null;
                    break;
            }
            if (parameters != null)
            {
                foreach (SEDbCommandParameter parameter in parameters)
                {
                    SqlParameter par = new SqlParameter();
                    par.ParameterName = parameter.Name;
                    par.Value = parameter.Value;
                    dbCmd.Parameters.Add(par);
                }
            }
            return _database.ExecuteNonQuery(dbCmd);
        }
    }
}
