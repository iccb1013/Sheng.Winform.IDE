/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public sealed class ConstantResolverPolicy : IDependencyResolverPolicy
	{
		private readonly object value;
		public ConstantResolverPolicy(object value)
		{
			this.value = value;
		}
		object IDependencyResolverPolicy.Resolve(IBuilderContext context)
		{
			return this.value;
		}
	}
}
