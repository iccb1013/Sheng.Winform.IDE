/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Manageability
{
	[ManagementEntity]
	public class LoggingExceptionHandlerSetting : ExceptionHandlerSetting
	{
		private int eventId;
		private string formatterType;
		private string logCategory;
		private int priority;
		private string severity;
		private string title;
		public LoggingExceptionHandlerSetting(ConfigurationElement sourceElement,
		                                      string name,
		                                      int eventId,
		                                      string formatterType,
		                                      string logCategory,
		                                      int priority,
		                                      string severity,
		                                      string title)
			: base(sourceElement, name)
		{
			this.eventId = eventId;
			this.formatterType = formatterType;
			this.logCategory = logCategory;
			this.priority = priority;
			this.severity = severity;
			this.title = title;
		}
		[ManagementConfiguration]
		public int EventId
		{
			get { return eventId; }
			set { eventId = value; }
		}
		[ManagementConfiguration]
		public string FormatterType
		{
			get { return formatterType; }
			set { formatterType = value; }
		}
		[ManagementConfiguration]
		public string LogCategory
		{
			get { return logCategory; }
			set { logCategory = value; }
		}
		[ManagementConfiguration]
		public int Priority
		{
			get { return priority; }
			set { priority = value; }
		}
		[ManagementConfiguration]
		public string Severity
		{
			get { return severity; }
			set { severity = value; }
		}
		[ManagementConfiguration]
		public string Title
		{
			get { return title; }
			set { title = value; }
		}
		[ManagementEnumerator]
		public static IEnumerable<LoggingExceptionHandlerSetting> GetInstances()
		{
			return GetInstances<LoggingExceptionHandlerSetting>();
		}
		[ManagementBind]
		public static LoggingExceptionHandlerSetting BindInstance(string ApplicationName,
		                                                          string SectionName,
		                                                          string Policy,
		                                                          string ExceptionType,
		                                                          string Name)
		{
			return BindInstance<LoggingExceptionHandlerSetting>(ApplicationName, SectionName, Policy, ExceptionType, Name);
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return LoggingExceptionHandlerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
