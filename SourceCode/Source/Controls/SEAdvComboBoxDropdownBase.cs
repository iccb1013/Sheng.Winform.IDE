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
    public partial class SEAdvComboBoxDropdownBase : UserControl
    {
        public SEAdvComboBoxDropdownBase()
        {
            InitializeComponent();
        }
        public virtual string GetText()
        {
            return String.Empty; 
        }
        public virtual object GetValue()
        {
            return null;
        }
        public virtual void SetText(object value)
        {
        }
        protected void Close()
        {
            this.FindForm().Hide();
        }
    }
}
