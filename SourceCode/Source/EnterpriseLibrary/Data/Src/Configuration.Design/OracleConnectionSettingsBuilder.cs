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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class OracleConnectionSettingsBuilder 
	{
		private IConfigurationUIHierarchy hierarchy;
		private OracleConnectionSettings oracleConnectionSettings;
		public OracleConnectionSettingsBuilder(IServiceProvider serviceProvider) 
		{
			this.hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}
		public OracleConnectionSettings Build()
		{
			oracleConnectionSettings = new OracleConnectionSettings();
			IList<ConfigurationNode> connections = hierarchy.FindNodesByType(typeof(OracleConnectionElementNode));
			for (int index = 0; index < connections.Count; ++index)
			{
				OracleConnectionData data = new OracleConnectionData();
				data.Name = connections[index].Parent.Name;
				foreach (OraclePackageElementNode node in connections[index].Nodes)
				{
					data.Packages.Add(node.OraclePackageElement);
				}
				oracleConnectionSettings.OracleConnectionsData.Add(data);
			}
			return oracleConnectionSettings;
		}
	}
}
