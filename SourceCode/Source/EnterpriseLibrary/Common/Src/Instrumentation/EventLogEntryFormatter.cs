/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	public class EventLogEntryFormatter : IEventLogEntryFormatter
	{
		private string applicationName;
		private string blockName;
		private static readonly string[] emptyExtraInformation = new string[0];
		public EventLogEntryFormatter(string blockName)
			: this(GetApplicationName(), blockName)
		{
		}
		public EventLogEntryFormatter(string applicationName, string blockName)
		{
			this.applicationName = applicationName;
			this.blockName = blockName;
		}
		public string GetEntryText(string message, params string[] extraInformation)
		{
			return BuildEntryText(message, null, extraInformation);
		}
		public string GetEntryText(string message, Exception exception, params string[] extraInformation)
		{
			return BuildEntryText(message, exception, extraInformation);
		}
		private string BuildEntryText(string message, Exception exception, string[] extraInformation)
		{
			StringBuilder entryTextBuilder
				= new StringBuilder(
					string.Format(
						Resources.Culture,
						Resources.EventLogEntryHeaderTemplate,
						applicationName,
						blockName));
			entryTextBuilder.AppendLine();
			entryTextBuilder.AppendLine(message);
			for (int i = 0; i < extraInformation.Length; i++ )
			{
				entryTextBuilder.AppendLine(extraInformation[i]);
			}
			if (exception != null)
			{
				entryTextBuilder.AppendLine(
				   string.Format(
					   Resources.Culture,
					   Resources.EventLogEntryExceptionTemplate,
					   exception.ToString()));
			}
			return entryTextBuilder.ToString();
		}
		private static string GetApplicationName()
		{
			return AppDomain.CurrentDomain.FriendlyName;
		}
		private string EntryTemplate
		{
			get
			{
				return "";
			}
		}
	}
}
