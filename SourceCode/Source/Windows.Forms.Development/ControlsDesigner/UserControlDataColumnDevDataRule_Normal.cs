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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    [ToolboxItem(false)]
    partial class UserControlDataColumnDevDataRule_Normal : UserControlViewBase, IFormElementDataListColumnDataRuleParameterSetControl
    {
        public UserControlDataColumnDevDataRule_Normal()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }
        public UIElementDataListColumnDataRuleAbstract GetParameter()
        {
            return new FormElementDataListColumnDataRulesDev.NormalDev();
        }
        public void SetParameter(UIElementDataListColumnDataRuleAbstract dataRule)
        {
        }
    }
}
