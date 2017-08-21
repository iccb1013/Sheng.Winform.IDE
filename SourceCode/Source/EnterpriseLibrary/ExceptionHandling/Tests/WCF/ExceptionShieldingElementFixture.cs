/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ServiceModel.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [TestClass]
    public class ExceptionShieldingElementFixture
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            ExceptionShieldingElement element = new ExceptionShieldingElement();
            Assert.IsNotNull(element);
            Assert.AreEqual(ExceptionShielding.DefaultExceptionPolicy, element.ExceptionPolicyName);
        }
        [TestMethod]
        public void CanAssignExceptionPolicyName()
        {
            ExceptionShieldingElement element = new ExceptionShieldingElement();
            element.ExceptionPolicyName = "MyPolicy";
            Assert.AreEqual("MyPolicy", element.ExceptionPolicyName);
        }
        [TestMethod]
        public void ShouldGetDefaultValueOnNullExceptionPolicyName()
        {
            ExceptionShieldingElement element = new ExceptionShieldingElement();
            element.ExceptionPolicyName = null;
            Assert.AreEqual(ExceptionShielding.DefaultExceptionPolicy, element.ExceptionPolicyName);
        }
        [TestMethod]
        public void ShouldGetDefaultValueOnEmptyExceptionPolicyName()
        {
            ExceptionShieldingElement element = new ExceptionShieldingElement();
            element.ExceptionPolicyName = "";
            Assert.AreEqual(ExceptionShielding.DefaultExceptionPolicy, element.ExceptionPolicyName);
        }
        [TestMethod]
        public void CanCopyFrom()
        {
            ExceptionShieldingElement element = new ExceptionShieldingElement();
            element.ExceptionPolicyName = "Policy1";
            ExceptionShieldingElement from = new ExceptionShieldingElement();
            from.ExceptionPolicyName = "Policy2";
            element.CopyFrom((ServiceModelExtensionElement)from);
            Assert.AreEqual(from.ExceptionPolicyName, element.ExceptionPolicyName);
            Assert.AreEqual("Policy2", element.ExceptionPolicyName);
        }
        [TestMethod]
        public void ShouldGetExceptionShieldingBehaviorType()
        {
            ExceptionShieldingElement element = new ExceptionShieldingElement();
            Assert.AreEqual(typeof(ExceptionShieldingBehavior), element.BehaviorType);
        }
    }
}
