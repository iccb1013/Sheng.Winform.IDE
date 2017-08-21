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
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.Unity
{
	[TestClass]
	public class CustomProviderPolicyCreatorFixture
	{
		[TestMethod]
		public void CreatesAppropriatePoliciesIfProviderHasProperConstructor()
		{
			TestCustomProviderData data = new TestCustomProviderData();
			data.Name = "name";
			data.Type = typeof(TestCustomProviderWithValidConstructor);
			data.Attributes = new NameValueCollection();
			data.Attributes["name1"] = "value1";
			data.Attributes["name2"] = "value2";
			CustomProviderPolicyCreator<TestCustomProviderData> policyCreator
				= new CustomProviderPolicyCreator<TestCustomProviderData>();
			TestHelperExtension extension = new TestHelperExtension();
			extension.initialize = context =>
				{
					((IContainerPolicyCreator)policyCreator).CreatePolicies(context.Policies, data.Name, data, null);
				};
			IUnityContainer container = new UnityContainer();
			container.AddExtension(extension);
			TestCustomProviderWithValidConstructor createdObject
				= container.Resolve<TestCustomProviderWithValidConstructor>("name");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(2, createdObject.Attributes.Count);
			Assert.AreEqual("value1", createdObject.Attributes["name1"]);
			Assert.AreEqual("value2", createdObject.Attributes["name2"]);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatesThrowsIfProviderDoesNotHaveProperConstructor()
		{
			TestCustomProviderData data = new TestCustomProviderData();
			data.Name = "name";
			data.Type = typeof(TestCustomProviderWithInvalidConstructor);
			data.Attributes = new NameValueCollection();
			data.Attributes["name1"] = "value1";
			data.Attributes["name2"] = "value2";
			CustomProviderPolicyCreator<TestCustomProviderData> policyCreator
				= new CustomProviderPolicyCreator<TestCustomProviderData>();
			TestHelperExtension extension = new TestHelperExtension();
			extension.initialize = context =>
				{
					((IContainerPolicyCreator)policyCreator).CreatePolicies(context.Policies, data.Name, data, null);
				};
			IUnityContainer container = new UnityContainer();
			container.AddExtension(extension);
		}
		public class TestCustomProviderWithValidConstructor
		{
			public TestCustomProviderWithValidConstructor(NameValueCollection attributes)
			{
				this.Attributes = attributes;
			}
			public NameValueCollection Attributes
			{
				get;
				set;
			}
		}
		public class TestCustomProviderWithInvalidConstructor
		{
		}
		public class TestCustomProviderData : NameTypeConfigurationElement,
			IHelperAssistedCustomConfigurationData<TestCustomProviderData>
		{
			public CustomProviderDataHelper<TestCustomProviderData> Helper
			{
				get { throw new NotImplementedException(); }
			}
			public object BaseGetPropertyValue(System.Configuration.ConfigurationProperty property)
			{
				throw new NotImplementedException();
			}
			public bool BaseIsModified()
			{
				throw new NotImplementedException();
			}
			public void BaseReset(System.Configuration.ConfigurationElement parentElement)
			{
				throw new NotImplementedException();
			}
			public void BaseSetPropertyValue(System.Configuration.ConfigurationProperty property, object value)
			{
				throw new NotImplementedException();
			}
			public void BaseUnmerge(System.Configuration.ConfigurationElement sourceElement, System.Configuration.ConfigurationElement parentElement, System.Configuration.ConfigurationSaveMode saveMode)
			{
				throw new NotImplementedException();
			}
			public System.Collections.Specialized.NameValueCollection Attributes
			{
				get;
				set;
			}
		}
	}
}
