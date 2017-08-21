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
    partial class DataBaseCreateWizard_Done : WizardPanelBase
    {
        public DataBaseCreateWizard_Done()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
        }
        public override void ProcessButton()
        {
            this.WizardView.BackButtonEnabled = false;
            this.WizardView.NextButtonEnabled = false;
            this.WizardView.FinishButtonEnabled = true;
        }
    }
}
