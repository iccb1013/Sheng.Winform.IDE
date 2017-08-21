/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
namespace Sheng.SailingEase.Controls.TextEditor
{
	public delegate void ToolTipRequestEventHandler(object sender, ToolTipRequestEventArgs e);
	public class ToolTipRequestEventArgs
	{
		Point mousePosition;
		TextLocation logicalPosition;
		bool inDocument;
		public Point MousePosition {
			get {
				return mousePosition;
			}
		}
		public TextLocation LogicalPosition {
			get {
				return logicalPosition;
			}
		}
		public bool InDocument {
			get {
				return inDocument;
			}
		}
		public bool ToolTipShown {
			get {
				return toolTipText != null;
			}
		}
		internal string toolTipText;
		public void ShowToolTip(string text)
		{
			toolTipText = text;
		}
		public ToolTipRequestEventArgs(Point mousePosition, TextLocation logicalPosition, bool inDocument)
		{
			this.mousePosition = mousePosition;
			this.logicalPosition = logicalPosition;
			this.inDocument = inDocument;
		}
	}
}
