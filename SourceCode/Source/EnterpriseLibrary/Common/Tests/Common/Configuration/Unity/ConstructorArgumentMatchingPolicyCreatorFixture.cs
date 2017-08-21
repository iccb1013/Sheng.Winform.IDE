/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.Unity
{
	[TestClass]
	public class ConstructorArgumentMatchingPolicyCreatorFixture
	{
		private IUnityContainer container;
		private const string instanceName = "provider";
		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();
		}
		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}
		[TestMethod]
		public void ChoosesNoArgsConstructor()
		{
			container.AddExtension(
				new TestHelperExtension(context =>
				{
					IContainerPolicyCreator creator
						= new ConstructorArgumentMatchingPolicyCreator(typeof(TestClassWithNoArgsConstructor));
					creator.CreatePolicies(context.Policies,
						instanceName,
						new TestClassEmptySource(),
						null);
				}));
			TestClassWithNoArgsConstructor provider
				= container.Resolve<TestClassWithNoArgsConstructor>(instanceName);
			Assert.IsNotNull(provider);
		}
		[TestMethod]
		public void ChoosesConstructorWithMaximalNumberOfParameters()
		{
			TestClassSource source = new TestClassSource();
			source.Property1 = 1;
			source.Property2 = "value2";
			container.AddExtension(
				new TestHelperExtension(context =>
					{
						IContainerPolicyCreator creator
							= new ConstructorArgumentMatchingPolicyCreator(typeof(TestClass));
						creator.CreatePolicies(context.Policies,
							instanceName,
							source,
							null);
					}));
			TestClass provider
				= container.Resolve<TestClass>(instanceName);
			Assert.IsNotNull(provider);
			Assert.AreEqual(source.Property1, provider.Property1);
			Assert.AreEqual(source.Property2, provider.Property2);
		}
		[TestMethod]
		public void ChoosesConstructorWithMaximalNumberOfMatchingParameters()
		{
			TestClassSource source = new TestClassSource();
			source.Property1 = 1;
			source.Property2 = "value2";
			container.AddExtension(
				new TestHelperExtension(context =>
				{
					IContainerPolicyCreator creator
						= new ConstructorArgumentMatchingPolicyCreator(typeof(TestClassWhereLongestConstructorCannotBeMatched));
					creator.CreatePolicies(context.Policies,
						instanceName,
						source,
						null);
				}));
			TestClassWhereLongestConstructorCannotBeMatched provider
				= container.Resolve<TestClassWhereLongestConstructorCannotBeMatched>(instanceName);
			Assert.IsNotNull(provider);
			Assert.AreEqual(source.Property1, provider.Property1);
			Assert.AreEqual(source.Property2, provider.Property2);
		}
		[TestMethod]
		public void IndexerPropertiesAreNotConsideredForMatching()
		{
			TestClassSourceWithIndexerProperty source = new TestClassSourceWithIndexerProperty();
			source.Property1 = 1;
			container.AddExtension(
				new TestHelperExtension(context =>
				{
					IContainerPolicyCreator creator
						= new ConstructorArgumentMatchingPolicyCreator(typeof(TestClassWithItemArgument));
					creator.CreatePolicies(context.Policies,
						instanceName,
						source,
						null);
				}));
			TestClassWithItemArgument provider
				= container.Resolve<TestClassWithItemArgument>(instanceName);
			Assert.IsNotNull(provider);
			Assert.AreEqual(source.Property1, provider.Property1);
			Assert.AreEqual("no value set", provider.Property2);		
		}
		[TestMethod]
		public void NonReadablePropertiesAreNotConsideredForMatching()
		{
			TestClassSourceWithUnreadableProperty source = new TestClassSourceWithUnreadableProperty();
			source.Property1 = 1;
			source.Property2 = "value2";
			container.AddExtension(
				new TestHelperExtension(context =>
				{
					IContainerPolicyCreator creator
						= new ConstructorArgumentMatchingPolicyCreator(typeof(TestClass));
					creator.CreatePolicies(context.Policies,
						instanceName,
						source,
						null);
				}));
			TestClass provider
				= container.Resolve<TestClass>(instanceName);
			Assert.IsNotNull(provider);
			Assert.AreEqual(source.Property1, provider.Property1);
			Assert.AreEqual("not set", provider.Property2);		
		}
		[TestMethod]
		public void TypesAreConsideredWhenMatchingProperties()
		{
			TestClassSourceWithPropertyOfNonMatchingType source = new TestClassSourceWithPropertyOfNonMatchingType();
			source.Property1 = 1;
			source.Property2 = new object();
			container.AddExtension(
				new TestHelperExtension(context =>
				{
					IContainerPolicyCreator creator
						= new ConstructorArgumentMatchingPolicyCreator(typeof(TestClass));
					creator.CreatePolicies(context.Policies,
						instanceName,
						source,
						null);
				}));
			TestClass provider
				= container.Resolve<TestClass>(instanceName);
			Assert.IsNotNull(provider);
			Assert.AreEqual(source.Property1, provider.Property1);
			Assert.AreEqual("not set", provider.Property2);		
		}
		[TestMethod]
		public void DerivedTypesAreAllowedWhenMatchingProperties()
		{
			TestClassSource source = new TestClassSource();
			source.Property1 = 1;
			source.Property2 = "value2";
			container.AddExtension(
				new TestHelperExtension(context =>
				{
					IContainerPolicyCreator creator
						= new ConstructorArgumentMatchingPolicyCreator(typeof(TestClassWithInterfaceArgument));
					creator.CreatePolicies(context.Policies,
						instanceName,
						source,
						null);
				}));
			TestClassWithInterfaceArgument provider
				= container.Resolve<TestClassWithInterfaceArgument>(instanceName);
			Assert.IsNotNull(provider);
			Assert.AreEqual(source.Property1, provider.Property1);
			Assert.AreEqual("value2", provider.Property2);		
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AttemptToCreatePoliciesWithNonMatchingConfigurationObjectThrows()
		{
			container.AddExtension(
				new TestHelperExtension(context =>
				{
					IContainerPolicyCreator creator = new ConstructorArgumentMatchingPolicyCreator(typeof(TestClass));
					creator.CreatePolicies(context.Policies,
						instanceName,
						new TestClassEmptySource(),
						null);
				}));
		}
		public class TestClassWithNoArgsConstructor { }
		public class TestClass
		{
			public TestClass(int property1)
			{
				this.Property1 = property1;
				this.Property2 = "not set";
			}
			public TestClass(string property2, int property1)
			{
				this.Property1 = property1;
				this.Property2 = property2;
			}
			public int Property1 { get; set; }
			public string Property2 { get; set; }
		}
		public class TestClassWithItemArgument
		{
			public TestClassWithItemArgument(int property1)
			{
				this.Property1 = property1;
				this.Property2 = "no value set";
			}
			public TestClassWithItemArgument(string item, int property1)
			{
				this.Property1 = property1;
				this.Property2 = item;
			}
			public int Property1 { get; set; }
			public string Property2 { get; set; }
		}
		public class TestClassWhereLongestConstructorCannotBeMatched
		{
			public TestClassWhereLongestConstructorCannotBeMatched(int property1)
			{
				this.Property1 = property1;
			}
			public TestClassWhereLongestConstructorCannotBeMatched(string property2, int property1)
			{
				this.Property1 = property1;
				this.Property2 = property2;
			}
			public TestClassWhereLongestConstructorCannotBeMatched(
				string property2,
				int property1,
				object someParameterThatCannotBeMatched)
			{ }
			public int Property1 { get; set; }
			public string Property2 { get; set; }
		}
		public class TestClassWithInterfaceArgument
		{
			public TestClassWithInterfaceArgument(ICloneable property2, int property1)
			{
				this.Property1 = property1;
				this.Property2 = property2;
			}
			public int Property1 { get; set; }
			public ICloneable Property2 { get; set; }
		}
		public class TestClassSource : ConfigurationElement
		{
			public int Property1 { get; set; }
			public string Property2 { get; set; }
		}
		public class TestClassSourceWithUnreadableProperty : ConfigurationElement
		{
			public int Property1 { get; set; }
			public string Property2 { set { ;} }
		}
		public class TestClassEmptySource : ConfigurationElement { }
		public class TestClassSourceWithIndexerProperty : ConfigurationElement
		{
			public int Property1 { get; set; }
			public string this[short index] { get { return null; } }
		}
		public class TestClassSourceWithPropertyOfNonMatchingType : ConfigurationElement
		{
			public int Property1 { get; set; }
			public object Property2 { get; set; }
		}
	}
}
