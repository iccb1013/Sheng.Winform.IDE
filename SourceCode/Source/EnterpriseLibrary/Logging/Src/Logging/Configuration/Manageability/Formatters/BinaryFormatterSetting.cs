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
	public partial class BinaryFormatterSetting : FormatterSetting
	{
		public BinaryFormatterSetting(BinaryLogFormatterData sourceElement, string name)
			: base(sourceElement, name)
		{ }
		[ManagementEnumerator]
		public static IEnumerable<BinaryFormatterSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<BinaryFormatterSetting>();
		}
		[ManagementBind]
		public static BinaryFormatterSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<BinaryFormatterSetting>(ApplicationName, SectionName, Name);
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return false; 
		}
	}
}
