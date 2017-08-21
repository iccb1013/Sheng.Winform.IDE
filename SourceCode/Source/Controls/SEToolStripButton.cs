/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEToolStripButton : ToolStripButton
    {
        public SEToolStripButton()
        {
            LicenseManager.Validate(typeof(SEToolStripButton)); 
        }
    }
}
