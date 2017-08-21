/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionHandlingSettingsFixture
    {
        const string badString = "SomeBunkString984t487y";
        const string wrapPolicy = "Wrap Policy";
        const string wrapHandler = "WrapHandler";
        const string newWrapHandler = "WrapHandler2";
        const string customPolicy = "Custom Policy";
        const string exceptionType = "Exception";
        const string customHandler = "CustomHandler";
        ExceptionPolicyData WrapPolicy
        {
            get
            {
                ExceptionHandlingSettings settings = new ExceptionHandlingConfigurationView(new SystemConfigurationSource()).ExceptionHandlingSettings;
                return settings.ExceptionPolicies.Get(wrapPolicy);
            }
        }
        [TestMethod]
        public void GetPolicyByNamePassTest()
        {
            ExceptionPolicyData testPolicy = WrapPolicy;
            Assert.IsNotNull(testPolicy);
        }
        [TestMethod]
        public void GetPolicyByNameFailTest()
        {
            ExceptionHandlingSettings settings = new ExceptionHandlingConfigurationView(new SystemConfigurationSource()).ExceptionHandlingSettings;
            settings.ExceptionPolicies.Get(badString);
        }
        [TestMethod]
        public void GetTypeByNamePassTest()
        {
            ExceptionTypeData testType = WrapPolicy.ExceptionTypes.Get(exceptionType);
            Assert.IsNotNull(testType);
            Assert.AreEqual(PostHandlingAction.ThrowNewException, testType.PostHandlingAction);
        }
        [TestMethod]
        public void GetTypeByNameFailTest()
        {
            ExceptionTypeData testType = WrapPolicy.ExceptionTypes.Get(badString);
            Assert.IsNull(testType);
        }
        [TestMethod]
        public void GetHandlerPassTest()
        {
            ExceptionTypeData testType = WrapPolicy.ExceptionTypes.Get(exceptionType);
            Assert.IsNotNull(testType);
            Assert.AreEqual(1, testType.ExceptionHandlers.Count);
            Assert.IsNotNull(testType.ExceptionHandlers.Get(wrapHandler));
        }
        [TestMethod]
        public void CanOpenAndSaveWithCustomHandler()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ExceptionHandlingSettings settings = (ExceptionHandlingSettings)config.Sections[ExceptionHandlingSettings.SectionName];
            CustomHandlerData data = (CustomHandlerData)settings.ExceptionPolicies.Get(customPolicy).ExceptionTypes.Get(exceptionType).ExceptionHandlers.Get(customHandler);
            data.Attributes.Add("Money", "0");
            config.Save();
            ConfigurationManager.RefreshSection(ExceptionHandlingSettings.SectionName);
            settings = (ExceptionHandlingSettings)ConfigurationManager.GetSection(ExceptionHandlingSettings.SectionName);
            data = (CustomHandlerData)settings.ExceptionPolicies.Get(customPolicy).ExceptionTypes.Get(exceptionType).ExceptionHandlers.Get(customHandler);
            Assert.IsNotNull(data);
            Assert.AreEqual(3, data.Attributes.Count);
            Assert.AreEqual("0", data.Attributes.Get("Money"));
            data = null;
            config = null;
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings = (ExceptionHandlingSettings)config.Sections[ExceptionHandlingSettings.SectionName];
            data = (CustomHandlerData)settings.ExceptionPolicies.Get(customPolicy).ExceptionTypes.Get(exceptionType).ExceptionHandlers.Get(customHandler);
            data.Attributes.Remove("Money");
            config.Save();
            ConfigurationManager.RefreshSection(ExceptionHandlingSettings.SectionName);
            settings = (ExceptionHandlingSettings)ConfigurationManager.GetSection(ExceptionHandlingSettings.SectionName);
            data = (CustomHandlerData)settings.ExceptionPolicies.Get(customPolicy).ExceptionTypes.Get(exceptionType).ExceptionHandlers.Get(customHandler);
            Assert.AreEqual(2, data.Attributes.Count);
        }
        [TestMethod]
        public void CanOpenAndSaveWithWrapHandler()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ExceptionHandlingSettings settings = (ExceptionHandlingSettings)config.Sections[ExceptionHandlingSettings.SectionName];
            WrapHandlerData data = (WrapHandlerData)settings.ExceptionPolicies.Get(wrapPolicy).ExceptionTypes.Get(exceptionType).ExceptionHandlers.Get(wrapHandler);
            string oldName = data.Name;
            data.Name = newWrapHandler;
            config.Save();
            ConfigurationManager.RefreshSection(ExceptionHandlingSettings.SectionName);
            settings = (ExceptionHandlingSettings)ConfigurationManager.GetSection(ExceptionHandlingSettings.SectionName);
            data = (WrapHandlerData)settings.ExceptionPolicies.Get(wrapPolicy).ExceptionTypes.Get(exceptionType).ExceptionHandlers.Get(newWrapHandler);
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Name, newWrapHandler);
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings = (ExceptionHandlingSettings)config.Sections[ExceptionHandlingSettings.SectionName];
            data = (WrapHandlerData)settings.ExceptionPolicies.Get(wrapPolicy).ExceptionTypes.Get(exceptionType).ExceptionHandlers.Get(newWrapHandler);
            data.Name = oldName;
            config.Save();
            ConfigurationManager.RefreshSection(ExceptionHandlingSettings.SectionName);
        }
    }
}
