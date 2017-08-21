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
	public interface IHighlightingStrategy
	{
		string Name {
			get;
		}
		string[] Extensions {
			get;
		}
		Dictionary<string, string> Properties {
			get;
		}
		HighlightColor GetColorFor(string name);
		void MarkTokens(IDocument document, List<LineSegment> lines);
		void MarkTokens(IDocument document);
	}
	public interface IHighlightingStrategyUsingRuleSets : IHighlightingStrategy
	{
		HighlightRuleSet GetRuleSet(Span span);
		HighlightColor GetColor(IDocument document, LineSegment keyWord, int index, int length);
	}
}
