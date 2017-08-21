/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
using System.Xml;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class HighlightBackground : HighlightColor
	{
		Image backgroundImage;
		public Image BackgroundImage {
			get {
				return backgroundImage;
			}
		}
		public HighlightBackground(XmlElement el) : base(el)
		{
			if (el.Attributes["image"] != null) {
				backgroundImage = new Bitmap(el.Attributes["image"].InnerText);
			}
		}
		public HighlightBackground(Color color, Color backgroundcolor, bool bold, bool italic) : base(color, backgroundcolor, bold, italic)
		{
		}
		public HighlightBackground(string systemColor, string systemBackgroundColor, bool bold, bool italic) : base(systemColor, systemBackgroundColor, bold, italic)
		{
		}
	}
}
