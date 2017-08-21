/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    sealed class LoggingSettingsNodeBuilder : NodeBuilder
    {
        private LoggingSettings settings;
        private LoggingSettingsNode node;
        public LoggingSettingsNodeBuilder(IServiceProvider serviceProvider, LoggingSettings settings)
            : base(serviceProvider)
        {
            this.settings = settings;
        }
        public LoggingSettingsNode Build()
        {
            node = new LoggingSettingsNode();
            BuildLogFilters();
            BuildFormatters();
            node.LogWarningWhenNoCategoriesMatch = settings.LogWarningWhenNoCategoriesMatch;
            node.TracingEnabled = settings.TracingEnabled;
            node.RequirePermission = settings.SectionInformation.RequirePermission;
            node.RevertImpersonation = settings.RevertImpersonation;
            return node;
        }
        private void BuildSpecialTraceSources(TraceListenerCollectionNode listeners)
        {
            SpecialTraceSourcesNodeBuilder builder
                = new SpecialTraceSourcesNodeBuilder(ServiceProvider, settings.SpecialTraceSources, listeners);
            node.AddNode(builder.Build());
        }
        private void BuildFormatters()
        {
            FormatterCollectionNodeBuilder builder
                = new FormatterCollectionNodeBuilder(ServiceProvider, settings.Formatters);
            FormatterCollectionNode formatters = builder.Build();
            BuildTraceListeners(formatters);
            node.AddNode(formatters);
        }
        private void BuildTraceListeners(FormatterCollectionNode formatters)
        {
            TraceListenerCollectionNodeBuilder builder
                = new TraceListenerCollectionNodeBuilder(ServiceProvider, settings.TraceListeners, formatters);
            TraceListenerCollectionNode listeners = builder.Build();
            BuildCategoryTraceSources(listeners);
            BuildSpecialTraceSources(listeners);
            node.AddNode(listeners);
        }
        private void BuildCategoryTraceSources(TraceListenerCollectionNode listeners)
        {
            CategoryTraceSourceCollectionNodeBuilder builder
                = new CategoryTraceSourceCollectionNodeBuilder(ServiceProvider, settings.TraceSources, listeners);
            CategoryTraceSourceCollectionNode traceSources = builder.Build();
            foreach (CategoryTraceSourceNode traceSource in traceSources.Nodes)
            {
                if (traceSource.Name == settings.DefaultCategory)
                {
                    node.DefaultCategory = traceSource;
                }
            }
            node.AddNode(traceSources);
        }
        private void BuildLogFilters()
        {
            LogFilterCollectionNodeBuilder builder
                = new LogFilterCollectionNodeBuilder(ServiceProvider, settings.LogFilters);
            node.AddNode(builder.Build());
        }
    }
}
