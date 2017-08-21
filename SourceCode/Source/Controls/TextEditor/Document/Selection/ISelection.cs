/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Drawing;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public interface ISelection
	{
		TextLocation StartPosition {
			get;
			set;
		}
		TextLocation EndPosition {
			get;
			set;
		}
		int Offset {
			get;
		}
		int EndOffset {
			get;
		}
		int Length {
			get;
		}
		bool IsRectangularSelection {
			get;
		}
		bool IsEmpty {
			get;
		}
		string SelectedText {
			get;
		}
		bool ContainsOffset(int offset);
		bool ContainsPosition(TextLocation position);
	}
}
