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
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    [ToolboxItem(false)]
    partial class DataBaseCreateWizard_DataBaseName : WizardPanelBase
    {
        private DataBaseCreateOption _option;
        public DataBaseCreateWizard_DataBaseName()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
        }
        private void ApplyLanguageResource()
        {
            this.txtDataBaseName.Title = Language.Current.UserControlCreateDataBaseWizardStepEnterDataBaseName_LabelDataBaseName;
        }
        public override void ProcessButton()
        {
            this.WizardView.BackButtonEnabled = false;
            this.WizardView.NextButtonEnabled = true;
            this.WizardView.FinishButtonEnabled = false;
        }
        public override void Submit()
        {
            base.Submit();
            if (this.DoValidate() == false)
            {
                ProcessButton();
                return;
            }
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringSettings connectionStringSettings =
               configuration.ConnectionStrings.ConnectionStrings[AppConstant.ConnectionStringName];
            if (connectionStringSettings == null)
            {
                MessageBox.Show(Language.Current.DataBaseCreateWizard_DataBaseName_MessageConnectionStringSettingsIsNull, 
                    CommonLanguage.Current.MessageCaption_Notice,MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProcessButton();
                return;
            }
            string strConnectionString = connectionStringSettings.ConnectionString;
            SqlConnectionStringBuilder qlConnectionStringBuilder;
            try
            {
                qlConnectionStringBuilder = new SqlConnectionStringBuilder(strConnectionString);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProcessButton();
                return;
            }
            qlConnectionStringBuilder.InitialCatalog = this.txtDataBaseName.Text;
            connectionStringSettings =
                configuration.ConnectionStrings.ConnectionStrings[AppConstant.ConnectionStringName2];
            if (connectionStringSettings == null)
            {
                connectionStringSettings = new ConnectionStringSettings(AppConstant.ConnectionStringName2,
                        qlConnectionStringBuilder.ConnectionString);
                connectionStringSettings.ProviderName = "System.Data.SqlClient";
                configuration.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);
            }
            else
            {
                connectionStringSettings.ConnectionString = qlConnectionStringBuilder.ConnectionString;
            }
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
            _option.DataBaseName = txtDataBaseName.Text;
            this.WizardView.NextPanel();
        }
        public override void Run()
        {
            _option = this.WizardView.GetOptionInstance<DataBaseCreateOption>();
            txtDataBaseName.Text = _option.DataBaseName;
        }
    }
}
