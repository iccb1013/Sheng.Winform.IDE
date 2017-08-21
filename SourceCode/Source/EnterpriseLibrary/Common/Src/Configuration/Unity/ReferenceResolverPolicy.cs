/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public sealed class ReferenceResolverPolicy : IDependencyResolverPolicy
	{
		private readonly NamedTypeBuildKey referenceKey;
		public ReferenceResolverPolicy(NamedTypeBuildKey referenceKey)
		{
			this.referenceKey = referenceKey;
		}
		object IDependencyResolverPolicy.Resolve(IBuilderContext context)
		{
			IBuilderContext recursiveContext = context.CloneForNewBuild(this.referenceKey, null);
			return recursiveContext.Strategies.ExecuteBuildUp(recursiveContext);
		}
	}
}
