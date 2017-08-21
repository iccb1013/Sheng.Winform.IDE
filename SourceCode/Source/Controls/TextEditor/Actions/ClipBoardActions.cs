/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Actions 
{
	public class Cut : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			if (textArea.Document.ReadOnly) {
				return;
			}
			textArea.ClipboardHandler.Cut(null, null);
		}
	}
	public class Copy : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			textArea.AutoClearSelection = false;
			textArea.ClipboardHandler.Copy(null, null);
		}
	}
	public class Paste : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			if (textArea.Document.ReadOnly) {
				return;
			}
			textArea.ClipboardHandler.Paste(null, null);
		}
	}
}
