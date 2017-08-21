/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    [ToolboxItem(false)]
    partial class DataBaseCreateWizard_Confirm : WizardPanelBase
    {
        public DataBaseCreateWizard_Confirm()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            this.BackSkip = true;
        }
        public override void ProcessButton()
        {
            this.WizardView.BackButtonEnabled = true;
            this.WizardView.NextButtonEnabled = true;
            this.WizardView.FinishButtonEnabled = false;
        }
        public override void Submit()
        {
            this.WizardView.NextPanel();
        }
    }
}
