/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Utility
{
    [TestClass]
    public class StringResolverFixture
    {
        [TestMethod]
        public void CanGetValueFromConstantStringResolver()
        {
            string value = "test string";
            IStringResolver resolver = new ConstantStringResolver(value);
            Assert.AreEqual(value, resolver.GetString());
        }
        [TestMethod]
        public void CanGetValueFromDelegateStringResolver()
        {
            string value = "test string";
            IStringResolver resolver = new DelegateStringResolver(() => value);
            Assert.AreEqual(value, resolver.GetString());
        }
        [TestMethod]
        public void ResourceStringResolverUsesTypeAndNameIfBothAreAvailable()
        {
            Type resourceType = typeof(Resources);
            string resourceName = "CategoryHelp";
            string fallbackValue = "fallback";
            IStringResolver resolver = new ResourceStringResolver(resourceType, resourceName, fallbackValue);
            Assert.AreEqual(Resources.CategoryHelp, resolver.GetString());
        }
        [TestMethod]
        public void ResourceStringResolverUsesFallbackValueIfTypeIsNull()
        {
            Type resourceType = null;
            string resourceName = "CategoryHelp";
            string fallbackValue = "fallback";
            IStringResolver resolver = new ResourceStringResolver(resourceType, resourceName, fallbackValue);
            Assert.AreEqual(fallbackValue, resolver.GetString());
        }
        [TestMethod]
        public void ResourceStringResolverUsesFallbackValueIfResourceNameIsNull()
        {
            Type resourceType = typeof(Resources);
            string resourceName = null;
            string fallbackValue = "fallback";
            IStringResolver resolver = new ResourceStringResolver(resourceType, resourceName, fallbackValue);
            Assert.AreEqual(fallbackValue, resolver.GetString());
        }
        [TestMethod]
        public void ResourceStringResolverUsesTypeNameAndNameIfBothAreAvailable()
        {
            string resourceTypeName = typeof(Resources).AssemblyQualifiedName;
            string resourceName = "CategoryHelp";
            string fallbackValue = "fallback";
            IStringResolver resolver = new ResourceStringResolver(resourceTypeName, resourceName, fallbackValue);
            Assert.AreEqual(Resources.CategoryHelp, resolver.GetString());
        }
    }
}
