/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	public class LogWriterStructureHolderCustomFactory : ICustomFactory
	{
		public static LogWriterStructureHolderCustomFactory Instance = new LogWriterStructureHolderCustomFactory();
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			LoggingSettings loggingSettings = LoggingSettings.GetLoggingSettings(configurationSource);
			ValidateLoggingSettings(loggingSettings);
			TraceListenerCustomFactory.TraceListenerCache traceListenerCache
				= TraceListenerCustomFactory.CreateTraceListenerCache(loggingSettings.TraceListeners.Count);
			ICollection<ILogFilter> logFilters = new List<ILogFilter>();
			foreach (LogFilterData logFilterData in loggingSettings.LogFilters)
			{
				logFilters.Add(LogFilterCustomFactory.Instance.Create(context, logFilterData, configurationSource, reflectionCache));
			}
			IDictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
			foreach (TraceSourceData traceSourceData in loggingSettings.TraceSources)
			{
				traceSources.Add(traceSourceData.Name, LogSourceCustomFactory.Instance.Create(context, traceSourceData, configurationSource, reflectionCache, traceListenerCache));
			}
			LogSource allEventsTraceSource
				= LogSourceCustomFactory.Instance.Create(context, loggingSettings.SpecialTraceSources.AllEventsTraceSource, configurationSource, reflectionCache, traceListenerCache);
			LogSource notProcessedTraceSource
				= LogSourceCustomFactory.Instance.Create(context, loggingSettings.SpecialTraceSources.NotProcessedTraceSource, configurationSource, reflectionCache, traceListenerCache);
			LogSource errorsTraceSource
				= LogSourceCustomFactory.Instance.Create(context, loggingSettings.SpecialTraceSources.ErrorsTraceSource, configurationSource, reflectionCache, traceListenerCache);
			LogWriterStructureHolder createdObject
				= new LogWriterStructureHolder(
					logFilters,
					traceSources,
					allEventsTraceSource,
					notProcessedTraceSource,
					errorsTraceSource,
					loggingSettings.DefaultCategory,
					loggingSettings.TracingEnabled,
					loggingSettings.LogWarningWhenNoCategoriesMatch,
                    loggingSettings.RevertImpersonation);
			return createdObject;
		}
		private void ValidateLoggingSettings(LoggingSettings loggingSettings)
		{
			if (loggingSettings == null)
			{
				throw new System.Configuration.ConfigurationErrorsException(Resources.ExceptionLoggingSectionNotFound);
			}
		}
	}
}
