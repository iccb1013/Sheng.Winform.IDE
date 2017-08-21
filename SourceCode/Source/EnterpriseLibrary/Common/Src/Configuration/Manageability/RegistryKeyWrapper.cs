/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Win32;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	internal class RegistryKeyWrapper : RegistryKeyBase, IRegistryKey
	{
		private RegistryKey registryKey;
		public RegistryKeyWrapper(RegistryKey registryKey)
		{
			this.registryKey = registryKey;
		}
		protected override object DoGetValue(string name)
		{
			return this.registryKey.GetValue(name);
		}
		public override void Close()
		{
			this.registryKey.Close();
		}
		public override string[] GetValueNames()
		{
			return this.registryKey.GetValueNames();
		}
		public override IRegistryKey DoOpenSubKey(string name)
		{
			RegistryKey subKey = this.registryKey.OpenSubKey(name, RegistryKeyPermissionCheck.ReadSubTree);
			if (subKey != null)
				return new RegistryKeyWrapper(subKey);
			return null;
		}
		public void Dispose()
		{
			this.Close();
		}
		public override string Name
		{
			get { return registryKey.Name; }
		}
	}
}
