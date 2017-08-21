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
	sealed class OracleConnectionCommandRegistrar : CommandRegistrar
	{
		public OracleConnectionCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}		
		public override void Register()
		{
			AddOracleConnectionElementCommand();
			AddDefaultCommands(typeof(OracleConnectionElementNode));
			AddOraclePacakgeElementCommand();
			AddDefaultCommands(typeof(OraclePackageElementNode));
		}
		private void AddOracleConnectionElementCommand()
		{
			ConfigurationUICommand item = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider,
				Resources.OracleConnectionUICommandText,
				Resources.OracleConnectionUICommandLongText,
				new AddOracleConnectionElementNodeCommand(ServiceProvider),
				typeof(OracleConnectionElementNode));
			AddUICommand(item, typeof(ConnectionStringSettingsNode));			
		}
		private void AddOraclePacakgeElementCommand()
		{
			AddMultipleChildNodeCommand(Resources.OraclePackageUICommandText,
				Resources.OraclePackageUICommandLongText,
				typeof(OraclePackageElementNode),
				typeof(OracleConnectionElementNode));
		}   
	}
}
