/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
	public class RollingTraceListenerNode : Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners.TraceListenerNode
	{
		private FormatterNode formatterNode;
		private string formatterName;
		private string header;
		private string footer;
		private RollInterval rollInterval;
		private RollFileExistsBehavior rollFileExistsBehavior;
		private string timeStampPattern;
		private int rollSizeKB;
		private string fileName;
		public RollingTraceListenerNode()
			: this(new RollingFlatFileTraceListenerData(
							Resources.RollingTraceListenerNodeUICommandText,
							DefaultValues.RollingFlatFileTraceListenerFileName,
							DefaultValues.FlatFileListenerHeader,
							DefaultValues.FlatFileListenerFooter,
							DefaultValues.RollSizeKB,
							DefaultValues.TimeStampPattern,
							DefaultValues.RollFileExistsBehaviorValue,
							DefaultValues.RollIntervalValue,
							System.Diagnostics.TraceOptions.None,
							null,
                            System.Diagnostics.SourceLevels.All
			))
		{
		}
		public RollingTraceListenerNode(Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.RollingFlatFileTraceListenerData data)
		{
			if (null == data) throw new ArgumentNullException("data");
			Rename(data.Name);
			this.fileName = data.FileName;
			this.rollSizeKB = data.RollSizeKB;
			this.timeStampPattern = data.TimeStampPattern;
			this.rollFileExistsBehavior = data.RollFileExistsBehavior;
			this.rollInterval = data.RollInterval;
			this.TraceOutputOptions = data.TraceOutputOptions;
            this.Filter = data.Filter;
			this.formatterName = data.Formatter;
			this.header = data.Header;
			this.footer = data.Footer;
		}
		[Browsable(false)]
		public override Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TraceListenerData TraceListenerData
		{
			get
			{
				RollingFlatFileTraceListenerData data = new RollingFlatFileTraceListenerData(this.Name,
																fileName,
																header,
																footer,
																rollSizeKB,
																timeStampPattern,
																rollFileExistsBehavior,
																rollInterval,
																this.TraceOutputOptions,
																formatterName,
                                                                this.Filter);
				return data;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			base.Dispose(disposing);
		}
		[Required]
		[SRDescription("FileNameDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public System.String FileName
		{
			get { return this.fileName; }
			set { this.fileName = value; }
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
		[Required]
		[SRDescription("RollSizeKBDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public int RollSizeKB
		{
			get { return rollSizeKB; }
			set { rollSizeKB = value; }
		}
		[Required]
		[SRDescription("TimeStampPatternDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string TimeStampPattern
		{
			get { return timeStampPattern; }
			set { timeStampPattern = value; }
		}
		[Required]
		[SRDescription("RollFileExistsBehaviorDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public RollFileExistsBehavior RollFileExistsBehavior
		{
			get { return rollFileExistsBehavior; }
			set { rollFileExistsBehavior = value; }
		}
		[Required]
		[SRDescription("RollIntervalDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public RollInterval RollInterval
		{
			get { return rollInterval; }
			set { rollInterval = value; }
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
