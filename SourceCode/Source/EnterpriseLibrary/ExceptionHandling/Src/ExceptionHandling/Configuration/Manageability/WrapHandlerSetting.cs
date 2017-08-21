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
	public class WrapHandlerSetting : ExceptionHandlerSetting
	{
		private string exceptionMessage;
		private string wrapExceptionType;
		public WrapHandlerSetting(ConfigurationElement sourceElement, string name, string exceptionMessage,
		                          string wrapExceptionType)
			: base(sourceElement, name)
		{
			this.exceptionMessage = exceptionMessage;
			this.wrapExceptionType = wrapExceptionType;
		}
		[ManagementConfiguration]
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
			set { exceptionMessage = value; }
		}
		[ManagementConfiguration]
		public string WrapExceptionType
		{
			get { return wrapExceptionType; }
			set { wrapExceptionType = value; }
		}
		[ManagementEnumerator]
		public static IEnumerable<WrapHandlerSetting> GetInstances()
		{
			return GetInstances<WrapHandlerSetting>();
		}
		[ManagementBind]
		public static WrapHandlerSetting BindInstance(string ApplicationName,
		                                              string SectionName,
		                                              string Policy,
		                                              string ExceptionType,
		                                              string Name)
		{
			return BindInstance<WrapHandlerSetting>(ApplicationName, SectionName, Policy, ExceptionType, Name);
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return WrapHandlerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
