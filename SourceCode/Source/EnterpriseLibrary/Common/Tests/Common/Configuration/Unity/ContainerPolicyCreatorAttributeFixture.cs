/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.Unity
{
	[TestClass]
	public class ContainerPolicyCreatorAttributeFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorWithNullTypeThrows()
		{
			new ContainerPolicyCreatorAttribute(null);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ContructorWithInvalidTypeThrows()
		{
			new ContainerPolicyCreatorAttribute(typeof(bool));
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructorWithTypeWithoutNoArgsCtorThrows()
		{
			new ContainerPolicyCreatorAttribute(typeof(TestContainerPolicyCreatorWithoutNoArgsCtor));
		}
		[TestMethod]
		public void ConstructorAcceptsValidType()
		{
			ContainerPolicyCreatorAttribute attribute = new ContainerPolicyCreatorAttribute(typeof(TestContainerPolicyCreator));
			Assert.AreSame(typeof(TestContainerPolicyCreator), attribute.PolicyCreatorType);
		}
		private class TestContainerPolicyCreatorWithoutNoArgsCtor : IContainerPolicyCreator
		{
			public TestContainerPolicyCreatorWithoutNoArgsCtor(int ignored)
			{
			}
			public void CreatePolicies(
				IPolicyList policyList,
				string instanceName,
				ConfigurationElement configurationObject,
				IConfigurationSource configurationSource)
			{
				throw new NotImplementedException();
			}
		}
		private class TestContainerPolicyCreator : IContainerPolicyCreator
		{
			public void CreatePolicies(
				IPolicyList policyList,
				string instanceName,
				ConfigurationElement configurationObject,
				IConfigurationSource configurationSource)
			{
				throw new NotImplementedException();
			}
		}
	}
}
