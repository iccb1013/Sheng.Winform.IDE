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
using System.Reflection;
using System.Drawing;
using System.ComponentModel;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SETabControl : TabControl, ISEValidate
    {
        public SETabControl()
        {
            LicenseManager.Validate(typeof(SETabControl));
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
        public bool SEValidate(out string validateMsg)
        {
            bool validateResult = true;
            string tabValidateMsg;
            validateMsg = String.Empty;
            foreach (TabPage tabPage in this.TabPages)
            {
                if (SEValidateHelper.ValidateContainerControl(tabPage, out tabValidateMsg) == false)
                {
                    validateMsg += tabValidateMsg;
                    validateResult = false;
                }
            }
            return validateResult;
        }
        public CustomValidateMethod CustomValidate
        {
            get;
            set;
        }
    }
}
