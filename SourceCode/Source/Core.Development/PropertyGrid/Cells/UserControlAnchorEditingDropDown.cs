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
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlAnchorEditingDropDown : SEAdvComboBoxDropdownBase
    {
        public UserControlAnchorEditingDropDown()
        {
            InitializeComponent();
        }
        private void btnAnchorAll_Click(object sender, EventArgs e)
        {
            if (scbTop.Check && scbRight.Check && scbBottom.Check && scbLeft.Check)
            {
                this.scbTop.Check = false;
                this.scbRight.Check = false;
                this.scbBottom.Check = false;
                this.scbLeft.Check = false;
            }
            else
            {
                this.scbTop.Check = true;
                this.scbRight.Check = true;
                this.scbBottom.Check = true;
                this.scbLeft.Check = true;
            }
            this.Close();
        }
        public override string GetText()
        {
            string strResult = String.Empty;
            if (this.scbTop.Check)
            {
                strResult += "Top,";
            }
            if (this.scbRight.Check)
            {
                strResult += "Right,";
            }
            if (this.scbBottom.Check)
            {
                strResult += "Bottom,";
            }
            if (this.scbLeft.Check)
            {
                strResult += "Left,";
            }
            return strResult.TrimEnd(',');
        }
        public override void SetText(object value)
        {
            string[] strAnchor = value.ToString().Split(',');
            foreach (string str in strAnchor)
            {
                switch (str)
                {
                    case "Top":
                        this.scbTop.Check = true;
                        break;
                    case "Right":
                        this.scbRight.Check = true;
                        break;
                    case "Bottom":
                        this.scbBottom.Check = true;
                        break;
                    case "Left":
                        this.scbLeft.Check = true;
                        break;
                }
            }
        }
    }
}
