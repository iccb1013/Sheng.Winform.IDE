/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    public class MockThrowingExceptionHandler : IExceptionHandler
    {
		private const string handlerFailed = "Handler Failed";
        public MockThrowingExceptionHandler()
        {
        }
		public MockThrowingExceptionHandler(CustomHandlerData customHandlerData)
			: this()
		{		
		}
        public Exception HandleException(Exception exception, Guid correlationID)
        {
            throw new NotImplementedException(handlerFailed);
        }
    }
}
