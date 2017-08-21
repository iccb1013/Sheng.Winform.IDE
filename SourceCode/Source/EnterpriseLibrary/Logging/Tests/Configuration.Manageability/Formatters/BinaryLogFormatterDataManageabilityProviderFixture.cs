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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.Formatters
{
    [TestClass]
    public class BinaryLogFormatterDataManageabilityProviderFixture
    {
        BinaryLogFormatterDataManageabilityProvider provider;
        IList<ConfigurationSetting> wmiSettings;
        BinaryLogFormatterData configurationObject;
        [TestInitialize]
        public void SetUp()
        {
            provider = new BinaryLogFormatterDataManageabilityProvider();
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new BinaryLogFormatterData();
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
            Assembly assembly = typeof(BinaryLogFormatterDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(BinaryLogFormatterDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }
            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(BinaryLogFormatterData), selectedAttribute.TargetType);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(new TestsConfigurationSection(), true, null, null, true, wmiSettings);
        }
        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, false, wmiSettings);
            Assert.AreEqual(0, wmiSettings.Count);
        }
        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, true, wmiSettings);
            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(BinaryFormatterSetting), wmiSettings[0].GetType());
        }
        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
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
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
