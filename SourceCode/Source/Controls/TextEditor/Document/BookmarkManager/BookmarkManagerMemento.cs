/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Xml;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class BookmarkManagerMemento
	{
		List<int> bookmarks = new List<int>();
		public List<int> Bookmarks {
			get {
				return bookmarks;
			}
			set {
				bookmarks = value;
			}
		}
		public void CheckMemento(IDocument document)
		{
			for (int i = 0; i < bookmarks.Count; ++i) {
				int mark = (int)bookmarks[i];
				if (mark < 0 || mark >= document.TotalNumberOfLines) {
					bookmarks.RemoveAt(i);
					--i;
				}
			}
		}
		public BookmarkManagerMemento()
		{
		}
		public BookmarkManagerMemento(XmlElement element)
		{
			foreach (XmlElement el in element.ChildNodes) {
				bookmarks.Add(Int32.Parse(el.Attributes["line"].InnerText));
			}
		}
		public BookmarkManagerMemento(List<int> bookmarks)
		{
			this.bookmarks = bookmarks;
		}
		public object FromXmlElement(XmlElement element)
		{
			return new BookmarkManagerMemento(element);
		}
		public XmlElement ToXmlElement(XmlDocument doc)
		{
			XmlElement bookmarknode  = doc.CreateElement("Bookmarks");
			foreach (int line in bookmarks) {
				XmlElement markNode = doc.CreateElement("Mark");
				XmlAttribute lineAttr = doc.CreateAttribute("line");
				lineAttr.InnerText = line.ToString();
				markNode.Attributes.Append(lineAttr);
				bookmarknode.AppendChild(markNode);
			}
			return bookmarknode;
		}
	}
}
