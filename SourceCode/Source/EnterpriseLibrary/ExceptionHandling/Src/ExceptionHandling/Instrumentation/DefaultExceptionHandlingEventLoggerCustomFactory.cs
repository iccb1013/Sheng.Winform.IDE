/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
	public class DefaultExceptionHandlingEventLoggerCustomFactory : DefaultEventLoggerCustomFactoryBase
	{
        protected override object DoCreateObject(InstrumentationConfigurationSection instrumentationConfigurationSection)
		{
			return new DefaultExceptionHandlingEventLogger(instrumentationConfigurationSection.EventLoggingEnabled, instrumentationConfigurationSection.WmiEnabled);
		}
	}
}
