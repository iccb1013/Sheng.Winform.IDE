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
    public class SEGroupBox : GroupBox, ISEValidate
    {
        public SEGroupBox()
        {
            LicenseManager.Validate(typeof(SEGroupBox));
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
            return SEValidateHelper.ValidateContainerControl(this, out validateMsg);
        }
        public CustomValidateMethod CustomValidate
        {
            get;
            set;
        }
    }
}
