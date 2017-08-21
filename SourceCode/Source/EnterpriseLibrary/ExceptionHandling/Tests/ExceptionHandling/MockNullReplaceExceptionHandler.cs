/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    public class MockReturnNullExceptionHandler : IExceptionHandler
    {
        public MockReturnNullExceptionHandler()
        {
        }
        public Exception HandleException(Exception exception, Guid handlingInstanceID)
        {
            return null;
        }
    }
}
