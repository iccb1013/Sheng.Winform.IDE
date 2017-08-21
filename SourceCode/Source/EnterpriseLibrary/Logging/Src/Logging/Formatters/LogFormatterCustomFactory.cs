/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
	public class LogFormatterCustomFactory : AssemblerBasedCustomFactory<ILogFormatter, FormatterData>
	{
		public static LogFormatterCustomFactory Instance = new LogFormatterCustomFactory();
		protected override FormatterData GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			LoggingSettings settings = LoggingSettings.GetLoggingSettings(configurationSource);
			ValidateSettings(settings);
			FormatterData objectConfiguration = settings.Formatters.Get(name);
			ValidateConfiguration(objectConfiguration, name);
			return objectConfiguration;
		}
		private void ValidateConfiguration(FormatterData objectConfiguration, string name)
		{
			if (objectConfiguration == null)
			{
				throw new ConfigurationErrorsException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionFormatterNotDefined,
						name));
			}
		}
		private void ValidateSettings(LoggingSettings settings)
		{
			if (settings == null)
			{
				throw new ConfigurationErrorsException(Resources.ExceptionLoggingSectionNotFound);
			}
		}
	}
}
