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
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlAnchorEditingControl : UserControl
    {
        public string AnchorText
        {
            get
            {
                return this.comboBoxAnchor.Text;
            }
            set
            {
                this.comboBoxAnchor.Text = value;
            }
        }
        public UserControlAnchorEditingControl()
        {
            InitializeComponent();
            this.comboBoxAnchor.TextBoxBorder = false;
            this.comboBoxAnchor.OnValueChange+=new EventHandler(comboBoxAnchor_OnValueChange);
            UserControlAnchorEditingDropDown userControlAnchorEditingDropDown = new UserControlAnchorEditingDropDown();
            userControlAnchorEditingDropDown.BorderStyle = BorderStyle.Fixed3D;
            this.comboBoxAnchor.DropUserControl = userControlAnchorEditingDropDown;
        }
        private void comboBoxAnchor_OnValueChange(object sender, EventArgs e)
        {
            OnValueChange();
        }
        public virtual void OnValueChange()
        {
        }
    }
}
