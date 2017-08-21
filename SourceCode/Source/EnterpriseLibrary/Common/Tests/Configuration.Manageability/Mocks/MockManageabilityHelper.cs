/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	internal class MockManageabilityHelper : IManageabilityHelper
	{
		public bool updateCalled = false;
		public bool addHandlerCalled = false;
		public ICollection<String> notifiedSections = new List<String>();
		private IEnumerable<String> sectionNames;
		public MockManageabilityHelper()
			: this(new String[0])
		{ }
		public MockManageabilityHelper(params String[] sectionNames)
		{
			this.sectionNames = sectionNames;
		}
		public void UpdateConfigurationManageability(IConfigurationAccessor configurationAccessor)
		{
			updateCalled = true;
			foreach (String sectionName in sectionNames)
			{
				configurationAccessor.GetSection(sectionName);
				notifiedSections.Add(sectionName);
			}
		}
		public void UpdateConfigurationSectionManageability(IConfigurationAccessor configurationAccessor, string sectionName)
		{
			throw new NotImplementedException();
		}
		public event EventHandler<ConfigurationSettingChangedEventArgs> ConfigurationSettingChanged
		{
			add { this.addHandlerCalled = true; }
			remove { }
		}
	}
}
