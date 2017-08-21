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
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public sealed class EventLogDefinitionAttribute : Attribute
	{
		string logName;
		string sourceName;
		int categoryCount;
		string categoryResourceFile;
		string messageResourceFile;
		string parameterResourceFile;
		public string LogName
		{
			get { return logName; }
		}
		public string SourceName
		{
			get { return sourceName; }
		}
		public int CategoryCount
		{
			get { return categoryCount; }
			set { categoryCount = value; }
		}
		public string CategoryResourceFile
		{
			get { return categoryResourceFile; }
			set { categoryResourceFile = value; }
		}
		public string MessageResourceFile
		{
			get { return messageResourceFile; }
			set { messageResourceFile = value; }
		}
		public string ParameterResourceFile
		{
			get { return parameterResourceFile; }
			set { parameterResourceFile = value; }
		}
		public EventLogDefinitionAttribute(string logName, string sourceName)
		{
			this.logName = logName;
			this.sourceName = sourceName;
		}
	}
}
