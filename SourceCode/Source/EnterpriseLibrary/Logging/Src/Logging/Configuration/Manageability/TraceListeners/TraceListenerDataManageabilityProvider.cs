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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	public abstract class TraceListenerDataManageabilityProvider<T>
		: ConfigurationElementManageabilityProviderBase<T>
		where T : TraceListenerData
	{
		public const String FormatterPropertyName = "formatter";
		public const String TraceOutputOptionsPropertyName = "traceOutputOptions";
		public const String FilterPropertyName = "filter";
		protected internal static void AddTraceOptionsPart(AdmContentBuilder contentBuilder,
			TraceOptions traceOutputOptions)
		{
			contentBuilder.AddDropDownListPartForEnumeration<TraceOptions>(Resources.TraceListenerTraceOptionsPartName,
				TraceOutputOptionsPropertyName,
				traceOutputOptions);
		}
		protected internal static void AddFilterPart(AdmContentBuilder contentBuilder,
			SourceLevels filter)
		{
			contentBuilder.AddDropDownListPartForEnumeration<SourceLevels>(Resources.TraceListenerFilterPartName,
				FilterPropertyName,
				filter);
		}
		protected internal static void AddFormattersPart(AdmContentBuilder contentBuilder,
			String formatterName,
			IConfigurationSource configurationSource)
		{
			LoggingSettings configurationSection = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);
			contentBuilder.AddDropDownListPartForNamedElementCollection<FormatterData>(Resources.TraceListenerFormatterPartName,
				FormatterPropertyName,
				configurationSection.Formatters,
				formatterName,
				true);
		}
		protected sealed override string ElementPolicyNameTemplate
		{
			get { return Resources.TraceListenerPolicyNameTemplate; }
		}
		protected internal static String GetFormatterPolicyOverride(IRegistryKey policyKey)
		{
			String overrideValue = policyKey.GetStringValue(FormatterPropertyName);
			return AdmContentBuilder.NoneListItem.Equals(overrideValue) ? String.Empty : overrideValue;
		}
	}
}
