/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
    [TestClass]
    public class ConfigurationNameProviderFixture
    {
        [TestMethod]
        public void NormalNamesAreNotConsideredMadeUp()
        {
            Assert.IsFalse(ConfigurationNameProvider.IsMadeUpName("foo"));
        }
        [TestMethod]
        public void MadeUpNamesAreConsideredMadeUp()
        {
            Assert.IsTrue(ConfigurationNameProvider.IsMadeUpName(ConfigurationNameProvider.MakeUpName()));
        }
        [TestMethod]
        public void EmptyStringIsNotConsideredMadeUp()
        {
            Assert.IsFalse(ConfigurationNameProvider.IsMadeUpName(String.Empty));
        }
        [TestMethod]
        public void NullNamesAreNotConsideredMadeUp()
        {
            Assert.IsFalse(ConfigurationNameProvider.IsMadeUpName(null));
        }
    }
}
