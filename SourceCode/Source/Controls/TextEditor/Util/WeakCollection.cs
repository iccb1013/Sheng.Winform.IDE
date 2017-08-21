/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Sheng.SailingEase.Controls.TextEditor.Util
{
	public class WeakCollection<T> : IEnumerable<T> where T : class
	{
		readonly List<WeakReference> innerList = new List<WeakReference>();
		public void Add(T item)
		{
			if (item == null)
				throw new ArgumentNullException("item");
			CheckNoEnumerator();
			if (innerList.Count == innerList.Capacity || (innerList.Count % 32) == 31)
				innerList.RemoveAll(delegate(WeakReference r) { return !r.IsAlive; });
			innerList.Add(new WeakReference(item));
		}
		public void Clear()
		{
			innerList.Clear();
			CheckNoEnumerator();
		}
		public bool Contains(T item)
		{
			if (item == null)
				throw new ArgumentNullException("item");
			CheckNoEnumerator();
			foreach (T element in this) {
				if (item.Equals(element))
					return true;
			}
			return false;
		}
		public bool Remove(T item)
		{
			if (item == null)
				throw new ArgumentNullException("item");
			CheckNoEnumerator();
			for (int i = 0; i < innerList.Count;) {
				T element = (T)innerList[i].Target;
				if (element == null) {
					RemoveAt(i);
				} else if (element == item) {
					RemoveAt(i);
					return true;
				} else {
					i++;
				}
			}
			return false;
		}
		void RemoveAt(int i)
		{
			int lastIndex = innerList.Count - 1;
			innerList[i] = innerList[lastIndex];
			innerList.RemoveAt(lastIndex);
		}
		bool hasEnumerator;
		void CheckNoEnumerator()
		{
			if (hasEnumerator)
				throw new InvalidOperationException("The WeakCollection is already being enumerated, it cannot be modified at the same time. Ensure you dispose the first enumerator before modifying the WeakCollection.");
		}
		public IEnumerator<T> GetEnumerator()
		{
			if (hasEnumerator)
				throw new InvalidOperationException("The WeakCollection is already being enumerated, it cannot be enumerated twice at the same time. Ensure you dispose the first enumerator before using another enumerator.");
			try {
				hasEnumerator = true;
				for (int i = 0; i < innerList.Count;) {
					T element = (T)innerList[i].Target;
					if (element == null) {
						RemoveAt(i);
					} else {
						yield return element;
						i++;
					}
				}
			} finally {
				hasEnumerator = false;
			}
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
