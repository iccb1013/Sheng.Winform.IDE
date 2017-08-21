/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	public class ExceptionHandlerCustomFactory : AssemblerBasedObjectFactory<IExceptionHandler, ExceptionHandlerData>
	{
		public static ExceptionHandlerCustomFactory Instance = new ExceptionHandlerCustomFactory();
	}
}
