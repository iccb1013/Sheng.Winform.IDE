/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Security.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig = System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [TestClass]
    public class APTCAFixture
    {
        [TestMethod]
        public void CheckAptcaIsPresentInExceptionHandlingWCF()
        {
            try
            {
                ZoneIdentityPermission zone = new ZoneIdentityPermission(PermissionState.None);
                zone.Deny();
                Type type = typeof(ExceptionShieldingElement);
                object objectCreated = Activator.CreateInstance(type);
            }
            finally
            {
                ZoneIdentityPermission.RevertDeny();
            }
        }
    }
}
