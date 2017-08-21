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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.ComponentModel.Design
{
    [ToolboxItem(false)]
    partial class UserControlColorChooseEditingControl : UserControl
    {
        public string ColorValue
        {
            get
            {
                return this.colorChooseComboBox.Value;
            }
            set
            {
                this.colorChooseComboBox.Value = value;
            }
        }
        public UserControlColorChooseEditingControl()
        {
            InitializeComponent();
            this.colorChooseComboBox.OwnerForm = EnvironmentHelper.MainForm;
            this.colorChooseComboBox.TextBoxBorder = false;
            this.colorChooseComboBox.OnSelectedChange+=new EventHandler(colorChooseComboBox_OnSelectedChange);
        }
        private void colorChooseComboBox_OnSelectedChange(object sender, EventArgs e)
        {
            OnColorChange();
        }
        public virtual void OnColorChange()
        {
        }
    }
}
