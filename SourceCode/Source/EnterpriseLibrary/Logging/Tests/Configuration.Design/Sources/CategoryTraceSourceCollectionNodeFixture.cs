/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class CategoryTraceSourceCollectionNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CategorySourcesNamePropertyIsReadOnly()
        {
            Assert.AreEqual(false, CommonUtil.IsPropertyReadOnly(typeof(CategoryTraceSourceCollectionNode), "Name"));
        }
    }
}
