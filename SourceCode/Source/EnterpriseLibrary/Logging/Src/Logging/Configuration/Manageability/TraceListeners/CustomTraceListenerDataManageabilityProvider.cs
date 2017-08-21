/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	public class CustomTraceListenerDataManageabilityProvider
		: BasicCustomTraceListenerDataManageabilityProvider<CustomTraceListenerData>
	{
		public const String FormatterPropertyName = "formatter";
		public CustomTraceListenerDataManageabilityProvider()
		{
			CustomTraceListenerDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			CustomTraceListenerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			base.AddElementAdministrativeTemplateParts(contentBuilder,
				configurationObject,
				configurationSource,
				elementPolicyKeyName);
			LoggingSettings configurationSection = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);
			contentBuilder.AddDropDownListPartForNamedElementCollection<FormatterData>(Resources.TraceListenerFormatterPartName,
				FormatterPropertyName,
				configurationSection.Formatters,
				configurationObject.Formatter,
				true);
		}
        protected override void OverrideWithGroupPolicies(CustomTraceListenerData configurationObject, IRegistryKey policyKey)
		{
			String formatterOverride = policyKey.GetStringValue(FormatterPropertyName);
			base.OverrideWithGroupPolicies(configurationObject, policyKey);
			configurationObject.Formatter = AdmContentBuilder.NoneListItem.Equals(formatterOverride) ? String.Empty : formatterOverride;
		}
        protected override void GenerateWmiObjects(CustomTraceListenerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			CustomTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
