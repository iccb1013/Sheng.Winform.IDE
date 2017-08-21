using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;

namespace Sheng.SailingEase.Controls
{
    partial class SEMessageBoxBase : Form
    {
        #region Fields

        protected ArrayList _buttons = new ArrayList();
        protected bool _allowSaveResponse;
        protected bool _playAlert = true;
        protected SEMessageBoxButton _cancelButton = null;
        protected SEMessageBoxButton _defaultButtonControl = null;

        protected int _maxLayoutWidth;
        protected int _maxLayoutHeight;

        protected int _maxWidth;
        protected int _maxHeight;

        protected bool _allowCancel = true;
        //protected string _result = null;
        protected SEMessageBoxButton _result = null;

        /// <summary>
        /// Used to determine the alert sound to play
        /// </summary>
        protected MessageBoxIcon _standardIcon = MessageBoxIcon.None;
        protected Icon _iconImage = null;

        protected Timer timerTimeout = null;
        protected int _timeout = 0;
        protected SEMessageBoxTimeoutResult _timeoutResult = SEMessageBoxTimeoutResult.Default;

        /// <summary>
        /// Maps MessageBoxEx buttons to Button controls
        /// </summary>
        protected Hashtable _buttonControlsTable = new Hashtable();
        #endregion

        #region Properties

        public string Caption
        {
            set { this.Text = value; }
        }

        public Font CustomFont
        {
            set { this.Font = value; }
        }

        public ArrayList Buttons
        {
            get { return _buttons; }
        }

        public bool AllowSaveResponse
        {
            get { return _allowSaveResponse; }
            set { _allowSaveResponse = value; }
        }


        public MessageBoxIcon StandardIcon
        {
            set { SetStandardIcon(value); }
        }

        public Icon CustomIcon
        {
            set
            {
                _standardIcon = MessageBoxIcon.None;
                _iconImage = value;
            }
        }

        public SEMessageBoxButton CustomCancelButton
        {
            set { _cancelButton = value; }
        }

        public SEMessageBoxButton Result
        {
            get { return _result; }
        }

        public bool PlayAlertSound
        {
            get { return _playAlert; }
            set { _playAlert = value; }
        }

        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public SEMessageBoxTimeoutResult TimeoutResult
        {
            get { return _timeoutResult; }
            set { _timeoutResult = value; }
        }

        #endregion

        #region 构造

