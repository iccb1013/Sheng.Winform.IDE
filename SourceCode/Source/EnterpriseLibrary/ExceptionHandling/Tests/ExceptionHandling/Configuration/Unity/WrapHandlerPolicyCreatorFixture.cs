/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Properties;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Configuration.Unity
{
    [TestClass]
    public class WrapHandlerPolicyCreatorFixture
    {
        private IUnityContainer container;
        private ExceptionHandlingSettings settings;
        private DictionaryConfigurationSource configurationSource;
        [TestInitialize]
        public void SetUp()
        {
            container = new UnityContainer();
            settings = new ExceptionHandlingSettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(ExceptionHandlingSettings.SectionName, settings);
            container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
        }
        [TestCleanup]
        public void TearDown()
        {
            container.Dispose();
        }
        [TestMethod]
        public void CanCreatePoliciesForHandler()
        {
            ExceptionPolicyData exceptionPolicyData = new ExceptionPolicyData("policy");
            settings.ExceptionPolicies.Add(exceptionPolicyData);
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
            exceptionPolicyData.ExceptionTypes.Add(exceptionTypeData);
            ExceptionHandlerData exceptionHandlerData = new WrapHandlerData("handler1", "wrapped", typeof(ArgumentException).AssemblyQualifiedName);
            exceptionTypeData.ExceptionHandlers.Add(exceptionHandlerData);
            container.AddExtension(new ExceptionHandlingBlockExtension());
            ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy");
            Exception originalException = new Exception("to be wrapped");
            try
            {
                policy.HandleException(originalException);
                Assert.Fail("a new exception should have been thrown");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("wrapped", e.Message);
                Assert.AreSame(originalException, e.InnerException);
            }
        }
        [TestMethod]
        public void CanGetExceptionMessageFromResource()
        {
            const string resourceName = "ExceptionMessage";
            ExceptionPolicyData exceptionPolicyData = new ExceptionPolicyData("policy");
            settings.ExceptionPolicies.Add(exceptionPolicyData);
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
            exceptionPolicyData.ExceptionTypes.Add(exceptionTypeData);
            WrapHandlerData exceptionHandlerData = new WrapHandlerData("handler1", "wrapped", typeof(ArgumentException).AssemblyQualifiedName);
            exceptionHandlerData.ExceptionMessageResourceName = resourceName;
            exceptionHandlerData.ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName;
            string resourceValue = Resources.ExceptionMessage;
            exceptionTypeData.ExceptionHandlers.Add(exceptionHandlerData);
            container.AddExtension(new ExceptionHandlingBlockExtension());
            ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy");
            Exception originalException = new Exception("to be wrapped");
            try
            {
                policy.HandleException(originalException);
                Assert.Fail("a new exception should have been thrown");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(resourceValue, e.Message);
                Assert.AreSame(originalException, e.InnerException);
            }
        }
        [TestMethod]
        [DeploymentItem(@"Tests\ExceptionHandling\bin\Debug\es\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.resources.dll", "es")]
        [DeploymentItem(@"Tests\ExceptionHandling\bin\Release\es\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.resources.dll", "es")]
        [DeploymentItem(@"ExceptionHandling\Tests\ExceptionHandling\bin\Debug\es\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.resources.dll", "es")]
        [DeploymentItem(@"ExceptionHandling\Tests\ExceptionHandling\bin\Release\es\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.resources.dll", "es")]
        public void CanGetExceptionMessageFromResourceForDifferentLocales()
        {
            const string resourceName = "ExceptionMessage";
            ExceptionPolicyData exceptionPolicyData = new ExceptionPolicyData("policy");
            settings.ExceptionPolicies.Add(exceptionPolicyData);
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
            exceptionPolicyData.ExceptionTypes.Add(exceptionTypeData);
            WrapHandlerData exceptionHandlerData = new WrapHandlerData("handler1", "wrapped", typeof(ArgumentException).AssemblyQualifiedName);
            exceptionHandlerData.ExceptionMessageResourceName = resourceName;
            exceptionHandlerData.ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName;
            exceptionTypeData.ExceptionHandlers.Add(exceptionHandlerData);
            container.AddExtension(new ExceptionHandlingBlockExtension());
            ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy");
            Exception originalException = new Exception("to be wrapped");
            string enMessage = null;
            string esMessage = null;
            CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                try
                {
                    policy.HandleException(originalException);
                    Assert.Fail("a new exception should have been thrown");
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual(Resources.ExceptionMessage, e.Message);
                    Assert.AreSame(originalException, e.InnerException);
                    enMessage = e.Message;
                }
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
                try
                {
                    policy.HandleException(originalException);
                    Assert.Fail("a new exception should have been thrown");
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual(Resources.ExceptionMessage, e.Message);
                    Assert.AreSame(originalException, e.InnerException);
                    esMessage = e.Message;
                }
                Assert.AreNotEqual(enMessage, esMessage);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = currentUICulture;
            }
        }
    }
}
