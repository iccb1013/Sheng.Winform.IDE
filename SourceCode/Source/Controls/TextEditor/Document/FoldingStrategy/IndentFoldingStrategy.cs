/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class IndentFoldingStrategy : IFoldingStrategy
	{
		public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation)
		{
			List<FoldMarker> l = new List<FoldMarker>();
			Stack<int> offsetStack = new Stack<int>();
			Stack<string> textStack = new Stack<string>();
			return l;
		}
		int GetLevel(IDocument document, int offset)
		{
			int level = 0;
			int spaces = 0;
			for (int i = offset; i < document.TextLength; ++i) {
				char c = document.GetCharAt(i);
				if (c == '\t' || (c == ' ' && ++spaces == 4)) {
					spaces = 0;
					++level;
				} else {
					break;
				}
			}
			return level;
		}
	}
}
