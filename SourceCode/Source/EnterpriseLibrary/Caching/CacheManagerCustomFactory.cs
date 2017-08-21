/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	public class CacheManagerCustomFactory : AssemblerBasedCustomFactory<ICacheManager, CacheManagerDataBase>
	{
		protected override CacheManagerDataBase GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			CachingConfigurationView view = new CachingConfigurationView(configurationSource);
			return view.GetCacheManagerData(name);
		}
		public override ICacheManager Create(IBuilderContext context, CacheManagerDataBase objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ICacheManager manager = base.Create(context, objectConfiguration, configurationSource, reflectionCache);
			RegisterObject(context, objectConfiguration.Name, manager);
			return manager;
		}
		private void RegisterObject(IBuilderContext context, string name, ICacheManager createdObject)
		{
			if (context.Locator != null)
			{
				context.Locator.Add(new NamedTypeBuildKey(typeof(ICacheManager), name), createdObject);
			}
			if (context.Lifetime != null)
			{
				context.Lifetime.Add(createdObject);
			}
		}		
	}
}
