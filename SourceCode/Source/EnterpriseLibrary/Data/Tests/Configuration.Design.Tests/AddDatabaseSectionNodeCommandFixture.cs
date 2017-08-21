/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    [TestClass]
    public class AddDatabaseSectionNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void ExectueAddsDefaultNodes()
        {
            AddDatabaseSectionNodeCommand cmd = new AddDatabaseSectionNodeCommand(ServiceProvider);
            cmd.Execute(ApplicationNode);
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            ConfigurationNode node = Hierarchy.FindNodeByType(typeof(DatabaseSectionNode));
            Assert.IsNotNull(node);
            node = Hierarchy.FindNodeByType(typeof(ConnectionStringSettingsNode));
            Assert.IsNotNull(node);
            node = Hierarchy.FindNodeByType(typeof(ProviderMappingsNode));
            Assert.IsNotNull(node);
        }
    }
}
