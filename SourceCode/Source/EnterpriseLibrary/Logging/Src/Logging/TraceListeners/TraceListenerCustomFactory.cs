/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    public class TraceListenerCustomFactory : AssemblerBasedCustomFactory<TraceListener, TraceListenerData>
    {
        public static TraceListenerCustomFactory Instance = new TraceListenerCustomFactory();
        public static TraceListenerCache CreateTraceListenerCache(int size)
        {
            return new TraceListenerCache(size);
        }
        public override TraceListener Create(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            TraceListener createdObject = base.Create(context, objectConfiguration, configurationSource, reflectionCache);
            createdObject.Name = objectConfiguration.Name;
            createdObject.TraceOutputOptions = objectConfiguration.TraceOutputOptions;
            if (objectConfiguration.Filter != SourceLevels.All)
            {
                createdObject.Filter = new EventTypeFilter(objectConfiguration.Filter);
            }
            return createdObject;
        }
        public TraceListener Create(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache, TraceListenerCache traceListenersCache)
        {
            TraceListener createdObject;
            if (!traceListenersCache.cache.TryGetValue(name, out createdObject))
            {
                createdObject = Create(context, name, configurationSource, reflectionCache);
                traceListenersCache.cache.Add(name, createdObject);
            }
            return createdObject;
        }
        protected override TraceListenerData GetConfiguration(string name, IConfigurationSource configurationSource)
        {
            LoggingSettings settings = LoggingSettings.GetLoggingSettings(configurationSource);
            ValidateSettings(settings);
            TraceListenerData objectConfiguration = settings.TraceListeners.Get(name);
            ValidateConfiguration(objectConfiguration, name);
            return objectConfiguration;
        }
        private void ValidateConfiguration(TraceListenerData objectConfiguration, string name)
        {
            if (objectConfiguration == null)
            {
                throw new ConfigurationErrorsException(
                    string.Format(
                        Resources.Culture,
                        Resources.ExceptionTraceListenerConfigurationNotFound,
                        name));
            }
        }
        private void ValidateSettings(LoggingSettings settings)
        {
            if (settings == null)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionLoggingSectionNotFound);
            }
        }
        public struct TraceListenerCache
        {
            internal Dictionary<string, TraceListener> cache;
            internal TraceListenerCache(int size)
            {
                cache = new Dictionary<string, TraceListener>(size);
            }
        }
    }
}
