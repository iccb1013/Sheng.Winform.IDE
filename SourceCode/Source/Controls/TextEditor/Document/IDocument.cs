/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using Sheng.SailingEase.Controls.TextEditor.Undo;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public interface IDocument
	{
		ITextEditorProperties TextEditorProperties {
			get;
			set;
		}
		UndoStack UndoStack {
			get;
		}
		bool ReadOnly {
			get;
			set;
		}
		IFormattingStrategy FormattingStrategy {
			get;
			set;
		}
		ITextBufferStrategy TextBufferStrategy {
			get;
		}
		FoldingManager FoldingManager {
			get;
		}
		IHighlightingStrategy HighlightingStrategy {
			get;
			set;
		}
		BookmarkManager BookmarkManager {
			get;
		}
		MarkerStrategy MarkerStrategy {
			get;
		}
		IList<LineSegment> LineSegmentCollection {
			get;
		}
		int TotalNumberOfLines {
			get;
		}
		int GetLineNumberForOffset(int offset);
		LineSegment GetLineSegmentForOffset(int offset);
		LineSegment GetLineSegment(int lineNumber);
		int GetFirstLogicalLine(int lineNumber);
		int GetLastLogicalLine(int lineNumber);
		int GetVisibleLine(int lineNumber);
		int GetNextVisibleLineAbove(int lineNumber, int lineCount);
		int GetNextVisibleLineBelow(int lineNumber, int lineCount);
		event EventHandler<LineLengthChangeEventArgs> LineLengthChanged;
		event EventHandler<LineCountChangeEventArgs> LineCountChanged;
		event EventHandler<LineEventArgs> LineDeleted;
		string TextContent {
			get;
			set;
		}
		int TextLength {
			get;
		}
		void Insert(int offset, string text);
		void Remove(int offset, int length);
		void Replace(int offset, int length, string text);
		char GetCharAt(int offset);
		string GetText(int offset, int length);
		string GetText(ISegment segment);
		TextLocation OffsetToPosition(int offset);
		int PositionToOffset(TextLocation p);
		List<TextAreaUpdate> UpdateQueue {
			get;
		}
		void RequestUpdate(TextAreaUpdate update);
		void CommitUpdate();
		void UpdateSegmentListOnDocumentChange<T>(List<T> list, DocumentEventArgs e) where T : ISegment;
		event EventHandler UpdateCommited;
		event DocumentEventHandler DocumentAboutToBeChanged;
		event DocumentEventHandler DocumentChanged;
		event EventHandler TextContentChanged;
	}
}
