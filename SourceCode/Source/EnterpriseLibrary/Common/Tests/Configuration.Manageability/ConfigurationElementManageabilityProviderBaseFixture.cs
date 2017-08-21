/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConfigurationElementManageabilityProviderBaseFixture
        : ConfigurationElementManageabilityProviderBase<NamedConfigurationElement>
    {
        NamedConfigurationElement configurationObject, configurationObjectParameter;
        IRegistryKey policyKeyParameter, machineKey, userKey;
        bool overrideCalled;
        bool throwOnOverride;
        List<Exception> loggedExceptions;
        static bool generateCalled;
        static string configurationObjectName;
        public ConfigurationElementManageabilityProviderBaseFixture() {}
        [TestInitialize]
        public void SetUp()
        {
            overrideCalled = false;
            generateCalled = false;
            throwOnOverride = false;
            configurationObject = new NamedConfigurationElement("original");
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            loggedExceptions = new List<Exception>();
            configurationObjectName = null;
        }
        [TestMethod]
        public void OverrideIsNotInvokedIfGroupPolicyIsDisabled()
        {
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           false, machineKey, userKey,
                                                           true, null);
            Assert.IsFalse(overrideCalled);
        }
        [TestMethod]
        public void OverrideIsNotInvokedIfRegistryKeysAreNull()
        {
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           true, null, null,
                                                           true, null);
            Assert.IsFalse(overrideCalled);
        }
        [TestMethod]
        public void OverrideIsInvokedWithMachineKeyIfOnlyKey()
        {
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           true, machineKey, null,
                                                           true, null);
            Assert.IsTrue(overrideCalled);
            Assert.AreSame(machineKey, policyKeyParameter);
            Assert.AreSame(configurationObject, configurationObjectParameter);
        }
        [TestMethod]
        public void OverrideIsInvokedWithUserKeyIfOnlyKey()
        {
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           true, null, userKey,
                                                           true, null);
            Assert.IsTrue(overrideCalled);
            Assert.AreSame(userKey, userKey);
            Assert.AreSame(configurationObject, configurationObjectParameter);
        }
        [TestMethod]
        public void OverrideIsInvokedWithMachineKeyIfBothKeys()
        {
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           true, machineKey, userKey,
                                                           true, null);
            Assert.IsTrue(overrideCalled);
            Assert.AreSame(machineKey, policyKeyParameter);
            Assert.AreSame(configurationObject, configurationObjectParameter);
        }
        [TestMethod]
        public void ExceptionIsLoggedIfExceptionIsThrownOnOverride()
        {
            throwOnOverride = true;
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           true, machineKey, userKey,
                                                           true, null);
            Assert.IsTrue(overrideCalled);
            Assert.AreSame(machineKey, policyKeyParameter);
            Assert.AreSame(configurationObject, configurationObjectParameter);
            Assert.AreEqual(1, loggedExceptions.Count);
            Assert.AreEqual("override", loggedExceptions[0].Message);
        }
        [TestMethod]
        public void GenerateIsNotInvokedIfDisabled()
        {
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           false, machineKey, userKey,
                                                           false, null);
            Assert.IsFalse(generateCalled);
        }
        [TestMethod]
        public void GenerateIsInvokedIfEnabled()
        {
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           false, machineKey, userKey,
                                                           true, null);
            Assert.IsTrue(generateCalled);
            Assert.AreEqual("original", configurationObjectName);
        }
        [TestMethod]
        public void GenerateIsInvokedIfEnabledAfterOverridesAreProcessed()
        {
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           true, machineKey, userKey,
                                                           true, null);
            Assert.IsTrue(generateCalled);
            Assert.AreEqual("overriden", configurationObjectName);
        }
        [TestMethod]
        public void GenerateIsInvokedIfEnabledAfterOverridesAreProcessedEvenIfOverrideThrows()
        {
            throwOnOverride = true;
            OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject,
                                                           true, machineKey, userKey,
                                                           true, null);
            Assert.IsTrue(generateCalled);
            Assert.AreEqual("original", configurationObjectName);
        }
        protected override void OverrideWithGroupPolicies(NamedConfigurationElement configurationObject,
                                                          IRegistryKey policyKey)
        {
            overrideCalled = true;
            configurationObjectParameter = configurationObject;
            policyKeyParameter = policyKey;
            if (throwOnOverride)
            {
                throw new Exception("override");
            }
            configurationObject.Name = "overriden";
        }
        protected override void GenerateWmiObjects(NamedConfigurationElement configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            generateCalled = true;
            configurationObjectName = configurationObject.Name;
        }
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
                                                                    NamedConfigurationElement configurationObject,
                                                                    IConfigurationSource configurationSource,
                                                                    string elementPolicyKeyName)
        {
        }
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      NamedConfigurationElement configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      string elementPolicyKeyName)
        {
        }
        protected override string ElementPolicyNameTemplate
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        protected override void LogExceptionWhileOverriding(Exception exception)
        {
            loggedExceptions.Add(exception);
        }
    }
}
