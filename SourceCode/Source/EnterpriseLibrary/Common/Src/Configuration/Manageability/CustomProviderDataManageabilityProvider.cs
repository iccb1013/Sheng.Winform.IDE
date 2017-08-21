/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    public abstract class CustomProviderDataManageabilityProvider<T> : ConfigurationElementManageabilityProviderBase<T>
        where T : NameTypeConfigurationElement, ICustomProviderData
    {
        public const String AttributesPropertyName = "attributes";
        public const String ProviderTypePropertyName = "type";
        readonly String policyTemplate;
        protected CustomProviderDataManageabilityProvider(String policyTemplate)
        {
            this.policyTemplate = policyTemplate;
        }
        protected override string ElementPolicyNameTemplate
        {
            get { return policyTemplate; }
        }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      T configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.CustomProviderTypePartName,
                                           ProviderTypePropertyName,
                                           configurationObject.Type.AssemblyQualifiedName,
                                           1024,
                                           true);
            contentBuilder.AddEditTextPart(Resources.CustomProviderAttributesPartName,
                                           AttributesPropertyName,
                                           GenerateAttributesString(configurationObject.Attributes),
                                           1024,
                                           false);
        }
        protected static String GenerateAttributesString(NameValueCollection attributes)
        {
            KeyValuePairEncoder encoder = new KeyValuePairEncoder();
            foreach (String key in attributes.AllKeys)
            {
                encoder.AppendKeyValuePair(key, attributes[key]);
            }
            return encoder.GetEncodedKeyValuePairs();
        }
        protected override void OverrideWithGroupPolicies(T configurationObject,
                                                          IRegistryKey policyKey)
        {
            Type providerTypeOverride = policyKey.GetTypeValue(ProviderTypePropertyName);
            String attributesOverride = policyKey.GetStringValue(AttributesPropertyName);
            configurationObject.Type = providerTypeOverride;
            configurationObject.Attributes.Clear();
            Dictionary<String, String> attributesDictionary = new Dictionary<string, string>();
            KeyValuePairParser.ExtractKeyValueEntries(attributesOverride, attributesDictionary);
            foreach (KeyValuePair<String, String> kvp in attributesDictionary)
            {
                configurationObject.Attributes.Add(kvp.Key, kvp.Value);
            }
        }
    }
}
