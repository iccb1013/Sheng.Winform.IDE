/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class InstrumentationAttachmentStrategy
	{
		InstrumentationAttacherFactory attacherFactory = new InstrumentationAttacherFactory();
		public void AttachInstrumentation(object createdObject, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ArgumentGenerator arguments = new ArgumentGenerator();
			AttachInstrumentation(arguments, createdObject, configurationSource, reflectionCache);
		}
		public void AttachInstrumentation(string instanceName, object createdObject, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ArgumentGenerator arguments = new ArgumentGenerator(instanceName);
			AttachInstrumentation(arguments, createdObject, configurationSource, reflectionCache);
		}
		private void AttachInstrumentation(ArgumentGenerator arguments, object createdObject,
										   IConfigurationSource configurationSource,
										   ConfigurationReflectionCache reflectionCache)
		{
			InstrumentationConfigurationSection section = GetConfigurationSection(configurationSource);
			if (section.InstrumentationIsEntirelyDisabled) return;
			if (createdObject is IInstrumentationEventProvider)
			{
				createdObject = ((IInstrumentationEventProvider)createdObject).GetInstrumentationEventProvider();
			}
			object[] constructorArgs = arguments.ToArguments(section);
			BindInstrumentationTo(createdObject, constructorArgs, reflectionCache);
		}
		private void BindInstrumentationTo(object createdObject, object[] constructorArgs, ConfigurationReflectionCache reflectionCache)
		{
			IInstrumentationAttacher attacher = attacherFactory.CreateBinder(createdObject, constructorArgs, reflectionCache);
			attacher.BindInstrumentation();
		}
		private InstrumentationConfigurationSection GetConfigurationSection(IConfigurationSource configurationSource)
		{
			InstrumentationConfigurationSection section =
				(InstrumentationConfigurationSection)configurationSource.GetSection(InstrumentationConfigurationSection.SectionName);
			if (section == null) section = new InstrumentationConfigurationSection(false, false, false);
			return section;
		}
		private class ArgumentGenerator
		{
			private string instanceName;
			public ArgumentGenerator(string instanceName)
			{
				this.instanceName = instanceName;
			}
			public ArgumentGenerator() { }
			public object[] ToArguments(InstrumentationConfigurationSection configSection)
			{
				return instanceName == null
						?
					   new object[] { configSection.PerformanceCountersEnabled, configSection.EventLoggingEnabled, configSection.WmiEnabled, configSection.ApplicationInstanceName }
						:
					   new object[]
				       	{
				       		instanceName, configSection.PerformanceCountersEnabled, configSection.EventLoggingEnabled,
				       		configSection.WmiEnabled, configSection.ApplicationInstanceName
				       	};
			}
		}
	}
}
