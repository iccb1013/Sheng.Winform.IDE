/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	public class DatabaseMapper : IConfigurationNameMapper
	{
		public string MapName(string name, IConfigurationSource configSource)
		{
			if (name != null)
				return name;
			return new DatabaseConfigurationView(configSource).DefaultName;
		}
	}
}
