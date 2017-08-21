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
	public class CommandFailedEvent : DataEvent
	{
		private string connectionString;
		private string commandText;
		private string exceptionMessage;
		public CommandFailedEvent(string instanceName, string connectionString, string commandText, string exceptionMessage)
			: base(instanceName)
		{
			this.connectionString = connectionString;
			this.commandText = commandText;
			this.exceptionMessage = exceptionMessage;
		}
		public string CommandText
		{
			get { return commandText; }
		}
		public string ConnectionString
		{
			get { return connectionString; }
		}
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
	}
}
