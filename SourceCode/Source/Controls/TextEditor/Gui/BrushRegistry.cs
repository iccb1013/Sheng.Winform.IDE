/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Sheng.SailingEase.Controls.TextEditor
{
	public class BrushRegistry
	{
		static Dictionary<Color, Brush> brushes = new Dictionary<Color, Brush>();
		static Dictionary<Color, Pen> pens = new Dictionary<Color, Pen>();
		static Dictionary<Color, Pen> dotPens = new Dictionary<Color, Pen>();
		public static Brush GetBrush(Color color)
		{
			lock (brushes) {
				Brush brush;
				if (!brushes.TryGetValue(color, out brush)) {
					brush = new SolidBrush(color);
					brushes.Add(color, brush);
				}
				return brush;
			}
		}
		public static Pen GetPen(Color color)
		{
			lock (pens) {
				Pen pen;
				if (!pens.TryGetValue(color, out pen)) {
					pen = new Pen(color);
					pens.Add(color, pen);
				}
				return pen;
			}
		}
		static readonly float[] dotPattern = { 1, 1, 1, 1 };
		public static Pen GetDotPen(Color color)
		{
			lock (dotPens) {
				Pen pen;
				if (!dotPens.TryGetValue(color, out pen)) {
					pen = new Pen(color);
					pen.DashPattern = dotPattern;
					dotPens.Add(color, pen);
				}
				return pen;
			}
		}
	}
}
