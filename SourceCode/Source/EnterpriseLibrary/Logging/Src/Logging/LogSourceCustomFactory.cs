/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	public class LogSourceCustomFactory
	{
        public static LogSourceCustomFactory Instance = new LogSourceCustomFactory();
		public LogSource Create(IBuilderContext context, TraceSourceData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache, TraceListenerCustomFactory.TraceListenerCache traceListenersCache)
		{
            List<TraceListener> traceListeners = new List<TraceListener>(objectConfiguration.TraceListeners.Count);
			foreach (TraceListenerReferenceData traceListenerReference in objectConfiguration.TraceListeners)
			{
				TraceListener traceListener
					= TraceListenerCustomFactory.Instance.Create(context, traceListenerReference.Name, configurationSource, reflectionCache, traceListenersCache);
                traceListeners.Add(traceListener);
			}
            LogSource createdObject
                = new LogSource(objectConfiguration.Name, traceListeners, objectConfiguration.DefaultLevel, objectConfiguration.AutoFlush);
            InstrumentationAttachmentStrategy instrumentationAttacher = new InstrumentationAttachmentStrategy();
            instrumentationAttacher.AttachInstrumentation(createdObject, configurationSource, reflectionCache);            
			return createdObject;
		}
	}
}
