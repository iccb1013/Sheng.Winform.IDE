/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlServerCe;
namespace Microsoft.Practices.EnterpriseLibrary.Data.SqlCe
{
    public class SqlCeConnectionPool
    {
        protected static Dictionary<string, DbConnection> connections = new Dictionary<string, DbConnection>();
        public static void CloseSharedConnection(Database database)
        {
            DbConnection connection;
            string connectionString = database.ConnectionStringWithoutCredentials;
            lock (connections)
            {
                if (connections.TryGetValue(connectionString, out connection))
                {
                    connection.Close();
                    connection.Dispose();
                    connections.Remove(connectionString);
                }
            }
        }
        public static void CloseSharedConnections()
        {
            lock (connections)
            {
                foreach (KeyValuePair<string, DbConnection> pair in connections)
                {
                    pair.Value.Close();
                    pair.Value.Dispose();
                }
                connections.Clear();
            }
        }
        public static DbConnection CreateConnection(SqlCeDatabase db)
        {
            string connectionString = db.ConnectionStringWithoutCredentials;
            if (!connections.ContainsKey(connectionString))
            {
                lock (connections)
                {
                    if (!connections.ContainsKey(connectionString))
                    {
                        DbConnection keepAliveConnection = new SqlCeConnection();
                        db.SetConnectionString(keepAliveConnection);
                        keepAliveConnection.Open();
                        connections.Add(connectionString, keepAliveConnection);
                    }
                }
            }
            return new SqlCeConnection();
        }
    }
}
