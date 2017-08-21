/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionHandlingConfigurationViewFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void WhenReadingUndefinedPolicyNameThrows()
        {
            ExceptionHandlingConfigurationView view = new ExceptionHandlingConfigurationView(new SystemConfigurationSource());
            view.GetExceptionPolicyData("Foo");
        }
        [TestMethod]
        public void GetExceptionHandlingSettingsFromConfiguration()
        {
            ExceptionHandlingConfigurationView view = new ExceptionHandlingConfigurationView(new SystemConfigurationSource());
            ExceptionHandlingSettings settings = view.ExceptionHandlingSettings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(6, settings.ExceptionPolicies.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPolicyWithNullNameThrows()
        {
            ExceptionHandlingConfigurationView view = new ExceptionHandlingConfigurationView(new SystemConfigurationSource());
            view.GetExceptionPolicyData(null);
        }
        [TestMethod]
        public void CanGetPolicyDataFromConfiguration()
        {
            const string policyName = "Wrap Policy";
            ExceptionHandlingConfigurationView view = new ExceptionHandlingConfigurationView(new SystemConfigurationSource());
            ExceptionPolicyData data = view.GetExceptionPolicyData(policyName);
            Assert.IsNotNull(data);
            Assert.AreEqual(policyName, data.Name);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructingWithANullConfigurationSourceThrows()
        {
            new ExceptionHandlingConfigurationView(null);
        }
    }
}
