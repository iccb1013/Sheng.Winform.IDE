/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Xml;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class PrevMarker
	{
		string      what;
		HighlightColor color;
		bool        markMarker = false;
		public string What {
			get {
				return what;
			}
		}
		public HighlightColor Color {
			get {
				return color;
			}
		}
		public bool MarkMarker {
			get {
				return markMarker;
			}
		}
		public PrevMarker(XmlElement mark)
		{
			color = new HighlightColor(mark);
			what  = mark.InnerText;
			if (mark.Attributes["markmarker"] != null) {
				markMarker = Boolean.Parse(mark.Attributes["markmarker"].InnerText);
			}
		}
	}
}
