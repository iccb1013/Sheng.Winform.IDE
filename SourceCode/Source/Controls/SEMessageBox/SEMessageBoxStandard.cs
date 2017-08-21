using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;

namespace Sheng.SailingEase.Controls
{
    partial class SEMessageBoxStandard : SEMessageBoxBase, ISEMessageBoxForm
    {
        #region Constants
        private const int LEFT_PADDING = 12;
        private const int RIGHT_PADDING = 12;
        private const int TOP_PADDING = 12;
        private const int BOTTOM_PADDING = 12;

        private const int BUTTON_LEFT_PADDING = 4;
        private const int BUTTON_RIGHT_PADDING = 4;
        private const int BUTTON_TOP_PADDING = 4;
        private const int BUTTON_BOTTOM_PADDING = 4;

        private const int MIN_BUTTON_HEIGHT = 23;
        private const int MIN_BUTTON_WIDTH = 74;

        private const int ITEM_PADDING = 10;
        private const int ICON_MESSAGE_PADDING = 15;

        private const int BUTTON_PADDING = 5;

        private const int CHECKBOX_WIDTH = 20;

        private const int IMAGE_INDEX_EXCLAMATION = 0;
        private const int IMAGE_INDEX_QUESTION = 1;
        private const int IMAGE_INDEX_STOP = 2;
        private const int IMAGE_INDEX_INFORMATION = 3;
        #endregion

        #region Properties
        public string Message
        {
            get { return rtbMessage.Text; }
            set { rtbMessage.Text = value; }
        }

        public bool SaveResponse
        {
            get { return chbSaveResponse.Checked; }
        }

        public string SaveResponseText
        {
            get { return chbSaveResponse.Text; }
            set { chbSaveResponse.Text = value; }
        }

        #endregion

        #region Ctor/Dtor
        public SEMessageBoxStandard()
        {
            InitializeComponent();

            _maxWidth = (int)(SystemInformation.WorkingArea.Width * 0.60);
            _maxHeight = (int)(SystemInformation.WorkingArea.Height * 0.90);
        }
        #endregion

        #region Overrides
        /// <summary>
        /// This will get called everytime we call ShowDialog on the form
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //Reset result
            _result = null;

            this.Size = new Size(_maxWidth, _maxHeight);

            //This is the rectangle in which all items will be layed out
            _maxLayoutWidth = this.ClientSize.Width - LEFT_PADDING - RIGHT_PADDING;
            _maxLayoutHeight = this.ClientSize.Height - TOP_PADDING - BOTTOM_PADDING;

            AddOkButtonIfNoButtonsPresent();
            DisableCloseIfMultipleButtonsAndNoCancelButton();

            SetIconSizeAndVisibility();
            SetMessageSizeAndVisibility();
            SetCheckboxSizeAndVisibility();

            //设置对话框大小
            SetOptimumSize();
            
            //布局控件
            LayoutControls();

            //使窗体居中
            CenterForm();

            //播放对话框弹出音
            PlayAlert();

            //选中默认按钮
            SelectDefaultButton();

            //开始自动关闭计时
            StartTimerIfTimeoutGreaterThanZero();

