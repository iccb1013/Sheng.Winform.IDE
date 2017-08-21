/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Data.SqlCe.Tests.VSTS
{
    [TestClass]
    public class APTCAFixture
    {
        [TestMethod]
        public void CheckAptcaIsPresentInDataSqlCe()
        {
            try
            {
                ZoneIdentityPermission zoneIdentityPermission = new ZoneIdentityPermission(PermissionState.None);
                zoneIdentityPermission.Deny();
                Type type = typeof(SqlCeDatabase);
                object createdObject = Activator.CreateInstance(type, "connectionString");
            }
            finally
            {
                ZoneIdentityPermission.RevertDeny();
            }
        }
    }
}
