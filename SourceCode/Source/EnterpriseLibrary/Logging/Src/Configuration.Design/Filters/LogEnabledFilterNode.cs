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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    public sealed class LogEnabledFilterNode : LogFilterNode
    {
        private bool enabled;
        public LogEnabledFilterNode()
            : this(new LogEnabledFilterData(Resources.LogEnabledFilterNode, false))
        {
        }
        public LogEnabledFilterNode(LogEnabledFilterData logEnabledFilterData)
			: base(null == logEnabledFilterData ? string.Empty : logEnabledFilterData.Name)
        {
			if (null == logEnabledFilterData) throw new ArgumentNullException("logEnabledFilterData");
			this.enabled = logEnabledFilterData.Enabled;
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("EnabledDescription", typeof(Resources))]
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
		public override LogFilterData LogFilterData
		{
			get { return new LogEnabledFilterData(Name, enabled); }
		}
    }
}
