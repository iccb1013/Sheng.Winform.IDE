/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
	[ManagementEntity]
	public abstract partial class LogFilterSetting : NamedConfigurationSetting
	{
		protected LogFilterSetting(LogFilterData sourceElement, string name)
			: base(sourceElement, name)
		{ }
	}
}
