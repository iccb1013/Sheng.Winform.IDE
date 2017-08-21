/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class RegistryKeyBaseFixture
    {
        MockRegistryKey registryKey;
        [TestInitialize]
        public void SetUp()
        {
            registryKey = new MockRegistryKey(true);
        }
        [TestMethod]
        public void CanGetStringValueFromKey()
        {
            registryKey.AddStringValue("name", "value");
            Assert.AreEqual("value", ((RegistryKeyBase)registryKey).GetStringValue("name"));
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetStringWhenValueIsMissingThrows()
        {
            registryKey.AddStringValue("name", "value");
            ((RegistryKeyBase)registryKey).GetStringValue("wrongname");
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetStringWhenValueIsOfWrongTypeThrows()
        {
            registryKey.AddIntValue("name", 0);
            ((RegistryKeyBase)registryKey).GetStringValue("name");
        }
        [TestMethod]
        public void CanGetIntValueFromKey()
        {
            registryKey.AddIntValue("name", 0);
            Assert.AreEqual(0, ((RegistryKeyBase)registryKey).GetIntValue("name").Value);
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetIntWhenValueIsMissingReturnsNull()
        {
            registryKey.AddIntValue("name", 0);
            ((RegistryKeyBase)registryKey).GetStringValue("wrongname");
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetIntWhenValueIsOfWrongTypeThrows()
        {
            registryKey.AddStringValue("name", "value");
            ((RegistryKeyBase)registryKey).GetIntValue("name");
        }
        [TestMethod]
        public void CanGetBoolValueFromKey()
        {
            registryKey.AddBooleanValue("name", true);
            Assert.AreEqual(true, ((RegistryKeyBase)registryKey).GetBoolValue("name").Value);
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetBoolWhenValueIsMissingReturnsNull()
        {
            registryKey.AddBooleanValue("name", true);
            ((RegistryKeyBase)registryKey).GetStringValue("wrongname");
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetBoolWhenValueIsOfWrongTypeThrows()
        {
            registryKey.AddStringValue("name", "value");
            ((RegistryKeyBase)registryKey).GetBoolValue("name");
        }
        [TestMethod]
        public void CanGetEnumValueFromKey()
        {
            registryKey.AddStringValue("name", UriFormat.Unescaped.ToString());
            Assert.AreEqual(UriFormat.Unescaped, ((RegistryKeyBase)registryKey).GetEnumValue<UriFormat>("name").Value);
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetEnumWhenValueIsMissingReturnsNull()
        {
            registryKey.AddStringValue("name", UriFormat.Unescaped.ToString());
            ((RegistryKeyBase)registryKey).GetStringValue("wrongname");
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetEnumWhenValueIsOfWrongTypeThrows()
        {
            registryKey.AddIntValue("name", 0);
            ((RegistryKeyBase)registryKey).GetEnumValue<UriFormat>("name");
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetEnumWhenValueIsNotEnumNameThrows()
        {
            registryKey.AddStringValue("name", "invalid value");
            ((RegistryKeyBase)registryKey).GetEnumValue<UriFormat>("name");
        }
        [TestMethod]
        public void CanGetTypeValueFromKey()
        {
            registryKey.AddStringValue("name", typeof(Object).AssemblyQualifiedName);
            Assert.AreEqual(typeof(Object), ((RegistryKeyBase)registryKey).GetTypeValue("name"));
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetTypeWhenValueIsMissingReturnsNull()
        {
            registryKey.AddStringValue("name", typeof(Object).AssemblyQualifiedName);
            ((RegistryKeyBase)registryKey).GetStringValue("wrongname");
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetTypeWhenValueIsOfWrongTypeThrows()
        {
            registryKey.AddIntValue("name", 0);
            ((RegistryKeyBase)registryKey).GetTypeValue("name");
        }
        [TestMethod]
        [ExpectedException(typeof(RegistryAccessException))]
        public void GetTypeWhenValueIsNotTypeNameThrows()
        {
            registryKey.AddStringValue("name", "invalid value");
            ((RegistryKeyBase)registryKey).GetTypeValue("name");
        }
        [TestMethod]
        public void CallToIsPolicyKeyOnKeyWithoutValueFails()
        {
            Assert.IsFalse(((RegistryKeyBase)registryKey).IsPolicyKey);
        }
        [TestMethod]
        public void CallToIsPolicyKeyOnKeyWithPolicyValueTrueSucceeds()
        {
            registryKey.AddBooleanValue(RegistryKeyBase.PolicyValueName, true);
            Assert.IsTrue(((RegistryKeyBase)registryKey).IsPolicyKey);
        }
        [TestMethod]
        public void CallToIsPolicyKeyOnKeyWithPolicyFalseTrueSucceeds()
        {
            registryKey.AddBooleanValue(RegistryKeyBase.PolicyValueName, false);
            Assert.IsTrue(((RegistryKeyBase)registryKey).IsPolicyKey);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OpenKeyWithNullNameThrows()
        {
            registryKey.OpenSubKey(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OpenKeyWithEmptyNameThrows()
        {
            registryKey.OpenSubKey("");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetValueWithNullNameThrows()
        {
            registryKey.GetStringValue(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetValueWithEmptyNameThrows()
        {
            registryKey.GetStringValue("");
        }
    }
}
