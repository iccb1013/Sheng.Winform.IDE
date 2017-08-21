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
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Sheng.SailingEase.Controls.Localisation;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public partial class SEUserControl : UserControl, ISEValidate
    {
        public SEUserControl()
        {
            InitializeComponent();
        }
        public virtual bool DoValidate()
        {
            bool validateResult = true;
            string validateMsg;
            validateResult = this.SEValidate(out validateMsg);
            if (validateResult == false)
            {
                MessageBox.Show(validateMsg, Language.Current.MessageBoxCaptiton_Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return validateResult;
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
        private bool highLight = false;
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
        public virtual bool SEValidate(out string validateMsg)
        {
            return SEValidateHelper.ValidateContainerControl(this, out validateMsg);
        }
        public CustomValidateMethod CustomValidate
        {
            get;
            set;
        }
    }
}
