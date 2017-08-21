/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public interface ITextBufferStrategy
	{
		int Length {
			get;
		}
		void Insert(int offset, string text);
		void Remove(int offset, int length);
		void Replace(int offset, int length, string text);
		string GetText(int offset, int length);
		char GetCharAt(int offset);
		void SetContent(string text);
	}
}
