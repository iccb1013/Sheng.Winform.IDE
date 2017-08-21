/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Text;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class DocumentFactory
	{
		public IDocument CreateDocument()
		{
			DefaultDocument doc = new DefaultDocument();
			doc.TextBufferStrategy  = new GapTextBufferStrategy();
			doc.FormattingStrategy  = new DefaultFormattingStrategy();
			doc.LineManager         = new LineManager(doc, null);
			doc.FoldingManager      = new FoldingManager(doc, doc.LineManager);
			doc.FoldingManager.FoldingStrategy       = null; 
			doc.MarkerStrategy      = new MarkerStrategy(doc);
			doc.BookmarkManager     = new BookmarkManager(doc, doc.LineManager);
			return doc;
		}
		public IDocument CreateFromTextBuffer(ITextBufferStrategy textBuffer)
		{
			DefaultDocument doc = (DefaultDocument)CreateDocument();
			doc.TextContent = textBuffer.GetText(0, textBuffer.Length);
			doc.TextBufferStrategy = textBuffer;
			return doc;
		}
		public IDocument CreateFromFile(string fileName)
		{
			IDocument document = CreateDocument();
			document.TextContent = Util.FileReader.ReadFileContent(fileName, Encoding.Default);
			return document;
		}
	}
}
