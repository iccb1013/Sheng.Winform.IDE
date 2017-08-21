/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Configuration.Unity
{
	[TestClass]
	public class CustomHandlerPolicyCreationFixture
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
		[TestMethod]
		public void CanCreatePoliciesForHandler()
		{
			ExceptionPolicyData exceptionPolicyData = new ExceptionPolicyData("policy");
			settings.ExceptionPolicies.Add(exceptionPolicyData);
			ExceptionTypeData exceptionTypeData = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.ThrowNewException);
			exceptionPolicyData.ExceptionTypes.Add(exceptionTypeData);
			CustomHandlerData exceptionHandlerData = new CustomHandlerData("handler1", typeof(TestCustomExceptionHandler));
			exceptionHandlerData.Attributes.Add(TestCustomExceptionHandler.AttributeKey, "custom handler");
			exceptionTypeData.ExceptionHandlers.Add(exceptionHandlerData);
			container.AddExtension(new ExceptionHandlingBlockExtension());
			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy");
			Exception originalException = new Exception("to be replaced");
			try
			{
				policy.HandleException(originalException);
				Assert.Fail("a new exception should have been thrown");
			}
			catch (Exception e)
			{
				Assert.AreEqual("custom handler", e.Message);
				Assert.AreSame(originalException, e.InnerException);
				Assert.AreSame(originalException, TestCustomExceptionHandler.handledException);
			}
		}
		public class TestCustomExceptionHandler : MockCustomProviderBase, IExceptionHandler
		{
			public static Exception handledException;
			public TestCustomExceptionHandler(NameValueCollection attributes)
				: base(attributes)
			{ }
			public Exception HandleException(Exception exception, Guid handlingInstanceId)
			{
				handledException = exception;
				return new Exception(this.customValue, exception);
			}
		}
	}
}
