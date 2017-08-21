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
	public class EmailTraceListenerDataManageabilityProvider
		: TraceListenerDataManageabilityProvider<EmailTraceListenerData>
	{
		public const String FromAddressPropertyName = "fromAddress";
		public const String SmtpPortPropertyName = "smtpPort";
		public const String SmtpServerPropertyName = "smtpServer";
		public const String SubjectLineEnderPropertyName = "subjectLineEnder";
		public const String SubjectLineStarterPropertyName = "subjectLineStarter";
		public const String ToAddressPropertyName = "toAddress";
		public EmailTraceListenerDataManageabilityProvider()
		{
			EmailTraceListenerDataWmiMapper.RegisterWmiTypes();
		}
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			EmailTraceListenerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddEditTextPart(Resources.EmailTraceListenerFromAddressPartName,
				FromAddressPropertyName,
				configurationObject.FromAddress,
				255,
				true);
			contentBuilder.AddEditTextPart(Resources.EmailTraceListenerToAddressPartName,
				ToAddressPropertyName,
				configurationObject.ToAddress,
				255,
				false);
			contentBuilder.AddNumericPart(Resources.EmailTraceListenerSmtpPortPartName,
				SmtpPortPropertyName,
				configurationObject.SmtpPort);
			contentBuilder.AddEditTextPart(Resources.EmailTraceListenerSmtpServerPartName,
				SmtpServerPropertyName,
				configurationObject.SmtpServer,
				255,
				true);
			contentBuilder.AddEditTextPart(Resources.EmailTraceListenerStarterPartName,
				SubjectLineStarterPropertyName,
				configurationObject.SubjectLineStarter,
				255,
				false);
			contentBuilder.AddEditTextPart(Resources.EmailTraceListenerEnderPartName,
				SubjectLineEnderPropertyName,
				configurationObject.SubjectLineEnder,
				255,
				false);
			AddTraceOptionsPart(contentBuilder, configurationObject.TraceOutputOptions);
			AddFilterPart(contentBuilder, configurationObject.Filter);
			AddFormattersPart(contentBuilder, configurationObject.Formatter, configurationSource);
		}
        protected override void OverrideWithGroupPolicies(EmailTraceListenerData configurationObject, IRegistryKey policyKey)
		{
			String formatterOverride = GetFormatterPolicyOverride(policyKey);
			String fromAddressOverride = policyKey.GetStringValue(FromAddressPropertyName);
			int? smtpPortOverride = policyKey.GetIntValue(SmtpPortPropertyName);
			String smtpServerOverride = policyKey.GetStringValue(SmtpServerPropertyName);
			String subjectLineEnderOverride = policyKey.GetStringValue(SubjectLineEnderPropertyName);
			String subjectLineStarterOverride = policyKey.GetStringValue(SubjectLineStarterPropertyName);
			String toAddressOverride = policyKey.GetStringValue(ToAddressPropertyName);
			TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);
			configurationObject.Formatter = formatterOverride;
			configurationObject.FromAddress = fromAddressOverride;
			configurationObject.SmtpPort = smtpPortOverride.Value;
			configurationObject.SmtpServer = smtpServerOverride;
			configurationObject.SubjectLineEnder = subjectLineEnderOverride;
			configurationObject.SubjectLineStarter = subjectLineStarterOverride;
			configurationObject.ToAddress = toAddressOverride;
			configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.Filter = filterOverride.Value;
		}
        protected override void GenerateWmiObjects(EmailTraceListenerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			EmailTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
