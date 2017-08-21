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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Controls.TextEditor.Gui.InsightWindow;
using Sheng.SailingEase.Controls.TextEditor.Document;
namespace Sheng.SailingEase.Core.Development
{
    class SqlRegexIntelliSenseTextEditorControl : TextEditorControl
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
        ICodeCompletionBinding[] codeCompletionBindings;
        CodeCompletionWindow codeCompletionWindow = null;
        InsightWindow insightWindow = null;
        Form _ownerForm = null;
        public virtual Form OwnerForm
        {
            get
            {
                return _ownerForm;
            }
            set
            {
                _ownerForm = value;
            }
        }
        public ICodeCompletionBinding[] CodeCompletionBindings
        {
            get
            {
                if (codeCompletionBindings == null)
                {
                    SqlRegexCompletionBinding sqlRegexCompletionBinding = new SqlRegexCompletionBinding();
                    sqlRegexCompletionBinding.FormEntity = this.FormEntity;
                    sqlRegexCompletionBinding.AllowFormElementControlType = this.AllowFormElementControlType;
                    sqlRegexCompletionBinding.AllowDataSourceType = this.AllowDataSourceType;
                    codeCompletionBindings = new ICodeCompletionBinding[] { sqlRegexCompletionBinding };
                }
                return codeCompletionBindings;
            }
        }
        public SqlRegexIntelliSenseTextEditorControl()
        {
            this.Text = String.Empty;
            this.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("SQL");
            this.ShowLineNumbers = false;
            this.ShowVRuler = false;
            this.BorderStyle = BorderStyle.Fixed3D;
        }
        protected override void InitializeTextAreaControl(TextAreaControl newControl)
        {
            base.InitializeTextAreaControl(newControl);
            newControl.TextArea.KeyEventHandler += new Sheng.SailingEase.Controls.TextEditor.KeyEventHandler(HandleKeyPress);
            newControl.MouseWheel += new MouseEventHandler(TextAreaMouseWheel);
            newControl.DoHandleMousewheel = false;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                CloseCodeCompletionWindow(this, EventArgs.Empty);
                CloseInsightWindow(this, EventArgs.Empty);
            }
        }
        protected override void OnFileNameChanged(EventArgs e)
        {
            base.OnFileNameChanged(e);
        }
        void TextAreaMouseWheel(object sender, MouseEventArgs e)
        {
            TextAreaControl textAreaControl = (TextAreaControl)sender;
            if (insightWindow != null && !insightWindow.IsDisposed && insightWindow.Visible)
            {
                insightWindow.HandleMouseWheel(e);
            }
            else if (codeCompletionWindow != null && !codeCompletionWindow.IsDisposed && codeCompletionWindow.Visible)
            {
                codeCompletionWindow.HandleMouseWheel(e);
            }
            else
            {
                textAreaControl.HandleMouseWheel(e);
            }
        }
        bool HandleKeyPress(char ch)
        {
            if (codeCompletionWindow != null && !codeCompletionWindow.IsDisposed)
            {
                if (codeCompletionWindow.ProcessKeyEvent(ch))
                {
                    return true;
                }
                if (codeCompletionWindow != null && !codeCompletionWindow.IsDisposed)
                {
                    return false;
                }
            }
                foreach (ICodeCompletionBinding ccBinding in CodeCompletionBindings)
                {
                    if (ccBinding.HandleKeyPress(this, ch))
                        return false;
                }
            return false;
        }
        public void ShowCompletionWindow(ICompletionDataProvider completionDataProvider, char ch)
        {
            codeCompletionWindow = CodeCompletionWindow.ShowCompletionWindow(this.FindForm(), this, this.FileName, completionDataProvider, ch);
            if (codeCompletionWindow != null)
            {
                codeCompletionWindow.Closed += new EventHandler(CloseCodeCompletionWindow);
            }
        }
        void CloseCodeCompletionWindow(object sender, EventArgs e)
        {
            if (codeCompletionWindow != null)
            {
                codeCompletionWindow.Closed -= new EventHandler(CloseCodeCompletionWindow);
                codeCompletionWindow.Dispose();
                codeCompletionWindow = null;
            }
        }
        public void ShowInsightWindow(IInsightDataProvider insightDataProvider)
        {
            if (insightWindow == null || insightWindow.IsDisposed)
            {
                insightWindow = new InsightWindow(OwnerForm, this);
                insightWindow.Closed += new EventHandler(CloseInsightWindow);
            }
            insightWindow.AddInsightDataProvider(insightDataProvider,FileName);
            insightWindow.ShowInsightWindow();
        }
        void CloseInsightWindow(object sender, EventArgs e)
        {
            if (insightWindow != null)
            {
                insightWindow.Closed -= new EventHandler(CloseInsightWindow);
                insightWindow.Dispose();
                insightWindow = null;
            }
        }
        public string GetWordBeforeCaret()
        {
            int start = TextUtilities.FindPrevWordStart(Document, ActiveTextAreaControl.TextArea.Caret.Offset);
            return Document.GetText(start, ActiveTextAreaControl.TextArea.Caret.Offset - start);
        }
        public void SetContent(string content)
        {
            this.BeginUpdate();
            this.Document.TextContent = content;
            this.Document.UpdateQueue.Clear();
            this.EndUpdate();
            this.OptionsChanged();
            this.Refresh();
        }
    }
}