            base.OnLoad(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_iconImage != null)
            {
                e.Graphics.DrawIcon(_iconImage, new Rectangle(panelIcon.Location, new Size(32, 32)));
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Sets the size and visibility of the Message
        /// </summary>
        private void SetMessageSizeAndVisibility()
        {
            if (rtbMessage.Text == null || rtbMessage.Text.Trim().Length == 0)
            {
                rtbMessage.Size = Size.Empty;
                rtbMessage.Visible = false;
            }
            else
            {
                int maxWidth = _maxLayoutWidth;
                if (panelIcon.Size.Width != 0)
                {
                    maxWidth = maxWidth - (panelIcon.Size.Width + ICON_MESSAGE_PADDING);
                }

                //We need to account for scroll bar width and height, otherwise for certains
                //kinds of text the scroll bar shows up unnecessarily
                maxWidth = maxWidth - SystemInformation.VerticalScrollBarWidth;
                Size messageRectSize = MeasureString(rtbMessage.Text, maxWidth);

                messageRectSize.Width += SystemInformation.VerticalScrollBarWidth;
                messageRectSize.Height = Math.Max(panelIcon.Height, messageRectSize.Height) + SystemInformation.HorizontalScrollBarHeight;

                rtbMessage.Size = messageRectSize;
                rtbMessage.Visible = true;
            }
        }

        /// <summary>
        /// Sets the size and visibility of the Icon
        /// </summary>
        private void SetIconSizeAndVisibility()
        {
            if (_iconImage == null)
            {
                panelIcon.Visible = false;
                panelIcon.Size = Size.Empty;
            }
            else
            {
                panelIcon.Size = new Size(32, 32);
                panelIcon.Visible = true;
            }
        }

        /// <summary>
        /// Sets the size and visibility of the save response checkbox
        /// </summary>
        private void SetCheckboxSizeAndVisibility()
        {
            if (!AllowSaveResponse)
            {
                chbSaveResponse.Visible = false;
                chbSaveResponse.Size = Size.Empty;
            }
            else
            {
                Size saveResponseTextSize = MeasureString(chbSaveResponse.Text, _maxLayoutWidth);
                saveResponseTextSize.Width += CHECKBOX_WIDTH;
                chbSaveResponse.Size = saveResponseTextSize;
                chbSaveResponse.Visible = true;
            }
        }

        /// <summary>
        /// Calculates the button size based on the text of the longest
        /// button text
        /// </summary>
        /// <returns></returns>
        private Size GetButtonSize()
        {
            string longestButtonText = GetLongestButtonText();
            if (longestButtonText == null)
            {
                //TODO:Handle this case
            }

            Size buttonTextSize = MeasureString(longestButtonText, _maxLayoutWidth);
            Size buttonSize = new Size(buttonTextSize.Width + BUTTON_LEFT_PADDING + BUTTON_RIGHT_PADDING,
                buttonTextSize.Height + BUTTON_TOP_PADDING + BUTTON_BOTTOM_PADDING);

            if (buttonSize.Width < MIN_BUTTON_WIDTH)
                buttonSize.Width = MIN_BUTTON_WIDTH;
            if (buttonSize.Height < MIN_BUTTON_HEIGHT)
                buttonSize.Height = MIN_BUTTON_HEIGHT;

            return buttonSize;
        }

        /// <summary>
        /// 设置对话框的大小
        /// Sets the optimum size for the form based on the controls that
        /// need to be displayed
        /// </summary>
        private void SetOptimumSize()
        {
            int ncWidth = this.Width - this.ClientSize.Width;
            int ncHeight = this.Height - this.ClientSize.Height;

            int iconAndMessageRowWidth = rtbMessage.Width + ICON_MESSAGE_PADDING + panelIcon.Width;
            int saveResponseRowWidth = chbSaveResponse.Width + (int)(panelIcon.Width / 2);
            int buttonsRowWidth = GetWidthOfAllButtons();
            int captionWidth = GetCaptionSize().Width;

            int maxItemWidth = Math.Max(saveResponseRowWidth, Math.Max(iconAndMessageRowWidth, buttonsRowWidth));

            int requiredWidth = LEFT_PADDING + maxItemWidth + RIGHT_PADDING + ncWidth;
            //Since Caption width is not client width, we do the check here
            if (requiredWidth < captionWidth)
                requiredWidth = captionWidth;

            int requiredHeight = TOP_PADDING + Math.Max(rtbMessage.Height, panelIcon.Height) + ITEM_PADDING + chbSaveResponse.Height + ITEM_PADDING + GetButtonSize().Height + BOTTOM_PADDING + ncHeight;

            //Fix the bug where if the message text is huge then the buttons are overwritten.
            //Incase the required height is more than the max height then adjust that in the
            //message height
            if (requiredHeight > _maxHeight)
            {
                rtbMessage.Height -= requiredHeight - _maxHeight;
            }

            int height = Math.Min(requiredHeight, _maxHeight);
            int width = Math.Min(requiredWidth, _maxWidth);
            this.Size = new Size(width, height);
        }

        /// <summary>
        /// 获得所有按钮加在一起的宽度
        /// Returns the width that will be occupied by all buttons including
        /// the inter-button padding
        /// </summary>
        private int GetWidthOfAllButtons()
        {
            Size buttonSize = GetButtonSize();
            int allButtonsWidth = buttonSize.Width * _buttons.Count + BUTTON_PADDING * (_buttons.Count - 1);

            return allButtonsWidth;
        }

        /// <summary>
        /// 布局所有控件
        /// Layout all the controls 
        /// </summary>
        private void LayoutControls()
        {
            panelIcon.Location = new Point(LEFT_PADDING, TOP_PADDING);
            rtbMessage.Location = new Point(LEFT_PADDING + panelIcon.Width + ICON_MESSAGE_PADDING * (panelIcon.Width == 0 ? 0 : 1), TOP_PADDING);

            chbSaveResponse.Location = new Point(LEFT_PADDING + (int)(panelIcon.Width / 2),
                TOP_PADDING + Math.Max(panelIcon.Height, rtbMessage.Height) + ITEM_PADDING);

            Size buttonSize = GetButtonSize();
            int allButtonsWidth = GetWidthOfAllButtons();

            int firstButtonX = ((int)(this.ClientSize.Width - allButtonsWidth) / 2);
            int firstButtonY = this.ClientSize.Height - BOTTOM_PADDING - buttonSize.Height;
            Point nextButtonLocation = new Point(firstButtonX, firstButtonY);

            bool foundDefaultButton = false;
            foreach (SEMessageBoxButton button in _buttons)
            {
                Button buttonCtrl = GetButton(button, buttonSize, nextButtonLocation);

                if (foundDefaultButton == false)
                {
                    _defaultButtonControl = button;
                    foundDefaultButton = true;
                }

                nextButtonLocation.X += buttonSize.Width + BUTTON_PADDING;
            }
        }

        /// <summary>
        /// 获得对话框按钮，如果已存在直接返回现有的
        /// Gets the button control for the specified MessageBoxExButton, if the
        /// control has not been created this method creates the control
        /// </summary>
        /// <param name="button"></param>
        /// <param name="size"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private Button GetButton(SEMessageBoxButton button, Size size, Point location)
        {
            Button buttonCtrl = null;
            if (_buttonControlsTable.ContainsKey(button))
            {
                buttonCtrl = _buttonControlsTable[button] as Button;
                buttonCtrl.Size = size;
                buttonCtrl.Location = location;
            }
            else
            {
                buttonCtrl = CreateButton(button, size, location);
                _buttonControlsTable[button] = buttonCtrl;
                this.Controls.Add(buttonCtrl);
            }

            return buttonCtrl;
        }

        /// <summary>
        /// 创建对话框按钮
        /// Creates a button control based on info from MessageBoxExButton
        /// </summary>
        /// <param name="button"></param>
        /// <param name="size"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private Button CreateButton(SEMessageBoxButton button, Size size, Point location)
        {
            Button buttonCtrl = new Button();
            button.Control = buttonCtrl;
            buttonCtrl.Size = size;
            buttonCtrl.Text = button.Text;
            buttonCtrl.TextAlign = ContentAlignment.MiddleCenter;
            buttonCtrl.FlatStyle = FlatStyle.System;
            if (button.Description != null && button.Description.Trim().Length != 0)
            {
                buttonToolTip.SetToolTip(buttonCtrl, button.Description);
            }
            buttonCtrl.Location = location;
            buttonCtrl.Click += new EventHandler(OnButtonClicked);
            buttonCtrl.Tag = button;

            return buttonCtrl;
        }

        #endregion

        #region Event Handlers

        protected void OnButtonClicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            //if (btn == null || btn.Tag == null)
            //    return;

            //string result = btn.Tag as string;
            SetResultAndClose(btn.Tag as SEMessageBoxButton);
        }
       
        #endregion

    }
}
