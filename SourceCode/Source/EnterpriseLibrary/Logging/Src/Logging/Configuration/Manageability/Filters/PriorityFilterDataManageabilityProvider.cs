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
    public class PriorityFilterDataManageabilityProvider
        : ConfigurationElementManageabilityProviderBase<PriorityFilterData>
    {
        public const String MaximumPriorityPropertyName = "maximumPriority";
        public const String MinimumPriorityPropertyName = "minimumPriority";
        public PriorityFilterDataManageabilityProvider()
        {
            PriorityFilterDataWmiMapper.RegisterWmiTypes();
        }
        protected override string ElementPolicyNameTemplate
        {
            get { return Resources.FilterPolicyNameTemplate; }
        }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      PriorityFilterData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddNumericPart(Resources.PriorityFilterMaximumPriorityPartName,
                                          null,
                                          MaximumPriorityPropertyName,
                                          configurationObject.MaximumPriority,
                                          0,
                                          999999999);
            contentBuilder.AddNumericPart(Resources.PriorityFilterMinimumPriorityPartName,
                                          null,
                                          MinimumPriorityPropertyName,
                                          configurationObject.MinimumPriority,
                                          0,
                                          999999999);
        }
        protected override void GenerateWmiObjects(PriorityFilterData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            PriorityFilterDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
        protected override void OverrideWithGroupPolicies(PriorityFilterData configurationObject,
                                                          IRegistryKey policyKey)
        {
            int? minimumPriorityOverride = policyKey.GetIntValue(MinimumPriorityPropertyName);
            int? maximumPriorityOverride = policyKey.GetIntValue(MaximumPriorityPropertyName);
            configurationObject.MinimumPriority = minimumPriorityOverride.Value;
            configurationObject.MaximumPriority = maximumPriorityOverride.Value;
        }
    }
}
