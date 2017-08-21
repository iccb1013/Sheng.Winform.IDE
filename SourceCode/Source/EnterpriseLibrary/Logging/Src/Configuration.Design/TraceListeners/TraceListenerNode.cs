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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    [Image(typeof (TraceListenerNode))]
    [SelectedImage(typeof (TraceListenerNode))]
    public abstract class TraceListenerNode : ConfigurationNode
    {
		private TraceOptions traceOptions;
        private SourceLevels filter;
        protected TraceListenerNode() : this(TraceOptions.None, SourceLevels.All)    
        {			
        }
        protected TraceListenerNode(TraceOptions traceOptions, SourceLevels filter)
        {
            this.filter = filter;
            this.traceOptions = traceOptions;
        }
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("TraceOutputOptionsDescription", typeof(Resources))]
		public TraceOptions TraceOutputOptions
		{
			get { return traceOptions; }
			set { traceOptions = value; }
		}
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("FilterDescription", typeof(Resources))]
        [DefaultValue(SourceLevels.All)]
        public SourceLevels Filter
        {
            get { return filter; }
            set { filter = value; }
        }
		[Browsable(false)]
		public abstract TraceListenerData TraceListenerData { get; }
		public void SetFormatter(ConfigurationNode formatters)
		{
			if (formatters == null) return;
			formatters.Nodes.ForEach(new Action<ConfigurationNode>(SetFormatterReference));
		}
		protected virtual void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
		}
    }
}
