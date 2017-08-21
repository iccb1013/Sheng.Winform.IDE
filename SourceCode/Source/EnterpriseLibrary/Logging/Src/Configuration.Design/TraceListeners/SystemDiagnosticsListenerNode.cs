/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Drawing.Design;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    public class SystemDiagnosticsTraceListenerNode : TraceListenerNode
    {
        private string typeName;
		private string initData;
        public SystemDiagnosticsTraceListenerNode()
            : this(new SystemDiagnosticsTraceListenerData(Resources.SystemDiagnosticsTraceListenerNode, string.Empty, string.Empty))
        {
        }
        public SystemDiagnosticsTraceListenerNode(SystemDiagnosticsTraceListenerData systemDiagnosticsTraceListenerData)
        {
			if (null == systemDiagnosticsTraceListenerData) throw new ArgumentNullException("systemDiagnosticsTraceListenerData");
			Rename(systemDiagnosticsTraceListenerData.Name);
			TraceOutputOptions = systemDiagnosticsTraceListenerData.TraceOutputOptions;
			this.typeName = systemDiagnosticsTraceListenerData.TypeName;
			this.initData = systemDiagnosticsTraceListenerData.InitData;
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("InitDataDescription", typeof(Resources))]
        public string InitData
        {
            get { return initData; }
            set { initData = value; }
        }
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(TraceListener))]
        [SRDescription("SystemDiagnosticsTraceListenerTypeDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return typeName; }
            set { typeName = value; }
        }
		public override TraceListenerData TraceListenerData
		{
			get 
			{ 
				SystemDiagnosticsTraceListenerData data = new SystemDiagnosticsTraceListenerData(Name, typeName, initData);
				data.TraceOutputOptions = TraceOutputOptions;
				return data;
			}
		}
    }
}
