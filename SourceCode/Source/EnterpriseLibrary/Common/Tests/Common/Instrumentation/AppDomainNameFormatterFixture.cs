/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class AppDomainNameFormatterFixture : MarshalByRefObject
    {
        [TestMethod]
        public void WillFormatNameWithAppDomainNamePrefix()
        {
            AppDomainNameFormatter nameFormatter = new AppDomainNameFormatter();
            string createdName = nameFormatter.CreateName("MyInstance");
            Assert.IsTrue(createdName.EndsWith(" - MyInstance"));
            Assert.IsTrue(createdName.Length <= 128);
        }
        [TestMethod]
        public void WillFormatNameUsingApplicationInstanceName()
        {
            string applicationInstanceName = "ApplicationInstanceName";
            string suffix = "MySuffix";
            string expectedInstanceName = string.Concat(applicationInstanceName, " - ", suffix);
            AppDomainNameFormatter formatter = new AppDomainNameFormatter(applicationInstanceName);
            string createdName = formatter.CreateName(suffix);
            Assert.AreEqual(expectedInstanceName, createdName);
        }
        [TestMethod]
        public void ShouldReplaceInvalidCharacters()
        {
            string invalidApplicationInstanceName = @"\\computer\object(parent/instance#index)\counter";
            string validApplicationIntanceName = "computerobjectparentinstanceindexcounter";
            validApplicationIntanceName = validApplicationIntanceName.Substring(0, 32);
            string suffix = "MySuffix";
            string expectedInstanceName = string.Concat(validApplicationIntanceName, " - ", suffix);
            AppDomainNameFormatter formatter = new AppDomainNameFormatter(invalidApplicationInstanceName);
            string createdName = formatter.CreateName(suffix);
            Assert.AreEqual(expectedInstanceName, createdName);
        }
    }
}
