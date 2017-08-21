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
namespace Sheng.SailingEase.Controls
{
    [ToolboxItem(false)]
    public partial class WizardPanelBase : SEUserControl
    {
        private IWizardView _wizardView;
        protected internal IWizardView WizardView
        {
            get { return _wizardView; }
            set { _wizardView = value; }
        }
        private bool backSkip = false;
        public bool BackSkip
        {
            get
            {
                return this.backSkip;
            }
            protected set
            {
                this.backSkip = value;
            }
        }
        public WizardPanelBase()
        {
            InitializeComponent();
        }
        public virtual void Submit()
        {
        }
        public virtual void ProcessButton()
        {
        }
        public virtual void Run()
        {
        }
    }
}
