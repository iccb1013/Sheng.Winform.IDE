/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public abstract class EnterpriseLibraryBuilderStrategy : BuilderStrategy
	{
		protected static IConfigurationSource GetConfigurationSource(IBuilderContext context)
		{
			IConfigurationObjectPolicy policy 
				= context.Policies.Get<IConfigurationObjectPolicy>(typeof(IConfigurationSource));
			if (policy == null)
				return new SystemConfigurationSource();
			else
				return policy.ConfigurationSource;
		}
		protected static ConfigurationReflectionCache GetReflectionCache(IBuilderContext context)
		{
			IReflectionCachePolicy policy
				= context.Policies.Get<IReflectionCachePolicy>(typeof(IReflectionCachePolicy));
			if (policy == null)
				return new ConfigurationReflectionCache();
			else
				return policy.ReflectionCache;
		}
	}
}
