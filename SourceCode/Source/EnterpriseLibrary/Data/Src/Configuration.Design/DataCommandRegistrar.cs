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
	sealed class DataCommandRegistrar : CommandRegistrar
	{
		public DataCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}		
		public override void Register()
		{
			AddDataCommand();
			AddDefaultCommands(typeof(DatabaseSectionNode));
			AddProviderMappingCommand();
			AddDefaultCommands(typeof(ProviderMappingNode));
		}
		private void AddDataCommand()
		{
			ConfigurationUICommand item = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider,
				Resources.DataUICommandText,
				Resources.DataUICommandLongText,
				new AddDatabaseSectionNodeCommand(ServiceProvider),
				typeof(DatabaseSectionNode));
			AddUICommand(item, typeof(ConfigurationApplicationNode));			
		}
		private void AddProviderMappingCommand()
		{
			AddMultipleChildNodeCommand(Resources.ProviderMappingUICommandText,
				Resources.ProviderMappingUICommandLongText,
				typeof(ProviderMappingNode),
				typeof(ProviderMappingsNode));
		}   
	}
}
