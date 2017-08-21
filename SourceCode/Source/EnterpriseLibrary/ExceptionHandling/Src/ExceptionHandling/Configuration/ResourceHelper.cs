/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    internal static class ResourceHelper
    {
        public static string GetExceptionMessage(string exceptionMessage, string exceptionMessageResourceName, string exceptionMessageResourceType)
        {
            if (!string.IsNullOrEmpty(exceptionMessageResourceName))
            {
                Type resourceType = Type.GetType(exceptionMessageResourceType, false);
                if (null != resourceType)
                {
                    exceptionMessage = ResourceStringLoader.LoadString(resourceType.FullName,
                        exceptionMessageResourceName,
                        resourceType.Assembly);
                }
            }
            return exceptionMessage;
        }
    }
}
