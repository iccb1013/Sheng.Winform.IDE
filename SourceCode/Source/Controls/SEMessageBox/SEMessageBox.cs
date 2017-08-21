/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls
{
	public class SEMessageBox
	{
        private ISEMessageBoxForm _msgBox;
		private bool _useSavedResponse = true;
		private string _name = null;
		internal string Name
		{
			get{ return _name; }
			set{ _name = value; }
		}
		public string Caption
		{
			set{_msgBox.Caption = value;}
		}
		public string Text
		{
            get { return _msgBox.Message; }
			set{_msgBox.Message = value;}
		}
		public Icon CustomIcon
		{
			set{_msgBox.CustomIcon = value;}
		}
		public SEMessageBoxIcon Icon
		{
			set{ _msgBox.StandardIcon = (MessageBoxIcon)Enum.Parse(typeof(MessageBoxIcon), value.ToString());}
		}
		public Font Font
		{
			set{_msgBox.Font = value;}
		}
		public bool AllowSaveResponse
		{
			get{ return _msgBox.AllowSaveResponse; }
			set{ _msgBox.AllowSaveResponse = value; }
		}
		public string SaveResponseText
		{
			set{_msgBox.SaveResponseText = value; }
		}
		public bool UseSavedResponse
		{
			get{ return _useSavedResponse; }
			set{ _useSavedResponse = value; }
		}
		public bool PlayAlertSound
		{
			get{ return _msgBox.PlayAlertSound; }
			set{ _msgBox.PlayAlertSound = value; }
		}
        public int Timeout
        {
            get{ return _msgBox.Timeout; }
            set{ _msgBox.Timeout = value; }
        }
        public SEMessageBoxTimeoutResult TimeoutResult
        {
            get{ return _msgBox.TimeoutResult; }
            set{ _msgBox.TimeoutResult = value; }
        }
        public SEMessageBoxButton Show()
		{
			return Show(null);
		}
        public SEMessageBoxButton Show(IWin32Window owner)
		{
			if(_useSavedResponse && this.Name != null)
			{
                SEMessageBoxButton savedResponse = SEMessageBoxManager.GetSavedResponse(this);
				if( savedResponse != null)
					return savedResponse;
			}
			if(owner == null)
			{
				_msgBox.ShowDialog();
			}
			else
			{
				_msgBox.ShowDialog(owner);
			}
            if(this.Name != null)
            {
                if(_msgBox.AllowSaveResponse && _msgBox.SaveResponse)
                    SEMessageBoxManager.SetSavedResponse(this, _msgBox.Result);
                else
                    SEMessageBoxManager.ResetSavedResponse(this.Name);
            }
            else
            {
                Dispose();
            }
			return _msgBox.Result;
		}
		public void AddButton(params SEMessageBoxButton [] buttons)
		{
            if (buttons == null)
				throw new ArgumentNullException("button","A null button cannot be added");
            foreach (SEMessageBoxButton btn in buttons)
            {
                _msgBox.Buttons.Add(btn);
                if (btn.IsCancelButton)
                {
                    _msgBox.CustomCancelButton = btn;
                }
            }
		}
		public void AddButton(string text)
		{
			if(text == null)
				throw new ArgumentNullException("text","Text of a button cannot be null");
			SEMessageBoxButton button = new SEMessageBoxButton();
			button.Text = text;
			AddButton(button);
		}
		public void AddButton(SEMessageBoxButtons button)
		{
            string buttonText = SEMessageBoxManager.GetLocalizedString(button.ToString());
            if(buttonText == null)
            {
                buttonText = button.ToString();
            }
            string buttonVal = button.ToString();
            SEMessageBoxButton btn = new SEMessageBoxButton();
            btn.Text = buttonText;
            if(button == SEMessageBoxButtons.Cancel)
            {
                btn.IsCancelButton = true;
            }
			AddButton(btn);
		}
		public void AddButtons(MessageBoxButtons buttons)
		{
			switch(buttons)
			{
				case MessageBoxButtons.OK:
					AddButton(SEMessageBoxButtons.Ok);
					break;
				case MessageBoxButtons.AbortRetryIgnore:
					AddButton(SEMessageBoxButtons.Abort);
					AddButton(SEMessageBoxButtons.Retry);
					AddButton(SEMessageBoxButtons.Ignore);
					break;
				case MessageBoxButtons.OKCancel:
					AddButton(SEMessageBoxButtons.Ok);
					AddButton(SEMessageBoxButtons.Cancel);
					break;
				case MessageBoxButtons.RetryCancel:
					AddButton(SEMessageBoxButtons.Retry);
					AddButton(SEMessageBoxButtons.Cancel);
					break;
				case MessageBoxButtons.YesNo:
					AddButton(SEMessageBoxButtons.Yes);
					AddButton(SEMessageBoxButtons.No);
					break;
				case MessageBoxButtons.YesNoCancel:
					AddButton(SEMessageBoxButtons.Yes);
					AddButton(SEMessageBoxButtons.No);
					AddButton(SEMessageBoxButtons.Cancel);
					break;
			}
		}
        public SEMessageBox()
            : this(SEMessageBoxStyle.Standard)
        {
        }
        public SEMessageBox(SEMessageBoxStyle style)
        {
            switch (style)
            {
                case SEMessageBoxStyle.Standard:
                default:
                    _msgBox = new SEMessageBoxStandard();
                    break;
                case SEMessageBoxStyle.CmdLink:
                    _msgBox = new SEMessageBoxCmdLink();
                    break;
            }
        }
		internal void Dispose()
		{
			if(_msgBox != null)
			{
				_msgBox.Dispose();
			}
		}
	}
    public enum SEMessageBoxStyle
    {
        Standard = 0,
        CmdLink = 1
    }
}
