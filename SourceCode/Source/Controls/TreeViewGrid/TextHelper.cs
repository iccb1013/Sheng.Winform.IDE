/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	public static class TextHelper
	{
		public static StringAlignment TranslateAligment(HorizontalAlignment aligment)
		{
			if (aligment == HorizontalAlignment.Left)
				return StringAlignment.Near;
			else if (aligment == HorizontalAlignment.Right)
				return StringAlignment.Far;
			else
				return StringAlignment.Center;
		}
	}
}
