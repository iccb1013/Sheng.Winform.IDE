/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.Docking
{
	internal class DragForm : Form
	{
		public DragForm()
		{
			FormBorderStyle = FormBorderStyle.None;
			ShowInTaskbar = false;
			SetStyle(ControlStyles.Selectable, false);
			Enabled = false;
		}
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= (int)Win32.WindowExStyles.WS_EX_TOOLWINDOW;
				return createParams;
			}
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == (int)Win32.Msgs.WM_NCHITTEST)
			{
				m.Result = (IntPtr)Win32.HitTest.HTTRANSPARENT;
				return;
			}
			base.WndProc (ref m);
		}
		public virtual void Show(bool bActivate)
		{
			if (bActivate)
				Show();
			else
				NativeMethods.ShowWindow(Handle, (int)Win32.ShowWindowStyles.SW_SHOWNOACTIVATE);
		}
	}
}
