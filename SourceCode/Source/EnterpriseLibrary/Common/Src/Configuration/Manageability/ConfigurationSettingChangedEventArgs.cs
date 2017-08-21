/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public class ConfigurationSettingChangedEventArgs : EventArgs
	{
		internal ConfigurationSettingChangedEventArgs(string sectionName)
		{
			this.SectionName = sectionName;
		}
		public string SectionName { get; private set; }
	}
}
