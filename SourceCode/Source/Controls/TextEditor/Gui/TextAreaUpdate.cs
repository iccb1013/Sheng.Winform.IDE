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
	public enum TextAreaUpdateType {
		WholeTextArea,
		SingleLine,
		SinglePosition,
		PositionToLineEnd,
		PositionToEnd,
		LinesBetween
	}
	public class TextAreaUpdate
	{
		TextLocation position;
		TextAreaUpdateType type;
		public TextAreaUpdateType TextAreaUpdateType {
			get {
				return type;
			}
		}
		public TextLocation Position {
			get {
				return position;
			}
		}
		public TextAreaUpdate(TextAreaUpdateType type)
		{
			this.type = type;
		}
		public TextAreaUpdate(TextAreaUpdateType type, TextLocation position)
		{
			this.type     = type;
			this.position = position;
		}
		public TextAreaUpdate(TextAreaUpdateType type, int startLine, int endLine)
		{
			this.type     = type;
			this.position = new TextLocation(startLine, endLine);
		}
		public TextAreaUpdate(TextAreaUpdateType type, int singleLine)
		{
			this.type     = type;
			this.position = new TextLocation(0, singleLine);
		}
		public override string ToString()
		{
			return String.Format("[TextAreaUpdate: Type={0}, Position={1}]", type, position);
		}
	}
}
