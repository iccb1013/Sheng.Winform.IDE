/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    public sealed class LoggingExceptionHandlerNode : ExceptionHandlerNode
    {
        int eventId;
        string formatterTypeName;
        string logCategoryName;
        CategoryTraceSourceNode logCategoryNode;
        EventHandler<ConfigurationNodeChangedEventArgs> logCategoryNodeRemoved;
        EventHandler<ConfigurationNodeChangedEventArgs> logCategoryNodeRenamed;
        int priority;
        TraceEventType severity;
        string title;
        bool useDefaultLogger;
        public LoggingExceptionHandlerNode()
            : this(new LoggingExceptionHandlerData(Resources.LoggingHandlerName, Resources.DefaultCategory, 100, TraceEventType.Error, Resources.DefaultTitle, string.Empty, 0)) {}
        public LoggingExceptionHandlerNode(LoggingExceptionHandlerData loggingExceptionHandlerData)
        {
            if (null == loggingExceptionHandlerData) throw new ArgumentNullException("loggingExceptionHandlerData");
            Rename(loggingExceptionHandlerData.Name);
            eventId = loggingExceptionHandlerData.EventId;
            severity = loggingExceptionHandlerData.Severity;
            title = loggingExceptionHandlerData.Title;
            formatterTypeName = loggingExceptionHandlerData.FormatterTypeName;
            logCategoryName = loggingExceptionHandlerData.LogCategory;
            priority = loggingExceptionHandlerData.Priority;
            logCategoryNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnLogCategoryNodeRemoved);
            logCategoryNodeRenamed = new EventHandler<ConfigurationNodeChangedEventArgs>(OnLogCategoryNodeRenamed);
            useDefaultLogger = loggingExceptionHandlerData.UseDefaultLogger;
        }
        [Required]
        [SRDescription("DefaultEventIdDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }
        public override ExceptionHandlerData ExceptionHandlerData
        {
            get { return new LoggingExceptionHandlerData(Name, logCategoryName, eventId, severity, title, formatterTypeName, priority, useDefaultLogger); }
        }
        [Required]
        [SRDescription("FormatterTypeNameDescription", typeof(Resources))]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ExceptionFormatter))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string FormatterType
        {
            get { return formatterTypeName; }
            set { formatterTypeName = value; }
        }
        [Required]
        [SRDescription("DefaultLogCategoryDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(CategoryTraceSourceNode))]
        public CategoryTraceSourceNode LogCategory
        {
            get { return logCategoryNode; }
            set
            {
                logCategoryNode = LinkNodeHelper.CreateReference<CategoryTraceSourceNode>(logCategoryNode,
                                                                                          value,
                                                                                          logCategoryNodeRemoved,
                                                                                          logCategoryNodeRenamed);
                logCategoryName = logCategoryNode == null ? String.Empty : logCategoryNode.Name;
            }
        }
        [Required]
        [SRDescription("MinimumPriorityDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        [Required]
        [SRDescription("DefaultSeverityDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public TraceEventType Severity
        {
            get { return severity; }
            set { severity = value; }
        }
        [Required]
        [SRDescription("DefaultTitleDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        [SRDescription("UseDefaultLoggerDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool UseDefaultLogger
        {
            get { return useDefaultLogger; }
            set { useDefaultLogger = value; }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != logCategoryNode)
                {
                    logCategoryNode.Removed -= logCategoryNodeRemoved;
                    logCategoryNode.Renamed -= logCategoryNodeRenamed;
                }
            }
            base.Dispose(disposing);
        }
        void OnLogCategoryNodeRemoved(object sender,
                                      ConfigurationNodeChangedEventArgs e)
        {
            logCategoryNode = null;
        }
        void OnLogCategoryNodeRenamed(object sender,
                                      ConfigurationNodeChangedEventArgs e)
        {
            logCategoryName = e.Node.Name;
        }
    }
}
