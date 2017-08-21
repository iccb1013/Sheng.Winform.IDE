/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	[ManagementEntity]
	public class CustomHandlerSetting : ExceptionHandlerSetting
	{
		private string handlerType;
		private string[] attributes;
		public CustomHandlerSetting(ConfigurationElement sourceElement, string name, string filterType, string[] attributes)
			: base(sourceElement, name)
		{
			this.handlerType = filterType;
			this.attributes = attributes;
		}
		[ManagementConfiguration]
		public string HandlerType
		{
			get { return handlerType; }
			set { handlerType = value; }
		}
		[ManagementConfiguration]
		public string[] Attributes
		{
			get { return attributes; }
			set { attributes = value; }
		}
		[ManagementEnumerator]
		public static IEnumerable<CustomHandlerSetting> GetInstances()
		{
			return GetInstances<CustomHandlerSetting>();
		}
		[ManagementBind]
		public static CustomHandlerSetting BindInstance(string ApplicationName,
		                                                string SectionName,
		                                                string Policy,
		                                                string ExceptionType,
		                                                string Name)
		{
			return BindInstance<CustomHandlerSetting>(ApplicationName, SectionName, Policy, ExceptionType, Name);
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return CustomExceptionHandlerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
