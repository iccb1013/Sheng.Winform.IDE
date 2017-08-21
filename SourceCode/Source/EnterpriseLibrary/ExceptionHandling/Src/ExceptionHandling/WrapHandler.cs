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
    [ConfigurationElementType(typeof(WrapHandlerData))]
    public class WrapHandler : IExceptionHandler
    {
        private readonly IStringResolver exceptionMessageResolver;
        private readonly Type wrapExceptionType;
        public WrapHandler(string exceptionMessage, Type wrapExceptionType)
            : this(new ConstantStringResolver(exceptionMessage), wrapExceptionType)
        { }
        public WrapHandler(IStringResolver exceptionMessageResolver, Type wrapExceptionType)
        {
            if (wrapExceptionType == null) throw new ArgumentNullException("wrapExceptionType");
            if (!typeof(Exception).IsAssignableFrom(wrapExceptionType))
            {
                throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionTypeNotException, wrapExceptionType.Name), "wrapExceptionType");
            }
            this.exceptionMessageResolver = exceptionMessageResolver;
            this.wrapExceptionType = wrapExceptionType;
        }
        public Type WrapExceptionType
        {
            get { return wrapExceptionType; }
        }
        public string WrapExceptionMessage
        {
            get { return exceptionMessageResolver.GetString(); }
        }
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            return WrapException(
                exception,
                ExceptionUtility.FormatExceptionMessage(WrapExceptionMessage, handlingInstanceId));
        }
        private Exception WrapException(Exception originalException, string wrapExceptionMessage)
        {
            object[] extraParameters = new object[2];
            extraParameters[0] = wrapExceptionMessage;
            extraParameters[1] = originalException;
            return (Exception)Activator.CreateInstance(wrapExceptionType, extraParameters);
        }
    }
}
