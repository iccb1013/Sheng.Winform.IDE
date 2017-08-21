/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionPolicyNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void ExceptionPolicyNodeDefaults()
        {
            ExceptionPolicyNode policyNode = new ExceptionPolicyNode();
            Assert.AreEqual("Exception Policy", policyNode.Name);
        }
    }
}
