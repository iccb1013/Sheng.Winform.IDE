/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    public class MockContextUtils : IContextUtils
    {
        public string GetActivityId()
        {
            throw new COMException();
        }
        public string GetApplicationId()
        {
            throw new COMException();
        }
        public string GetTransactionId()
        {
            throw new COMException();
        }
        public string GetDirectCallerAccountName()
        {
            throw new COMException();
        }
        public string GetOriginalCallerAccountName()
        {
            throw new COMException();
        }
    }
}
