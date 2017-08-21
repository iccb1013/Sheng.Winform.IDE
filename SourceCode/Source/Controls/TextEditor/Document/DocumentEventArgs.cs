/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public delegate void DocumentEventHandler(object sender, DocumentEventArgs e);
	public class DocumentEventArgs : EventArgs
	{
		IDocument document;
		int       offset;
		int       length;
		string    text;
		public IDocument Document {
			get {
				return document;
			}
		}
		public int Offset {
			get {
				return offset;
			}
		}
		public string Text {
			get {
				return text;
			}
		}
		public int Length {
			get {
				return length;
			}
		}
		public DocumentEventArgs(IDocument document) : this(document, -1, -1, null)
		{
		}
		public DocumentEventArgs(IDocument document, int offset) : this(document, offset, -1, null)
		{
		}
		public DocumentEventArgs(IDocument document, int offset, int length) : this(document, offset, length, null)
		{
		}
		public DocumentEventArgs(IDocument document, int offset, int length, string text)
		{
			this.document = document;
			this.offset   = offset;
			this.length   = length;
			this.text     = text;
		}
		public override string ToString()
		{
			return String.Format("[DocumentEventArgs: Document = {0}, Offset = {1}, Text = {2}, Length = {3}]",
			                     Document,
			                     Offset,
			                     Text,
			                     Length);
		}
	}
}
