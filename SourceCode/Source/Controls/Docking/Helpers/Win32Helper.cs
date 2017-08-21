/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.Docking
{
	internal static class Win32Helper
	{
		public static Control ControlAtPoint(Point pt)
		{
			return Control.FromChildHandle(NativeMethods.WindowFromPoint(pt));
		}
		public static uint MakeLong(int low, int high)
		{
			return (uint)((high << 16) + low);
		}
	}
}
