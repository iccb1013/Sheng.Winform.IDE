/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	[SuppressMessage("Microsoft.Design", "CA1005",
		Justification = "Three types are needed here to achieve type safety.")]
	public sealed class ReferenceDictionaryResolverPolicy<TDictionary, T, TKey> : IDependencyResolverPolicy
		where TDictionary : IDictionary<TKey, T>, new()
	{
		private readonly IEnumerable<KeyValuePair<string, TKey>> dependencyKeys;
		public ReferenceDictionaryResolverPolicy(IEnumerable<KeyValuePair<string, TKey>> dependencyKeys)
		{
			this.dependencyKeys = dependencyKeys;
		}
		object IDependencyResolverPolicy.Resolve(IBuilderContext context)
		{
			TDictionary dictionary = new TDictionary();
			foreach (KeyValuePair<string, TKey> keyPair in dependencyKeys)
			{
				IBuilderContext buildContext = context.CloneForNewBuild(NamedTypeBuildKey.Make<T>(keyPair.Key), null);
				T createdElement = (T)buildContext.Strategies.ExecuteBuildUp(buildContext);
				dictionary.Add(keyPair.Value, createdElement);
			}
			return dictionary;
		}
	}
}
