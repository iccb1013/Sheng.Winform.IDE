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
using Sheng.SailingEase.Controls.TextEditor;
using Sheng.SailingEase.Controls.TextEditor.Gui.CompletionWindow;
namespace Sheng.SailingEase.Core.Development
{
    abstract class AbstractCompletionDataProvider : ICompletionDataProvider
    {
         private ImageList _imageList;
         public virtual System.Windows.Forms.ImageList ImageList
        {
            get
            {
                if (_imageList == null)
                {
                    _imageList = new ImageList();
                }
                return _imageList;
            }
        }
        protected string _preSelection = null;
        public string PreSelection
        {
            get
            {
                return this._preSelection;
            }
        }
        private int _defaultIndex = -1;
        public int DefaultIndex
        {
            get
            {
                return _defaultIndex;
            }
        }
        bool insertSpace;
        public bool InsertSpace
        {
            get
            {
                return insertSpace;
            }
            set
            {
                insertSpace = value;
            }
        }
        public virtual CompletionDataProviderKeyResult ProcessKey(char key)
        {
            CompletionDataProviderKeyResult res;
            if (key == ' ' && insertSpace)
            {
                insertSpace = false; 
                res = CompletionDataProviderKeyResult.BeforeStartKey;
            }
            else if (char.IsLetterOrDigit(key) || key == '_')
            {
                insertSpace = false; 
                res = CompletionDataProviderKeyResult.NormalKey;
            }
            else
            {
                res = CompletionDataProviderKeyResult.InsertionKey;
            }
            return res;
        }
        public virtual bool InsertAction(ICompletionData data, 
            TextArea textArea, int insertionOffset, char key)
        {
            if (InsertSpace)
            {
                textArea.Document.Insert(insertionOffset++, " ");
            }
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);
            return data.InsertAction(textArea, key);
        }
        public abstract ICompletionData[] GenerateCompletionData(string fileName,
            TextArea textArea, char charTyped);
    }
}
