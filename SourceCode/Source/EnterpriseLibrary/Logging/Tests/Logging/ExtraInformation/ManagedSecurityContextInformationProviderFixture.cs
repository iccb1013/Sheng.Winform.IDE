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
    public class ManagedSecurityContextInformationProviderFixture
    {
        Dictionary<string, object> dictionary;
        ManagedSecurityContextInformationProvider provider;
        [TestInitialize]
        public void SetUp()
        {
            dictionary = new Dictionary<string, object>();
        }
        [TestMethod]
        public void PopulateDictionaryFilledCorrectly()
        {
            provider = new ManagedSecurityContextInformationProvider();
            provider.PopulateDictionary(dictionary);
            Assert.IsTrue(dictionary.Count > 0, "Dictionary contains no items");
            AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.ManagedSecurity_AuthenticationType] as string, string.Format(Resources.ExtendedPropertyError, ""), "Authentication Type");
            AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.ManagedSecurity_IdentityName] as string, string.Format(Resources.ExtendedPropertyError, ""), "Identity Name");
            AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.ManagedSecurity_IsAuthenticated] as string, string.Format(Resources.ExtendedPropertyError, ""), "Is Authenticated");
        }
    }
}
