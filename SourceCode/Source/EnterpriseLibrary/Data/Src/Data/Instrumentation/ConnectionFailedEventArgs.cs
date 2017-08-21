/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    public class ConnectionFailedEventArgs : EventArgs
    {
        string connectionString;
		Exception exception;
        public string ConnectionString
        {
            get { return connectionString; }
        }
		public Exception Exception
		{
			get { return exception; }
		}
		public ConnectionFailedEventArgs(string connectionString, Exception exception)
        {
            this.connectionString = connectionString;
			this.exception = exception;
        }
    }
}
