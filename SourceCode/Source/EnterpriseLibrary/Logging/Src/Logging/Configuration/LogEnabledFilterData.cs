/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(LogEnabledFilterAssembler))]
	public class LogEnabledFilterData : LogFilterData
	{
		private const string enabledProperty = "enabled";
		public LogEnabledFilterData()
		{
		}
		public LogEnabledFilterData(bool enabled)
			: this("enabled", enabled)
		{
		}
		public LogEnabledFilterData(string name, bool enabled)
			: base(name, typeof(LogEnabledFilter))
		{
			this.Enabled = enabled;
		}
		[ConfigurationProperty(enabledProperty, IsRequired = true)]
		public bool Enabled
		{
			get { return (bool)base[enabledProperty]; }
			set { base[enabledProperty] = value; }
		}
	}
	public class LogEnabledFilterAssembler : IAssembler<ILogFilter, LogFilterData>
	{
		public ILogFilter Assemble(IBuilderContext context, LogFilterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			LogEnabledFilterData castedObjectConfiguration = (LogEnabledFilterData)objectConfiguration;
			ILogFilter createdObject
				= new LogEnabledFilter(
					castedObjectConfiguration.Name,
					castedObjectConfiguration.Enabled);
			return createdObject;
		}
	}
}
