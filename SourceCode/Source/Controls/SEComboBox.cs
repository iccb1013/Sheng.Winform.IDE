/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Sheng.SailingEase.Win32;
using System.Drawing;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEComboBox : ComboBox, ISEValidate
    {
        private bool allowEmpty = true;
        [Description("�Ƿ������")]
        [Category("SEControl")]
        public bool AllowEmpty
        {
            get
            {
                return this.allowEmpty;
            }
            set
            {
                this.allowEmpty = value;
            }
        }
        private string waterText = String.Empty;
        [Description("ˮӡ�ı�")]
        [Category("SEControl")]
        public string WaterText
        {
            get { return this.waterText; }
            set
            {
                this.waterText = value;
                this.Invalidate();
            }
        }
        public SEComboBox()
        {
            LicenseManager.Validate(typeof(SEComboBox));
        }
        protected override void WndProc(ref   Message m)
        {
            base.WndProc(ref   m);
            if (m.Msg == User32.WM_PAINT || m.Msg == User32.WM_ERASEBKGND || m.Msg == User32.WM_NCPAINT)
            {
                if (!this.Focused && this.Text == String.Empty  && this.WaterText != String.Empty)
                {
                    Graphics g = Graphics.FromHwnd(this.Handle);
                    g.DrawString(this.WaterText, this.Font, Brushes.Gray, 2, 2);
                }
            }
        }
        private string title;
        [Description("����")]
        [Category("SEControl")]
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }
        private bool highLight = true;
        [Description("Ҫƥ���������ʽ")]
        [Category("SEControl")]
        public bool HighLight
        {
            get
            {
                return this.highLight;
            }
            set
            {
                this.highLight = value;
            }
        }
        public bool SEValidate(out string msg)
        {
            msg = String.Empty;
            if (!this.AllowEmpty && this.Text == "")
            {
                msg += String.Format("[ {0} ] {1}", this.Title, "������Ϊ��");
                return false;
            }
            if (CustomValidate != null)
            {
                string customValidateMsg;
                if (CustomValidate(this, out customValidateMsg) == false)
                {
                    msg += String.Format("[ {0} ] {1}", this.Title, customValidateMsg);
                    return false;
                }
            }
            return true;
        }
        public CustomValidateMethod CustomValidate
        {
            get;
            set;
        }
    }
}
