/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Gui.CompletionWindow
{
	public interface ICompletionData
	{
		int ImageIndex {
			get;
		}
		string Text {
			get;
			set;
		}
		string Description {
			get;
		}
		double Priority {
			get;
		}
		bool InsertAction(TextArea textArea, char ch);
	}
	public class DefaultCompletionData : ICompletionData
	{
		string text;
		string description;
		int imageIndex;
		public int ImageIndex {
			get {
				return imageIndex;
			}
		}
		public string Text {
			get {
				return text;
			}
			set {
				text = value;
			}
		}
		public virtual string Description {
			get {
				return description;
			}
		}
		double priority;
		public double Priority {
			get {
				return priority;
			}
			set {
				priority = value;
			}
		}
		public virtual bool InsertAction(TextArea textArea, char ch)
		{
			textArea.InsertString(text);
			return false;
		}
		public DefaultCompletionData(string text, int imageIndex)
		{
			this.text        = text;
			this.imageIndex  = imageIndex;
		}
		public DefaultCompletionData(string text, string description, int imageIndex)
		{
			this.text        = text;
			this.description = description;
			this.imageIndex  = imageIndex;
		}
		public static int Compare(ICompletionData a, ICompletionData b)
		{
			if (a == null)
				throw new ArgumentNullException("a");
			if (b == null)
				throw new ArgumentNullException("b");
			return string.Compare(a.Text, b.Text, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
