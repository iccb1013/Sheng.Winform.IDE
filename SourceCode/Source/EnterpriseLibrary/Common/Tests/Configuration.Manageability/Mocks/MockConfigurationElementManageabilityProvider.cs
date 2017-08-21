/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
    public class MockConfigurationElementManageabilityProvider : ConfigurationElementManageabilityProvider
    {
        public const String Part = "mock part";
        public const String Policy = "mock policy";
        protected static IDictionary<Type, ConfigurationElementManageabilityProvider> NoProviders
            = new Dictionary<Type, ConfigurationElementManageabilityProvider>(0);
        public bool addPart = false;
        public bool addPolicy = false;
        public bool called = false;
        public List<ConfigurationElement> configurationObjects;
        public bool generateWmiObjects;
        public IRegistryKey machineKey;
        public bool readGroupPolicies;
        public IRegistryKey userKey;
        public MockConfigurationElementManageabilityProvider()
            : this(false, false) {}
        public MockConfigurationElementManageabilityProvider(bool addPolicy,
                                                             bool addPart)
        {
            this.addPolicy = addPolicy;
            this.addPart = addPart;
            configurationObjects = new List<ConfigurationElement>();
        }
        public ConfigurationElement LastConfigurationObject
        {
            get { return configurationObjects.Count > 0 ? configurationObjects[configurationObjects.Count - 1] : null; }
        }
        public override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
                                                                             ConfigurationElement configurationObject,
                                                                             IConfigurationSource configurationSource,
                                                                             String parentKey)
        {
            called = true;
            configurationObjects.Add(configurationObject);
            if (addPolicy)
                contentBuilder.StartPolicy(Policy, "policy");
            if (addPart)
                contentBuilder.AddTextPart(Part);
            if (addPolicy)
                contentBuilder.EndPolicy();
        }
        public override bool OverrideWithGroupPoliciesAndGenerateWmiObjects(ConfigurationElement configurationObject,
                                                                            bool readGroupPolicies,
                                                                            IRegistryKey machineKey,
                                                                            IRegistryKey userKey,
                                                                            bool generateWmiObjects,
                                                                            ICollection<ConfigurationSetting> wmiSettings)
        {
            called = true;
            configurationObjects.Add(configurationObject);
            this.readGroupPolicies = readGroupPolicies;
            this.machineKey = machineKey;
            this.userKey = userKey;
            this.generateWmiObjects = generateWmiObjects;
            if (readGroupPolicies)
            {
                IRegistryKey policyKey = machineKey != null ? machineKey : userKey;
                if (policyKey != null
                    && policyKey.IsPolicyKey
                    && !policyKey.GetBoolValue(PolicyValueName).Value)
                {
                    return false;
                }
            }
            if (generateWmiObjects)
            {
                wmiSettings.Add(new TestConfigurationSettings(configurationObject.ToString()));
            }
            return true;
        }
    }
}
