/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class EmailTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInEmailTraceListenerNodeThrows()
        {
            new EmailTraceListenerNode(null);
        }
        [TestMethod]
        public void EmailTraceListenerNodeTest()
        {
            string name = "some name";
            string subjectSuffix = "subject suffix";
            string subjectPrefix = "subject prefix";
            string toAddress = "some to address";
            string fromAddress = "some from address";
            string smtpServer = "some server";
            int smtpPort = 123;
            EmailTraceListenerNode emailTraceListenerNode = new EmailTraceListenerNode();
            emailTraceListenerNode.Name = name;
            emailTraceListenerNode.SubjectLineStarter = subjectPrefix;
            emailTraceListenerNode.SubjectLineEnder = subjectSuffix;
            emailTraceListenerNode.ToAddress = toAddress;
            emailTraceListenerNode.FromAddress = fromAddress;
            emailTraceListenerNode.SmtpServer = smtpServer;
            emailTraceListenerNode.SmtpPort = smtpPort;
            ApplicationNode.AddNode(emailTraceListenerNode);
            EmailTraceListenerData nodeData = (EmailTraceListenerData)emailTraceListenerNode.TraceListenerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(subjectSuffix, nodeData.SubjectLineEnder);
            Assert.AreEqual(subjectPrefix, nodeData.SubjectLineStarter);
            Assert.AreEqual(toAddress, nodeData.ToAddress);
            Assert.AreEqual(fromAddress, nodeData.FromAddress);
            Assert.AreEqual(smtpPort, nodeData.SmtpPort);
            Assert.AreEqual(smtpServer, nodeData.SmtpServer);
        }
        [TestMethod]
        public void EmailTraceListenerNodeDataTest()
        {
            string name = "some name";
            string subjectSuffix = "subject suffix";
            string subjectPrefix = "subject prefix";
            string toAddress = "some to address";
            string fromAddress = "some from address";
            string smtpServer = "some server";
            int smtpPort = 123;
            SourceLevels filter = SourceLevels.Critical;
            EmailTraceListenerData emailTraceListenerData = new EmailTraceListenerData();
            emailTraceListenerData.Name = name;
            emailTraceListenerData.ToAddress = toAddress;
            emailTraceListenerData.FromAddress = fromAddress;
            emailTraceListenerData.SmtpServer = smtpServer;
            emailTraceListenerData.SmtpPort = smtpPort;
            emailTraceListenerData.SubjectLineEnder = subjectSuffix;
            emailTraceListenerData.SubjectLineStarter = subjectPrefix;
            emailTraceListenerData.Filter = filter;
            EmailTraceListenerNode emailTraceListenerNode = new EmailTraceListenerNode(emailTraceListenerData);
            ApplicationNode.AddNode(emailTraceListenerNode);
            Assert.AreEqual(name, emailTraceListenerNode.Name);
            Assert.AreEqual(subjectSuffix, emailTraceListenerNode.SubjectLineEnder);
            Assert.AreEqual(subjectPrefix, emailTraceListenerNode.SubjectLineStarter);
            Assert.AreEqual(toAddress, emailTraceListenerNode.ToAddress);
            Assert.AreEqual(fromAddress, emailTraceListenerNode.FromAddress);
            Assert.AreEqual(smtpPort, emailTraceListenerNode.SmtpPort);
            Assert.AreEqual(smtpServer, emailTraceListenerNode.SmtpServer);
            Assert.AreEqual(filter, emailTraceListenerNode.Filter);
        }
    }
}
