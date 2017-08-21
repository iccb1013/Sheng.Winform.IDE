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
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEDatetimePicker:DateTimePicker
    {
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
        private string relationType;
        [Description("�������ͣ�Start��End�����")]
        [Category("SEControl")]
        public string RelationType
        {
            get
            {
                return this.relationType;
            }
            set
            {
                this.relationType = value;
            }
        }
        private SEDatetimePicker relation;
        [Description("��������")]
        [Category("SEControl")]
        public SEDatetimePicker Relation
        {
            get
            {
                return this.relation;
            }
            set
            {
                this.relation = value;
            }
        }
        public SEDatetimePicker()
        {
            LicenseManager.Validate(typeof(SEDatetimePicker)); 
        }
        protected override void OnValueChanged(EventArgs eventargs)
        {
            base.OnValueChanged(eventargs);
        }
    }
}
