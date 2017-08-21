/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers
{
	public interface IDebugUtils
	{
		string GetStackTraceWithSourceInfo( StackTrace stackTrace );
	}
}
