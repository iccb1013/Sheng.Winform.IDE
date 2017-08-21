/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    public class MockDebugUtilsThrowsSecurityException : IDebugUtils
    {
        public string GetStackTraceWithSourceInfo(StackTrace stackTrace)
        {
            throw new SecurityException();
        }
    }
}
