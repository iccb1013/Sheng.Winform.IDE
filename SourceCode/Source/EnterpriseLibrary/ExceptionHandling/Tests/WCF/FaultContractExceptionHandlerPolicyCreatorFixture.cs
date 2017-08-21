/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
	[TestClass]
	public class FaultContractExceptionHandlerPolicyCreatorFixture
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
			FaultContractExceptionHandlerData exceptionHandlerData = new FaultContractExceptionHandlerData("handler1", 
				typeof(MockFaultContract).AssemblyQualifiedName);
			exceptionHandlerData.ExceptionMessage = "fault message";
			exceptionHandlerData.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("Message", "{Message}"));
			exceptionHandlerData.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("Data", "{Data}"));
			exceptionHandlerData.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("SomeNumber", "{OffendingNumber}"));
			exceptionTypeData.ExceptionHandlers.Add(exceptionHandlerData);
			container.AddExtension(new ExceptionHandlingBlockExtension());
			ExceptionPolicyImpl policy = container.Resolve<ExceptionPolicyImpl>("policy");
			NotFiniteNumberException originalException = new NotFiniteNumberException("MyException", 12341234123412);
			originalException.Data.Add("someKey", "someValue");
			try
			{
				policy.HandleException(originalException);
				Assert.Fail("a new exception should have been thrown");
			}
			catch (FaultContractWrapperException e)
			{
				MockFaultContract fault = (MockFaultContract)e.FaultContract;
				Assert.AreEqual(originalException.Message, fault.Message);
				Assert.AreEqual(originalException.Data.Count, fault.Data.Count);
				Assert.AreEqual(originalException.Data["someKey"], fault.Data["someKey"]);
				Assert.AreEqual(originalException.OffendingNumber, fault.SomeNumber);
			}
		}
	}
}
