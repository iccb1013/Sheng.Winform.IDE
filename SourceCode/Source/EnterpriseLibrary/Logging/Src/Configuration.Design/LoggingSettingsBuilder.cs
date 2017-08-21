/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    public sealed class LoggingSettingsBuilder
    {
        IConfigurationUIHierarchy hierarchy;
        LoggingSettings loggingSettings;
        LoggingSettingsNode loggingSettingsNode;
        public LoggingSettingsBuilder(IServiceProvider serviceProvider,
                                      LoggingSettingsNode loggingSettingsNode)
        {
            this.loggingSettingsNode = loggingSettingsNode;
            hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
        }
        public LoggingSettings Build()
        {
            loggingSettings
                = new LoggingSettings(
                    loggingSettingsNode.Name,
                    loggingSettingsNode.TracingEnabled,
                    loggingSettingsNode.DefaultCategory == null ? string.Empty : loggingSettingsNode.DefaultCategory.Name);
            loggingSettings.LogWarningWhenNoCategoriesMatch = loggingSettingsNode.LogWarningWhenNoCategoriesMatch;
            if (!loggingSettingsNode.RevertImpersonation) 
                loggingSettings.RevertImpersonation = loggingSettingsNode.RevertImpersonation;
            if (!loggingSettingsNode.RequirePermission) 
                loggingSettings.SectionInformation.RequirePermission = loggingSettingsNode.RequirePermission;
            BuilFormatters();
            BuildLogFilters();
            BuildTraceListeners();
            BuildTraceSources();
            BuildSpecialTraceSources();
            return loggingSettings;
        }
        void BuildLogFilters()
        {
            LogFilterCollectionNode logFilterCollectionNode
                = (LogFilterCollectionNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(LogFilterCollectionNode));
            if (logFilterCollectionNode != null)
            {
                foreach (LogFilterNode filterNode in logFilterCollectionNode.Nodes)
                {
                    loggingSettings.LogFilters.Add(filterNode.LogFilterData);
                }
            }
        }
        void BuildSpecialTraceSources()
        {
            SpecialTraceSourcesNode specialSourcesNode
                = (SpecialTraceSourcesNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(SpecialTraceSourcesNode));
            if (specialSourcesNode != null)
            {
                ErrorsTraceSourceNode errorSourceNode
                    = (ErrorsTraceSourceNode)hierarchy.FindNodeByType(specialSourcesNode, typeof(ErrorsTraceSourceNode));
                if (errorSourceNode != null)
                {
                    loggingSettings.SpecialTraceSources.ErrorsTraceSource = errorSourceNode.TraceSourceData;
                    BuildTraceListenerReferences(loggingSettings.SpecialTraceSources.ErrorsTraceSource, errorSourceNode);
                }
                AllTraceSourceNode allTraceSourceNodes
                    = (AllTraceSourceNode)hierarchy.FindNodeByType(specialSourcesNode, typeof(AllTraceSourceNode));
                if (allTraceSourceNodes != null)
                {
                    loggingSettings.SpecialTraceSources.AllEventsTraceSource = allTraceSourceNodes.TraceSourceData;
                    BuildTraceListenerReferences(loggingSettings.SpecialTraceSources.AllEventsTraceSource, allTraceSourceNodes);
                }
                NotProcessedTraceSourceNode notProcessedSourceNode
                    = (NotProcessedTraceSourceNode)hierarchy.FindNodeByType(specialSourcesNode, typeof(NotProcessedTraceSourceNode));
                if (notProcessedSourceNode != null)
                {
                    loggingSettings.SpecialTraceSources.NotProcessedTraceSource = notProcessedSourceNode.TraceSourceData;
                    BuildTraceListenerReferences(loggingSettings.SpecialTraceSources.NotProcessedTraceSource, notProcessedSourceNode);
                }
            }
        }
        static void BuildTraceListenerReferences(TraceSourceData data,
                                                 TraceSourceNode node)
        {
            foreach (TraceListenerReferenceNode refNode in node.Nodes)
            {
                data.TraceListeners.Add(refNode.TraceListenerReferenceData);
            }
        }
        void BuildTraceListeners()
        {
            TraceListenerCollectionNode traceListenerCollectionNode
                = (TraceListenerCollectionNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(TraceListenerCollectionNode));
            if (traceListenerCollectionNode != null)
            {
                foreach (TraceListenerNode listenerNode in traceListenerCollectionNode.Nodes)
                {
                    TraceListenerData data = listenerNode.TraceListenerData;
                    data.Filter = listenerNode.Filter;
                    data.TraceOutputOptions = listenerNode.TraceOutputOptions;
                    loggingSettings.TraceListeners.Add(data);
                }
            }
        }
        void BuildTraceSources()
        {
            CategoryTraceSourceCollectionNode categoryTracesourceCollectioNode
                = (CategoryTraceSourceCollectionNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(CategoryTraceSourceCollectionNode));
            if (categoryTracesourceCollectioNode != null)
            {
                foreach (CategoryTraceSourceNode traceSourceNode in categoryTracesourceCollectioNode.Nodes)
                {
                    TraceSourceData traceSourceData = traceSourceNode.TraceSourceData;
                    BuildTraceListenerReferences(traceSourceData, traceSourceNode);
                    loggingSettings.TraceSources.Add(traceSourceData);
                }
            }
        }
        void BuilFormatters()
        {
            FormatterCollectionNode formatterCollectionNode
                = (FormatterCollectionNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(FormatterCollectionNode));
            if (formatterCollectionNode != null)
            {
                foreach (FormatterNode formatterNode in formatterCollectionNode.Nodes)
                {
                    loggingSettings.Formatters.Add(formatterNode.FormatterData);
                }
            }
        }
    }
}
