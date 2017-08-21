/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
	public class DefaultLoggingEventLoggerCustomFactory : ICustomFactory
    {
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            InstrumentationConfigurationSection objectConfiguration
                = GetConfiguration(configurationSource);
            DefaultLoggingEventLogger createdObject
                = new DefaultLoggingEventLogger(objectConfiguration.EventLoggingEnabled, objectConfiguration.WmiEnabled);
            return createdObject;
        }
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
