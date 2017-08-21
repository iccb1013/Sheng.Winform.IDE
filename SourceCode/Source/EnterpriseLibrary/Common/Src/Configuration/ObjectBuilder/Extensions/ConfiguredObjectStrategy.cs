/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class ConfiguredObjectStrategy : EnterpriseLibraryBuilderStrategy
	{
		public override void PreBuildUp(IBuilderContext context)
		{
			base.PreBuildUp(context);
			IConfigurationSource configurationSource = GetConfigurationSource(context);
			ConfigurationReflectionCache reflectionCache = GetReflectionCache(context);
			NamedTypeBuildKey key = (NamedTypeBuildKey) context.BuildKey;
			string id = key.Name;
			Type t = key.Type;
			ICustomFactory factory = GetCustomFactory(t, reflectionCache);
			if (factory != null)
			{
				context.Existing = factory.CreateObject(context, id, configurationSource, reflectionCache);
			}
			else
			{
				throw new InvalidOperationException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionCustomFactoryAttributeNotFound,
						t.FullName,
						id));
			}
		}
		private static ICustomFactory GetCustomFactory(Type t, ConfigurationReflectionCache reflectionCache)
		{
			ICustomFactory customFactory = reflectionCache.GetCustomFactory(t);
			return customFactory;
		}
	}
}
