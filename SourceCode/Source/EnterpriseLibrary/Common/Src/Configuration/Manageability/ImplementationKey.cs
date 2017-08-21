/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    public struct ImplementationKey
    {
        public String ApplicationName;
        public Boolean EnableGroupPolicies;
        public String FileName;
        public ImplementationKey(String fileName,
                                 String applicationName,
                                 Boolean enableGroupPolicies)
        {
            FileName = fileName != null ? fileName.ToLower(CultureInfo.CurrentCulture) : null;
            ApplicationName = applicationName != null ? applicationName.ToLower(CultureInfo.CurrentCulture) : null;
            EnableGroupPolicies = enableGroupPolicies;
        }
    }
}
