/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls.TextEditor.Gui.CompletionWindow;
using Sheng.SailingEase.Controls.TextEditor;
namespace Sheng.SailingEase.Core.Development
{
    class CommentCompletionData : ICompletionData
    {
        private int _imageIndex;
        public int ImageIndex
        {
            get
            {
                return this._imageIndex;
            }
            set
            {
                this._imageIndex = value;
            }
        }
        private string _text;
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }
        private string _description;
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }
        private double _priority = 0;
        public double Priority
        {
            get
            {
                return this._priority;
            }
        }
        public bool InsertAction(TextArea textArea, char ch)
        {
            textArea.InsertString(this.Text);
            SendKeys.Send("{.}");
            return true;
        }
        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is CodeCompletionData))
            {
                return -1;
            }
            return Text.CompareTo(((CodeCompletionData)obj).Text);
        }
    }
}
