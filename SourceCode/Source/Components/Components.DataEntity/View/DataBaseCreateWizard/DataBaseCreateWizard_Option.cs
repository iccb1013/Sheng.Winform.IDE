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
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    [ToolboxItem(false)]
    partial class DataBaseCreateWizard_Option : WizardPanelBase
    {
        private DataBaseCreateOption _option;
        public DataBaseCreateWizard_Option()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
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
            _option.CreateDataBase = !cbNoCreateDataBase.Checked;
            _option.InsertEnum = cbInsertEnum.Checked;
            this.WizardView.NextPanel();
        }
        public override void Run()
        {
            _option = this.WizardView.GetOptionInstance<DataBaseCreateOption>();
            cbNoCreateDataBase.Checked = !_option.CreateDataBase;
            cbInsertEnum.Checked = _option.InsertEnum;
        }
    }
}
