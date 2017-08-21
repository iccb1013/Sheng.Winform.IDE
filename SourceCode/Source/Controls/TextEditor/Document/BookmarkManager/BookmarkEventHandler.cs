/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public delegate void BookmarkEventHandler(object sender, BookmarkEventArgs e);
	public class BookmarkEventArgs : EventArgs
	{
		Bookmark bookmark;
		public Bookmark Bookmark {
			get {
				return bookmark;
			}
		}
		public BookmarkEventArgs(Bookmark bookmark)
		{
			this.bookmark = bookmark;
		}
	}
}
