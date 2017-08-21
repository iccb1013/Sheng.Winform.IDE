/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class LogFilterCollectionNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void LogFilterCollectionNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(LogFilterCollectionNode), "Name"));
        }
    }
}
