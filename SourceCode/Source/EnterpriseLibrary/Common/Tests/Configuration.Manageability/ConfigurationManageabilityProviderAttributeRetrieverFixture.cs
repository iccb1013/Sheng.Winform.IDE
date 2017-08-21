/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConfigurationManageabilityProviderAttributeRetrieverFixture
    {
        [TestMethod]
        public void CanLoadAttributesFromAssembly()
        {
            List<ConfigurationSectionManageabilityProviderAttribute> sectionManageabilityProviderAttributes
                = new List<ConfigurationSectionManageabilityProviderAttribute>();
            List<ConfigurationElementManageabilityProviderAttribute> elementManageabilityProviderAttributes
                = new List<ConfigurationElementManageabilityProviderAttribute>();
            String[] assemblyNames = new String[] { Assembly.GetExecutingAssembly().GetName().Name + ".dll" };
            ConfigurationManageabilityProviderAttributeRetriever retriever = new ConfigurationManageabilityProviderAttributeRetriever(assemblyNames);
            sectionManageabilityProviderAttributes.AddRange(retriever.SectionManageabilityProviderAttributes);
            elementManageabilityProviderAttributes.AddRange(retriever.ElementManageabilityProviderAttributes);
            Assert.AreEqual(1, sectionManageabilityProviderAttributes.Count);
            Assert.AreEqual("section1", sectionManageabilityProviderAttributes[0].SectionName);
            Assert.AreSame(typeof(MockConfigurationSectionManageabilityProvider), sectionManageabilityProviderAttributes[0].ManageabilityProviderType);
            Assert.AreEqual(1, elementManageabilityProviderAttributes.Count);
        }
    }
}
