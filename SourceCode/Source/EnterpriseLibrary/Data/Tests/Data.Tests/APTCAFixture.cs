/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Data.Common;
using System.Security.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestClass]
    public class APTCAFixture
    {
        [TestMethod]
        public void AptcaIsPresentInData()
        {
            try
            {
                ZoneIdentityPermission zoneIdentityPermission = new ZoneIdentityPermission(PermissionState.None);
                zoneIdentityPermission.Deny();
                DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                Type type = typeof(GenericDatabase);
                object createdObject = Activator.CreateInstance(type, "connectionString", factory);
            }
            finally
            {
                ZoneIdentityPermission.RevertDeny();
            }
        }
    }
}
