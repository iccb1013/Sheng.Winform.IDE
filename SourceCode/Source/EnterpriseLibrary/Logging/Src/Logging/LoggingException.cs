/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    [Serializable]
    [ComVisible(false)]
    public class LoggingException : Exception
    {
        public LoggingException() : base()
        {
        }
        public LoggingException(string message) : base(message)
        {
        }
        public LoggingException(string message, Exception exception) : base(message, exception)
        {
        }
        protected LoggingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
