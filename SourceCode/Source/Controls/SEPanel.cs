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
using System.Drawing.Drawing2D;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEPanel : Panel, ISEValidate
    {
        private bool showBorder = false;
        public bool ShowBorder
        {
            get
            {
                return this.showBorder;
            }
            set
            {
                this.showBorder = value;
                InitBrush();
                InitPen();
                this.Invalidate();
            }
        }
        private Color borderColor = Color.Black;
        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                this.borderColor = value;
                InitPen();
            }
        }
        private Color fillColorStart;
        public Color FillColorStart
        {
            get
            {
                if (this.fillColorStart == null)
                {
                    this.fillColorStart = this.Parent.BackColor;
                }
                return this.fillColorStart;
            }
            set
            {
                this.fillColorStart = value;
                InitBrush();
            }
        }
        private Color fillColorEnd;
        public Color FillColorEnd
        {
            get
            {
                if (this.fillColorEnd == null)
                {
                    this.fillColorEnd = this.Parent.BackColor;
                }
                return this.fillColorEnd;
            }
            set
            {
                this.fillColorEnd = value;
                InitBrush();
            }
        }
        private Brush fillBrush;
        public Brush FillBrush
        {
            get
            {
                return this.fillBrush;
            }
            set
            {
                this.fillBrush = value;
                this.Invalidate();
            }
        }
        private Pen borderPen;
        public Pen BorderPen
        {
            get
            {
                return this.borderPen;
            }
            set
            {
                this.borderPen = value;
                this.Invalidate();
            }
        }
        private FillStyle fillStyle = FillStyle.Solid;
        public FillStyle FillStyle
        {
            get
            {
                return this.fillStyle;
            }
            set
            {
                this.fillStyle = value;
                InitBrush();
            }
        }
        private LinearGradientMode fillMode;
        public LinearGradientMode FillMode
        {
            get
            {
                return this.fillMode;
            }
            set
            {
                this.fillMode = value;
                InitBrush();
            }
        }
        private Rectangle FillRectangle
        {
            get
            {
                if (this.ClientRectangle == new Rectangle())
                {
                    return new Rectangle(0, 0, 1, 1);
                }
                Rectangle rect;
                if (this.ShowBorder)
                {
                    rect = new Rectangle(1, 1, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
                }
                else
                {
                    rect = this.ClientRectangle;
                }
                if (rect.Width == 0)
                {
                    rect.Width++;
                }
                if (rect.Height == 0)
                {
                    rect.Height++;
                }
                return rect;
            }
        }
        private Rectangle DrawRectangle
        {
            get
            {
                if (this.ClientRectangle == new Rectangle())
                {
                    return new Rectangle(0, 0, 1, 1);
                }
                return new Rectangle(0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
            }
        }
        public SEPanel()
        {
            LicenseManager.Validate(typeof(SEPanel));
            EnableDoubleBuffering();
            this.FillBrush = new SolidBrush(this.FillColorStart);
            this.BorderPen = new Pen(this.BorderColor);;
        }
        private void EnableDoubleBuffering()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint,
               true);
            this.UpdateStyles();
        }
        private void InitPen()
        {
            if (this.ShowBorder)
            {
                this.BorderPen = new Pen(this.BorderColor);
            }
        }
        private void InitBrush()
        {
            if (this.FillStyle == FillStyle.Solid)
            {
                this.FillBrush = new SolidBrush(this.FillColorStart);
            }
            else
            {
                this.FillBrush = new LinearGradientBrush(this.FillRectangle, this.FillColorStart, this.FillColorEnd, this.FillMode);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(this.FillBrush, this.FillRectangle);
            if (this.ShowBorder)
            {
                e.Graphics.DrawRectangle(this.BorderPen, this.DrawRectangle);
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Height <= 0 || this.Width <= 0)
            {
                return;
            }
            InitBrush();
            InitPen();
            this.Invalidate();
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
