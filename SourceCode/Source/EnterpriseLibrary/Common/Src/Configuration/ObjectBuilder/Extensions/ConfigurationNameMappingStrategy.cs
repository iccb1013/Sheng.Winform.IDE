/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class ConfigurationNameMappingStrategy : EnterpriseLibraryBuilderStrategy
	{
		public override void PreBuildUp(IBuilderContext context)
		{
			base.PreBuildUp(context);
			NamedTypeBuildKey key = (NamedTypeBuildKey) context.BuildKey;
			if (key.Name == null)
			{
				ConfigurationReflectionCache reflectionCache = GetReflectionCache(context);
				IConfigurationNameMapper mapper = reflectionCache.GetConfigurationNameMapper(key.Type);
				if (mapper != null)
				{
					context.BuildKey = new NamedTypeBuildKey(key.Type, mapper.MapName(null, GetConfigurationSource(context)));
				}
			}
		}
	}
}
