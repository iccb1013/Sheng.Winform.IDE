/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Sheng.SailingEase.Controls.TextEditor.Document;
namespace Sheng.SailingEase.Controls.TextEditor.Util
{
	public class LookupTable
	{
		Node root = new Node(null, null);
		bool casesensitive;
		int  length;
		public int Count {
			get {
				return length;
			}
		}
		public object this[IDocument document, LineSegment line, int offset, int length] {
			get {
				if(length == 0) {
					return null;
				}
				Node next = root;
				int wordOffset = line.Offset + offset;
				if (casesensitive) {
					for (int i = 0; i < length; ++i) {
						int index = ((int)document.GetCharAt(wordOffset + i)) % 256;
						next = next[index];
						if (next == null) {
							return null;
						}
						if (next.color != null && TextUtility.RegionMatches(document, wordOffset, length, next.word)) {
							return next.color;
						}
					}
				} else {
					for (int i = 0; i < length; ++i) {
						int index = ((int)Char.ToUpper(document.GetCharAt(wordOffset + i))) % 256;
						next = next[index];
						if (next == null) {
							return null;
						}
						if (next.color != null && TextUtility.RegionMatches(document, casesensitive, wordOffset, length, next.word)) {
							return next.color;
						}
					}
				}
				return null;
			}
		}
		public object this[string keyword] {
			set {
				Node node = root;
				Node next = root;
				if (!casesensitive) {
					keyword = keyword.ToUpper();
				}
				++length;
				for (int i = 0; i < keyword.Length; ++i) {
					int index = ((int)keyword[i]) % 256; 
					bool d = keyword[i] == '\\';
					next = next[index];             
					if (next == null) { 
						node[index] = new Node(value, keyword);
						break;
					}
					if (next.word != null && next.word.Length != i) { 
						string tmpword  = next.word;                  
						object tmpcolor = next.color;                 
						next.color = next.word = null;
						this[tmpword] = tmpcolor;
					}
					if (i == keyword.Length - 1) { 
						next.word = keyword;       
						next.color = value;
						break;
					}
					node = next;
				}
			}
		}
		public LookupTable(bool casesensitive)
		{
			this.casesensitive = casesensitive;
		}
		class Node
		{
			public Node(object color, string word)
			{
				this.word  = word;
				this.color = color;
			}
			public string word;
			public object color;
			public Node this[int index] {
				get { 
					if (children != null)
						return children[index];
					else
						return null;
				}
				set {
					if (children == null)
						children = new Node[256];
					children[index] = value;
				}
			}
			private Node[] children;
		}
	}
}
