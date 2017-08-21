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
using System.Drawing;
using System.ComponentModel;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SELine:Control
    {
        public SELine()
        {
            LicenseManager.Validate(typeof(SELine)); 
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(Pens.Gray, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(Pens.White, 0, 1, this.Width, 1);
        }
    }
}
