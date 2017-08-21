/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
	[ManagementEntity]
	public abstract class FormatterSetting : NamedConfigurationSetting
	{
		protected FormatterSetting(FormatterData sourceElement, string name)
			: base(sourceElement, name)
		{ }
	}
}
