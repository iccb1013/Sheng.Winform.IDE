using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using PushButtonState = System.Windows.Forms.VisualStyles.PushButtonState;

namespace Sheng.SailingEase.Controls
{
    /// <summary>
    /// Represents a glass button control.
    /// </summary>
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public partial class SEGlassButton : Button
    {
        #region " Constructors "

        /// <summary>
        /// Initializes a new instance of the <see cref="Glass.GlassButton" /> class.
        /// </summary>
        public SEGlassButton()
        {
            LicenseManager.Validate(typeof(SEGlassButton)); 

            InitializeComponent();
            timer.Interval = animationLength / framesCount;
            base.BackColor = Color.Transparent;
            BackColor = Color.Black;
            ForeColor = Color.White;
            OuterBorderColor = Color.White;
            InnerBorderColor = Color.Black;
            ShineColor = Color.White;
            GlowColor = Color.FromArgb(-7488001);//unchecked((int)(0xFF8DBDFF)));
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);
        }

        #endregion

        #region " Fields and Properties "

        private Color _backColor;
        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color" /> value representing the background color.</returns>
        [DefaultValue(typeof(Color), "Black")]
        public virtual new Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (!_backColor.Equals(value))
                {
                    _backColor = value;
                    UseVisualStyleBackColor = false;
                    CreateFrames();
                    OnBackColorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        /// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the control.</returns>
        [DefaultValue(typeof(Color), "White")]
        public virtual new Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
            }
        }

        private Color _innerBorderColor;
        /// <summary>
        /// Gets or sets the inner border color of the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color" /> value representing the color of the inner border.</returns>
        [DefaultValue(typeof(Color), "Black"), Category("Appearance"), Description("The inner border color of the control.")]
        public virtual Color InnerBorderColor
        {
            get { return _innerBorderColor; }
            set
            {
                if (_innerBorderColor != value)
                {
                    _innerBorderColor = value;
                    CreateFrames();
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                    OnInnerBorderColorChanged(EventArgs.Empty);
                }
            }
        }

        private Color _outerBorderColor;
        /// <summary>
        /// Gets or sets the outer border color of the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color" /> value representing the color of the outer border.</returns>
        [DefaultValue(typeof(Color), "White"), Category("Appearance"), Description("The outer border color of the control.")]
        public virtual Color OuterBorderColor
        {
            get { return _outerBorderColor; }
            set
            {
                if (_outerBorderColor != value)
                {
                    _outerBorderColor = value;
                    CreateFrames();
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                    OnOuterBorderColorChanged(EventArgs.Empty);
                }
            }
        }

        private Color _shineColor;
        /// <summary>
        /// Gets or sets the shine color of the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color" /> value representing the shine color.</returns>
        [DefaultValue(typeof(Color), "White"), Category("Appearance"), Description("The shine color of the control.")]
        public virtual Color ShineColor
        {
            get { return _shineColor; }
            set
            {
                if (_shineColor != value)
                {
                    _shineColor = value;
                    CreateFrames();
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                    OnShineColorChanged(EventArgs.Empty);
                }
            }
        }

        private Color _glowColor;
        /// <summary>
        /// Gets or sets the glow color of the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color" /> value representing the glow color.</returns>
        [DefaultValue(typeof(Color), "255,141,189,255"), Category("Appearance"), Description("The glow color of the control.")]
        public virtual Color GlowColor
        {
            get { return _glowColor; }
            set
            {
                if (_glowColor != value)
                {
                    _glowColor = value;
                    CreateFrames();
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                    OnGlowColorChanged(EventArgs.Empty);
                }
            }
        }

        private bool _fadeOnFocus;
        /// <summary>
        /// Gets or sets a value indicating whether the button should fade in and fade out when it's getting and loosing the focus.
        /// </summary>
        /// <value><c>true</c> if fading with changing the focus; otherwise, <c>false</c>.</value>
        [DefaultValue(false), Category("Appearance"), Description("Indicates whether the button should fade in and fade out when it is getting and loosing the focus.")]
        public virtual bool FadeOnFocus
        {
            get { return _fadeOnFocus; }
            set
            {
                if (_fadeOnFocus != value)
                {
                    _fadeOnFocus = value;
                }
            }
        }

        private bool _isHovered;
        private bool _isFocused;
        private bool _isFocusedByKey;
        private bool _isKeyDown;
        private bool _isMouseDown;
        private bool IsPressed { get { return _isKeyDown || (_isMouseDown && _isHovered); } }

        /// <summary>
        /// Gets the state of the button control.
        /// </summary>
        /// <value>The state of the button control.</value>
        [Browsable(false)]
        public PushButtonState State
        {
            get
            {
                if (!Enabled)
                {
                    return PushButtonState.Disabled;
                }
                if (IsPressed)
                {
                    return PushButtonState.Pressed;
                }
                if (_isHovered)
                {
                    return PushButtonState.Hot;
                }
                if (_isFocused || IsDefault)
                {
                    return PushButtonState.Default;
                }
                return PushButtonState.Normal;
            }
        }

        #endregion

        #region " Events "

        /// <summary>Occurs when the value of the <see cref="P:Glass.GlassButton.InnerBorderColor" /> property changes.</summary>
        [Description("Event raised when the value of the InnerBorderColor property is changed."), Category("Property Changed")]
        public event EventHandler InnerBorderColorChanged;

        /// <summary>
        /// Raises the <see cref="E:Glass.GlassButton.InnerBorderColorChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected virtual void OnInnerBorderColorChanged(EventArgs e)
        {
            if (InnerBorderColorChanged != null)
            {
                InnerBorderColorChanged(this, e);
            }
        }

        /// <summary>Occurs when the value of the <see cref="P:Glass.GlassButton.OuterBorderColor" /> property changes.</summary>
        [Description("Event raised when the value of the OuterBorderColor property is changed."), Category("Property Changed")]
        public event EventHandler OuterBorderColorChanged;

        /// <summary>
        /// Raises the <see cref="E:Glass.GlassButton.OuterBorderColorChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected virtual void OnOuterBorderColorChanged(EventArgs e)
        {
            if (OuterBorderColorChanged != null)
            {
                OuterBorderColorChanged(this, e);
            }
        }

        /// <summary>Occurs when the value of the <see cref="P:Glass.GlassButton.ShineColor" /> property changes.</summary>
        [Description("Event raised when the value of the ShineColor property is changed."), Category("Property Changed")]
        public event EventHandler ShineColorChanged;

        /// <summary>
        /// Raises the <see cref="E:Glass.GlassButton.ShineColorChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected virtual void OnShineColorChanged(EventArgs e)
        {
            if (ShineColorChanged != null)
            {
                ShineColorChanged(this, e);
            }
        }

        /// <summary>Occurs when the value of the <see cref="P:Glass.GlassButton.GlowColor" /> property changes.</summary>
        [Description("Event raised when the value of the GlowColor property is changed."), Category("Property Changed")]
        public event EventHandler GlowColorChanged;

        /// <summary>
        /// Raises the <see cref="E:Glass.GlassButton.GlowColorChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected virtual void OnGlowColorChanged(EventArgs e)
        {
            if (GlowColorChanged != null)
            {
                GlowColorChanged(this, e);
            }
        }

        #endregion

        #region " Overrided Methods "

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            CreateFrames();
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected override void OnClick(EventArgs e)
        {
            _isKeyDown = _isMouseDown = false;
            base.OnClick(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnEnter(EventArgs e)
        {
            _isFocused = _isFocusedByKey = true;
            base.OnEnter(e);
            if (_fadeOnFocus)
            {
                FadeIn();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            _isFocused = _isFocusedByKey = _isKeyDown = _isMouseDown = false;
            Invalidate();
            if (_fadeOnFocus)
            {
                FadeOut();
            }
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnKeyUp(System.Windows.Forms.KeyEventArgs)" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _isKeyDown = true;
                Invalidate();
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnKeyUp(System.Windows.Forms.KeyEventArgs)" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (_isKeyDown && e.KeyCode == Keys.Space)
            {
                _isKeyDown = false;
                Invalidate();
            }
            base.OnKeyUp(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!_isMouseDown && e.Button == MouseButtons.Left)
            {
                _isMouseDown = true;
                _isFocusedByKey = false;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                _isMouseDown = false;
                Invalidate();
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseMove(System.Windows.Forms.MouseEventArgs)" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.None)
            {
                if (!ClientRectangle.Contains(e.X, e.Y))
                {
                    if (_isHovered)
                    {
                        _isHovered = false;
                        Invalidate();
                    }
                }
                else if (!_isHovered)
                {
                    _isHovered = true;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            FadeIn();
            Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            if (!(FadeOnFocus && _isFocusedByKey)) FadeOut();
            Invalidate();
            base.OnMouseLeave(e);
        }

        #endregion

        #region " Painting "

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawButtonBackgroundFromBuffer(e.Graphics);
            DrawForegroundFromButton(e);
            DrawButtonForeground(e.Graphics);

            if (Paint != null)
            {
                Paint(this, e);
            }
        }

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        public new event PaintEventHandler Paint;

        private void DrawButtonBackgroundFromBuffer(Graphics graphics)
        {
            int frame;
            if (!Enabled)
            {
                frame = FRAME_DISABLED;
            }
            else if (IsPressed)
            {
                frame = FRAME_PRESSED;
            }
            else if (!IsAnimating && _currentFrame == 0)
            {
                frame = FRAME_NORMAL;
            }
            else
            {
                if (!HasAnimationFrames)
                {
                    CreateFrames(true);
                }
                frame = FRAME_ANIMATED + _currentFrame;
            }
            if (_frames == null || _frames.Count == 0)
            {
                CreateFrames();
            }
            graphics.DrawImage(_frames[frame], Point.Empty);
        }

        private Image CreateBackgroundFrame(bool pressed, bool hovered,
            bool animating, bool enabled, float glowOpacity)
        {
            Rectangle rect = ClientRectangle;
            if (rect.Width <= 0)
            {
                rect.Width = 1;
            }
            if (rect.Height <= 0)
            {
                rect.Height = 1;
            }
            Image img = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.Clear(Color.Transparent);
                DrawButtonBackground(g, rect, pressed, hovered, animating, enabled,
                    _outerBorderColor, _backColor, _glowColor, _shineColor, _innerBorderColor,
                    glowOpacity);
            }
            return img;
        }

        private static void DrawButtonBackground(Graphics g, Rectangle rectangle,
            bool pressed, bool hovered, bool animating, bool enabled,
            Color outerBorderColor, Color backColor, Color glowColor, Color shineColor,
            Color innerBorderColor, float glowOpacity)
        {
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            #region " white border "
            Rectangle rect = rectangle;
            rect.Width--;
            rect.Height--;
            using (GraphicsPath bw = CreateRoundRectangle(rect, 4))
            {
                using (Pen p = new Pen(outerBorderColor))
                {
                    g.DrawPath(p, bw);
                }
            }
            #endregion

            rect.X++;
            rect.Y++;
            rect.Width -= 2;
            rect.Height -= 2;
            Rectangle rect2 = rect;
            rect2.Height >>= 1;

            #region " content "
            using (GraphicsPath bb = CreateRoundRectangle(rect, 2))
            {
                int opacity = pressed ? 0xcc : 0x7f;
                using (Brush br = new SolidBrush(Color.FromArgb(opacity, backColor)))
                {
                    g.FillPath(br, bb);
                }
            }
            #endregion

            #region " glow "
            if ((hovered || animating) && !pressed)
            {
                using (GraphicsPath clip = CreateRoundRectangle(rect, 2))
                {
                    g.SetClip(clip, CombineMode.Intersect);
                    using (GraphicsPath brad = CreateBottomRadialPath(rect))
                    {
                        using (PathGradientBrush pgr = new PathGradientBrush(brad))
                        {
                            int opacity = (int)(0xB2 * glowOpacity + .5f);
                            RectangleF bounds = brad.GetBounds();
                            pgr.CenterPoint = new PointF((bounds.Left + bounds.Right) / 2f, (bounds.Top + bounds.Bottom) / 2f);
                            pgr.CenterColor = Color.FromArgb(opacity, glowColor);
                            pgr.SurroundColors = new Color[] { Color.FromArgb(0, glowColor) };
                            g.FillPath(pgr, brad);
                        }
                    }
                    g.ResetClip();
                }
            }
            #endregion

            #region " shine "
            if (rect2.Width > 0 && rect2.Height > 0)
            {
                rect2.Height++;
                using (GraphicsPath bh = CreateTopRoundRectangle(rect2, 2))
                {
                    rect2.Height++;
                    int opacity = 0x99;
                    if (pressed | !enabled)
                    {
                        opacity = (int)(.4f * opacity + .5f);
                    }
                    using (LinearGradientBrush br = new LinearGradientBrush(rect2, Color.FromArgb(opacity, shineColor), Color.FromArgb(opacity / 3, shineColor), LinearGradientMode.Vertical))
                    {
                        g.FillPath(br, bh);
                    }
                }
                rect2.Height -= 2;
            }
            #endregion

            #region " black border "
            using (GraphicsPath bb = CreateRoundRectangle(rect, 3))
            {
                using (Pen p = new Pen(innerBorderColor))
                {
                    g.DrawPath(p, bb);
                }
            }
            #endregion

            g.SmoothingMode = sm;
        }

        private void DrawButtonForeground(Graphics g)
        {
            if (Focused && ShowFocusCues/* && isFocusedByKey*/)
            {
                Rectangle rect = ClientRectangle;
                rect.Inflate(-4, -4);
                ControlPaint.DrawFocusRectangle(g, rect);
            }
        }

        private Button _imageButton;
        private void DrawForegroundFromButton(PaintEventArgs pevent)
        {
            if (_imageButton == null)
            {
                _imageButton = new Button();
                _imageButton.Parent = new TransparentControl();
                _imageButton.SuspendLayout();
                _imageButton.BackColor = Color.Transparent;
                _imageButton.FlatAppearance.BorderSize = 0;
                _imageButton.FlatStyle = FlatStyle.Flat;
            }
            else
            {
                _imageButton.SuspendLayout();
            }
            _imageButton.AutoEllipsis = AutoEllipsis;
            if (Enabled)
            {
                _imageButton.ForeColor = ForeColor;
            }
            else
            {
                _imageButton.ForeColor = Color.FromArgb((3 * ForeColor.R + _backColor.R) >> 2,
                    (3 * ForeColor.G + _backColor.G) >> 2,
                    (3 * ForeColor.B + _backColor.B) >> 2);
            }
            _imageButton.Font = Font;
            _imageButton.RightToLeft = RightToLeft;
            _imageButton.Image = Image;
            if (Image != null && !Enabled)
            {
                Size size = Image.Size;
                float[][] newColorMatrix = new float[5][];
                newColorMatrix[0] = new float[] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f };
                newColorMatrix[1] = new float[] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f };
                newColorMatrix[2] = new float[] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f };
                float[] arr = new float[5];
                arr[3] = 1f;
                newColorMatrix[3] = arr;
                newColorMatrix[4] = new float[] { 0.38f, 0.38f, 0.38f, 0f, 1f };
                System.Drawing.Imaging.ColorMatrix matrix = new System.Drawing.Imaging.ColorMatrix(newColorMatrix);
                System.Drawing.Imaging.ImageAttributes disabledImageAttr = new System.Drawing.Imaging.ImageAttributes();
                disabledImageAttr.ClearColorKey();
                disabledImageAttr.SetColorMatrix(matrix);
                _imageButton.Image = new Bitmap(Image.Width, Image.Height);
                using (Graphics gr = Graphics.FromImage(_imageButton.Image))
                {
                    gr.DrawImage(Image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, disabledImageAttr);
                }
            }
            _imageButton.ImageAlign = ImageAlign;
            _imageButton.ImageIndex = ImageIndex;
            _imageButton.ImageKey = ImageKey;
            _imageButton.ImageList = ImageList;
            _imageButton.Padding = Padding;
            _imageButton.Size = Size;
            _imageButton.Text = Text;
            _imageButton.TextAlign = TextAlign;
            _imageButton.TextImageRelation = TextImageRelation;
            _imageButton.UseCompatibleTextRendering = UseCompatibleTextRendering;
            _imageButton.UseMnemonic = UseMnemonic;
            _imageButton.ResumeLayout();
            InvokePaint(_imageButton, pevent);
            if (_imageButton.Image != null && _imageButton.Image != Image)
            {
                _imageButton.Image.Dispose();
                _imageButton.Image = null;
            }
        }

        private class TransparentControl : Control
        {
            protected override void OnPaintBackground(PaintEventArgs pevent) { }
            protected override void OnPaint(PaintEventArgs e) { }
        }

        private static GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;
            path.AddArc(l, t, d, d, 180, 90); // topleft
            path.AddLine(l + radius, t, l + w - radius, t); // top
            path.AddArc(l + w - d, t, d, d, 270, 90); // topright
            path.AddLine(l + w, t + radius, l + w, t + h - radius); // right
            path.AddArc(l + w - d, t + h - d, d, d, 0, 90); // bottomright
            path.AddLine(l + w - radius, t + h, l + radius, t + h); // bottom
            path.AddArc(l, t + h - d, d, d, 90, 90); // bottomleft
            path.AddLine(l, t + h - radius, l, t + radius); // left
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateTopRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;
            path.AddArc(l, t, d, d, 180, 90); // topleft
            path.AddLine(l + radius, t, l + w - radius, t); // top
            path.AddArc(l + w - d, t, d, d, 270, 90); // topright
            path.AddLine(l + w, t + radius, l + w, t + h); // right
            path.AddLine(l + w, t + h, l, t + h); // bottom
            path.AddLine(l, t + h, l, t + radius); // left
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateBottomRadialPath(Rectangle rectangle)
        {
            GraphicsPath path = new GraphicsPath();
            RectangleF rect = rectangle;
            rect.X -= rect.Width * .35f;
            rect.Y -= rect.Height * .15f;
            rect.Width *= 1.7f;
            rect.Height *= 2.3f;
            path.AddEllipse(rect);
            path.CloseFigure();
            return path;
        }

        #endregion

        #region " Unused Properties & Events "

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new FlatButtonAppearance FlatAppearance
        {
            get { return base.FlatAppearance; }
        }

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool UseVisualStyleBackColor
        {
            get { return base.UseVisualStyleBackColor; }
            set { base.UseVisualStyleBackColor = value; }
        }

        #endregion

        #region " Animation Support "

        private List<Image> _frames;

        private const int FRAME_DISABLED = 0;
        private const int FRAME_PRESSED = 1;
        private const int FRAME_NORMAL = 2;
        private const int FRAME_ANIMATED = 3;

        private bool HasAnimationFrames
        {
            get
            {
                return _frames != null && _frames.Count > FRAME_ANIMATED;
            }
        }

        private void CreateFrames()
        {
            CreateFrames(false);
        }

        private void CreateFrames(bool withAnimationFrames)
        {
            DestroyFrames();
            if (!IsHandleCreated)
            {
                return;
            }
            if (_frames == null)
            {
                _frames = new List<Image>();
            }
            _frames.Add(CreateBackgroundFrame(false, false, false, false, 0));
            _frames.Add(CreateBackgroundFrame(true, true, false, true, 0));
            _frames.Add(CreateBackgroundFrame(false, false, false, true, 0));
            if (!withAnimationFrames)
            {
                return;
            }
            for (int i = 0; i < framesCount; i++)
            {
                _frames.Add(CreateBackgroundFrame(false, true, true, true, (float)i / (framesCount - 1F)));
            }
        }

        private void DestroyFrames()
        {
            if (_frames != null)
            {
                while (_frames.Count > 0)
                {
                    _frames[_frames.Count - 1].Dispose();
                    _frames.RemoveAt(_frames.Count - 1);
                }
            }
        }

        private const int animationLength = 300;
        private const int framesCount = 10;
        private int _currentFrame;
        private int _direction;

        private bool IsAnimating
        {
            get
            {
                return _direction != 0;
            }
        }

        private void FadeIn()
        {
            _direction = 1;
            timer.Enabled = true;
        }

        private void FadeOut()
        {
            _direction = -1;
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!timer.Enabled)
            {
                return;
            }
            Refresh();
            _currentFrame += _direction;
            if (_currentFrame == -1)
            {
                _currentFrame = 0;
                timer.Enabled = false;
                _direction = 0;
                return;
            }
            if (_currentFrame == framesCount)
            {
                _currentFrame = framesCount - 1;
                timer.Enabled = false;
                _direction = 0;
            }
        }

        #endregion
    }
}
