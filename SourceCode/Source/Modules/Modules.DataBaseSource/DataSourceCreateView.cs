/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Modules.DataBaseSourceModule.Localisation;
namespace Sheng.SailingEase.Modules.DataBaseSourceModule
{
    partial class DataSourceCreateView : FormViewBase
    {
        SqlDataSourceEnumerator sqlDataSourceEnumerator;
        SqlConnectionStringBuilder sqlConnectionStringBuilder;
        public string ConnectionString
        {
            get
            {
                sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
                sqlConnectionStringBuilder.DataSource = this.ddlDataSource.Text;
                sqlConnectionStringBuilder.IntegratedSecurity = this.radioButtonIntegratedSecurity.Checked;
                if (this.radioButtonNoIntegratedSecurity.Checked)
                {
                    sqlConnectionStringBuilder.UserID = this.txtUserId.Text;
                    sqlConnectionStringBuilder.Password = this.txtPassword.Text;
                }
                try
                {
                    return sqlConnectionStringBuilder.ConnectionString;
                }
                catch
                {
                    return String.Empty;
                }
            }
        }
        public DataSourceCreateView()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
        }
        private void ApplyLanguageResource()
        {
            this.txtDataSourceType.Text =  EnumDescConverter.Get(typeof(EnumDataBase)).Select
                ("Value = '" + ((int)EnumDataBase.SqlServer).ToString() + "'")[0]["Text"].ToString();
            this.ddlDataSource.Title = Language.Current.DataSourceCreateView_LabelDataSourceName;
            this.txtUserId.Title = Language.Current.DataSourceCreateView_LabelUserId;
            this.txtPassword.Title = Language.Current.DataSourceCreateView_LabelPassword;
        }
        private void btnRefreshDataSource_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            sqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance;
            DataTable dtDataSource = sqlDataSourceEnumerator.GetDataSources();
            DataTable dtDataSource2 = dtDataSource.Clone();
            foreach (DataRow dr in dtDataSource.Rows)
            {
                DataRow dr2;
                if (dr["Version"].ToString() != String.Empty &&
                    Convert.ToInt32(dr["Version"].ToString().Split('.')[0]) < 9)
                {
                    continue;
                }
                dr2 = dtDataSource2.NewRow();
                dr2["ServerName"] = dr["ServerName"].ToString();
                if (dr["InstanceName"].ToString() != String.Empty)
                {
                    dr2["ServerName"] += "\\" + dr["InstanceName"].ToString();
                }
                dtDataSource2.Rows.Add(dr2);
            }
            ddlDataSource.DataSource = dtDataSource2;
            Cursor.Current = Cursors.Default;
        }
        private void RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            this.txtUserId.Enabled = radioButtonNoIntegratedSecurity.Checked;
            this.txtPassword.Enabled = radioButtonNoIntegratedSecurity.Checked;
            this.txtUserId.AllowEmpty = !radioButtonNoIntegratedSecurity.Checked;
        }
        public void ResetFormState()
        {
            this.ddlDataSource.Text = String.Empty;
            this.radioButtonIntegratedSecurity.Checked = true;
            this.txtUserId.Text = String.Empty;
            this.txtPassword.Text = String.Empty;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.DoValidate())
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
