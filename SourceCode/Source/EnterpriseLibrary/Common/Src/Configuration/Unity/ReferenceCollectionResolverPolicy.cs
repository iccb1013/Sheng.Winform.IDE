/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public sealed class ReferenceCollectionResolverPolicy : IDependencyResolverPolicy
	{
		private readonly Type collectionType;
		private readonly Type elementType;
		private readonly IEnumerable<string> keys;
		public ReferenceCollectionResolverPolicy(Type collectionType, Type elementType, IEnumerable<string> keys)
		{
			this.collectionType = collectionType;
			this.elementType = elementType;
			this.keys = keys;
		}
		object IDependencyResolverPolicy.Resolve(IBuilderContext context)
		{
			IList result = (IList)Activator.CreateInstance(collectionType);
			foreach (string key in keys)
			{
				IBuilderContext buildContext = context.CloneForNewBuild(new NamedTypeBuildKey(elementType, key), null);
				result.Add(buildContext.Strategies.ExecuteBuildUp(buildContext));
			}
			return result;
		}
	}
}
