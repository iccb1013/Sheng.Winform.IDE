/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Security.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class APTCAFixture
    {
        [TestMethod]
        public void AptcaIsPresentInLogging()
        {
            try
            {
                ZoneIdentityPermission zoneIdentityPermission = new ZoneIdentityPermission(PermissionState.None);
                zoneIdentityPermission.Deny();
                Type type = typeof(LogEntry);
                object createdObject = Activator.CreateInstance(type);
            }
            finally
            {
                ZoneIdentityPermission.RevertDeny();
            }
        }
    }
}
