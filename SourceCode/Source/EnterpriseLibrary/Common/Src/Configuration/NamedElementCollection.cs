/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	public class NamedElementCollection<T> : ConfigurationElementCollection, IEnumerable<T>
		where T : NamedConfigurationElement, new()
	{
		public void ForEach(Action<T> action)
		{
			for (int index = 0; index < Count ; index++)
			{
				action(Get(index));
			}
		}
		public T Get(int index)
		{
			return (T)base.BaseGet(index);		}	
		public void Add(T element)
		{
			BaseAdd(element, true);
		}
		public T Get(string name)
		{
			return BaseGet(name) as T;
		}	
		public bool Contains(string name)
		{
			return BaseGet(name) != null;
		}
		public void Remove(string name)
		{
			BaseRemove(name);
		}
		public void Clear()
		{
			BaseClear();
		}
		public new IEnumerator<T> GetEnumerator()
		{
			return new GenericEnumeratorWrapper<T>(base.GetEnumerator());
		}
		protected override ConfigurationElement CreateNewElement()
		{
			return new T();
		}
		protected override object GetElementKey(ConfigurationElement element)
		{
			T namedElement = (T)element;
			return namedElement.Name;
		}
	}
}
