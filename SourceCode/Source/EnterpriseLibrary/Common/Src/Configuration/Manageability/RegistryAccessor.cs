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
	internal class RegistryAccessor : IRegistryAccessor
	{
		public IRegistryKey CurrentUser
		{
			get { return new RegistryKeyWrapper(Registry.CurrentUser); }
		}
		public IRegistryKey LocalMachine
		{
			get { return new RegistryKeyWrapper(Registry.LocalMachine); }
		}
	}
}
