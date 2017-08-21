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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    [ToolboxItem(false)]
    partial class DataBaseCreateWizard_Account : WizardPanelBase
    {
        private DataBaseCreateOption _option;
        public DataBaseCreateWizard_Account()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
        }
        private void ApplyLanguageResource()
        {
            this.txtLoginName.Title = Language.Current.UserControlCreateDataBaseWizardStepAccount_LabelAccount;
            this.txtPassword.Title = Language.Current.UserControlCreateDataBaseWizardStepAccount_LabelPassword;
        }
        public override void ProcessButton()
        {
            this.WizardView.BackButtonEnabled = true;
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
            _option.LoginName = this.txtLoginName.Text;
            _option.Password = this.txtPassword.Text;
            this.WizardView.NextPanel();
        }
        public override void Run()
        {
            _option = this.WizardView.GetOptionInstance<DataBaseCreateOption>();
            this.txtLoginName.Text = _option.LoginName;
            this.txtPassword.Text = _option.Password;
        }
        private void cbNoPasswordChar_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbNoPasswordChar.Checked)
            {
                this.txtPassword.PasswordChar = '\0';
            }
            else
            {
                this.txtPassword.PasswordChar = '*';
            }
        }
    }
}
