//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig = System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Configuration
{
    [TestClass]
    public class ConfigurationObjectsFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void AppropriateDefaultsAreSet()
        {
            ManageableConfigurationSourceElement element = new ManageableConfigurationSourceElement();
            Assert.AreEqual(true, element.EnableWmi);
            Assert.AreEqual(true, element.EnableGroupPolicies);
            Assert.AreEqual(string.Empty, element.FilePath);
            Assert.AreEqual("Application", element.ApplicationName);
        }

        [TestMethod]
        public void ConfigurationCanBeDeserialized()
        {
            ConfigurationManager.RefreshSection(ConfigurationSourceSection.SectionName);

            ConfigurationSourceSection section = (ConfigurationSourceSection)ConfigurationManager.GetSection(ConfigurationSourceSection.SectionName);

            Assert.IsNotNull(section);
            Assert.AreEqual(1, section.Sources.Count);
            ManageableConfigurationSourceElement element = section.Sources.Get(0) as ManageableConfigurationSourceElement;
            Assert.IsNotNull(element);
            Assert.AreEqual(2, element.ConfigurationManageabilityProviders.Count);
            ConfigurationSectionManageabilityProviderData data1 = element.ConfigurationManageabilityProviders.Get("section1");
            Assert.IsNotNull(data1);
            Assert.AreEqual(typeof(MockConfigurationSectionManageabilityProvider), data1.Type);
            Assert.AreEqual(3, data1.ManageabilityProviders.Count);
            ConfigurationElementManageabilityProviderData elementData11 = data1.ManageabilityProviders.Get("subProvider1");
            Assert.IsNotNull(elementData11);
            Assert.AreEqual(typeof(Object), elementData11.TargetType);
            ConfigurationSectionManageabilityProviderData data2 = element.ConfigurationManageabilityProviders.Get("section2");
            Assert.IsNotNull(data2);
            Assert.AreEqual(typeof(MockConfigurationSectionManageabilityProvider), data1.Type);
            Assert.AreEqual(0, data2.ManageabilityProviders.Count);
        }

        [TestMethod]
        public void ConfigurationElementWithNoProvidersCanBeSerialized()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = otherConfigurationFile;

            File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

            try
            {
                System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                ConfigurationSourceSection rwConfigurationSourceSection
                    = rwConfiguration.GetSection(ConfigurationSourceSection.SectionName) as ConfigurationSourceSection;
                rwConfigurationSourceSection.Sources.Clear();

                ManageableConfigurationSourceElement rwConfigurationSourceElement
                    = new ManageableConfigurationSourceElement("manageable", otherConfigurationFile, "TestApplication");
                rwConfigurationSourceSection.Sources.Add(rwConfigurationSourceElement);

                rwConfiguration.Save();

                System.Configuration.Configuration roConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                ConfigurationSourceSection roConfigurationSourceSection
                    = roConfiguration.GetSection(ConfigurationSourceSection.SectionName) as ConfigurationSourceSection;
                Assert.IsNotNull(roConfigurationSourceSection);
                Assert.AreEqual(1, roConfigurationSourceSection.Sources.Count);
                ManageableConfigurationSourceElement roConfigurationSourceElement
                    = roConfigurationSourceSection.Sources.Get("manageable") as ManageableConfigurationSourceElement;
                Assert.IsNotNull(roConfigurationSourceElement);
                Assert.AreEqual(0, roConfigurationSourceElement.ConfigurationManageabilityProviders.Count);
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        public void ConfigurationElementWithProvidersCanBeSerialized()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = otherConfigurationFile;

            File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

            try
            {
                System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                ConfigurationSourceSection rwConfigurationSourceSection
                    = rwConfiguration.GetSection(ConfigurationSourceSection.SectionName) as ConfigurationSourceSection;
                rwConfigurationSourceSection.Sources.Clear();

                ManageableConfigurationSourceElement rwConfigurationSourceElement
                    = new ManageableConfigurationSourceElement("manageable", otherConfigurationFile, "TestApplication");
                rwConfigurationSourceElement.ConfigurationManageabilityProviders.Add(new ConfigurationSectionManageabilityProviderData("section1", typeof(MockConfigurationSectionManageabilityProvider)));
                rwConfigurationSourceSection.Sources.Add(rwConfigurationSourceElement);

                rwConfiguration.Save();

                System.Configuration.Configuration roConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                ConfigurationSourceSection roConfigurationSourceSection
                    = roConfiguration.GetSection(ConfigurationSourceSection.SectionName) as ConfigurationSourceSection;
                Assert.IsNotNull(roConfigurationSourceSection);
                Assert.AreEqual(1, roConfigurationSourceSection.Sources.Count);
                ManageableConfigurationSourceElement roConfigurationSourceElement
                    = roConfigurationSourceSection.Sources.Get("manageable") as ManageableConfigurationSourceElement;
                Assert.IsNotNull(roConfigurationSourceElement);
                Assert.AreEqual(1, roConfigurationSourceElement.ConfigurationManageabilityProviders.Count);
                ConfigurationSectionManageabilityProviderData roProviderData
                    = roConfigurationSourceElement.ConfigurationManageabilityProviders.Get(0);
                Assert.AreEqual("section1", roProviderData.Name);
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        [Ignore] // TODO temporary ignore
        public void CanCreateConfigurationSourceFromConfigurationElement()
        {
            ManageableConfigurationSourceElement element
                = new ManageableConfigurationSourceElement(
                    "manageable",
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                    "testapp",
                    true,
                    false);
            element.ConfigurationManageabilityProviders.Add(new ConfigurationSectionManageabilityProviderData("section1", typeof(MockConfigurationSectionManageabilityProvider)));
            element.ConfigurationManageabilityProviders.Get(0).ManageabilityProviders.Add(new ConfigurationElementManageabilityProviderData("1", typeof(MockConfigurationElementManageabilityProvider), typeof(String)));
            element.ConfigurationManageabilityProviders.Add(new ConfigurationSectionManageabilityProviderData("section2", typeof(MockConfigurationSectionManageabilityProvider)));
            element.ConfigurationManageabilityProviders.Get(1).ManageabilityProviders.Add(new ConfigurationElementManageabilityProviderData("2", typeof(MockConfigurationElementManageabilityProvider), typeof(Boolean)));
            element.ConfigurationManageabilityProviders.Get(1).ManageabilityProviders.Add(new ConfigurationElementManageabilityProviderData("3", typeof(MockConfigurationElementManageabilityProvider), typeof(Int32)));

            IConfigurationSource configurationSource = element.CreateSource();
            Assert.IsNotNull(configurationSource);
            Assert.AreSame(typeof(ManageableConfigurationSource), configurationSource.GetType());

            ManageableConfigurationSourceImplementation implementation
                = ((ManageableConfigurationSource)configurationSource).Implementation;

            Assert.AreSame(typeof(ManageabilityHelper), implementation.ManageabilityHelper.GetType());
            ManageabilityHelper manageabilityHelper = (ManageabilityHelper)implementation.ManageabilityHelper;
            Assert.AreEqual(2, manageabilityHelper.ManageabilityProviders.Count);
        }
    }
}
