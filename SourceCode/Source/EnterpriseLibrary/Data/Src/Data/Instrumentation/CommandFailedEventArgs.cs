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
	public class CommandFailedEventArgs : EventArgs
	{
		string commandText;
		string connectionString;
		Exception exception;
		public string CommandText
		{
			get { return commandText; }
		}
		public string ConnectionString
		{
			get { return connectionString; }
		}
		public Exception Exception
		{
			get { return exception; }
		}
		public CommandFailedEventArgs(string commandText, string connectionString, Exception exception)
		{
			this.commandText = commandText;
			this.connectionString = connectionString;
			this.exception = exception;
		}
	}
}
