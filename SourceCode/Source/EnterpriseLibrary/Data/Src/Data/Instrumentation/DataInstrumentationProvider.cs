/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	[InstrumentationListener(typeof(DataInstrumentationListener), typeof(DataInstrumentationListenerBinder))]
	public class DataInstrumentationProvider
	{
		[InstrumentationProvider("ConnectionOpened")]
		public event EventHandler<EventArgs> connectionOpened;
		[InstrumentationProvider("ConnectionFailed")]
		public event EventHandler<ConnectionFailedEventArgs> connectionFailed;
		[InstrumentationProvider("CommandExecuted")]
		public event EventHandler<CommandExecutedEventArgs> commandExecuted;
		[InstrumentationProvider("CommandFailed")]
		public event EventHandler<CommandFailedEventArgs> commandFailed;
		public void FireCommandExecutedEvent(DateTime startTime)
		{
			if (commandExecuted != null) commandExecuted(this, new CommandExecutedEventArgs(startTime));
		}
		public void FireCommandFailedEvent(string commandText, string connectionString, Exception exception)
		{
			if (commandFailed != null) commandFailed(this, new CommandFailedEventArgs(commandText, connectionString, exception));
		}
		public void FireConnectionOpenedEvent()
		{
			if (connectionOpened != null) connectionOpened(this, new EventArgs());
		}
		public void FireConnectionFailedEvent(string connectionString, Exception exception)
		{
			if (connectionFailed != null) connectionFailed(this, new ConnectionFailedEventArgs(connectionString, exception));
		}
	}
}
