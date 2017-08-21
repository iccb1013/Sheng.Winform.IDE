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
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Drawing;
using Sheng.SailingEase.Win32;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SETextBox : TextBox, ISEValidate
    {
        public SETextBox()
        {
            LicenseManager.Validate(typeof(SETextBox));
        }
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
        private string regex = String.Empty;
        [Description("Ҫƥ���������ʽ")]
        [Category("SEControl")]
        public string Regex
        {
            get
            {
                if (this.regex == null)
                {
                    this.regex = String.Empty;
                }
                return this.regex;
            }
            set
            {
                this.regex = value;
            }
        }
        private string regexMsg;
        [Description("������֤��ͨ��ʱ����ʾ��Ϣ")]
        [Category("SEControl")]
        public string RegexMsg
        {
            get
            {
                return this.regexMsg;
            }
            set
            {
                this.regexMsg = value;
            }
        }
        private SETextBox valueCompareTo;
        [Description("��ָ����SETextBox��ֵ���Ƚϣ�������ͬ")]
        [Category("SEControl")]
        public SETextBox ValueCompareTo
        {
            get
            {
                return this.valueCompareTo;
            }
            set
            {
                this.valueCompareTo = value;
            }
        }
        private bool limitMaxValue = false;
        [Description("��ֻ�����������ֵ������,�Ƿ��������ֵ")]
        [Category("SEControl")]
        public bool LimitMaxValue
        {
            get { return this.limitMaxValue; }
            set { this.limitMaxValue = value; }
        }
        private long maxValue = Int32.MaxValue;
        [Description("��ֻ�����������ֵ������,��������ֵ")]
        [Category("SEControl")]
        public long MaxValue
        {
            get { return this.maxValue; }
            set { this.maxValue = value; }
        }
        protected override void WndProc(ref   Message m)
        {
            base.WndProc(ref   m);
            if (m.Msg == User32.WM_PAINT || m.Msg == User32.WM_ERASEBKGND || m.Msg == User32.WM_NCPAINT)
            {
                if (!this.Focused && this.Text == String.Empty && this.WaterText != String.Empty)
                {
                    Graphics g = Graphics.FromHwnd(this.Handle);
                    g.DrawString(this.WaterText, this.Font, Brushes.Gray, this.ClientRectangle);
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
        [Description("��֤ʧ��ʱ�Ƿ���Ҫ������ʾ���ı䱳��ɫ��")]
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
            if (this.Text != "" && this.Regex != String.Empty)
            {
                System.Text.RegularExpressions.Regex r = new Regex(this.Regex, RegexOptions.Singleline);
                Match m = r.Match(this.Text);
                if (m.Success == false)
                {
                    msg += String.Format("[ {0} ] {1}", this.Title, this.RegexMsg);
                    return false;
                }
            }
            if (LimitMaxValue && this.Text != String.Empty)
            {
                Regex regex = new Regex(@"^\d+$");
                Match match = regex.Match(this.Text);
                if (match.Success)
                {
                    long value = Int64.Parse(this.Text);
                    if (value > this.MaxValue)
                    {
                        msg += String.Format("[ {0} ] {1}", this.Title, "���ܴ��� " + this.MaxValue.ToString());
                        return false;
                    }
                }
            }
            if (this.ValueCompareTo != null)
            {
                if (this.Text != this.ValueCompareTo.Text)
                {
                    msg += String.Format("[ {0} ] �� [ {1} ] {2}", this.Title, this.ValueCompareTo.Title, "���������ݱ�����ͬ");
                    this.ValueCompareTo.BackColor = Color.Pink;
                    return false;
                }
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
            msg = String.Empty;
            return true;
        }
        public CustomValidateMethod CustomValidate
        {
            get;
            set;
        }
    }
}
