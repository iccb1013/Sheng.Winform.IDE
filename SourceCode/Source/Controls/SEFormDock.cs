/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Sheng.SailingEase.Controls.Localisation;
using Sheng.SailingEase.Controls.Docking;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public partial class SEFormDock : DockContent, ISEForm
    {
        public SEFormDock()
        {
            LicenseManager.Validate(typeof(SEFormDock)); 
            InitializeComponent();
        }
        public virtual bool DoValidate()
        {
            string validateMsg;
            bool validateResult =  SEValidateHelper.ValidateContainerControl(this, out validateMsg); 
            if (validateResult == false)
            {
                MessageBox.Show(validateMsg, Language.Current.MessageBoxCaptiton_Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return validateResult;
        }
    }
}
