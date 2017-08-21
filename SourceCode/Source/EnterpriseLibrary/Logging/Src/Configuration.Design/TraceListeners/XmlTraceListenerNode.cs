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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
	public class XmlTraceListenerNode : TraceListenerNode
	{
		public XmlTraceListenerNode()
			: this(new XmlTraceListenerData(Resources.XmlTraceListenerNodeUICommandText,DefaultValues.XmlTraceListenerFileName))
		{
		}
		public XmlTraceListenerNode(XmlTraceListenerData data)
		{
			if (null == data) throw new ArgumentNullException("data");
			Rename(data.Name);
			this.fileName = data.FileName;
		}
		[Browsable(false)]
		public override TraceListenerData TraceListenerData
		{
			get
			{
				XmlTraceListenerData data = new XmlTraceListenerData(this.Name,this.fileName);
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
		private System.String fileName;
		[Required]
		[Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
		[FilteredFileNameEditor(typeof(Resources), "XmlTraceListenerFileDialogFilter")]
		[SRDescription("FileNameDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public System.String FileName
		{
			get { return this.fileName; }
			set { this.fileName = value; }
		}
	}
}
