/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    [Image(typeof(LoggingSettingsNode))]
    [SelectedImage(typeof(LoggingSettingsNode))]
    public sealed class LoggingSettingsNode : ConfigurationSectionNode
    {
        private bool loggingEnabled;
        private bool logWarning;
        private bool revertImpersonation;
        private CategoryTraceSourceNode defaultCategoryTraceSourceNode;
        public LoggingSettingsNode()
            : base(Resources.LogSettingsNode)
        {
            this.revertImpersonation = true;
        }
        [Browsable(false)]
        public override bool SortChildren
        {
            get { return false; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("LoggingSettingsNodeNameDescription", typeof(Resources))]
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("LoggingSettingsNodeTracingEnabledDescription", typeof(Resources))]
        public bool TracingEnabled
        {
            get { return loggingEnabled; }
            set { loggingEnabled = value; }
        }
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(CategoryTraceSourceNode))]
        [SRDescription("DefaultCategoryDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public CategoryTraceSourceNode DefaultCategory
        {
            get { return defaultCategoryTraceSourceNode; }
            set
            {
                defaultCategoryTraceSourceNode
                    = LinkNodeHelper.CreateReference<CategoryTraceSourceNode>(defaultCategoryTraceSourceNode,
                        value,
                        OnDefaultCategoryNodeRemoved,
                        null);
            }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("LogWarningWhenNoCategoryMatchDescription", typeof(Resources))]
        public bool LogWarningWhenNoCategoriesMatch
        {
            get { return logWarning; }
            set { logWarning = value; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("RevertImpersonationDescription", typeof(Resources))]
        [DefaultValue(true)]
        public bool RevertImpersonation
        {
            get { return revertImpersonation; }
            set { revertImpersonation = value; }
        }
        private void OnDefaultCategoryNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.defaultCategoryTraceSourceNode = null;
        }
    }
}
