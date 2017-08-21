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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
	[ManagementEntity]
	public partial class CustomFormatterSetting : FormatterSetting
	{
		private string formatterType;
		private string[] attributes;
		public CustomFormatterSetting(CustomFormatterData sourceElement, string name, string formatterType, string[] attributes)
			: base(sourceElement, name)
		{
			this.formatterType = formatterType;
			this.attributes = attributes;
		}
		[ManagementConfiguration]
		public string FormatterType
		{
			get { return formatterType; }
			set { formatterType = value; }
		}
		[ManagementConfiguration]
		public string[] Attributes
		{
			get { return attributes; }
			set { attributes = value; }
		}
		[ManagementEnumerator]
		public static IEnumerable<CustomFormatterSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<CustomFormatterSetting>();
		}
		[ManagementBind]
		public static CustomFormatterSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<CustomFormatterSetting>(ApplicationName, SectionName, Name);
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return CustomFormatterDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
