/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    static class ExceptionUtility
    {
        private static readonly Regex guidExpression =
            new Regex("[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", RegexOptions.Compiled);
        public static Guid LogServerException(Exception exception)
        {
            Guid handlingInstanceId = GetHandlingInstanceId(exception);
            bool logged = false;
            try
            {
                if (Logger.IsLoggingEnabled())
                {
                    IDictionary<string, object> properties = new Dictionary<string, object>();
                    properties.Add(Properties.Resources.HandlingInstanceID, handlingInstanceId);
                    Logger.Write(exception, properties);
                    logged = true;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(Properties.Resources.ServerUnhandledExceptionNotLogged, handlingInstanceId, e);
            }
            finally
            {
                if (!logged)
                {
                    Trace.TraceError(Properties.Resources.ServerUnhandledException, handlingInstanceId, exception);
                }                
            }
            return handlingInstanceId;
        }
        public static Guid GetHandlingInstanceId(Exception exception)
        {
            return GetHandlingInstanceId(exception, Guid.NewGuid());
        }
        public static Guid GetHandlingInstanceId(Exception exception, Guid optionalHandlingInstanceId)
        {
            Guid result = optionalHandlingInstanceId;
            Match match = guidExpression.Match(exception.Message);
            if (match.Success)
            {
                result = new Guid(match.Value);
            }
            return result;
        }
        public static string FormatExceptionMessage(string message, Guid handlingInstanceId)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = FormatExceptionMessage(Properties.Resources.ClientUnhandledExceptionMessage, handlingInstanceId);
            }
            return Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionUtility.FormatExceptionMessage(message, handlingInstanceId);
        }
        public static string GetMessage(Exception exception, string optionalMessage, Guid handlingInstanceId)
        {
            string result = exception.Message;
            if (!string.IsNullOrEmpty(optionalMessage))
            {
                result = FormatExceptionMessage(optionalMessage, handlingInstanceId);
            }
            return result;
        }
    }
}
