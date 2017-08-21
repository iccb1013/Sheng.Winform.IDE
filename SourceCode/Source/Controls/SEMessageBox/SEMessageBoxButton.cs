/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls
{
	public class SEMessageBoxButton
	{
		private string _text = null;
		public string Text
		{
			get{ return _text; }
			set{ _text = value; }
		}
        private string _description = null;
		public string Description
		{
            get { return _description; }
            set { _description = value; }
		}
		private bool _isCancelButton = false;
		public bool IsCancelButton
		{
			get{ return _isCancelButton; }
			set{ _isCancelButton = value; }
        }
        private DialogResult _result = DialogResult.None;
        public DialogResult Result
        {
            get { return _result; }
            set { _result = value; }
        }
        private Control _control;
        public Control Control
        {
            get { return _control; }
            set { _control = value; }
        }
        public SEMessageBoxButton()
        {
        }
        public SEMessageBoxButton(string text)
            : this(text, null)
        {
        }
        public SEMessageBoxButton(string text,string description)
        {
            Text = text;
            Description = description;
        }
    }
}
