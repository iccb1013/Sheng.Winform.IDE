/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class InstrumentationStrategy : EnterpriseLibraryBuilderStrategy
	{
		public override void PreBuildUp(IBuilderContext context)
		{
			base.PreBuildUp(context);
			if (context.Existing != null && context.Existing is IInstrumentationEventProvider)
			{
				IConfigurationSource configurationSource = GetConfigurationSource(context);
				ConfigurationReflectionCache reflectionCache = GetReflectionCache(context);
				NamedTypeBuildKey key = (NamedTypeBuildKey)context.BuildKey;
				string id = key.Name;
				InstrumentationAttachmentStrategy instrumentation = new InstrumentationAttachmentStrategy();
				if (ConfigurationNameProvider.IsMadeUpName(id))
				{
					instrumentation.AttachInstrumentation(context.Existing, configurationSource, reflectionCache);
				}
				else
				{
					instrumentation.AttachInstrumentation(id, context.Existing, configurationSource, reflectionCache);
				}
			}
		}
	}
}
