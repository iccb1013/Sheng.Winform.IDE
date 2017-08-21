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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Sheng.SailingEase.Drawing;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public partial class SEFlatButton : UserControl
    {
        int imageLocationX ;
        int imageLocationY ;
        int textLocationX ;
        int textLocationY;
        SolidBrush textBrush;
        Rectangle imageRect;
        LinearGradientBrush backBrush;
        LinearGradientBrush backBrush_Selected;
        Rectangle fillRect;
        Rectangle drawRect;
        Pen drawPen;
        Pen drawPen_Selected;
        private bool allowSelect = true;
        public bool AllowSelect
        {
            get { return allowSelect; }
            set { allowSelect = value; }
        }
        private bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                this.Refresh();
            }
        }
        private Image image;
        public Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                this.Refresh();
            }
        }
        private string showText;
        public string ShowText
        {
            get
            {
                return this.showText;
            }
            set
            {
                this.showText = value;
                this.Refresh();
            }
        }
        StringFormat stringFormat = new StringFormat();
        public SEFlatButton()
        {
            LicenseManager.Validate(typeof(SEFlatButton)); 
            InitializeComponent();
            EnableDoubleBuffering();
            stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            imageLocationX = 8;
            imageLocationY = (int)Math.Round((float)(this.ClientRectangle.Height - (int)Math.Round(this.Font.SizeInPoints)) / 2);
            imageLocationY = imageLocationY - 2;
            textLocationX = 26;
            textLocationY = (int)Math.Round((float)(this.ClientRectangle.Height - (int)Math.Round(this.Font.SizeInPoints)) / 2);
            textLocationY = textLocationY - 1;
            fillRect = new Rectangle(0, 0, this.Bounds.Width, this.Bounds.Height);
            drawRect = new Rectangle(0, 0, this.Bounds.Width - 2, this.Bounds.Height - 2);
            textBrush = new SolidBrush(this.ForeColor);
            imageRect = new Rectangle(imageLocationX, imageLocationY, 16, 16);
            backBrush = new LinearGradientBrush(drawRect,
                 Color.White,Color.FromArgb(236,233,217),LinearGradientMode.ForwardDiagonal);
            backBrush_Selected = new LinearGradientBrush(drawRect,
                 Color.FromArgb(241, 243, 236), Color.FromArgb(188, 196, 166), LinearGradientMode.ForwardDiagonal);
            drawPen = new Pen(SystemColors.ActiveCaption);
            drawPen_Selected = new Pen(SystemColors.ActiveCaption);
            if (this.Selected)
            {
                e.Graphics.FillPath(backBrush_Selected, DrawingTool.RoundedRect(fillRect, 3));
                e.Graphics.DrawPath(drawPen_Selected, DrawingTool.RoundedRect(drawRect, 3));
            }
            else
            {
                e.Graphics.FillPath(backBrush, DrawingTool.RoundedRect(fillRect, 3));
                e.Graphics.DrawPath(drawPen, DrawingTool.RoundedRect(drawRect, 3));
            }
            if (this.Image != null)
            {
                e.Graphics.DrawImage(this.Image, imageRect);
                e.Graphics.DrawString(this.Text, this.Font, textBrush, new Point(textLocationX + 14, textLocationY), stringFormat);
            }
            e.Graphics.DrawString(this.ShowText, this.Font, textBrush, new Point(textLocationX, textLocationY), stringFormat);
        }
        private void EnableDoubleBuffering()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw,
               true);
            this.UpdateStyles();
        }
        protected override void OnClick(EventArgs e)
        {
            if (this.Selected)
            {
                return;
            }
            if (this.AllowSelect && !this.Selected)
            {
                this.Selected = true;
            }
            base.OnClick(e);
        }
        protected override bool ShowFocusCues
        {
            get
            {
                return true;
            }
        }
    }
}
