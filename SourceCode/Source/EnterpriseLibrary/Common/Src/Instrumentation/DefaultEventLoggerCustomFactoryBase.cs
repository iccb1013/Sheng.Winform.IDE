/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	public abstract class DefaultEventLoggerCustomFactoryBase : ICustomFactory
	{
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			InstrumentationConfigurationSection objectConfiguration	= GetConfiguration(configurationSource);
			object createdObject = DoCreateObject((objectConfiguration));
			return createdObject;
		}
		protected abstract object DoCreateObject(InstrumentationConfigurationSection instrumentationConfigurationSection);
		private InstrumentationConfigurationSection GetConfiguration(IConfigurationSource configurationSource)
		{
			InstrumentationConfigurationSection configurationSection
				= (InstrumentationConfigurationSection)configurationSource.GetSection(InstrumentationConfigurationSection.SectionName);
			if (configurationSection == null) configurationSection
				= new InstrumentationConfigurationSection(false, false, false);
			return configurationSection;
		}
	}
}
