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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    [Image(typeof(TraceListenerCollectionNode))]
    public class TraceListenerCollectionNode : ConfigurationNode
    {
        public TraceListenerCollectionNode()
            : base(Resources.TraceListenerCollectionNode)
        {
        }
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("LogFilterCollectionNameDesciption", typeof(Resources))]
        public override string Name
        {
            get { return base.Name; }            
        }
    }
}
