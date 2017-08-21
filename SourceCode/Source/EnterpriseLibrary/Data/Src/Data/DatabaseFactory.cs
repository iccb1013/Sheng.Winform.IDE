/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	public static class DatabaseFactory
	{
		public static Database CreateDatabase()
		{
            try
            {
				DatabaseProviderFactory factory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
                return factory.CreateDefault();
            }
            catch (ConfigurationErrorsException configurationException)
            {
                TryLogConfigurationError(configurationException, "default");
                throw;
            }
		}
		public static Database CreateDatabase(string name)
		{
            try
            {
				DatabaseProviderFactory factory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
				return factory.Create(name);
			}
            catch (ConfigurationErrorsException configurationException)
            {
                TryLogConfigurationError(configurationException, name);
                throw;
            }
		}
        private static void TryLogConfigurationError(ConfigurationErrorsException configurationException, string instanceName)
        {
            try
            {
                DefaultDataEventLogger eventLogger = EnterpriseLibraryFactory.BuildUp<DefaultDataEventLogger>();
                if (eventLogger != null)
                {
                    eventLogger.LogConfigurationError(configurationException, instanceName);
                }
            }
            catch { }
		}
	}
}
