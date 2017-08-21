/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.TextEditor.Util
{
	class MouseWheelHandler
	{
		const int WHEEL_DELTA = 120;
		int mouseWheelDelta;
		public int GetScrollAmount(MouseEventArgs e)
		{
			mouseWheelDelta += e.Delta;
			int linesPerClick = Math.Max(SystemInformation.MouseWheelScrollLines, 1);
			int scrollDistance = mouseWheelDelta * linesPerClick / WHEEL_DELTA;
			mouseWheelDelta %= Math.Max(1, WHEEL_DELTA / linesPerClick);
			return scrollDistance;
		}
	}
}
