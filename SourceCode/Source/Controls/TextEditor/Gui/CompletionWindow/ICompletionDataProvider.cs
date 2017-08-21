/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.TextEditor.Gui.CompletionWindow
{
	public interface ICompletionDataProvider
	{
		ImageList ImageList {
			get;
		}
		string PreSelection {
			get;
		}
		int DefaultIndex {
			get;
		}
		CompletionDataProviderKeyResult ProcessKey(char key);
		bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key);
		ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped);
	}
	public enum CompletionDataProviderKeyResult
	{
		NormalKey,
		InsertionKey,
		BeforeStartKey
	}
}
