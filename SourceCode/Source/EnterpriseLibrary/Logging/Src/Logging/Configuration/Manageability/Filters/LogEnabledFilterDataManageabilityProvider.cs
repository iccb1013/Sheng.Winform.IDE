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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    public class LogEnabledFilterDataManageabilityProvider
        : ConfigurationElementManageabilityProviderBase<LogEnabledFilterData>
    {
        public const String EnabledPropertyName = "enabled";
        public LogEnabledFilterDataManageabilityProvider()
        {
            LogEnabledFilterDataWmiMapper.RegisterWmiTypes();
        }
        protected override string ElementPolicyNameTemplate
        {
            get { return Resources.FilterPolicyNameTemplate; }
        }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      LogEnabledFilterData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddCheckboxPart(Resources.LogEnabledFilterEnabledPartName,
                                           EnabledPropertyName,
                                           configurationObject.Enabled);
        }
        protected override void GenerateWmiObjects(LogEnabledFilterData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            LogEnabledFilterDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
        protected override void OverrideWithGroupPolicies(LogEnabledFilterData configurationObject,
                                                          IRegistryKey policyKey)
        {
            bool? enabledOverride = policyKey.GetBoolValue(EnabledPropertyName);
            configurationObject.Enabled = enabledOverride.Value;
        }
    }
}
