/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Sheng.SailingEase.Controls.TextEditor.Document;
using Sheng.SailingEase.Controls.TextEditor.Util;
namespace Sheng.SailingEase.Controls.TextEditor
{
	public class TextAreaClipboardHandler
	{
		TextArea textArea;
		public bool EnableCut {
			get {
				return textArea.EnableCutOrPaste; 
			}
		}
		public bool EnableCopy {
			get {
				return true; 
			}
		}
		public delegate bool ClipboardContainsTextDelegate();
		public static ClipboardContainsTextDelegate GetClipboardContainsText;
		public bool EnablePaste {
			get {
				if (!textArea.EnableCutOrPaste)
					return false;
				ClipboardContainsTextDelegate d = GetClipboardContainsText;
				if (d != null) {
					return d();
				} else {
					try {
						return Clipboard.ContainsText();
					} catch (ExternalException) {
						return false;
					}
				}
			}
		}
		public bool EnableDelete {
			get {
				return textArea.SelectionManager.HasSomethingSelected && !textArea.SelectionManager.SelectionIsReadonly;
			}
		}
		public bool EnableSelectAll {
			get {
				return true;
			}
		}
		public TextAreaClipboardHandler(TextArea textArea)
		{
			this.textArea = textArea;
			textArea.SelectionManager.SelectionChanged += new EventHandler(DocumentSelectionChanged);
		}
		void DocumentSelectionChanged(object sender, EventArgs e)
		{
		}
		const string LineSelectedType = "MSDEVLineSelect";  
		bool CopyTextToClipboard(string stringToCopy, bool asLine)
		{
			if (stringToCopy.Length > 0) {
				DataObject dataObject = new DataObject();
				dataObject.SetData(DataFormats.UnicodeText, true, stringToCopy);
				if (asLine) {
					MemoryStream lineSelected = new MemoryStream(1);
					lineSelected.WriteByte(1);
					dataObject.SetData(LineSelectedType, false, lineSelected);
				}
				if (textArea.Document.HighlightingStrategy.Name != "Default") {
					dataObject.SetData(DataFormats.Rtf, RtfWriter.GenerateRtf(textArea));
				}
				OnCopyText(new CopyTextEventArgs(stringToCopy));
				SafeSetClipboard(dataObject);
				return true;
			} else {
				return false;
			}
		}
		[ThreadStatic] static int SafeSetClipboardDataVersion;
		static void SafeSetClipboard(object dataObject)
		{
			int version = unchecked(++SafeSetClipboardDataVersion);
			try {
				Clipboard.SetDataObject(dataObject, true);
			} catch (ExternalException) {
				Timer timer = new Timer();
				timer.Interval = 100;
				timer.Tick += delegate {
					timer.Stop();
					timer.Dispose();
					if (SafeSetClipboardDataVersion == version) {
						try {
							Clipboard.SetDataObject(dataObject, true, 10, 50);
						} catch (ExternalException) { }
					}
				};
				timer.Start();
			}
		}
		bool CopyTextToClipboard(string stringToCopy)
		{
			return CopyTextToClipboard(stringToCopy, false);
		}
		public void Cut(object sender, EventArgs e)
		{
			if (textArea.SelectionManager.HasSomethingSelected) {
				if (CopyTextToClipboard(textArea.SelectionManager.SelectedText)) {
					if (textArea.SelectionManager.SelectionIsReadonly)
						return;
					textArea.BeginUpdate();
					textArea.Caret.Position = textArea.SelectionManager.SelectionCollection[0].StartPosition;
					textArea.SelectionManager.RemoveSelectedText();
					textArea.EndUpdate();
				}
			} else if (textArea.Document.TextEditorProperties.CutCopyWholeLine) {
				int curLineNr = textArea.Document.GetLineNumberForOffset(textArea.Caret.Offset);
				LineSegment lineWhereCaretIs = textArea.Document.GetLineSegment(curLineNr);
				string caretLineText = textArea.Document.GetText(lineWhereCaretIs.Offset, lineWhereCaretIs.TotalLength);
				textArea.SelectionManager.SetSelection(textArea.Document.OffsetToPosition(lineWhereCaretIs.Offset), textArea.Document.OffsetToPosition(lineWhereCaretIs.Offset + lineWhereCaretIs.TotalLength));
				if (CopyTextToClipboard(caretLineText, true)) {
					if (textArea.SelectionManager.SelectionIsReadonly)
						return;
					textArea.BeginUpdate();
					textArea.Caret.Position = textArea.Document.OffsetToPosition(lineWhereCaretIs.Offset);
					textArea.SelectionManager.RemoveSelectedText();
					textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.PositionToEnd, new TextLocation(0, curLineNr)));
					textArea.EndUpdate();
				}
			}
		}
		public void Copy(object sender, EventArgs e)
		{
			if (!CopyTextToClipboard(textArea.SelectionManager.SelectedText) && textArea.Document.TextEditorProperties.CutCopyWholeLine) {
				int curLineNr = textArea.Document.GetLineNumberForOffset(textArea.Caret.Offset);
				LineSegment lineWhereCaretIs = textArea.Document.GetLineSegment(curLineNr);
				string caretLineText = textArea.Document.GetText(lineWhereCaretIs.Offset, lineWhereCaretIs.TotalLength);
				CopyTextToClipboard(caretLineText, true);
			}
		}
		public void Paste(object sender, EventArgs e)
		{
			if (!textArea.EnableCutOrPaste) {
				return;
			}
			for (int i = 0;; i++) {
				try {
					IDataObject data = Clipboard.GetDataObject();
					if (data == null)
						return;
					bool fullLine = data.GetDataPresent(LineSelectedType);
					if (data.GetDataPresent(DataFormats.UnicodeText)) {
						string text = (string)data.GetData(DataFormats.UnicodeText);
						if (text.Length > 0) {
							textArea.Document.UndoStack.StartUndoGroup();
							try {
								if (textArea.SelectionManager.HasSomethingSelected) {
									textArea.Caret.Position = textArea.SelectionManager.SelectionCollection[0].StartPosition;
									textArea.SelectionManager.RemoveSelectedText();
								}
								if (fullLine) {
									int col = textArea.Caret.Column;
									textArea.Caret.Column = 0;
									if (!textArea.IsReadOnly(textArea.Caret.Offset))
										textArea.InsertString(text);
									textArea.Caret.Column = col;
								} else {
									textArea.InsertString(text);
								}
							} finally {
								textArea.Document.UndoStack.EndUndoGroup();
							}
						}
					}
					return;
				} catch (ExternalException) {
					if (i > 5) throw;
				}
			}
		}
		public void Delete(object sender, EventArgs e)
		{
			new Sheng.SailingEase.Controls.TextEditor.Actions.Delete().Execute(textArea);
		}
		public void SelectAll(object sender, EventArgs e)
		{
			new Sheng.SailingEase.Controls.TextEditor.Actions.SelectWholeDocument().Execute(textArea);
		}
		protected virtual void OnCopyText(CopyTextEventArgs e)
		{
			if (CopyText != null) {
				CopyText(this, e);
			}
		}
		public event CopyTextEventHandler CopyText;
	}
	public delegate void CopyTextEventHandler(object sender, CopyTextEventArgs e);
	public class CopyTextEventArgs : EventArgs
	{
		string text;
		public string Text {
			get {
				return text;
			}
		}
		public CopyTextEventArgs(string text)
		{
			this.text = text;
		}
	}
}
