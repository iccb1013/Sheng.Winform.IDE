/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    [ConfigurationElementType(typeof(ReplaceHandlerData))]
    public class ReplaceHandler : IExceptionHandler
    {
        private readonly IStringResolver exceptionMessageResolver;
        private readonly Type replaceExceptionType;
        public ReplaceHandler(string exceptionMessage, Type replaceExceptionType)
            : this(new ConstantStringResolver(exceptionMessage), replaceExceptionType)
        { }
        public ReplaceHandler(IStringResolver exceptionMessageResolver, Type replaceExceptionType)
        {
            if (replaceExceptionType == null) throw new ArgumentNullException("replaceExceptionType");
            if (!typeof(Exception).IsAssignableFrom(replaceExceptionType))
            {
                throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionTypeNotException, replaceExceptionType.Name), "replaceExceptionType");
            }
            this.exceptionMessageResolver = exceptionMessageResolver;
            this.replaceExceptionType = replaceExceptionType;
        }
        public Type ReplaceExceptionType
        {
            get { return replaceExceptionType; }
        }
        public string ExceptionMessage
        {
            get { return exceptionMessageResolver.GetString(); }
        }
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            return ReplaceException(
                ExceptionUtility.FormatExceptionMessage(ExceptionMessage, handlingInstanceId));
        }
        private Exception ReplaceException(string replaceExceptionMessage)
        {
            object[] extraParameters = new object[] { replaceExceptionMessage };
            return (Exception)Activator.CreateInstance(replaceExceptionType, extraParameters);
        }
    }
}