        public SEMessageBoxBase()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((int)keyData == (int)(Keys.Alt | Keys.F4) && !_allowCancel)
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_result == null)
            {
                if (_allowCancel)
                {
                    _result = _cancelButton;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (timerTimeout != null)
            {
                timerTimeout.Stop();
            }

            base.OnClosing(e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Measures a string using the Graphics object for this form with
        /// the specified font
        /// </summary>
        /// <param name="str">The string to measure</param>
        /// <param name="maxWidth">The maximum width available to display the string</param>
        /// <param name="font">The font with which to measure the string</param>
        /// <returns></returns>
        protected Size MeasureString(string str, int maxWidth, Font font)
        {
            Graphics g = this.CreateGraphics();
            SizeF strRectSizeF = g.MeasureString(str, font, maxWidth);
            g.Dispose();

            return new Size((int)Math.Ceiling(strRectSizeF.Width), (int)Math.Ceiling(strRectSizeF.Height));
        }

        /// <summary>
        /// Measures a string using the Graphics object for this form and the
        /// font of this form
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        protected Size MeasureString(string str, int maxWidth)
        {
            return MeasureString(str, maxWidth, this.Font);
        }

        /// <summary>
        /// Gets the longest button text
        /// </summary>
        /// <returns></returns>
        protected string GetLongestButtonText()
        {
            int maxLen = 0;
            string maxStr = null;
            foreach (SEMessageBoxButton button in _buttons)
            {
                if (button.Text != null && button.Text.Length > maxLen)
                {
                    maxLen = button.Text.Length;
                    maxStr = button.Text;
                }
            }

            return maxStr;
        }

        /// <summary>
        /// Set the icon
        /// </summary>
        /// <param name="icon"></param>
        protected void SetStandardIcon(MessageBoxIcon icon)
        {
            _standardIcon = icon;

            switch (icon)
            {
                case MessageBoxIcon.Asterisk:
                    _iconImage = SystemIcons.Asterisk;
                    break;
                case MessageBoxIcon.Error:
                    _iconImage = SystemIcons.Error;
                    break;
                case MessageBoxIcon.Exclamation:
                    _iconImage = SystemIcons.Exclamation;
                    break;
                //				case MessageBoxIcon.Hand:
                //					_iconImage = SystemIcons.Hand;
                //					break;
                //				case MessageBoxIcon.Information:
                //					_iconImage = SystemIcons.Information;
                //					break;
                case MessageBoxIcon.Question:
                    _iconImage = SystemIcons.Question;
                    break;
                //				case MessageBoxIcon.Stop:
                //					_iconImage = SystemIcons.Stop;
                //					break;
                //				case MessageBoxIcon.Warning:
                //					_iconImage = SystemIcons.Warning;
                //					break;

                case MessageBoxIcon.None:
                    _iconImage = null;
                    break;
            }
        }

        protected void AddOkButtonIfNoButtonsPresent()
        {
            if (_buttons.Count == 0)
            {
                SEMessageBoxButton okButton = new SEMessageBoxButton();
                okButton.Text = SEMessageBoxButtons.Ok.ToString();
                //okButton.Value = SEMessageBoxButtons.Ok.ToString();

                _buttons.Add(okButton);
            }
        }

        /// <summary>
        /// Centers the form on the screen
        /// </summary>
        protected void CenterForm()
        {
            int x = (SystemInformation.WorkingArea.Width - this.Width) / 2;
            int y = (SystemInformation.WorkingArea.Height - this.Height) / 2;

            this.Location = new Point(x, y);
        }

        /// <summary>
        /// Gets the width of the caption
        /// </summary>
        protected Size GetCaptionSize()
        {
            Font captionFont = GetCaptionFont();
            if (captionFont == null)
            {
                //some error occured while determining system font
                captionFont = new Font("Tahoma", 11);
            }

            int availableWidth = _maxWidth - SystemInformation.CaptionButtonSize.Width - SystemInformation.Border3DSize.Width * 2;
            Size captionSize = MeasureString(this.Text, availableWidth, captionFont);

            captionSize.Width += SystemInformation.CaptionButtonSize.Width + SystemInformation.Border3DSize.Width * 2;
            return captionSize;
        }

        protected void DisableCloseIfMultipleButtonsAndNoCancelButton()
        {
            if (_buttons.Count > 1)
            {
                if (_cancelButton != null)
                    return;

                //See if standard cancel button is present
                foreach (SEMessageBoxButton button in _buttons)
                {
                    //if (button.Text == SEMessageBoxButtons.Cancel.ToString() && button.Value == SEMessageBoxButtons.Cancel.ToString())
                    if (button.Text == SEMessageBoxButtons.Cancel.ToString())
                    {
                        _cancelButton = button;
                        return;
                    }
                }

                //Standard cancel button is not present, Disable
                //close button
                DisableCloseButton(this);
                _allowCancel = false;

            }
            else if (_buttons.Count == 1)
            {
                _cancelButton = _buttons[0] as SEMessageBoxButton;
            }
            else
            {
                //This condition should never get called
                _allowCancel = false;
            }
        }

        /// <summary>
        /// Plays the alert sound based on the icon set for the message box
        /// </summary>
        protected void PlayAlert()
        {
            if (_playAlert)
            {
                if (_standardIcon != MessageBoxIcon.None)
                {
                    MessageBeep((uint)_standardIcon);
                }
                else
                {
                    MessageBeep(0 /*MB_OK*/);
                }
            }
        }

        protected void SelectDefaultButton()
        {
            if (_defaultButtonControl != null && _defaultButtonControl.Control != null)
            {
                _defaultButtonControl.Control.Select();
            }
        }

        protected void StartTimerIfTimeoutGreaterThanZero()
        {
            if (_timeout > 0)
            {
                if (timerTimeout == null)
                {
                    timerTimeout = new Timer(this.components);
                    timerTimeout.Tick += new EventHandler(timerTimeout_Tick);
                }

                if (!timerTimeout.Enabled)
                {
                    timerTimeout.Interval = _timeout;
                    timerTimeout.Start();
                }
            }
        }

        protected void SetResultAndClose(SEMessageBoxButton result)
        {
            _result = result;
            this.DialogResult = DialogResult.OK;
        }

        #endregion

        #region Event Handlers

        private void timerTimeout_Tick(object sender, EventArgs e)
        {
            timerTimeout.Stop();

            switch (_timeoutResult)
            {
                case SEMessageBoxTimeoutResult.Default:
                    //_defaultButtonControl.PerformClick();
                    _defaultButtonControl.Control.Select();
                    SendKeys.Send("{ENTER}");
                    break;

                case SEMessageBoxTimeoutResult.Cancel:
                    if (_cancelButton != null)
                    {
                        SetResultAndClose(_cancelButton);
                    }
                    else
                    {
                        //_defaultButtonControl.PerformClick();
                        _defaultButtonControl.Control.Select();
                        SendKeys.Send("{ENTER}");
                    }
                    break;

                case SEMessageBoxTimeoutResult.Timeout:
                    //SetResultAndClose(SEMessageBoxResult.Timeout);
                    SetResultAndClose(null);
                    break;
            }
        }

        #endregion

        #region P/Invoke - SystemParametersInfo, GetSystemMenu, EnableMenuItem, MessageBeep
        protected Font GetCaptionFont()
        {

            NONCLIENTMETRICS ncm = new NONCLIENTMETRICS();
            ncm.cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS));
            try
            {
                bool result = SystemParametersInfo(SPI_GETNONCLIENTMETRICS, ncm.cbSize, ref ncm, 0);

                if (result)
                {
                    return Font.FromLogFont(ncm.lfCaptionFont);

                }
                else
                {
                    int lastError = Marshal.GetLastWin32Error();
                    return null;
                }
            }
            catch (Exception /*ex*/)
            {
                //System.Console.WriteLine(ex.Message);
            }

            return null;
        }

        private const int SPI_GETNONCLIENTMETRICS = 41;
        private const int LF_FACESIZE = 32;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct LOGFONT
        {
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string lfFaceSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct NONCLIENTMETRICS
        {
            public int cbSize;
            public int iBorderWidth;
            public int iScrollWidth;
            public int iScrollHeight;
            public int iCaptionWidth;
            public int iCaptionHeight;
            public LOGFONT lfCaptionFont;
            public int iSmCaptionWidth;
            public int iSmCaptionHeight;
            public LOGFONT lfSmCaptionFont;
            public int iMenuWidth;
            public int iMenuHeight;
            public LOGFONT lfMenuFont;
            public LOGFONT lfStatusFont;
            public LOGFONT lfMessageFont;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SystemParametersInfo(int uiAction, int uiParam,
            ref NONCLIENTMETRICS ncMetrics, int fWinIni);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem,
        uint uEnable);

        private const int SC_CLOSE = 0xF060;
        private const int MF_BYCOMMAND = 0x0;
        private const int MF_GRAYED = 0x1;
        private const int MF_ENABLED = 0x0;

        protected void DisableCloseButton(Form form)
        {
            try
            {
                EnableMenuItem(GetSystemMenu(form.Handle, false), SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            }
            catch (Exception /*ex*/)
            {
                //System.Console.WriteLine(ex.Message);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        protected static extern bool MessageBeep(uint type);
        #endregion
    }
}
