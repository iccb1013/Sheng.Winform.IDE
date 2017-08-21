//Visual C# Kicks - .Net Articles and Resources
//http://vckicks.110mb.com/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Sheng.SailingEase.Controls
{
    [DefaultEvent("Click")] //Already built-in
    public partial class SECommandLink : UserControl
    {
        public SECommandLink()
        {
            InitializeComponent();
            this.DoubleBuffered = true; //Smooth redrawing
        }

        #region Fields---------------------------------
        public enum State
        {
            Normal,
            Hover,
            Pushed,
            Disabled
        }

        public enum VerticalAlign
        {
            Top,
            Middle,
            Bottom
        }

        private State state = State.Normal;
        private int offset = 0;

        private string headerText = "Header Text";
        private string descriptionText = "Description";

        private Bitmap image;
        private Bitmap grayImage;
        private Size imageSize = new Size(24, 24);
        private VerticalAlign imageAlign = VerticalAlign.Middle;

        private Font descriptFont;
        private DialogResult diagResult = DialogResult.None;

        #endregion

        #region Properties----------------------------
        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue("Header Text")]
        public string HeaderText
        {
            get
            {
                return headerText;
            }
            set
            {
                headerText = value;
                this.Refresh();
            }
        }

        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue("Description")]
        public string DescriptionText
        {
            get
            {
                return descriptionText;
            }
            set
            {
                descriptionText = value;
                this.Refresh();
            }
        }

        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue(null)]
        public Bitmap Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                if (image != null)
                    grayImage = GetGrayscale(image); //generate image for disabled state
                else
                    grayImage = null;
                this.Refresh();
            }
        }

        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue(typeof(Size), "24,24")]
        public Size ImageScalingSize
        {
            get
            {
                return imageSize;
            }
            set
            {
                imageSize = value;
                this.Refresh();
            }
        }

        [Category("Command Appearance"),
         Browsable(true),
         DefaultValue(VerticalAlign.Top)]
        public VerticalAlign ImageVerticalAlign
        {
            get
            {
                return imageAlign;
            }
            set
            {
                imageAlign = value;
                this.Refresh();
            }
        }

        [Category("Command Appearance")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                //Update the description font, which is the same just 3 sizes smaller
                descriptFont = new Font(this.Font.FontFamily, this.Font.Size - 3);
            }
        }

        [Category("Behavior"),
         DefaultValue(DialogResult.None)]
        public DialogResult DialogResult
        {
            get
            {
                return diagResult;
            }
            set
            {
                diagResult = value;
            }
        }

        #endregion

        #region Events-----------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); //draws the regular background stuff

            if (this.Focused && state == State.Normal)
                DrawHighlight(e.Graphics);

            switch (state)
            {
                case State.Normal:
                    DrawNormalState(e.Graphics);
                    break;
                case State.Hover:
                    DrawHoverState(e.Graphics);
                    break;
                case State.Pushed:
                    DrawPushedState(e.Graphics);
                    break;
                case State.Disabled:
                    DrawNormalState(e.Graphics); //DrawForeground takes care of drawing the disabled state
                    break;
                default:
                    break;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (diagResult != DialogResult.None)
            {
                this.ParentForm.DialogResult = diagResult;
                this.ParentForm.Close();
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                this.PerformClick();

            base.OnKeyPress(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            this.Refresh();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.Refresh();
            base.OnLostFocus(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.Enabled)
                state = State.Hover;
            this.Refresh();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.Enabled)
                state = State.Normal;
            this.Refresh();

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.Enabled)
                state = State.Pushed;
            this.Refresh();

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.Enabled)
            {
                if (this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position))
                    state = State.Hover;
                else
                    state = State.Normal;
            }
            this.Refresh();

            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (this.Enabled)
                state = State.Normal;
            else
                state = State.Disabled;

            this.Refresh();

            base.OnEnabledChanged(e);
        }
        #endregion

        #region Drawing Methods-------------------------

        //Draws the light-blue rectangle around the button when it is focused (by Tab for example)
        private void DrawHighlight(Graphics g)
        {
            //The outline is drawn inside the button
            GraphicsPath innerRegion = RoundedRect(this.Width - 3, this.Height - 3, 3);

            //----Shift the inner region inwards
            Matrix translate = new Matrix();
            translate.Translate(1, 1);
            innerRegion.Transform(translate);
            translate.Dispose();
            //-----

            Pen inlinePen = new Pen(Color.FromArgb(192, 233, 243)); //Light-blue

            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawPath(inlinePen, innerRegion);

            //Clean-up
            inlinePen.Dispose();
            innerRegion.Dispose();
        }

        //Draws the button when the mouse is over it
        private void DrawHoverState(Graphics g)
        {
            GraphicsPath outerRegion = RoundedRect(this.Width - 1, this.Height - 1, 3);
            GraphicsPath innerRegion = RoundedRect(this.Width - 3, this.Height - 3, 2);
            //----Shift the inner region inwards
            Matrix translate = new Matrix();
            translate.Translate(1, 1);
            innerRegion.Transform(translate);
            translate.Dispose();
            //-----
            Rectangle backgroundRect = new Rectangle(1, 1, this.Width - 2, (int)(this.Height * 0.75f) - 2);

            Pen outlinePen = new Pen(Color.FromArgb(189, 189, 189)); //SystemColors.ControlDark
            Pen inlinePen = new Pen(Color.FromArgb(245, 255, 255, 255)); //Slightly transparent white

            //Gradient brush for the background, goes from white to transparent 75% of the way down
            LinearGradientBrush backBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, backgroundRect.Height), Color.White, Color.Transparent);
            backBrush.WrapMode = WrapMode.TileFlipX; //keeps the gradient smooth incase of the glitch where there's an extra gradient line

            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.FillRectangle(backBrush, backgroundRect);
            g.DrawPath(inlinePen, innerRegion);
            g.DrawPath(outlinePen, outerRegion);

            //Text/Image
            offset = 0; //Text/Image doesn't move
            DrawForeground(g);

            //Clean up
            outlinePen.Dispose();
            inlinePen.Dispose();
            outerRegion.Dispose();
            innerRegion.Dispose();
        }

        //Draws the button when it's clicked down
        private void DrawPushedState(Graphics g)
        {
            GraphicsPath outerRegion = RoundedRect(this.Width - 1, this.Height - 1, 3);
            GraphicsPath innerRegion = RoundedRect(this.Width - 3, this.Height - 3, 2);
            //----Shift the inner region inwards
            Matrix translate = new Matrix();
            translate.Translate(1, 1);
            innerRegion.Transform(translate);
            translate.Dispose();
            //-----
            Rectangle backgroundRect = new Rectangle(1, 1, this.Width - 3, this.Height - 3);

            Pen outlinePen = new Pen(Color.FromArgb(167, 167, 167)); //Outline is darker than normal
            Pen inlinePen = new Pen(Color.FromArgb(227, 227, 227)); //Darker white
            SolidBrush backBrush = new SolidBrush(Color.FromArgb(234, 234, 234)); //SystemColors.ControlLight

            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.FillRectangle(backBrush, backgroundRect);
            g.DrawPath(inlinePen, innerRegion);
            g.DrawPath(outlinePen, outerRegion);

            //Text/Image
            offset = 1; //moves image inwards 1 pixel (x and y) to create the illusion that the button was pushed
            DrawForeground(g);

            //Clean up
            outlinePen.Dispose();
            inlinePen.Dispose();
            outerRegion.Dispose();
            innerRegion.Dispose();
        }

        //Draws the button in it's regular state
        private void DrawNormalState(Graphics g)
        {
            //Nothing needs to be drawn but the text and image

            //Text/Image
            offset = 0; //Text/Image doesn't move
            DrawForeground(g);
        }

        //Draws Text and Image
        private void DrawForeground(Graphics g)
        {
            //Make sure drawing is of good quality
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //Image Coordinates-------------------------------
            int imageLeft = 9;
            int imageTop = 0;
            //

            //Text Layout--------------------------------
            //Gets the width/height of the text once it's drawn out
            SizeF headerLayout = g.MeasureString(headerText, this.Font);
            SizeF descriptLayout = g.MeasureString(descriptionText, descriptFont);

            //Merge the two sizes into one big rectangle
            Rectangle totalRect = new Rectangle(0, 0, (int)Math.Max(headerLayout.Width, descriptLayout.Width), (int)(headerLayout.Height + descriptLayout.Height) - 4);

            //Align the total rectangle-------------------------
            if (image != null)
                totalRect.X = imageLeft + imageSize.Width + 5; //consider the image is there
            else
                totalRect.X = 20;

            totalRect.Y = (this.Height / 2) - (totalRect.Height / 2); //center vertically  
            //---------------------------------------------------

            //Align the top of the image---------------------
            if (image != null)
            {
                switch (imageAlign)
                {
                    case VerticalAlign.Top:
                        imageTop = totalRect.Y;
                        break;
                    case VerticalAlign.Middle:
                        imageTop = totalRect.Y + (totalRect.Height / 2) - (imageSize.Height / 2);
                        break;
                    case VerticalAlign.Bottom:
                        imageTop = totalRect.Y + totalRect.Height - imageSize.Height;
                        break;
                    default:
                        break;
                }
            }
            //-----------------------------------------------

            //Brushes--------------------------------
            // Determine text color depending on whether the control is enabled or not
            Color textColor = this.ForeColor;
            if (!this.Enabled)
                textColor = SystemColors.GrayText;

            SolidBrush textBrush = new SolidBrush(textColor);
            //------------------------------------------

            g.DrawString(headerText, this.Font, textBrush, totalRect.Left + offset, totalRect.Top + offset);
            //cxs
            //g.DrawString(DescriptionText, descriptFont, textBrush, totalRect.Left + 1 + offset, totalRect.Bottom - (int)descriptLayout.Height + offset);
            g.DrawString(DescriptionText, descriptFont, textBrush, totalRect.Left + 1 + offset, totalRect.Bottom - (int)descriptLayout.Height + offset + 3);
            //Note: the + 1 in "totalRect.Left + 1 + offset" compensates for GDI+ inconsistency

            if (image != null)
            {
                if (this.Enabled)
                    g.DrawImage(image, new Rectangle(imageLeft + offset, imageTop + offset, imageSize.Width, imageSize.Height));
                else
                {
                    //make sure there is a gray-image
                    if (grayImage == null)
                        grayImage = GetGrayscale(image); //generate grayscale now

                    g.DrawImage(grayImage, new Rectangle(imageLeft + offset, imageTop + offset, imageSize.Width, imageSize.Height));
                }
            }

            //Clean-up
            textBrush.Dispose();
        }
        #endregion

        #region Helper Methods--------------------------
        private static GraphicsPath RoundedRect(int width, int height, int radius)
        {
            RectangleF baseRect = new RectangleF(0, 0, width, height);
            float diameter = radius * 2.0f;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new GraphicsPath();

            // top left arc 
            path.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private static Bitmap GetGrayscale(Image original)
        {
            //Set up the drawing surface
            Bitmap grayscale = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(grayscale);

            //Grayscale Color Matrix
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                      {
                                                         new float[] {0.3f, 0.3f, 0.3f, 0, 0},
                                                         new float[] {0.59f, 0.59f, 0.59f, 0, 0},
                                                         new float[] {0.11f, 0.11f, 0.11f, 0, 0},
                                                         new float[] {0, 0, 0, 1, 0},
                                                         new float[] {0, 0, 0, 0, 1}
                                                      });

            //Create attributes
            ImageAttributes att = new ImageAttributes();
            att.SetColorMatrix(colorMatrix);

            //Draw the image with the new attributes
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, att);

            //Clean up
            g.Dispose();

            return grayscale;
        }

        public void PerformClick()
        {
            this.OnClick(null);
        }
        #endregion
    }
}
