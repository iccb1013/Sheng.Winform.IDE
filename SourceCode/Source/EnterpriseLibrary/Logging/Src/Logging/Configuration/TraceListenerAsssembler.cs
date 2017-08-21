/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	public abstract class TraceListenerAsssembler : IAssembler<TraceListener, TraceListenerData>
	{
		public abstract TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache);
		protected ILogFormatter GetFormatter(IBuilderContext context, string formatterName, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ILogFormatter formatter
				= string.IsNullOrEmpty(formatterName)
					? null
					: LogFormatterCustomFactory.Instance.Create(context, formatterName, configurationSource, reflectionCache);
			return formatter;
		}
	}
}
