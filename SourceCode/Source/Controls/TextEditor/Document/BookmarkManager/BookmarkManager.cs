/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sheng.SailingEase.Controls.TextEditor.Util;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public interface IBookmarkFactory
	{
		Bookmark CreateBookmark(IDocument document, TextLocation location);
	}
	public class BookmarkManager
	{
		IDocument      document;
		#if DEBUG
		IList<Bookmark> bookmark = new CheckedList<Bookmark>();
		#else
		List<Bookmark> bookmark = new List<Bookmark>();
		#endif
		public ReadOnlyCollection<Bookmark> Marks {
			get {
				return new ReadOnlyCollection<Bookmark>(bookmark);
			}
		}
		public IDocument Document {
			get {
				return document;
			}
		}
		internal BookmarkManager(IDocument document, LineManager lineTracker)
		{
			this.document = document;
		}
		public IBookmarkFactory Factory { get; set;}
		public void ToggleMarkAt(TextLocation location)
		{
			Bookmark newMark;
			if (Factory != null) {
				newMark = Factory.CreateBookmark(document, location);
			} else {
				newMark = new Bookmark(document, location);
			}
			Type newMarkType = newMark.GetType();
			for (int i = 0; i < bookmark.Count; ++i) {
				Bookmark mark = bookmark[i];
				if (mark.LineNumber == location.Line && mark.CanToggle && mark.GetType() == newMarkType) {
					bookmark.RemoveAt(i);
					OnRemoved(new BookmarkEventArgs(mark));
					return;
				}
			}
			bookmark.Add(newMark);
			OnAdded(new BookmarkEventArgs(newMark));
		}
		public void AddMark(Bookmark mark)
		{
			bookmark.Add(mark);
			OnAdded(new BookmarkEventArgs(mark));
		}
		public void RemoveMark(Bookmark mark)
		{
			bookmark.Remove(mark);
			OnRemoved(new BookmarkEventArgs(mark));
		}
		public void RemoveMarks(Predicate<Bookmark> predicate)
		{
			for (int i = 0; i < bookmark.Count; ++i) {
				Bookmark bm = bookmark[i];
				if (predicate(bm)) {
					bookmark.RemoveAt(i--);
					OnRemoved(new BookmarkEventArgs(bm));
				}
			}
		}
		public bool IsMarked(int lineNr)
		{
			for (int i = 0; i < bookmark.Count; ++i) {
				if (bookmark[i].LineNumber == lineNr) {
					return true;
				}
			}
			return false;
		}
		public void Clear()
		{
			foreach (Bookmark mark in bookmark) {
				OnRemoved(new BookmarkEventArgs(mark));
			}
			bookmark.Clear();
		}
		public Bookmark GetFirstMark(Predicate<Bookmark> predicate)
		{
			if (bookmark.Count < 1) {
				return null;
			}
			Bookmark first = null;
			for (int i = 0; i < bookmark.Count; ++i) {
				if (predicate(bookmark[i]) && bookmark[i].IsEnabled && (first == null || bookmark[i].LineNumber < first.LineNumber)) {
					first = bookmark[i];
				}
			}
			return first;
		}
		public Bookmark GetLastMark(Predicate<Bookmark> predicate)
		{
			if (bookmark.Count < 1) {
				return null;
			}
			Bookmark last = null;
			for (int i = 0; i < bookmark.Count; ++i) {
				if (predicate(bookmark[i]) && bookmark[i].IsEnabled && (last == null || bookmark[i].LineNumber > last.LineNumber)) {
					last = bookmark[i];
				}
			}
			return last;
		}
		bool AcceptAnyMarkPredicate(Bookmark mark)
		{
			return true;
		}
		public Bookmark GetNextMark(int curLineNr)
		{
			return GetNextMark(curLineNr, AcceptAnyMarkPredicate);
		}
		public Bookmark GetNextMark(int curLineNr, Predicate<Bookmark> predicate)
		{
			if (bookmark.Count == 0) {
				return null;
			}
			Bookmark next = GetFirstMark(predicate);
			foreach (Bookmark mark in bookmark) {
				if (predicate(mark) && mark.IsEnabled && mark.LineNumber > curLineNr) {
					if (mark.LineNumber < next.LineNumber || next.LineNumber <= curLineNr) {
						next = mark;
					}
				}
			}
			return next;
		}
		public Bookmark GetPrevMark(int curLineNr)
		{
			return GetPrevMark(curLineNr, AcceptAnyMarkPredicate);
		}
		public Bookmark GetPrevMark(int curLineNr, Predicate<Bookmark> predicate)
		{
			if (bookmark.Count == 0) {
				return null;
			}
			Bookmark prev = GetLastMark(predicate);
			foreach (Bookmark mark in bookmark) {
				if (predicate(mark) && mark.IsEnabled && mark.LineNumber < curLineNr) {
					if (mark.LineNumber > prev.LineNumber || prev.LineNumber >= curLineNr) {
						prev = mark;
					}
				}
			}
			return prev;
		}
		protected virtual void OnRemoved(BookmarkEventArgs e)
		{
			if (Removed != null) {
				Removed(this, e);
			}
		}
		protected virtual void OnAdded(BookmarkEventArgs e)
		{
			if (Added != null) {
				Added(this, e);
			}
		}
		public event BookmarkEventHandler Removed;
		public event BookmarkEventHandler Added;
	}
}
