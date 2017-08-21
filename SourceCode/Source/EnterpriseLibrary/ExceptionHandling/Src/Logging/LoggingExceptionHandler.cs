/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging
{
    [ConfigurationElementType(typeof(LoggingExceptionHandlerData))]
    public class LoggingExceptionHandler : IExceptionHandler
    {
        private readonly string logCategory;
        private readonly int eventId;
        private readonly TraceEventType severity;
        private readonly string defaultTitle;
        private readonly Type formatterType;
        private readonly int minimumPriority;
        private readonly LogWriter logWriter;
        public LoggingExceptionHandler(
            string logCategory,
            int eventId,
            TraceEventType severity,
            string title,
            int priority,
            Type formatterType,
            LogWriter writer)
        {
            this.logCategory = logCategory;
            this.eventId = eventId;
            this.severity = severity;
            this.defaultTitle = title;
            this.minimumPriority = priority;
            this.formatterType = formatterType;
            this.logWriter = writer;
        }
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            WriteToLog(CreateMessage(exception, handlingInstanceId), exception.Data);
            return exception;
        }
        protected virtual void WriteToLog(string logMessage, IDictionary exceptionData)
        {
            LogEntry entry = new LogEntry(
                logMessage,
                logCategory,
                minimumPriority,
                eventId,
                severity,
                defaultTitle,
                null);
            foreach (DictionaryEntry dataEntry in exceptionData)
            {
                if (dataEntry.Key is string)
                {
                    entry.ExtendedProperties.Add(dataEntry.Key as string, dataEntry.Value);
                }
            }
            this.logWriter.Write(entry);
        }
        protected virtual StringWriter CreateStringWriter()
        {
            return new StringWriter(CultureInfo.InvariantCulture);
        }
        protected virtual ExceptionFormatter CreateFormatter(
            StringWriter writer,
            Exception exception,
            Guid handlingInstanceID)
        {
            ConstructorInfo constructor = GetFormatterConstructor();
            return (ExceptionFormatter)constructor.Invoke(
                new object[] { writer, exception, handlingInstanceID }
                );
        }
        private ConstructorInfo GetFormatterConstructor()
        {
            Type[] types = new Type[] { typeof(TextWriter), typeof(Exception), typeof(Guid) };
            ConstructorInfo constructor = formatterType.GetConstructor(types);
            if (constructor == null)
            {
                throw new ExceptionHandlingException(
                    string.Format(Resources.Culture, Resources.MissingConstructor, formatterType.AssemblyQualifiedName));
            }
            return constructor;
        }
        private string CreateMessage(Exception exception, Guid handlingInstanceID)
        {
            StringWriter writer = null;
            StringBuilder stringBuilder = null;
            try
            {
                writer = CreateStringWriter();
                ExceptionFormatter formatter = CreateFormatter(writer, exception, handlingInstanceID);
                formatter.Format();
                stringBuilder = writer.GetStringBuilder();
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            return stringBuilder.ToString();
        }
    }
}
