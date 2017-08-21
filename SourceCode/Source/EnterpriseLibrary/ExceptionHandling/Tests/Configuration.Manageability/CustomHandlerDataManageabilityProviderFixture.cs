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
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Tests
{
    [TestClass]
    public class CustomHandlerDataManageabilityProviderFixture
    {
        CustomHandlerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        CustomHandlerData configurationObject;
        [TestInitialize]
        public void SetUp()
        {
            provider = new CustomHandlerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new CustomHandlerData();
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
            Assembly assembly = typeof(CustomHandlerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(CustomHandlerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }
            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(ExceptionHandlingSettingsManageabilityProvider),
                           selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(CustomHandlerData), selectedAttribute.TargetType);
        }
        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Type = typeof(object);
            configurationObject.Attributes.Add("name1", "value1");
            configurationObject.Attributes.Add("name2", "value2");
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                                    false,
                                                                    machineKey,
                                                                    userKey,
                                                                    true,
                                                                    wmiSettings);
            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(CustomHandlerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, ((CustomHandlerSetting)wmiSettings[0]).HandlerType);
            Dictionary<String, String> attributesDictionary = new Dictionary<string, string>();
            foreach (String entry in ((CustomHandlerSetting)wmiSettings[0]).Attributes)
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
            contentBuilder.StartPolicy("policy", "policy key");
            provider.AddAdministrativeTemplateDirectives(contentBuilder, configurationObject, configurationSource, "TestApp");
            contentBuilder.EndPolicy();
            contentBuilder.EndCategory();
            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(CustomHandlerDataManageabilityProvider.ProviderTypePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, ((AdmEditTextPart)partsEnumerator.Current).DefaultValue);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(CustomHandlerDataManageabilityProvider.AttributesPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsNotNull(partsEnumerator.Current.KeyName);
            IDictionary<String, String> attributes = new Dictionary<String, String>();
            KeyValuePairParser.ExtractKeyValueEntries(((AdmEditTextPart)partsEnumerator.Current).DefaultValue, attributes);
            Assert.AreEqual(2, attributes.Count);
            Assert.AreEqual("valu;e1", attributes["name1"]);
            Assert.AreEqual("value2", attributes["name2"]);
            Assert.IsFalse(partsEnumerator.MoveNext());
        }
    }
}
