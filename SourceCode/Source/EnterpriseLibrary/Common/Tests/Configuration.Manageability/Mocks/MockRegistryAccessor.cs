/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class MockRegistryAccessor : IRegistryAccessor
	{
		private MockRegistryKey currentUser;
		private MockRegistryKey localMachine;
		public MockRegistryAccessor(MockRegistryKey currentUser, MockRegistryKey localMachine)
		{
			this.currentUser = currentUser;
			this.localMachine = localMachine;
		}
		public IRegistryKey CurrentUser
		{
			get { return currentUser; }
		}
		public IRegistryKey LocalMachine
		{
			get { return localMachine; }
		}
	}
}
