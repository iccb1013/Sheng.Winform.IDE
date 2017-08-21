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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    public class CustomLogFilterDataManageabilityProvider
        : CustomProviderDataManageabilityProvider<CustomLogFilterData>
    {
        public new const String AttributesPropertyName = CustomProviderDataManageabilityProvider<CustomLogFilterData>.AttributesPropertyName;
        public const String TypePropertyName = ProviderTypePropertyName;
        public CustomLogFilterDataManageabilityProvider()
            : base(Resources.FilterPolicyNameTemplate)
        {
            CustomLogFilterDataWmiMapper.RegisterWmiTypes();
        }
        protected override void GenerateWmiObjects(CustomLogFilterData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            CustomLogFilterDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
    }
}
