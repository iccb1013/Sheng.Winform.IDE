/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    public class AddLoggingSettingsNodeCommand : AddChildNodeCommand
    {
        private IServiceProvider serviceProvider;
        public AddLoggingSettingsNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(LoggingSettingsNode))
        {
            this.serviceProvider = serviceProvider;
        }
        protected override void ExecuteCore(ConfigurationNode node)
        {
            base.ExecuteCore(node);
            LoggingSettingsNode loggingNode = ChildNode as LoggingSettingsNode;
            if (loggingNode == null) return;
            TextFormatterNode defaultTextFormatterNode = new TextFormatterNode();
            FormattedEventLogTraceListenerNode defaultTraceListenerNode = new FormattedEventLogTraceListenerNode();
            CategoryTraceSourceNode generalCategoryNode
                = new CategoryTraceSourceNode(new TraceSourceData(Resources.TraceSourceCategoryGeneral, SourceLevels.All));
            loggingNode.AddNode(new LogFilterCollectionNode());
            CategoryTraceSourceCollectionNode categoryTraceSourcesNode = new CategoryTraceSourceCollectionNode();
            TraceListenerReferenceNode generalCategoryListenerRef
                = new TraceListenerReferenceNode(new TraceListenerReferenceData(defaultTraceListenerNode.Name));
            categoryTraceSourcesNode.AddNode(generalCategoryNode);
            generalCategoryNode.AddNode(generalCategoryListenerRef);
            generalCategoryListenerRef.ReferencedTraceListener = defaultTraceListenerNode;
            loggingNode.AddNode(categoryTraceSourcesNode);
            SpecialTraceSourcesNode specialTraceSourcesNode = new SpecialTraceSourcesNode();
            ErrorsTraceSourceNode errorsTraceSourcesNode = new ErrorsTraceSourceNode(new TraceSourceData());
            TraceListenerReferenceNode errorsTraceListenerReferenceNode = new TraceListenerReferenceNode();
            errorsTraceSourcesNode.AddNode(errorsTraceListenerReferenceNode);
            errorsTraceListenerReferenceNode.ReferencedTraceListener = defaultTraceListenerNode;
            specialTraceSourcesNode.AddNode(errorsTraceSourcesNode);
            specialTraceSourcesNode.AddNode(new NotProcessedTraceSourceNode(new TraceSourceData()));
            specialTraceSourcesNode.AddNode(new AllTraceSourceNode(new TraceSourceData()));
            loggingNode.AddNode(specialTraceSourcesNode);
            TraceListenerCollectionNode traceListenerCollectionNode = new TraceListenerCollectionNode();
            traceListenerCollectionNode.AddNode(defaultTraceListenerNode);
            defaultTraceListenerNode.Formatter = defaultTextFormatterNode;
            loggingNode.AddNode(traceListenerCollectionNode);
            FormatterCollectionNode formattersNode = new FormatterCollectionNode();
            formattersNode.AddNode(defaultTextFormatterNode);
            loggingNode.AddNode(formattersNode);
            loggingNode.DefaultCategory = generalCategoryNode;
            loggingNode.LogWarningWhenNoCategoriesMatch = true;
            loggingNode.TracingEnabled = true;
            loggingNode.RevertImpersonation = true;
            ServiceHelper.GetUIService(serviceProvider).RefreshPropertyGrid();
        }
    }
}
