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
	struct DeferredEventList
	{
		internal List<LineSegment> removedLines;
		internal List<TextAnchor> textAnchor;
		public void AddRemovedLine(LineSegment line)
		{
			if (removedLines == null)
				removedLines = new List<LineSegment>();
			removedLines.Add(line);
		}
		public void AddDeletedAnchor(TextAnchor anchor)
		{
			if (textAnchor == null)
				textAnchor = new List<TextAnchor>();
			textAnchor.Add(anchor);
		}
		public void RaiseEvents()
		{
			if (textAnchor != null) {
				foreach (TextAnchor a in textAnchor) {
					a.RaiseDeleted();
				}
			}
		}
	}
}
