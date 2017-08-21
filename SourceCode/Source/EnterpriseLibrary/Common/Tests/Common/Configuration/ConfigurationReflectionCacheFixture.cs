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
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class ConfigurationReflectionCacheFixture
    {
        ConfigurationReflectionCache cache;
        [TestInitialize]
        public void SetUp()
        {
            cache = new ConfigurationReflectionCache();
        }
        [TestMethod]
        public void CanGetExistingAttributeForTheFirstTime()
        {
            MockAttribute attribute = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA));
            Assert.IsNotNull(attribute);
            Assert.AreEqual(1, attribute.value);
        }
        [TestMethod]
        public void ExistingAttributeIsCached()
        {
            cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA));
            Assert.IsTrue(cache.HasCachedCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA)));
        }
        [TestMethod]
        public void CanGetExistingAttributeForTheSecondTime()
        {
            cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA));
            MockAttribute attribute = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA));
            Assert.IsNotNull(attribute);
            Assert.AreEqual(1, attribute.value);
        }
        [TestMethod]
        public void GetsNullForNonExistingAttributeForTheFirstTime()
        {
            MockAttribute attribute = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithoutMockAttribute));
            Assert.IsNull(attribute);
        }
        [TestMethod]
        public void GetsNullForInheritedAttributeForTheFirstTime()
        {
            MockAttribute attribute = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithInheritedMockAttribute));
            Assert.IsNull(attribute);
        }
        [TestMethod]
        public void NonExistingAttributeIsCached()
        {
            cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithoutMockAttribute));
            cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithoutMockAttribute));
            Assert.IsTrue(cache.HasCachedCustomAttribute<MockAttribute>(typeof(TypeWithoutMockAttribute)));
        }
        [TestMethod]
        public void CanGetNonExistingAttributeForTheSecondTime()
        {
            cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithoutMockAttribute));
            MockAttribute attribute = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithoutMockAttribute));
            Assert.IsNull(attribute);
        }
        [TestMethod]
        public void CanCacheSameAttributeForDifferentTypes()
        {
            MockAttribute attributeA = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA));
            Assert.IsTrue(cache.HasCachedCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA)));
            Assert.IsFalse(cache.HasCachedCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeB)));
            MockAttribute attributeB = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeB));
            Assert.IsTrue(cache.HasCachedCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA)));
            Assert.IsTrue(cache.HasCachedCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeB)));
            Assert.AreEqual(1, attributeA.value);
            Assert.AreEqual(2, attributeB.value);
        }
        [TestMethod]
        public void CanGetExistingAttributeForTheFirstTimeWithInheritance()
        {
            MockAttribute attribute = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA), true);
            Assert.IsNotNull(attribute);
            Assert.AreEqual(1, attribute.value);
        }
        [TestMethod]
        public void CanGetExistingAttributeForTheSecondTimeWithInheritance()
        {
            cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA), true);
            MockAttribute attribute = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithMockAttributeA), true);
            Assert.IsNotNull(attribute);
            Assert.AreEqual(1, attribute.value);
        }
        [TestMethod]
        public void GetsNullForNonExistingAttributeForTheFirstTimeWithInheritance()
        {
            MockAttribute attribute = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithoutMockAttribute), true);
            Assert.IsNull(attribute);
        }
        [TestMethod]
        public void CanGetInheritedAttributeForTheFirstTimeWithInheritance()
        {
            MockAttribute attribute = cache.GetCustomAttribute<MockAttribute>(typeof(TypeWithInheritedMockAttribute), true);
            Assert.IsNotNull(attribute);
            Assert.AreEqual(1, attribute.value);
        }
        [TestMethod]
        public void CanGetCustomFactoryForAnnotatedTypeTheFirstTime()
        {
            MockImplementor.constructorCalls = 0;
            ICustomFactory factory = cache.GetCustomFactory(typeof(TypeWithCustomFactoryAttribute));
            Assert.IsNotNull(factory);
            Assert.AreSame(typeof(MockImplementor), factory.GetType());
            Assert.AreEqual(1, MockImplementor.constructorCalls);
        }
        [TestMethod]
        public void CanGetCachedCustomFactoryForAnnotatedTypeTheSecondTime()
        {
            MockImplementor.constructorCalls = 0;
            ICustomFactory factory1 = cache.GetCustomFactory(typeof(TypeWithCustomFactoryAttribute));
            ICustomFactory factory2 = cache.GetCustomFactory(typeof(TypeWithCustomFactoryAttribute));
            Assert.IsNotNull(factory1);
            Assert.IsNotNull(factory2);
            Assert.AreSame(typeof(MockImplementor), factory1.GetType());
            Assert.AreSame(typeof(MockImplementor), factory2.GetType());
            Assert.AreEqual(1, MockImplementor.constructorCalls);
            Assert.AreSame(factory1, factory2);
        }
        [TestMethod]
        public void CanGetNullCustomFactoryForNonAnnotatedTypeTheFirstTime()
        {
            MockImplementor.constructorCalls = 0;
            ICustomFactory factory = cache.GetCustomFactory(typeof(TypeWithNoCustomFactoryAttribute));
            Assert.IsNull(factory);
            Assert.AreEqual(0, MockImplementor.constructorCalls);
        }
        [TestMethod]
        public void CanGetNameMapperForAnnotatedTypeTheFirstTime()
        {
            MockImplementor.constructorCalls = 0;
            IConfigurationNameMapper mapper = cache.GetConfigurationNameMapper(typeof(TypeWithCustomFactoryAttribute));
            Assert.IsNotNull(mapper);
            Assert.AreSame(typeof(MockImplementor), mapper.GetType());
            Assert.AreEqual(1, MockImplementor.constructorCalls);
        }
        [TestMethod]
        public void CanGetCachedNameMapperForAnnotatedTypeTheSecondTime()
        {
            MockImplementor.constructorCalls = 0;
            IConfigurationNameMapper mapper1 = cache.GetConfigurationNameMapper(typeof(TypeWithCustomFactoryAttribute));
            IConfigurationNameMapper mapper2 = cache.GetConfigurationNameMapper(typeof(TypeWithCustomFactoryAttribute));
            Assert.IsNotNull(mapper1);
            Assert.IsNotNull(mapper2);
            Assert.AreSame(typeof(MockImplementor), mapper1.GetType());
            Assert.AreSame(typeof(MockImplementor), mapper2.GetType());
            Assert.AreEqual(1, MockImplementor.constructorCalls);
            Assert.AreSame(mapper1, mapper2);
        }
        [TestMethod]
        public void CanGetNullNameMapperForNonAnnotatedTypeTheFirstTime()
        {
            MockImplementor.constructorCalls = 0;
            IConfigurationNameMapper mapper = cache.GetConfigurationNameMapper(typeof(TypeWithNoCustomFactoryAttribute));
            Assert.IsNull(mapper);
            Assert.AreEqual(0, MockImplementor.constructorCalls);
        }
    }
    public class MockAttribute : Attribute
    {
        internal int value;
        public MockAttribute(int value)
        {
            this.value = value;
        }
    }
    public class TypeWithoutMockAttribute {}
    [Mock(1)]
    public class TypeWithMockAttributeA {}
    [Mock(2)]
    public class TypeWithMockAttributeB {}
    public class TypeWithInheritedMockAttribute : TypeWithMockAttributeA {}
    [CustomFactory(typeof(MockImplementor))]
    [ConfigurationNameMapper(typeof(MockImplementor))]
    public class TypeWithCustomFactoryAttribute {}
    public class TypeWithNoCustomFactoryAttribute {}
    public class MockImplementor : ICustomFactory, IConfigurationNameMapper
    {
        internal static int constructorCalls = 0;
        public MockImplementor()
        {
            constructorCalls++;
        }
        public object CreateObject(IBuilderContext context,
                                   string name,
                                   IConfigurationSource configurationSource,
                                   ConfigurationReflectionCache reflectionCache)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public string MapName(string name,
                              IConfigurationSource configSource)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
