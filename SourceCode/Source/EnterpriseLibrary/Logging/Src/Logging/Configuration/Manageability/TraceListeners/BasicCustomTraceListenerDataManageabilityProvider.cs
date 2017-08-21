/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	public abstract class BasicCustomTraceListenerDataManageabilityProvider<T>
		: CustomProviderDataManageabilityProvider<T>
		where T : BasicCustomTraceListenerData
	{
		public const String InitDataPropertyName = "initData";
		public const String TraceOutputOptionsPropertyName = "traceOutputOptions";
		public const String FilterPropertyName = "filter";
		public new const String AttributesPropertyName =
			CustomProviderDataManageabilityProvider<CustomTraceListenerData>.AttributesPropertyName;
		public new const String ProviderTypePropertyName =
			CustomProviderDataManageabilityProvider<CustomTraceListenerData>.ProviderTypePropertyName;
		protected BasicCustomTraceListenerDataManageabilityProvider()
			: base(Resources.TraceListenerPolicyNameTemplate)
		{ }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder, T configurationObject, IConfigurationSource configurationSource, string elementPolicyKeyName)
		{
			base.AddElementAdministrativeTemplateParts(contentBuilder,
				configurationObject,
				configurationSource,
				elementPolicyKeyName);
			contentBuilder.AddEditTextPart(Resources.CustomTraceListenerInitializationPartName,
				InitDataPropertyName,
				configurationObject.InitData,
				1024,
				false);
			contentBuilder.AddDropDownListPartForEnumeration<TraceOptions>(Resources.TraceListenerTraceOptionsPartName,
				TraceOutputOptionsPropertyName,
				configurationObject.TraceOutputOptions);
			contentBuilder.AddDropDownListPartForEnumeration<SourceLevels>(Resources.TraceListenerFilterPartName,
				FilterPropertyName,
				configurationObject.Filter);
		}
        protected override void OverrideWithGroupPolicies(T configurationObject, IRegistryKey policyKey)
		{
			String initDataOverride = policyKey.GetStringValue(InitDataPropertyName);
			TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);
			base.OverrideWithGroupPolicies(configurationObject, policyKey);
			configurationObject.InitData = initDataOverride;
			configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.Filter = filterOverride.Value;
		}
	}
}
