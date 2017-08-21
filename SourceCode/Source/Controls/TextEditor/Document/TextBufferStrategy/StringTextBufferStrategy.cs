/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.IO;
using System.Text;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class StringTextBufferStrategy : ITextBufferStrategy
	{
		string storedText = "";
		public int Length {
			get {
				return storedText.Length;
			}
		}
		public void Insert(int offset, string text)
		{
			if (text != null) {
				storedText = storedText.Insert(offset, text);
			}
		}
		public void Remove(int offset, int length)
		{
			storedText = storedText.Remove(offset, length);
		}
		public void Replace(int offset, int length, string text)
		{
			Remove(offset, length);
			Insert(offset, text);
		}
		public string GetText(int offset, int length)
		{
			if (length == 0) {
				return "";
			}
			if (offset == 0 && length >= storedText.Length) {
				return storedText;
			}
			return storedText.Substring(offset, Math.Min(length, storedText.Length - offset));
		}
		public char GetCharAt(int offset)
		{
			if (offset == Length) {
				return '\0';
			}
			return storedText[offset];
		}
		public void SetContent(string text)
		{
			storedText = text;
		}
		public StringTextBufferStrategy()
		{
		}
		public static ITextBufferStrategy CreateTextBufferFromFile(string fileName)
		{
			if (!File.Exists(fileName)) {
				throw new System.IO.FileNotFoundException(fileName);
			}
			StringTextBufferStrategy s = new StringTextBufferStrategy();
			s.SetContent(Util.FileReader.ReadFileContent(fileName, Encoding.Default));
			return s;
		}
	}
}
