/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	public static class Resolve
	{
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to indicate the resolution to type T")]
		[SuppressMessage("Microsoft.Usage", "CA1801",
			Justification = "The purpose of this method is to indicate the resolution to type T, parameters are not used.")]
		public static T Reference<T>(string key)
		{
			return default(T);
		}
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to indicate the resolution to type T")]
		[SuppressMessage("Microsoft.Usage", "CA1801",
			Justification = "The purpose of this method is to indicate the resolution to type T, parameters are not used.")]
		public static T OptionalReference<T>(string key)
		{
			return default(T);
		}
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to indicate the resolution to a collection TCollection of elements of type T")]
		[SuppressMessage("Microsoft.Usage", "CA1801",
			Justification = "The purpose of this method is to indicate the resolution to type T, parameters are not used.")]
		public static TCollection ReferenceCollection<TCollection, T>(IEnumerable<string> keys)
			where TCollection : ICollection<T>, new()
		{
			return default(TCollection);
		}
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to indicate the resolution to a dictionary TDictionary of elements of type T")]
		[SuppressMessage("Microsoft.Usage", "CA1801",
			Justification = "The purpose of this method is to indicate the resolution to type T, parameters are not used.")]
		public static TDictionary ReferenceDictionary<TDictionary, T, TKey>(IEnumerable<KeyValuePair<string, TKey>> keys)
			where TDictionary : IDictionary<TKey, T>, new()
		{
			return default(TDictionary);
		}
	}
}
