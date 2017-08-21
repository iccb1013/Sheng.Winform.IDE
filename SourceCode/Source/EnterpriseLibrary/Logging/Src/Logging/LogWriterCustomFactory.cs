/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	public class LogWriterCustomFactory : ICustomFactory
	{
		public object CreateObject(IBuilderContext context,
								   string name,
								   IConfigurationSource configurationSource,
								   ConfigurationReflectionCache reflectionCache)
		{
			LogWriterStructureHolder structureHolder
				=
				(LogWriterStructureHolder)
				LogWriterStructureHolderCustomFactory.Instance.CreateObject(context, name, configurationSource, reflectionCache);
			LogWriterStructureHolderUpdater structureHolderUpdater = new LogWriterStructureHolderUpdater(configurationSource);
			LogWriter createdObject = new LogWriter(structureHolder, structureHolderUpdater);
			structureHolderUpdater.SetLogWriter(createdObject);
			if (context.Locator != null)
			{
				context.Locator.Add(new NamedTypeBuildKey(typeof(LogWriter), name), createdObject);
			}
			if (context.Lifetime != null)
			{
				context.Lifetime.Add(createdObject);
			}
			return createdObject;
		}
	}
}
