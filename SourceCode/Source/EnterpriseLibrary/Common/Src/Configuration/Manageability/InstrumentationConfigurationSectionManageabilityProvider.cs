/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    public class InstrumentationConfigurationSectionManageabilityProvider
        : ConfigurationSectionManageabilityProviderBase<InstrumentationConfigurationSection>
    {
        public const String EventLoggingEnabledPropertyName = "eventLoggingEnabled";
        public const String PerformanceCountersEnabledPropertyName = "performanceCountersEnabled";
        public const String WmiEnabledPropertyName = "wmiEnabled";
        public InstrumentationConfigurationSectionManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
            : base(subProviders)
        {
            InstrumentationConfigurationSectionWmiMapper.RegisterWmiTypes();
        }
        protected override string SectionCategoryName
        {
            get { return Resources.InstrumentationSectionCategoryName; }
        }
        protected override string SectionName
        {
            get { return InstrumentationConfigurationSection.SectionName; }
        }
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
                                                                    InstrumentationConfigurationSection configurationSection,
                                                                    IConfigurationSource configurationSource,
                                                                    String sectionKey)
        {
            contentBuilder.StartPolicy(Resources.InstrumentationSectionPolicyName, sectionKey);
            {
                contentBuilder.AddCheckboxPart(Resources.InstrumentationSectionEventLoggingEnabledPartName,
                                               EventLoggingEnabledPropertyName,
                                               configurationSection.EventLoggingEnabled);
                contentBuilder.AddCheckboxPart(Resources.InstrumentationSectionPerformanceCountersEnabledPartName,
                                               PerformanceCountersEnabledPropertyName,
                                               configurationSection.PerformanceCountersEnabled);
                contentBuilder.AddCheckboxPart(Resources.InstrumentationSectionWmiEnabledPartName,
                                               WmiEnabledPropertyName,
                                               configurationSection.WmiEnabled);
            }
            contentBuilder.EndPolicy();
        }
        protected override void GenerateWmiObjectsForConfigurationSection(InstrumentationConfigurationSection configurationSection,
                                                                          ICollection<ConfigurationSetting> wmiSettings)
        {
            InstrumentationConfigurationSectionWmiMapper.GenerateWmiObjects(configurationSection, wmiSettings);
        }
        protected override void OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationElements(InstrumentationConfigurationSection configurationSection,
                                                                                                       bool readGroupPolicies,
                                                                                                       IRegistryKey machineKey,
                                                                                                       IRegistryKey userKey,
                                                                                                       bool generateWmiObjects,
                                                                                                       ICollection<ConfigurationSetting> wmiSettings)
        {
        }
        protected override void OverrideWithGroupPoliciesForConfigurationSection(InstrumentationConfigurationSection configurationSection,
                                                                                 IRegistryKey policyKey)
        {
            bool? eventLoggingEnabledOverride = policyKey.GetBoolValue(EventLoggingEnabledPropertyName);
            bool? performanceCountersEnabledOverride = policyKey.GetBoolValue(PerformanceCountersEnabledPropertyName);
            bool? wmiEnabledOverride = policyKey.GetBoolValue(WmiEnabledPropertyName);
            configurationSection.EventLoggingEnabled = eventLoggingEnabledOverride.Value;
            configurationSection.PerformanceCountersEnabled = performanceCountersEnabledOverride.Value;
            configurationSection.WmiEnabled = wmiEnabledOverride.Value;
        }
    }
}
