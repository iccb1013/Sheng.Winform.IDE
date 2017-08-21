/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
	[Image(typeof(CategoryTraceSourceNode))]
	[SelectedImage(typeof(CategoryTraceSourceNode))]
	public sealed class CategoryTraceSourceNode : TraceSourceNode
	{
		public CategoryTraceSourceNode()
			: this(new TraceSourceData(Resources.CategoryTraceSourceNode, SourceLevels.All))
		{
		}
		public CategoryTraceSourceNode(TraceSourceData traceSourceData)
		{
			if (null == traceSourceData) throw new ArgumentNullException("traceSourceData");
			Rename(traceSourceData.Name);
			SourceLevels = traceSourceData.DefaultLevel;
		}
		[ReadOnly(false)]
		public override string Name
		{
			get { return base.Name; }
		}
	}
}
