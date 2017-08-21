/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionFormatterFixture
    {
        const string fileNotFoundMessage = "The file can't be found";
        const string theFile = "theFile";
        const string loggedTimeStampFailMessage = "Logged TimeStamp is not within a one minute time window";
        const string machineName = "MachineName";
        const string timeStamp = "TimeStamp";
        const string appDomainName = "AppDomainName";
        const string threadIdentity = "ThreadIdentity";
        const string windowsIdentity = "WindowsIdentity";
        const string fieldString = "FieldString";
        const string mockFieldString = "MockFieldString";
        const string propertyString = "PropertyString";
        const string mockPropertyString = "MockPropertyString";
        const string message = "Message";
        const string computerName = "COMPUTERNAME";
        const string permissionDenied = "Permission Denied";
        [TestMethod]
        public void AdditionalInfoTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception exception = new FileNotFoundException(fileNotFoundMessage, theFile);
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);
            formatter.Format();
            if (string.Compare(permissionDenied, formatter.AdditionalInfo[machineName]) != 0)
            {
                Assert.AreEqual(Environment.MachineName, formatter.AdditionalInfo[machineName]);
            }
            DateTime minimumTime = DateTime.UtcNow.AddMinutes(-1);
            DateTime loggedTime = DateTime.Parse(formatter.AdditionalInfo[timeStamp]);
            if (DateTime.Compare(minimumTime, loggedTime) > 0)
            {
                Assert.Fail(loggedTimeStampFailMessage);
            }
            Assert.AreEqual(AppDomain.CurrentDomain.FriendlyName, formatter.AdditionalInfo[appDomainName]);
            Assert.AreEqual(Thread.CurrentPrincipal.Identity.Name, formatter.AdditionalInfo[threadIdentity]);
            if (string.Compare(permissionDenied, formatter.AdditionalInfo[windowsIdentity]) != 0)
            {
                Assert.AreEqual(WindowsIdentity.GetCurrent().Name, formatter.AdditionalInfo[windowsIdentity]);
            }
        }
        [TestMethod]
        public void ReflectionFormatterReadTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            MockTextExceptionFormatter formatter
                = new MockTextExceptionFormatter(writer, new MockException(), Guid.Empty);
            formatter.Format();
            Assert.AreEqual(formatter.fields[fieldString], mockFieldString);
            Assert.AreEqual(formatter.properties[propertyString], mockPropertyString);
            Assert.AreEqual(null, formatter.properties[message]);
        }
        [TestMethod]
        public void ReflectionFormatterReadSecurityExceptionPropertiesWithoutPermissionTest()
        {
            SecurityPermission denyPermission
                = new SecurityPermission(SecurityPermissionFlag.ControlPolicy | SecurityPermissionFlag.ControlEvidence);
            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(denyPermission);
            permissions.Deny();
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            SecurityException exception = null;
            try
            {
                DemandException(denyPermission);
            }
            catch (SecurityException e)
            {
                exception = e;
            }
            MockTextExceptionFormatter formatter = new MockTextExceptionFormatter(writer, exception, Guid.Empty);
            formatter.Format();
            CodeAccessPermission.RevertDeny();
            formatter = new MockTextExceptionFormatter(writer, exception, Guid.Empty);
            formatter.Format();
            Assert.AreEqual(exception.Demanded.ToString(), formatter.properties["Demanded"]);
        }
        static void DemandException(SecurityPermission denyPermission)
        {
            denyPermission.Demand();
        }
        [TestMethod]
        public void SkipsIndexerProperties()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception exception = new FileNotFoundExceptionWithIndexer(fileNotFoundMessage, theFile);
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);
            formatter.Format();
            if (string.Compare(permissionDenied, formatter.AdditionalInfo[machineName]) != 0)
            {
                Assert.AreEqual(Environment.MachineName, formatter.AdditionalInfo[machineName]);
            }
            DateTime minimumTime = DateTime.UtcNow.AddMinutes(-1);
            DateTime loggedTime = DateTime.Parse(formatter.AdditionalInfo[timeStamp]);
            if (DateTime.Compare(minimumTime, loggedTime) > 0)
            {
                Assert.Fail(loggedTimeStampFailMessage);
            }
            Assert.AreEqual(AppDomain.CurrentDomain.FriendlyName, formatter.AdditionalInfo[appDomainName]);
            Assert.AreEqual(Thread.CurrentPrincipal.Identity.Name, formatter.AdditionalInfo[threadIdentity]);
            if (string.Compare(permissionDenied, formatter.AdditionalInfo[windowsIdentity]) != 0)
            {
                Assert.AreEqual(WindowsIdentity.GetCurrent().Name, formatter.AdditionalInfo[windowsIdentity]);
            }
        }
        public class FileNotFoundExceptionWithIndexer : FileNotFoundException
        {
            public FileNotFoundExceptionWithIndexer(string message,
                                                    string fileName)
                : base(message, fileName) { }
            public string this[int index]
            {
                get { return null; }
            }
        }
    }
}
