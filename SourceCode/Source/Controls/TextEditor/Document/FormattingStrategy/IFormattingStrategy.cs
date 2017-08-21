/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public interface IFormattingStrategy
	{
		void FormatLine(TextArea textArea, int line, int caretOffset, char charTyped);
		int IndentLine(TextArea textArea, int line);
		void IndentLines(TextArea textArea, int begin, int end);
		int SearchBracketBackward(IDocument document, int offset, char openBracket, char closingBracket);
		int SearchBracketForward(IDocument document, int offset, char openBracket, char closingBracket);
	}
}
