/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	public class LogWriterFactory
	{
		private IConfigurationSource configurationSource;
		public LogWriterFactory()
			: this(ConfigurationSourceFactory.Create())
		{
		}
		public LogWriterFactory(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}
		public LogWriter Create()
		{
			return EnterpriseLibraryFactory.BuildUp<LogWriter>(configurationSource);
		}
	}
}
