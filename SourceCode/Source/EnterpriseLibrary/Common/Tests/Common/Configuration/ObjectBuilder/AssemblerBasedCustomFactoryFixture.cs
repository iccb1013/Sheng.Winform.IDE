/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
    [TestClass]
    public class AssemblerBasedCustomFactoryFixture
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
		}
        [TestMethod]
        public void RequestForExistingIdWorks()
        {
            object createdObject
                = factory.CreateObject(context, "existing name", configurationSource, reflectionCache);
            Assert.IsNotNull(createdObject);
            Assert.AreSame(typeof(MockAssembledObject), createdObject.GetType());
        }
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void RequestForMissingIdThrows()
        {
            factory.CreateObject(context, "non existing name", configurationSource, reflectionCache);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestForNullIdThrows()
        {
            factory.CreateObject(context, null, configurationSource, reflectionCache);
        }
    }
}
