/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace EnterpriseLibrary.Logging.Database.Tests
{
    [TestClass]
    public class APTCAFixture
    {
        [TestMethod]
        public void AptcaIsPresentInLoggingDatabase()
        {
            try
            {
                ZoneIdentityPermission zoneIdentityPermission = new ZoneIdentityPermission(PermissionState.None);
                zoneIdentityPermission.Deny();
                Type type = typeof(FormattedDatabaseTraceListenerData);
                object createdObject = Activator.CreateInstance(type);
            }
            finally
            {
                ZoneIdentityPermission.RevertDeny();
            }
        }
    }
}
