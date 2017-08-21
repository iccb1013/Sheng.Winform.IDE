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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    [Image(typeof(NotProcessedTraceSourceNode))]
    [SelectedImage(typeof(NotProcessedTraceSourceNode))]
    public sealed class NotProcessedTraceSourceNode : TraceSourceNode
    { 
        public NotProcessedTraceSourceNode(TraceSourceData traceSourceData) : base(Resources.UnprocessedTraceSourceNode)
        {
			if (null == traceSourceData) throw new ArgumentNullException("traceSourceData");
			SourceLevels = traceSourceData.DefaultLevel;
        }		
    }
}
