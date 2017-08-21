/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Transactions;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    public static class TransactionScopeConnections
    {
        static readonly Dictionary<Transaction, Dictionary<string, DbConnection>> transactionConnections =
            new Dictionary<Transaction, Dictionary<string, DbConnection>>();
        public static DbConnection GetConnection(Database db)
        {
            Transaction currentTransaction = Transaction.Current;
            if (currentTransaction == null)
                return null;
            Dictionary<string, DbConnection> connectionList;
            DbConnection connection;
            lock (transactionConnections)
            {
                if (!transactionConnections.TryGetValue(currentTransaction, out connectionList))
                {
                    connectionList = new Dictionary<string, DbConnection>();
                    transactionConnections.Add(currentTransaction, connectionList);
                    currentTransaction.TransactionCompleted += OnTransactionCompleted;
                }
            }
            lock (connectionList)
            {
                if (!connectionList.TryGetValue(db.ConnectionString, out connection))
                {
                    connection = db.GetNewOpenConnection();
                    connectionList.Add(db.ConnectionString, connection);
                }
            }
            return connection;
        }
        static void OnTransactionCompleted(object sender, TransactionEventArgs e)
        {
            Dictionary<string, DbConnection> connectionList;
            lock (transactionConnections)
            {
                if (!transactionConnections.TryGetValue(e.Transaction, out connectionList))
                {
                    return;
                }
                transactionConnections.Remove(e.Transaction);
            }
            lock (connectionList)
            {
                foreach (DbConnection connection in connectionList.Values)
                {
                    connection.Dispose();
                }
            }
        }
    }
}
