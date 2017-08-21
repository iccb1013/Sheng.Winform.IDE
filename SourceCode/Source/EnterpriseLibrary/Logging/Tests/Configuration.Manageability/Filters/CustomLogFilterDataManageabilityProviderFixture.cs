/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.Filters
{
    [TestClass]
    public class CustomLogFilterDataManageabilityProviderFixture
    {
        CustomLogFilterDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        CustomLogFilterData configurationObject;
        [TestInitialize]
        public void SetUp()
        {
            provider = new CustomLogFilterDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new CustomLogFilterData();
        }
        [TestCleanup]
        public void TearDown()
        {
            ManagementEntityTypesRegistrar.UnregisterAll();
        }
        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;
            Assembly assembly = typeof(CustomLogFilterDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(CustomLogFilterDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }
            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(CustomLogFilterData), selectedAttribute.TargetType);
        }
        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Type = typeof(object);
            configurationObject.Attributes.Add("name1", "value1");
            configurationObject.Attributes.Add("name2", "value2");
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);
            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(CustomFilterSetting), wmiSettings[0].GetType());
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, ((CustomFilterSetting)wmiSettings[0]).FilterType);
            Dictionary<String, String> attributesDictionary = new Dictionary<string, string>();
            foreach (String entry in ((CustomFilterSetting)wmiSettings[0]).Attributes)
            {
                KeyValuePairParsingTestHelper.ExtractKeyValueEntries(entry, attributesDictionary);
            }
            Assert.AreEqual(2, attributesDictionary.Count);
            Assert.AreEqual("value1", attributesDictionary["name1"]);
            Assert.AreEqual("value2", attributesDictionary["name2"]);
        }
        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationObject.Type = typeof(object);
            configurationObject.Attributes.Add("name1", "valu;e1");
            configurationObject.Attributes.Add("name2", "value2");
            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();
            contentBuilder.StartCategory("category");
            provider.AddAdministrativeTemplateDirectives(contentBuilder, configurationObject, configurationSource, "TestApp");
            contentBuilder.EndCategory();
            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(CustomLogFilterDataManageabilityProvider.TypePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, ((AdmEditTextPart)partsEnumerator.Current).DefaultValue);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(CustomLogFilterDataManageabilityProvider.AttributesPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsNull(partsEnumerator.Current.KeyName);
            IDictionary<String, String> attributes = new Dictionary<String, String>();
            KeyValuePairParser.ExtractKeyValueEntries(((AdmEditTextPart)partsEnumerator.Current).DefaultValue, attributes);
            Assert.AreEqual(2, attributes.Count);
            Assert.AreEqual("valu;e1", attributes["name1"]);
            Assert.AreEqual("value2", attributes["name2"]);
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
