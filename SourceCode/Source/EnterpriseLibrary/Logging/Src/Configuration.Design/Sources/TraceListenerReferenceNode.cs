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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    [Image(typeof(TraceListenerReferenceNode))]
    public sealed class TraceListenerReferenceNode : ConfigurationNode
    {
		private TraceListenerNode referencedTraceListener;
        public TraceListenerReferenceNode()
            : this(new TraceListenerReferenceData(Resources.TraceListenerReferenceNode))
        {
        }
        public TraceListenerReferenceNode(TraceListenerReferenceData traceListenerReferenceData)
            : base()
        {
            if (traceListenerReferenceData == null)
            {
                throw new ArgumentNullException("traceListenerReferenceData");
            }
            Rename(traceListenerReferenceData.Name);
        }
        [ReadOnly(true)]
        [SRDescription("TraceListenerReferenceNode", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public override string Name
        {
            get { return base.Name; }   
        }
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(TraceListenerNode))]
        [SRDescription("ReferencedTraceListenerDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Required]
        public TraceListenerNode ReferencedTraceListener
        {
            get { return referencedTraceListener; }
            set
            {
                referencedTraceListener = LinkNodeHelper.CreateReference<TraceListenerNode>(referencedTraceListener,
                    value,
                    OnFormatterNodeRemoved,
                    OnFormatterNodeRenamed);
                if (referencedTraceListener != null)
                {
                    try
                    {
                        Name = referencedTraceListener.Name;
                    }
                    catch (InvalidOperationException)
                    {
                        string message = string.Format(Resources.Culture, Resources.ReferenceAlreadyExists, referencedTraceListener.Name);
                        ServiceHelper.GetUIService(Site).ShowError(message);
                        referencedTraceListener = null;
                    }
                }
            }
        }
		[Browsable(false)]
		public TraceListenerReferenceData TraceListenerReferenceData
		{
			get
			{
				return new TraceListenerReferenceData(Name);
			}
		}
        private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            ReferencedTraceListener = null;
        }
        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            Name = e.Node.Name;
        }
    }
}
