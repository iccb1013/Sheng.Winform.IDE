/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	[ManagementEntity]
	public class CustomTraceListenerSetting : TraceListenerSetting
	{
		string[] attributes;
		string formatter;
		string initData;
		string listenerType;
		public CustomTraceListenerSetting(BasicCustomTraceListenerData sourceElement,
										  string name,
										  string listenerType,
										  string initData,
										  string[] attributes,
										  string traceOutputOptions,
										  string filter,
										  string formatter)
			: base(sourceElement, name, traceOutputOptions, filter)
		{
			this.listenerType = listenerType;
			this.initData = initData;
			this.attributes = attributes;
			this.formatter = formatter;
		}
		[ManagementConfiguration]
		public string[] Attributes
		{
			get { return attributes; }
			set { attributes = value; }
		}
		[ManagementConfiguration]
		public string Formatter
		{
			get { return formatter; }
			set { formatter = value; }
		}
		[ManagementConfiguration]
		public string InitData
		{
			get { return initData; }
			set { initData = value; }
		}
		[ManagementConfiguration]
		public string ListenerType
		{
			get { return listenerType; }
			set { listenerType = value; }
		}
		[ManagementBind]
		public static CustomTraceListenerSetting BindInstance(string ApplicationName,
															  string SectionName,
															  string Name)
		{
			return BindInstance<CustomTraceListenerSetting>(ApplicationName, SectionName, Name);
		}
		[ManagementEnumerator]
		public static IEnumerable<CustomTraceListenerSetting> GetInstances()
		{
			return GetInstances<CustomTraceListenerSetting>();
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return CustomTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
