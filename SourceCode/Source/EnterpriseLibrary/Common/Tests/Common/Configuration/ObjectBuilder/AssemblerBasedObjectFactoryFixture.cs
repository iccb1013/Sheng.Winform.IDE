/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	[TestClass]
	public class AssemblerBasedObjectFactoryFixture
	{
		private MockAssembledObjectFactory factory;
		private IBuilderContext context;
		private DictionaryConfigurationSource configurationSource;
		private ConfigurationReflectionCache reflectionCache;
		[TestInitialize]
		public void SetUp()
		{
			factory = new MockAssembledObjectFactory();
			context = new BuilderContext(null, null, null, null, null, null);
			configurationSource = new DictionaryConfigurationSource();
			reflectionCache = new ConfigurationReflectionCache();
			MockAssembler.ConstructorCalls = 0;
		}
		[TestCleanup]
		public void TearDown()
		{
			MockAssembler.ConstructorCalls = 0;
		}
		[TestMethod]
		[ExpectedException(typeof (InvalidOperationException))]
		public void CreationFromConfigurationWithoutAssemblerAttributeThrows()
		{
			MockConfigurationObjectBase configurationObject
				= new MockConfigurationObjectWithoutAssemblerAttribute();
			factory.Create(context, configurationObject, configurationSource, reflectionCache);
		}
		[TestMethod]
		[ExpectedException(typeof (InvalidOperationException))]
		public void CreationFromConfigurationWithInvalidAssemblerThrows()
		{
			MockConfigurationObjectBase configurationObject
				= new MockConfigurationObjectWithAssemblerAttributeForInvalidAssembler();
			factory.Create(context, configurationObject, configurationSource, reflectionCache);
		}
		[TestMethod]
		public void CreationFromConfigurationWithAssemblerAttributeSucceds()
		{
			MockConfigurationObjectBase configurationObject
				= new MockConfigurationObjectWithAssemblerAttribute();
			MockAssembledObject createdObject
				= factory.Create(context, configurationObject, configurationSource, reflectionCache);
			Assert.IsNotNull(createdObject);
		}
		[TestMethod]
		public void SecondCreationFromConfigurationWithAssemblerAttributeSuccedsUsingCachedAssembler()
		{
			MockConfigurationObjectBase configurationObject
				= new MockConfigurationObjectWithAssemblerAttribute();
			MockAssembledObject createdObject1
				= factory.Create(context, configurationObject, configurationSource, reflectionCache);
			MockAssembledObject createdObject2
				= factory.Create(context, configurationObject, configurationSource, reflectionCache);
			Assert.IsNotNull(createdObject1);
			Assert.IsNotNull(createdObject2);
			Assert.AreEqual(1, MockAssembler.ConstructorCalls);
		}
	}
    public class MockAssembledObject {}
    public class MockAssembledObjectFactory : AssemblerBasedCustomFactory<MockAssembledObject, MockConfigurationObjectBase>
    {
        protected override MockConfigurationObjectBase GetConfiguration(string name,
                                                                        IConfigurationSource configurationSource)
        {
            if ("existing name".Equals(name))
            {
                return new MockConfigurationObjectWithAssemblerAttribute();
            }
            return null;
        }
    }
    public class MockConfigurationObjectBase {}
    public class MockConfigurationObjectWithoutAssemblerAttribute : MockConfigurationObjectBase {}
    [Assembler(typeof(MockAssembler))]
    public class MockConfigurationObjectWithAssemblerAttribute : MockConfigurationObjectBase {}
    [Assembler(typeof(MockInvalidAssembler))]
    public class MockConfigurationObjectWithAssemblerAttributeForInvalidAssembler : MockConfigurationObjectBase {}
    public class MockAssembler : IAssembler<MockAssembledObject, MockConfigurationObjectBase>
    {
        public static int ConstructorCalls = 0;
        public MockAssembler()
        {
            ConstructorCalls++;
        }
        public MockAssembledObject Assemble(IBuilderContext context,
                                            MockConfigurationObjectBase objectConfiguration,
                                            IConfigurationSource configurationSource,
                                            ConfigurationReflectionCache reflectionCache)
        {
            return new MockAssembledObject();
        }
    }
    public class MockInvalidAssembler : IAssembler<MockAssembledObject, object>
    {
        public MockAssembledObject Assemble(IBuilderContext context,
                                            object objectConfiguration,
                                            IConfigurationSource configurationSource,
                                            ConfigurationReflectionCache reflectionCache)
        {
            return null;
        }
    }
}
