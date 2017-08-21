/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    public class TracerInstrumentationListenerCustomFactory : ICustomFactory
    {
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            InstrumentationConfigurationSection objectConfiguration
                = GetConfiguration(configurationSource);
            TracerInstrumentationListener createdObject
                = new TracerInstrumentationListener(objectConfiguration.PerformanceCountersEnabled);
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
