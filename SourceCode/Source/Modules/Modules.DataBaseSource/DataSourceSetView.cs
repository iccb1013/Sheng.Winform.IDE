/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Modules.DataBaseSourceModule
{
    partial class DataSourceSetView : FormViewBase
    {
        Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        DataSourceCreateView formDataSourceCreate;
        const string ConnectionStringName = AppConstant.ConnectionStringName;
        public DataSourceSetView()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            pictureBox1.Image = ImagesLibrary.DataSource;
        }
        private void DataSourceSetView_Load(object sender, EventArgs e)
        {
            ConnectionStringSettings connectionStringSettings = 
                configuration.ConnectionStrings.ConnectionStrings[ConnectionStringName];
            if (connectionStringSettings != null)
            {
                this.txtConnectionString.Text = connectionStringSettings.ConnectionString;
                this.txtConnectionString.SelectedText = String.Empty;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            string connStr = this.txtConnectionString.Text;
            ConnectionStringSettings connectionStringSettings =
                configuration.ConnectionStrings.ConnectionStrings[ConnectionStringName];
            if (connectionStringSettings == null)
            {
                connectionStringSettings = new ConnectionStringSettings(ConnectionStringName, connStr);
                connectionStringSettings.ProviderName = "System.Data.SqlClient";
                configuration.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);
            }
            else
            {
                connectionStringSettings.ConnectionString = connStr;
            }
            configuration.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void linkLabelCreateDataSource_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (formDataSourceCreate == null)
            {
                formDataSourceCreate = new DataSourceCreateView();
            }
            else
            {
                formDataSourceCreate.ResetFormState();
            }
            if (formDataSourceCreate.ShowDialog() == DialogResult.OK)
            {
                this.txtConnectionString.Text = formDataSourceCreate.ConnectionString;
            }
        }
    }
}
