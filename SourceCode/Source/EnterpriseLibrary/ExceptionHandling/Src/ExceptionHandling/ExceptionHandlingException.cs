/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Runtime.Serialization;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    [Serializable]
    public class ExceptionHandlingException : Exception
    {
        public ExceptionHandlingException() : base()
        {
        }
        public ExceptionHandlingException(string message) : base(message)
        {
        }
        public ExceptionHandlingException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected ExceptionHandlingException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
