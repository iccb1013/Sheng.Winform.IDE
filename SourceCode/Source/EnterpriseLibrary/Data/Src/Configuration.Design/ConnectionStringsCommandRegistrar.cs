/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class ConnectionStringsCommandRegistrar : CommandRegistrar
	{
		public ConnectionStringsCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}				
		public override void Register()
		{
			AddConnectionStringsSectionCommand();
			AddValidateCommand(typeof(ConnectionStringsSectionNode));
			AddConnectionStringSettingsCommand();
			AddDefaultCommands(typeof(ConnectionStringSettingsNode));			
		}
		private void AddConnectionStringsSectionCommand()
		{			
			ConfigurationUICommand item = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider,
				Resources.ConnectionStringsUICommandText,
				Resources.ConnectionStringsUICommandLongText,
				new AddConnectionStringsSectionNodeCommand(ServiceProvider),
				typeof(ConnectionStringSettingsNode));
			AddUICommand(item, typeof(DatabaseSectionNode));
		}
		private void AddConnectionStringSettingsCommand()
		{
			ConfigurationUICommand item = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
				Resources.ConnectionStringUICommandText,
				Resources.ConnectionStringUICommandLongText,
				new AddConnectionStringSettingsNodeCommand(ServiceProvider),
				typeof(ConnectionStringSettingsNode));
			AddUICommand(item, typeof(ConnectionStringsSectionNode));
		}        
	}
}
