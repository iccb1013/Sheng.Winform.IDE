/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    [TestClass]
    public class UnmanagedSecurityContextInformationProviderFixture
    {
        Dictionary<string, object> dictionary;
        UnmanagedSecurityContextInformationProvider provider;
        [TestInitialize]
        public void SetUp()
        {
            dictionary = new Dictionary<string, object>();
        }
        [TestMethod]
        public void PopulateDictionaryFilledCorrectly()
        {
            provider = new UnmanagedSecurityContextInformationProvider();
            provider.PopulateDictionary(dictionary);
            Assert.AreEqual(2, dictionary.Count);
            AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.UnmanagedSecurity_CurrentUser] as string, string.Format(Resources.ExtendedPropertyError, ""), "CurrentUser");
            AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.UnmanagedSecurity_ProcessAccountName] as string, string.Format(Resources.ExtendedPropertyError, ""), "ProcessAccountName");
        }
    }
}
