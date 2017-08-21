/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	public class FlatFileTraceListenerDataManageabilityProvider
		: TraceListenerDataManageabilityProvider<FlatFileTraceListenerData>
	{
		public const String FileNamePropertyName = "fileName";
		public const String FooterPropertyName = "footer";
		public const String HeaderPropertyName = "header";
		public FlatFileTraceListenerDataManageabilityProvider()
		{
			FlatFileTraceListenerDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			FlatFileTraceListenerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddEditTextPart(Resources.FlatFileTraceListenerFileNamePartName,
				FileNamePropertyName,
				configurationObject.FileName,
				255,
				true);
			contentBuilder.AddEditTextPart(Resources.FlatFileTraceListenerHeaderPartName,
				HeaderPropertyName,
				configurationObject.Header,
				512,
				false);
			contentBuilder.AddEditTextPart(Resources.FlatFileTraceListenerFooterPartName,
				FooterPropertyName,
				configurationObject.Footer,
				512,
				false);
			AddTraceOptionsPart(contentBuilder, configurationObject.TraceOutputOptions);
			AddFilterPart(contentBuilder, configurationObject.Filter);
			AddFormattersPart(contentBuilder, configurationObject.Formatter, configurationSource);
		}
        protected override void OverrideWithGroupPolicies(FlatFileTraceListenerData configurationObject, IRegistryKey policyKey)
		{
			String fileNameOverride = policyKey.GetStringValue(FileNamePropertyName);
			String footerOverride = policyKey.GetStringValue(FooterPropertyName);
			String formatterOverride = GetFormatterPolicyOverride(policyKey);
			String headerOverride = policyKey.GetStringValue(HeaderPropertyName);
			TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);
			configurationObject.FileName = fileNameOverride;
			configurationObject.Footer = footerOverride;
			configurationObject.Formatter = formatterOverride;
			configurationObject.Header = headerOverride;
			configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.Filter = filterOverride.Value;
		}
        protected override void GenerateWmiObjects(FlatFileTraceListenerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			FlatFileTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
