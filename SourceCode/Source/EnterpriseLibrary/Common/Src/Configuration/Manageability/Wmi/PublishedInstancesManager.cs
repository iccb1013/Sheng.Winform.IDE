/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public class PublishedInstancesManager<TSetting, TKey>
		where TSetting : ConfigurationSetting
	{
		private IDictionary<Type, IDictionary<TKey, TSetting>> publishedInstances
			= new Dictionary<Type, IDictionary<TKey, TSetting>>();
		private readonly object publishedInstancesLock = new object();
		public void Publish(TSetting setting, TKey key)
		{
			Type thisType = setting.GetType();
			lock (publishedInstancesLock)
			{
				IDictionary<TKey, TSetting> publishedInstancesForType;
				if (!publishedInstances.TryGetValue(thisType, out publishedInstancesForType))
				{
					publishedInstancesForType = new Dictionary<TKey, TSetting>();
					publishedInstances[thisType] = publishedInstancesForType;
				}
				try
				{
					publishedInstancesForType.Add(key, setting);
				}
				catch (ArgumentException)
				{
					if (setting != publishedInstancesForType[key])
					{
						throw;
					}
				}
			}
		}
		public void Revoke(TSetting setting, TKey key)
		{
			Type thisType = setting.GetType();
			lock (publishedInstancesLock)
			{
				IDictionary<TKey, TSetting> publishedInstancesForType;
				if (publishedInstances.TryGetValue(thisType, out publishedInstancesForType))
				{
					publishedInstancesForType.Remove(key);
				}
			}
		}
		public void ClearPublishedInstances()
		{
			lock (publishedInstancesLock)
			{
				publishedInstances = new Dictionary<Type, IDictionary<TKey, TSetting>>();
			}
		}
		public IEnumerable<T> GetInstances<T>()
			where T : TSetting
		{
			lock (publishedInstancesLock)
			{
				IDictionary<TKey, TSetting> publishedInstancesForType;
				if (publishedInstances.TryGetValue(typeof(T), out publishedInstancesForType))
				{
					T[] values = new T[publishedInstancesForType.Count];
					publishedInstancesForType.Values.CopyTo(values, 0);
					return values;
				}
				else
				{
					return new T[0];
				}
			}
		}
		public T BindInstance<T>(TKey key)
			where T : TSetting
		{
			lock (publishedInstancesLock)
			{
				IDictionary<TKey, TSetting> publishedInstancesForType;
				if (publishedInstances.TryGetValue(typeof(T), out publishedInstancesForType))
				{
					TSetting instance;
					publishedInstancesForType.TryGetValue(key, out instance);
					return instance as T;
				}
				else
				{
					return null;
				}
			}
		}
	}
}
