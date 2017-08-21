/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using System.Drawing.Design;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    [Image(typeof(ConnectionStringSettingsNode))]
    public sealed class ConnectionStringSettingsNode : ConfigurationNode, IDatabaseProviderName
    {
        private string providerName;
        private string connectionString;
        public ConnectionStringSettingsNode()
			: this(new ConnectionStringSettings(Resources.ConnectionStringNodeDefaultName, @"Database=Database;Server=(local)\SQLEXPRESS;Integrated Security=SSPI", typeof(SqlConnection).Namespace))
        {
        }
        public ConnectionStringSettingsNode(ConnectionStringSettings connectionString)
            : base()
        {
            if (null == connectionString)
            {
                throw new ArgumentNullException("connectionString");
            }
            this.providerName = connectionString.ProviderName;
            this.connectionString = connectionString.ConnectionString;
            Rename(connectionString.Name);
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("ProviderNameDescription", typeof(Resources))]
        [Editor(typeof(ProviderEditor), typeof(UITypeEditor))]
		[EnvironmentOverridable(false)]
        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }
        [Required]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("ConnectionStringDescription", typeof(Resources))]
        [Editor(typeof(ConnectionStringEditor), typeof(UITypeEditor))]
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }
        [Browsable(false)]
        public override bool SortChildren
        {
            get
            {
                return false;
            }
        }
        [Browsable(false)]
        string IDatabaseProviderName.DatabaseProviderName
        {
            get { return ProviderName; }
        }
    }
}
