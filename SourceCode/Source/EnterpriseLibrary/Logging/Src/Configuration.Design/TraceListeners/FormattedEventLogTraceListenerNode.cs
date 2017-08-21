/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using System.Diagnostics;
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    public class FormattedEventLogTraceListenerNode : TraceListenerNode
    {
        private FormatterNode formatterNode;
		private string machineName;
		private string formatterName;
		private string log;
		private string source;
        public FormattedEventLogTraceListenerNode()
            : this(new FormattedEventLogTraceListenerData(Resources.FormattedEventLogTraceListenerNode, DefaultValues.EventLogListenerEventSource, DefaultValues.EventLogListenerLogName, string.Empty, string.Empty))
        {
        }
        public FormattedEventLogTraceListenerNode(FormattedEventLogTraceListenerData traceListenerData)            
        {
			if (null == traceListenerData) throw new ArgumentNullException("traceListenerData");
			Rename(traceListenerData.Name);
			TraceOutputOptions = traceListenerData.TraceOutputOptions;
            this.machineName = traceListenerData.MachineName;
			this.formatterName = traceListenerData.Formatter;
			this.log = traceListenerData.Log;
			this.source = traceListenerData.Source;
        }
        [SRDescription("EventLogTraceListenerEventLogNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Log
        {
            get { return log; }
            set { log = value; }
        }
        [SRDescription("EventLogTraceListenerSourceNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Required]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }
        [SRDescription("MachineNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
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
				FormattedEventLogTraceListenerData data = new FormattedEventLogTraceListenerData(Name, source, log, machineName, formatterName);
				data.TraceOutputOptions = TraceOutputOptions;
				return data;
			}
		}
		protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
			if (formatterName == formatterNodeReference.Name) Formatter = (FormatterNode)formatterNodeReference;
		}
        private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
			formatterName = null;
        }
        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
			formatterName = e.Node.Name;
        }
    }
}
