/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConfigurationManageabilityProviderFixture
    {
        [TestMethod]
        public void SubProvidersAreSetForNewInstance()
        {
            MockConfigurationElementManageabilityProvider subProvider1 = new MockConfigurationElementManageabilityProvider();
            MockConfigurationElementManageabilityProvider subProvider2 = new MockConfigurationElementManageabilityProvider();
            IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(String), subProvider1);
            subProviders.Add(typeof(Boolean), subProvider2);
            MockConfigurationSectionManageabilityProvider provider = new MockConfigurationSectionManageabilityProvider(subProviders);
            Assert.IsNull(provider.GetSubProvider(typeof(Int32)));
            Assert.AreSame(subProvider1, provider.GetSubProvider(typeof(String)));
            Assert.AreSame(subProvider2, provider.GetSubProvider(typeof(Boolean)));
        }
    }
}
