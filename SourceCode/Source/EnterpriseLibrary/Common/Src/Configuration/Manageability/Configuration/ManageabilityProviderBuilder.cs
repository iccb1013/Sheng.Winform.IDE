/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration
{
    public class ManageabilityProviderBuilder
    {
        public ConfigurationElementManageabilityProvider CreateConfigurationElementManageabilityProvider(
            ConfigurationElementManageabilityProviderData manageabilityProviderData)
        {
            return (ConfigurationElementManageabilityProvider)Activator.CreateInstance(manageabilityProviderData.Type);
        }
        public ConfigurationSectionManageabilityProvider CreateConfigurationSectionManageabilityProvider(
            ConfigurationSectionManageabilityProviderData manageabilityProviderData)
        {
            IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            foreach (ConfigurationElementManageabilityProviderData subProviderData in manageabilityProviderData.ManageabilityProviders)
            {
                ConfigurationElementManageabilityProvider subManageabilityProvider
                    = CreateConfigurationElementManageabilityProvider(subProviderData);
                subProviders.Add(subProviderData.TargetType, subManageabilityProvider);
            }
            return (ConfigurationSectionManageabilityProvider)Activator.CreateInstance(manageabilityProviderData.Type,
                                                                                       subProviders);
        }
    }
}
