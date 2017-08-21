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
    class CommentCompletionDataProvider : AbstractCompletionDataProvider
    {
        private EnumEventDataSource _allowDataSourceType = EnumEventDataSource.ANY;
        public EnumEventDataSource AllowDataSourceType
        {
            get
            {
                return this._allowDataSourceType;
            }
            set
            {
                this._allowDataSourceType = value;
            }
        }
        public override ICompletionData[] GenerateCompletionData(string fileName, 
            TextArea textArea, char charTyped)
        {
            if (charTyped == '{')
            {
                return GenerateCompletionData();
            }
            return new ICompletionData[] { };
        }
        private ICompletionData[] GenerateCompletionData()
        {
            CommentCompletionData[] codeCompletionData = new CommentCompletionData[2];
            if ((this.AllowDataSourceType & EnumEventDataSource.FormElement) == EnumEventDataSource.FormElement)
            {
                codeCompletionData[0] = new CommentCompletionData();
                codeCompletionData[0].Text = "FormElement";
                codeCompletionData[0].Description = "窗体元素";
            }
            if ((this.AllowDataSourceType & EnumEventDataSource.System) == EnumEventDataSource.System)
            {
                codeCompletionData[1] = new CommentCompletionData();
                codeCompletionData[1].Text = "System";
                codeCompletionData[1].Description = "系统";
            }
            return codeCompletionData;
        }
    }
}
