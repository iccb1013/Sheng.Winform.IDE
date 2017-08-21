/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
    [TestClass]
    public class APTCAFixture
    {
        [TestMethod]
        public void AptcaIsPresentInCommon()
        {
            try
            {
                ZoneIdentityPermission zoneIdentityPermission = new ZoneIdentityPermission(PermissionState.None);
                zoneIdentityPermission.Deny();
                Type type = typeof(ByteArrayTypeConverter);
                object createdObject = Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ZoneIdentityPermission.RevertDeny();
            }
        }
    }
}
