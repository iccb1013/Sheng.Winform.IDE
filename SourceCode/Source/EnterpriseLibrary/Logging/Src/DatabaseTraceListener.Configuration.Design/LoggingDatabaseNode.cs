/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
    public sealed class LoggingDatabaseNode : TraceListenerNode
    {
        const string addCategoryStoredProcedureDefault = "AddCategory";
        const string writeLogStoredProcedureDefault = "WriteLog";
        string addCategoryStoredProcedure;
        ConnectionStringSettingsNode connectionStringNode;
        string databaseName;
        string formatterName;
        FormatterNode formatterNode;
        EventHandler<ConfigurationNodeChangedEventArgs> onConnectionStringNodeRemoved;
        EventHandler<ConfigurationNodeChangedEventArgs> onConnectionStringNodeRenamed;
        string writeLogStoredProcedureName;
        public LoggingDatabaseNode()
            : this(new FormattedDatabaseTraceListenerData(Resources.DatabaseTraceListenerName, writeLogStoredProcedureDefault, addCategoryStoredProcedureDefault, string.Empty, string.Empty)) {}
        public LoggingDatabaseNode(FormattedDatabaseTraceListenerData traceListenerData)
        {
            if (null == traceListenerData) throw new ArgumentNullException("traceListenerData");
            Rename(traceListenerData.Name);
            TraceOutputOptions = traceListenerData.TraceOutputOptions;
            databaseName = traceListenerData.DatabaseInstanceName;
            addCategoryStoredProcedure = traceListenerData.AddCategoryStoredProcName;
            writeLogStoredProcedureName = traceListenerData.WriteLogStoredProcName;
            formatterName = traceListenerData.Formatter;
            onConnectionStringNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringNodeRemoved);
            onConnectionStringNodeRenamed = new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringNodeRenamed);
        }
        [Required]
        [SRDescription("AddCategoryStoredProcNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string AddCategoryStoredProcedure
        {
            get { return addCategoryStoredProcedure; }
            set { addCategoryStoredProcedure = value; }
        }
        [Required]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(ConnectionStringSettingsNode))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("DatabaseInstanceDescription", typeof(Resources))]
        public ConnectionStringSettingsNode DatabaseInstance
        {
            get { return connectionStringNode; }
            set
            {
                connectionStringNode = LinkNodeHelper.CreateReference<ConnectionStringSettingsNode>(connectionStringNode,
                                                                                                    value,
                                                                                                    onConnectionStringNodeRemoved,
                                                                                                    onConnectionStringNodeRenamed);
                databaseName = (connectionStringNode == null) ? String.Empty : connectionStringNode.Name;
            }
        }
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(FormatterNode))]
        [SRDescription("FormatDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public FormatterNode Formatter
        {
            get { return formatterNode; }
            set
            {
                formatterNode = LinkNodeHelper.CreateReference<FormatterNode>(formatterNode,
                                                                              value,
                                                                              OnFormatterNodeRemoved,
                                                                              OnFormatterNodeRenamed);
                formatterName = formatterNode == null ? string.Empty : formatterNode.Name;
            }
        }
        public override TraceListenerData TraceListenerData
        {
            get
            {
                return new FormattedDatabaseTraceListenerData(Name, writeLogStoredProcedureName, addCategoryStoredProcedure, databaseName,
                                                              formatterName);
            }
        }
        [Required]
        [SRDescription("WriteStoredProcNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string WriteLogStoredProcedureName
        {
            get { return writeLogStoredProcedureName; }
            set { writeLogStoredProcedureName = value; }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connectionStringNode != null)
                {
                    connectionStringNode.Removed -= onConnectionStringNodeRemoved;
                    connectionStringNode.Renamed -= onConnectionStringNodeRenamed;
                }
            }
            base.Dispose(disposing);
        }
        void OnConnectionStringNodeRemoved(object sender,
                                           ConfigurationNodeChangedEventArgs e)
        {
            connectionStringNode = null;
        }
        void OnConnectionStringNodeRenamed(object sender,
                                           ConfigurationNodeChangedEventArgs e)
        {
            databaseName = e.Node.Name;
        }
        void OnFormatterNodeRemoved(object sender,
                                    ConfigurationNodeChangedEventArgs e)
        {
            formatterName = null;
        }
        void OnFormatterNodeRenamed(object sender,
                                    ConfigurationNodeChangedEventArgs e)
        {
            formatterName = e.Node.Name;
        }
        protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
        {
            if (formatterName == formatterNodeReference.Name) Formatter = (FormatterNode)formatterNodeReference;
        }
    }
}
