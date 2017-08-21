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
    public class FormattedEventLogTraceListenerDataManageabilityProvider
        : TraceListenerDataManageabilityProvider<FormattedEventLogTraceListenerData>
    {
        public const String LogPropertyName = "log";
        public const String MachineNamePropertyName = "machineName";
        public const String SourcePropertyName = "source";
        public FormattedEventLogTraceListenerDataManageabilityProvider()
        {
            FormattedEventLogTraceListenerDataWmiMapper.RegisterWmiTypes();
        }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      FormattedEventLogTraceListenerData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.EventLogTraceListenerSourcePartName,
                                           SourcePropertyName,
                                           configurationObject.Source,
                                           255,
                                           true);
            contentBuilder.AddEditTextPart(Resources.EventLogTraceListenerLogPartName,
                                           LogPropertyName,
                                           configurationObject.Log,
                                           255,
                                           false);
            contentBuilder.AddEditTextPart(Resources.EventLogTraceListenerMachineNamePartName,
                                           MachineNamePropertyName,
                                           configurationObject.MachineName,
                                           255,
                                           false);
            AddTraceOptionsPart(contentBuilder, configurationObject.TraceOutputOptions);
			AddFilterPart(contentBuilder, configurationObject.Filter);
            AddFormattersPart(contentBuilder, configurationObject.Formatter, configurationSource);
        }
        protected override void GenerateWmiObjects(FormattedEventLogTraceListenerData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            FormattedEventLogTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
        protected override void OverrideWithGroupPolicies(FormattedEventLogTraceListenerData configurationObject,
                                                          IRegistryKey policyKey)
        {
            String formatterOverride = GetFormatterPolicyOverride(policyKey);
            String logOverride = policyKey.GetStringValue(LogPropertyName);
            String machineNameOverride = policyKey.GetStringValue(MachineNamePropertyName);
            String sourceOverride = policyKey.GetStringValue(SourcePropertyName);
            TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);
            configurationObject.Formatter = formatterOverride;
            configurationObject.Log = logOverride;
            configurationObject.MachineName = machineNameOverride;
            configurationObject.Source = sourceOverride;
            configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.Filter = filterOverride.Value;
		}
    }
}
