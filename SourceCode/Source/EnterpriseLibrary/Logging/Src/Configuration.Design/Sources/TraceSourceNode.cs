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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    [Image(typeof(TraceSourceNode))]
    public abstract class TraceSourceNode : ConfigurationNode
    {
		private SourceLevels switchLevel;
        private bool autoFlush;
        private const string CategoryName = "CategoryGeneral";
		protected TraceSourceNode() : this(string.Empty, SourceLevels.All, LogSource.DefaultAutoFlushProperty)   
        {			
        }
		protected TraceSourceNode(string name) : this(name, SourceLevels.All)
		{
		}
		protected TraceSourceNode(string name, SourceLevels switchLevel) : base(name)
		{
			this.switchLevel = switchLevel;
		}
        protected TraceSourceNode(string name, SourceLevels switchLevel, bool autoFlush)
            : this(name, switchLevel)
        {
            this.autoFlush = autoFlush;
        }
        [SRCategory(CategoryName, typeof(Resources))]
        [SRDescription("DefaultLevelDescription", typeof(Resources))]
        public SourceLevels SourceLevels
        {
            get { return switchLevel; }
            set { switchLevel = value; }
        }
        [SRCategory(CategoryName, typeof(Resources))]
        [SRDescription("AutoFlush", typeof(Resources))]
        [DefaultValue(LogSource.DefaultAutoFlushProperty)]
        public bool AutoFlush
        {
            get { return autoFlush; }
            set { autoFlush = value; }
        }
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
        }
		[Browsable(false)]
		public TraceSourceData TraceSourceData 
		{
			get { return new TraceSourceData(Name, switchLevel);  }
		}		
    }
}
