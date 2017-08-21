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
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class DatabaseSectionBuilder 
	{
		private DatabaseSectionNode databaseSectionNode;
		private IConfigurationUIHierarchy hierarchy;		
		public DatabaseSectionBuilder(IServiceProvider serviceProvider, DatabaseSectionNode databaseSectionNode) 
		{
			this.databaseSectionNode = databaseSectionNode;
			this.hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}		
		public DatabaseSettings Build()
		{
			DatabaseSettings settings = new DatabaseSettings();
			if (!this.databaseSectionNode.RequirePermission)	
				settings.SectionInformation.RequirePermission = this.databaseSectionNode.RequirePermission;
			foreach (ProviderMappingNode node in hierarchy.FindNodesByType(databaseSectionNode, typeof(ProviderMappingNode)))
			{
				settings.ProviderMappings.Add(node.ProviderMapping);
			}
			if (null != databaseSectionNode.DefaultDatabase) settings.DefaultDatabase = databaseSectionNode.DefaultDatabase.Name;
			return settings;
		}		
	}
}
