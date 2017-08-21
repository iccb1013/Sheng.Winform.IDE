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
	public class ReplaceHandlerSetting : ExceptionHandlerSetting
	{
		private string exceptionMessage;
		private string replaceExceptionType;
		public ReplaceHandlerSetting(ConfigurationElement sourceElement, string name, string exceptionMessage,
		                               string replaceExceptionType)
			: base(sourceElement, name)
		{
			this.exceptionMessage = exceptionMessage;
			this.replaceExceptionType = replaceExceptionType;
		}
		[ManagementConfiguration]
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
			set { exceptionMessage = value; }
		}
		[ManagementConfiguration]
		public string ReplaceExceptionType
		{
			get { return replaceExceptionType; }
			set { replaceExceptionType = value; }
		}
		[ManagementEnumerator]
		public static IEnumerable<ReplaceHandlerSetting> GetInstances()
		{
			return GetInstances<ReplaceHandlerSetting>();
		}
		[ManagementBind]
		public static ReplaceHandlerSetting BindInstance(string ApplicationName,
		                                                 string SectionName,
		                                                 string Policy,
		                                                 string ExceptionType,
		                                                 string Name)
		{
			return BindInstance<ReplaceHandlerSetting>(ApplicationName, SectionName, Policy, ExceptionType, Name);
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return ReplaceHandlerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
