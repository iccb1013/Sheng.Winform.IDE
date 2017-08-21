/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	internal class LogWriterStructureHolderUpdater : ILogWriterStructureUpdater
	{
		private LogWriter logWriter;
		private IConfigurationSource configurationSource;
		public LogWriterStructureHolderUpdater(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
			configurationSource.AddSectionChangeHandler(LoggingSettings.SectionName, UpdateLogWriter);
		}
		public void Dispose()
		{
			configurationSource.RemoveSectionChangeHandler(LoggingSettings.SectionName, UpdateLogWriter);
		}
		public void SetLogWriter(LogWriter logWriter)
		{
			this.logWriter = logWriter;
		}
		public void UpdateLogWriter(object sender, ConfigurationChangedEventArgs args)
		{
			if (logWriter != null)
			{
				try
				{
					LogWriterStructureHolder newStructureHolder
						= EnterpriseLibraryFactory.BuildUp<LogWriterStructureHolder>(configurationSource);
					logWriter.ReplaceStructureHolder(newStructureHolder);
				}
				catch (ConfigurationErrorsException configurationException)
				{
					logWriter.ReportConfigurationFailure(configurationException);
				}
			}
		}
	}
}
