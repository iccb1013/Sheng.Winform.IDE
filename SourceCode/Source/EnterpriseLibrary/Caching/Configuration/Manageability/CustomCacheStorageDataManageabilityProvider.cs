/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    public class CustomCacheStorageDataManageabilityProvider
        : CustomProviderDataManageabilityProvider<CustomCacheStorageData>
    {
        public new const String AttributesPropertyName = CustomProviderDataManageabilityProvider<CustomCacheStorageData>.AttributesPropertyName;
        public new const String ProviderTypePropertyName = CustomProviderDataManageabilityProvider<CustomCacheStorageData>.ProviderTypePropertyName;
        public CustomCacheStorageDataManageabilityProvider()
            : base("")
        {
            CustomCacheStorageDataWmiMapper.RegisterWmiTypes();
        }
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
                                                                    CustomCacheStorageData configurationObject,
                                                                    IConfigurationSource configurationSource,
                                                                    String elementPolicyKeyName)
        {
            AddElementAdministrativeTemplateParts(contentBuilder,
                                                  configurationObject,
                                                  configurationSource,
                                                  elementPolicyKeyName);
        }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      CustomCacheStorageData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.CustomProviderTypePartName,
                                           elementPolicyKeyName,
                                           ProviderTypePropertyName,
                                           configurationObject.Type.AssemblyQualifiedName,
                                           1024,
                                           true);
            contentBuilder.AddEditTextPart(Resources.CustomProviderAttributesPartName,
                                           elementPolicyKeyName,
                                           AttributesPropertyName,
                                           GenerateAttributesString(configurationObject.Attributes),
                                           1024,
                                           false);
        }
        protected override void GenerateWmiObjects(CustomCacheStorageData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            CustomCacheStorageDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
    }
}
