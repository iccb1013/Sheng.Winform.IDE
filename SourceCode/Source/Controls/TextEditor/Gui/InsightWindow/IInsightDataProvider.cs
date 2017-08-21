/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Gui.InsightWindow
{
	public interface IInsightDataProvider
	{
		void SetupDataProvider(string fileName, TextArea textArea);
		bool CaretOffsetChanged();
		string GetInsightData(int number);
		int InsightDataCount {
			get;
		}
		int DefaultIndex {
			get;
		}
	}
}
