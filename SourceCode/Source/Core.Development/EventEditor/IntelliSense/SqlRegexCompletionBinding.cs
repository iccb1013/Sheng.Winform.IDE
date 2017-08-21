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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Controls.TextEditor;
namespace Sheng.SailingEase.Core.Development
{
    class SqlRegexCompletionBinding : ICodeCompletionBinding
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
        bool enableMethodInsight = true;
        bool enableIndexerInsight = true;
        bool enableXmlCommentCompletion = true;
        bool enableDotCompletion = true;
        public bool EnableMethodInsight
        {
            get
            {
                return enableMethodInsight;
            }
            set
            {
                enableMethodInsight = value;
            }
        }
        public bool EnableIndexerInsight
        {
            get
            {
                return enableIndexerInsight;
            }
            set
            {
                enableIndexerInsight = value;
            }
        }
        public bool EnableXmlCommentCompletion
        {
            get
            {
                return enableXmlCommentCompletion;
            }
            set
            {
                enableXmlCommentCompletion = value;
            }
        }
        public bool EnableDotCompletion
        {
            get
            {
                return enableDotCompletion;
            }
            set
            {
                enableDotCompletion = value;
            }
        }
        public virtual bool HandleKeyPress(SqlRegexIntelliSenseTextEditorControl editor, char ch)
        {
            switch (ch)
            {
                case '.':
                    if (enableDotCompletion)
                    {
                        CodeCompletionDataProvider completionDataProvider = new CodeCompletionDataProvider();
                        completionDataProvider.FormEntity = this.FormEntity;
                        completionDataProvider.AllowFormElementControlType = this.AllowFormElementControlType;
                        editor.ShowCompletionWindow(completionDataProvider, ch);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case '{':
                    if (enableDotCompletion)
                    {
                        CommentCompletionDataProvider commentCompletionDataProvider = new CommentCompletionDataProvider();
                        commentCompletionDataProvider.AllowDataSourceType = this.AllowDataSourceType;
                        editor.ShowCompletionWindow(commentCompletionDataProvider, ch);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }
        public virtual bool HandleKeyword(TextEditorControl editor, string word)
        {
            return false;
        }
    }
}
