/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
	[ConfigurationElementType(typeof(CustomHandlerData))]
	public class MockExceptionHandler : IExceptionHandler
    {
        public static int handleExceptionCount = 0;
        public static string lastMessage;
        public static Guid handlingInstanceID;
		public static NameValueCollection attributes;
		public MockExceptionHandler(NameValueCollection attributes)
		{
			MockExceptionHandler.attributes = attributes;
		}
        public static void Clear()
        {
            handleExceptionCount = 0;
            lastMessage = String.Empty;
            handlingInstanceID = Guid.Empty;
			attributes = null;
        }
        public static string FormatExceptionMessage(string message, Guid handlingInstanceID)
        {
            return ExceptionUtility.FormatExceptionMessage(message, handlingInstanceID);
        }
        public Exception HandleException(Exception ex, Guid handlingInstanceID)
        {
            handleExceptionCount++;
            lastMessage = ex.Message;
            MockExceptionHandler.handlingInstanceID = handlingInstanceID;
            return ex;
        }
    }
}
