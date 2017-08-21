/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExcpetionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionHandlingSettingsNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void ExceptionHandlingSettingsNodeDefaults()
        {
            ExceptionHandlingSettingsNode settingsNode = new ExceptionHandlingSettingsNode();
            Assert.AreEqual("Exception Handling Application Block", settingsNode.Name);
        }
        [TestMethod]
        public void ExceptionHandlingSettingsNodeHasReadonlyName()
        {
            Assert.IsTrue(CommonUtil.IsPropertyReadOnly(typeof(ExceptionHandlingSettingsNode), "Name"));
        }
    }
}
