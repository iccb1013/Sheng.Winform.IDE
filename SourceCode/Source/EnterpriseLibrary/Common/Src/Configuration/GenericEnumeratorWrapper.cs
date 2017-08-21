/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	internal class GenericEnumeratorWrapper<T> : IEnumerator<T>
	{
		private IEnumerator wrappedEnumerator;
		internal GenericEnumeratorWrapper(IEnumerator wrappedEnumerator)
		{
			this.wrappedEnumerator = wrappedEnumerator;
		}
		T IEnumerator<T>.Current
		{
			get { return (T) this.wrappedEnumerator.Current; }
		}
		void IDisposable.Dispose()
		{
			this.wrappedEnumerator = null;
		}		
		object IEnumerator.Current
		{
			get { return this.wrappedEnumerator.Current; }
		}
		bool IEnumerator.MoveNext()
		{
			return this.wrappedEnumerator.MoveNext();
		}
		void IEnumerator.Reset()
		{
			this.wrappedEnumerator.Reset();
		}	
	}
}
