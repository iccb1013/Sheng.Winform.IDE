/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
	public class CustomFormatterDataManageabilityProvider
		: CustomProviderDataManageabilityProvider<CustomFormatterData>
	{
		public new const String AttributesPropertyName = CustomProviderDataManageabilityProvider<CustomFormatterData>.AttributesPropertyName;
		public CustomFormatterDataManageabilityProvider()
			: base(Resources.FormatterPolicyNameTemplate)
		{
			CustomFormatterDataWmiMapper.RegisterWmiTypes();
		}
        protected override void GenerateWmiObjects(CustomFormatterData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			CustomFormatterDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
