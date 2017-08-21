/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Tests
{
    [TestClass]
    public class ConfigurationSerializationFixture
    {
        const string policyName1 = "policy1";
        const string policyName2 = "policy2";
        const string typeName11 = "type11";
        const string typeName12 = "type12";
        const string handlerName111 = "handler111";
        const string handlerMessage111 = "hander message 111";
        const string handlerName112 = "handler112";
        const string handlerMessage112 = "hander message 112";
        const string handlerName121 = "handler121";
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }
        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            ExceptionHandlingSettings settings = new ExceptionHandlingSettings();
            ExceptionTypeData typeData11 = new ExceptionTypeData(typeName11, typeof(ArgumentNullException), PostHandlingAction.None);
            typeData11.ExceptionHandlers.Add(new ReplaceHandlerData(handlerName111, handlerMessage111, typeof(ApplicationException).AssemblyQualifiedName));
            typeData11.ExceptionHandlers.Add(new WrapHandlerData(handlerName112, handlerMessage112, typeof(ApplicationException).AssemblyQualifiedName));
            ExceptionTypeData typeData12 = new ExceptionTypeData(typeName12, typeof(ArgumentException), PostHandlingAction.NotifyRethrow);
            typeData12.ExceptionHandlers.Add(new CustomHandlerData(handlerName121, typeof(MockExceptionHandler)));
            ExceptionPolicyData policyData1 = new ExceptionPolicyData(policyName1);
            policyData1.ExceptionTypes.Add(typeData11);
            policyData1.ExceptionTypes.Add(typeData12);
            ExceptionPolicyData policyData2 = new ExceptionPolicyData(policyName2);
            settings.ExceptionPolicies.Add(policyData1);
            settings.ExceptionPolicies.Add(policyData2);
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ExceptionHandlingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);
            ExceptionHandlingSettings roSettigs = (ExceptionHandlingSettings)configurationSource.GetSection(ExceptionHandlingSettings.SectionName);
            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(2, roSettigs.ExceptionPolicies.Count);
            Assert.IsNotNull(roSettigs.ExceptionPolicies.Get(policyName1));
            Assert.AreEqual(2, roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Count);
            Assert.IsNotNull(roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11));
            Assert.AreSame(typeof(ArgumentNullException), roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).Type);
            Assert.AreEqual(2, roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Count);
            Assert.AreSame(typeof(ReplaceHandlerData), roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111).GetType());
            Assert.AreEqual(handlerMessage111, ((ReplaceHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111)).ExceptionMessage);
            Assert.AreEqual(typeof(ApplicationException), ((ReplaceHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName111)).ReplaceExceptionType);
            Assert.AreSame(typeof(WrapHandlerData), roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName112).GetType());
            Assert.AreEqual(handlerMessage112, ((WrapHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName112)).ExceptionMessage);
            Assert.AreEqual(typeof(ApplicationException), ((WrapHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName11).ExceptionHandlers.Get(handlerName112)).WrapExceptionType);
            Assert.IsNotNull(roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName12));
            Assert.AreSame(typeof(ArgumentException), roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName12).Type);
            Assert.AreEqual(1, roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName12).ExceptionHandlers.Count);
            Assert.AreSame(typeof(CustomHandlerData), roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName12).ExceptionHandlers.Get(handlerName121).GetType());
            Assert.AreEqual(typeof(MockExceptionHandler), ((CustomHandlerData)roSettigs.ExceptionPolicies.Get(policyName1).ExceptionTypes.Get(typeName12).ExceptionHandlers.Get(handlerName121)).Type);
            Assert.IsNotNull(roSettigs.ExceptionPolicies.Get(policyName2));
            Assert.AreEqual(0, roSettigs.ExceptionPolicies.Get(policyName2).ExceptionTypes.Count);
        }
    }
}
