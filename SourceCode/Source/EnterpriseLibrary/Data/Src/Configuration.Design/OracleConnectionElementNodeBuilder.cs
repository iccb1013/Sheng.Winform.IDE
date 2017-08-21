/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using System.Globalization;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{	
	sealed class OracleConnectionNodeBuilder : NodeBuilder
	{
		private IConfigurationUIHierarchy hierarchy;
		private OracleConnectionSettings oracleConnectionSettings;
		public OracleConnectionNodeBuilder(IServiceProvider serviceProvider, OracleConnectionSettings oracleConnectionSettings)
			: base(serviceProvider)
		{
			this.hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
			this.oracleConnectionSettings = oracleConnectionSettings;
		}
		public void Build()
		{			
			ConnectionStringsSectionNode node = hierarchy.FindNodeByType(typeof(ConnectionStringsSectionNode)) as ConnectionStringsSectionNode;			
			if (null == node)
			{
				LogError(hierarchy.RootNode, Resources.ExceptionMissingConnectionStrings);
				return;
			}
			for (int index = 0; index < oracleConnectionSettings.OracleConnectionsData.Count; ++index)
			{
				OracleConnectionData oracleConnection = oracleConnectionSettings.OracleConnectionsData.Get(index);
				ConnectionStringSettingsNode connectionStringNode = hierarchy.FindNodeByName(node, oracleConnection.Name) as ConnectionStringSettingsNode;
				if (null == connectionStringNode) 
				{
					LogError(node, string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionConnectionStringMissing, oracleConnection.Name));
					continue;
				}
				OracleConnectionElementNode oracleElementNode = new OracleConnectionElementNode();				
				foreach (OraclePackageData packageData in oracleConnection.Packages)
				{
					oracleElementNode.AddNode(new OraclePackageElementNode(packageData));
				}
				connectionStringNode.AddNode(oracleElementNode);
			}				
		}		
	}
}
