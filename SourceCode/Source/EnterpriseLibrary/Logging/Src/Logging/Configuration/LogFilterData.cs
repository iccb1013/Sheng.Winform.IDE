/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	public class LogFilterData : NameTypeConfigurationElement
	{
		public LogFilterData()
		{
		}
		public LogFilterData(string name, Type type)
			: base(name, type)
		{
		}
	}
}
