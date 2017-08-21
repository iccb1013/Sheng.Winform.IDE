/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Controls.TextEditor.Gui.CompletionWindow;
using Sheng.SailingEase.Controls.TextEditor;
using Sheng.SailingEase.Controls.TextEditor.Document;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core.Development
{
    class CodeCompletionDataProvider : AbstractCompletionDataProvider
    {
        public WindowEntity FormEntity
        {
            get;
            set;
        }
        private UIElementEntityTypeCollection _allowFormElementControlType = new UIElementEntityTypeCollection();
        public UIElementEntityTypeCollection AllowFormElementControlType
        {
            get
            {
                return this._allowFormElementControlType;
            }
            set
            {
                this._allowFormElementControlType = value;
            }
        }
        public override ICompletionData[] GenerateCompletionData(string fileName,
            TextArea textArea, char charTyped)
        {
            string expression = FindExpression(textArea);
            if (expression == "{FormElement")
            {
                return GenerateFormElementCompletionData();
            }
            else if (expression == "{System")
            {
                return GenerateSystemCompletionData();
            }
            else
            {
                return new ICompletionData[] { };
            }
        }
        private string FindExpression(TextArea textArea)
        {
            string expression = String.Empty;
            IDocument document = textArea.Document;
            string text = document.TextContent;
            for (int i = textArea.Caret.Offset - 1; i >= 0; i--)
            {
                switch (text[i])
                {
                    case '{':
                        return "{" + expression;
                    case '.':
                        return "." + expression;
                    default:
                        expression = text[i] + expression;
                        break;
                }
            }
            return expression;
        }
        private ICompletionData[] GenerateFormElementCompletionData()
        {
            if (this.FormEntity == null)
                return new ICompletionData[] { };
            UIElementCollection elements =
                this.FormEntity.GetFormElement(this.AllowFormElementControlType);
            CodeCompletionData[] codeCompletionData = new CodeCompletionData[elements.Count];
            for (int i = 0; i < elements.Count; i++)
            {
                codeCompletionData[i] = new CodeCompletionData();
                codeCompletionData[i].Text = elements[i].FullCode;
                codeCompletionData[i].Description = elements[i].Name + "\r\n" + elements[i].Remark;
            }
            return codeCompletionData;
        }
        private ICompletionData[] GenerateSystemCompletionData()
        {
            CodeCompletionData[] codeCompletionData = 
                new CodeCompletionData[EnumDescConverter.Get(typeof(EnumSystemDataSource)).Rows.Count];
            for (int i = 0; i < EnumDescConverter.Get(typeof(EnumSystemDataSource)).Rows.Count; i++)
            {
                codeCompletionData[i] = new CodeCompletionData();
                codeCompletionData[i].Text = ((EnumSystemDataSource)Convert.ToInt32(
                    EnumDescConverter.Get(typeof(EnumSystemDataSource)).Rows[i]["Value"].ToString())).ToString();
                codeCompletionData[i].Description = EnumDescConverter.Get(typeof(EnumSystemDataSource)).Rows[i]["Text"].ToString();
            }
            return codeCompletionData;
        }
        public override bool InsertAction(ICompletionData data, TextArea textArea,
            int insertionOffset, char key)
        {
            if (InsertSpace)
            {
                textArea.Document.Insert(insertionOffset++, " ");
            }
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);
            return data.InsertAction(textArea, '}');
        }
    }
}
