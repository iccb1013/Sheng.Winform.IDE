/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    [Image(typeof(DatabaseSectionNode))]
    public sealed class DatabaseSectionNode : ConfigurationSectionNode
    {        
		private ConnectionStringSettingsNode connectionStringSettingsNode;
		private EventHandler<ConfigurationNodeChangedEventArgs> connectionStringNodeRemovedHandler;		        
		public DatabaseSectionNode()
			: base(Resources.DataUICommandText)
        {
			this.connectionStringNodeRemovedHandler = new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringNodeRemoved);            
        }
		protected override void Dispose(bool disposing)
		{			
			if (disposing)
			{
				if (connectionStringSettingsNode != null)
				{
					connectionStringSettingsNode.Removed -= new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringNodeRemoved);				
				}				
			}
			base.Dispose(disposing);
		}
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }            
        }
		[Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
		[ReferenceType(typeof(ConnectionStringSettingsNode))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("DefaultDatabaseDescription", typeof(Resources))]
		public ConnectionStringSettingsNode DefaultDatabase
		{
			get { return connectionStringSettingsNode; }
			set
			{
				connectionStringSettingsNode = LinkNodeHelper.CreateReference<ConnectionStringSettingsNode>(connectionStringSettingsNode,
																								 value,
																								 connectionStringNodeRemovedHandler,
																								 null);				
			}
		}        
        private void OnConnectionStringNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
			connectionStringSettingsNode = null;            
        }        
    }
}
