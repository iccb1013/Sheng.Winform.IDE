/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    [CustomFactory(typeof(LogWriterStructureHolderCustomFactory))]
    public class LogWriterStructureHolder : IDisposable
    {
        private ICollection<ILogFilter> filters;
        private IDictionary<string, LogSource> traceSources;
        private LogSource allEventsTraceSource;
        private LogSource notProcessedTraceSource;
        private LogSource errorsTraceSource;
        private string defaultCategory;
        private bool tracingEnabled;
        private bool logWarningsWhenNoCategoriesMatch;
        private bool revertImpersonation;
        public LogWriterStructureHolder(
            ICollection<ILogFilter> filters,
            IDictionary<string, LogSource> traceSources,
            LogSource allEventsTraceSource,
            LogSource notProcessedTraceSource,
            LogSource errorsTraceSource,
            string defaultCategory,
            bool tracingEnabled,
            bool logWarningsWhenNoCategoriesMatch,
            bool revertImpersonation)
        {
            if (filters == null)
                throw new ArgumentNullException("filters");
            if (traceSources == null)
                throw new ArgumentNullException("traceSources");
            if (errorsTraceSource == null)
                throw new ArgumentNullException("errorsTraceSource");
            this.filters = filters;
            this.traceSources = traceSources;
            this.allEventsTraceSource = allEventsTraceSource;
            this.notProcessedTraceSource = notProcessedTraceSource;
            this.errorsTraceSource = errorsTraceSource;
            this.defaultCategory = defaultCategory;
            this.tracingEnabled = tracingEnabled;
            this.logWarningsWhenNoCategoriesMatch = logWarningsWhenNoCategoriesMatch;
            this.revertImpersonation = revertImpersonation;
        }
        public ICollection<ILogFilter> Filters
        {
            get { return filters; }
        }
        public IDictionary<string, LogSource> TraceSources
        {
            get { return traceSources; }
        }
        public LogSource AllEventsTraceSource
        {
            get { return allEventsTraceSource; }
        }
        public LogSource NotProcessedTraceSource
        {
            get { return notProcessedTraceSource; }
        }
        public LogSource ErrorsTraceSource
        {
            get { return errorsTraceSource; }
        }
        public string DefaultCategory
        {
            get { return defaultCategory; }
        }
        public bool TracingEnabled
        {
            get { return tracingEnabled; }
        }
        public bool LogWarningsWhenNoCategoriesMatch
        {
            get { return logWarningsWhenNoCategoriesMatch; }
        }
        public bool RevertImpersonation
        {
            get { return revertImpersonation; }
        }
        public void Dispose()
        {
            foreach (LogSource source in traceSources.Values)
            {
                source.Dispose();
            }
            DisposeSpecialLogSource(errorsTraceSource);
            DisposeSpecialLogSource(notProcessedTraceSource);
            DisposeSpecialLogSource(allEventsTraceSource);
        }
        private void DisposeSpecialLogSource(LogSource specialLogSource)
        {
            if (specialLogSource != null)
            {
                specialLogSource.Dispose();
            }
        }
    }
}
