/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	[HasInstallableResources()]
	[EventLogDefinition("Application", EventLogSourceName)]
	public static class ManageabilityExtensionsLogger
	{
		internal const String EventLogSourceName = "Enterprise Library Manageability Extensions";
		public static void LogExceptionWhileOverriding(Exception exception)
		{
			LogException(exception, Resources.ExceptionErrorWhileOverriding);
		}
		public static void LogException(Exception exception, String title)
		{
			StringBuilder entryTextBuilder = new StringBuilder();
			entryTextBuilder.AppendLine(title);
			entryTextBuilder.Append(exception.Message);
			try
			{
				EventLog.WriteEntry(EventLogSourceName, entryTextBuilder.ToString(), EventLogEntryType.Error);
			}
			catch
			{
			}
		}
	}
}
