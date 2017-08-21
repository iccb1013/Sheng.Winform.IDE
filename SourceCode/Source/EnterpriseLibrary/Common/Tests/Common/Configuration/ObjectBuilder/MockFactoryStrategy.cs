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
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	public class MockFactoryStrategy : BuilderStrategy
	{
		private readonly ICustomFactory factory;
		private readonly IConfigurationSource configurationSource;
		private readonly ConfigurationReflectionCache reflectionCache;
		public MockFactoryStrategy(ICustomFactory factory,
		                           IConfigurationSource configurationSource,
		                           ConfigurationReflectionCache reflectionCache)
		{
			this.factory = factory;
			this.configurationSource = configurationSource;
			this.reflectionCache = reflectionCache;
		}
		public override void PreBuildUp(IBuilderContext context)
		{
			base.PreBuildUp(context);
			NamedTypeBuildKey key = (NamedTypeBuildKey) context.BuildKey;
			context.Existing = factory.CreateObject(context, key.Name, configurationSource, reflectionCache);
		}
	}
}
