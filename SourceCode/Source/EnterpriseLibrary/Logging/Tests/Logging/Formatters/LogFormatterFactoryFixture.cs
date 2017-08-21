/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
    [TestClass]
    public class LogFormatterFactoryFixture
    {
        IBuilderContext context;
        ConfigurationReflectionCache reflectionCache;
        [TestInitialize]
        public void SetUp()
        {
			context = new BuilderContext(new StrategyChain(), null, null, new PolicyList(), null, null);
            reflectionCache = new ConfigurationReflectionCache();
        }
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void CreationWithNullLoggingSettingsThrows()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            LogFormatterCustomFactory.Instance.Create(context, "formatter", configurationSource, reflectionCache);
        }
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void CreationWithMissingFormatterDataThrows()
        {
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            LogFormatterCustomFactory.Instance.Create(context, "formatter", helper.configurationSource, reflectionCache);
        }
    }
}
