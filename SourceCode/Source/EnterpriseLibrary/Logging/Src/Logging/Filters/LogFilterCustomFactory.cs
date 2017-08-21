/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	public class LogFilterCustomFactory : AssemblerBasedObjectFactory<ILogFilter, LogFilterData>
	{
		public static LogFilterCustomFactory Instance = new LogFilterCustomFactory();
	}
}
