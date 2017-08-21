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
    public class FlatFileTraceListenerNode : TraceListenerNode
    {
        private FormatterNode formatterNode;
		private string formatterName;
		private string fileName;
		private string header;
		private string footer;
        public FlatFileTraceListenerNode()
            : this(new FlatFileTraceListenerData(Resources.FlatFileTraceListenerNode, DefaultValues.FlatFileListenerFileName, DefaultValues.FlatFileListenerFooter, DefaultValues.FlatFileListenerHeader, string.Empty))
        {
        }
        public FlatFileTraceListenerNode(FlatFileTraceListenerData traceListenerData)
        {
			if (null == traceListenerData) throw new ArgumentNullException("traceListenerData");
			Rename(traceListenerData.Name);
			TraceOutputOptions = traceListenerData.TraceOutputOptions;
			this.formatterName = traceListenerData.Formatter;
			this.fileName = traceListenerData.FileName;
			this.header = traceListenerData.Header;
			this.footer = traceListenerData.Footer;
        }        
        [Required]
        [Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
        [FilteredFileNameEditor(typeof(Resources), "FlatFileTraceListenerFileDialogFilter")]
        [SRDescription("FlatFileTraceListenerFlatFileName", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Filename
        {
            get { return fileName; }
            set { fileName = value; }
        }
        [SRDescription("FlatFileTraceListenerHeader", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        [SRDescription("FlatFileTraceListenerFooter", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Footer
        {
            get { return footer; }
            set { footer = value; }
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
				FlatFileTraceListenerData data = new FlatFileTraceListenerData(Name, fileName, header, footer, formatterName);
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
            formatterNode = null;
        }
        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            formatterName = e.Node.Name;
        }
    }
}
