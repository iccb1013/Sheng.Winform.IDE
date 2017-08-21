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
using System.Drawing.Drawing2D;
using System.ComponentModel;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SETitleLabel:Control
    {
        private string showText = String.Empty;
        [Description("Ҫ��ʾ���ı�")]
        [Category("SEControl")]
        public string ShowText
        {
            get
            {
                return this.showText;
            }
            set
            {
                this.showText = value;
            }
        }
        public SETitleLabel()
        {
            LicenseManager.Validate(typeof(SETitleLabel)); 
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            LinearGradientBrush brush = new LinearGradientBrush(e.ClipRectangle, Color.Gray, Color.LightGray, 90);
            e.Graphics.FillRectangle(brush, e.ClipRectangle);
            e.Graphics.DrawString(this.ShowText, new Font("����", 10), new SolidBrush(Color.White), 10, 10);
        }
    }
}
