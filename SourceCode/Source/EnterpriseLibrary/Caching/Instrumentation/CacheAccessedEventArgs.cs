/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public class CacheAccessedEventArgs : EventArgs
	{
		private string key;
		private bool hit;
		public CacheAccessedEventArgs(string key, bool hit)
		{
			this.key = key;
			this.hit = hit;
		}
		public string Key
		{
			get { return key; }
		}
		public bool Hit
		{
			get { return hit; }
		}
	}
}
