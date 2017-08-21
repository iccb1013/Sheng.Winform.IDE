/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Sheng.SailingEase.Controls.MozBar
{
	public delegate void MozScrollEventHandler(object sender, MozScrollEventArgs e);
	[StructLayout(LayoutKind.Sequential)]
	public struct SCROLLINFO
	{
		public int cbSize;
		public int fMask;
		public int nMin;
		public int nMax;
		public int nPage;
		public int nPos;
		public int nTrackPos;
	}
	[ToolboxItem(false)]
	public class ScrollableControlWithScrollEvents : ScrollableControl
	{
		private const int WS_HSCROLL = 0x100000;
		private const int WM_HSCROLL = 0x114;
		private const int WM_VSCROLL = 0x115;
		private const int SB_HORZ = 0;
		private const int SB_VERT = 1;
		private const int SIF_RANGE =0x1;
		private const int SIF_PAGE = 0x2;
		private const int SIF_POS = 0x4;
		private const int SIF_DISABLENOSCROLL = 0x8;
		private const int SIF_TRACKPOS = 0x10;
		private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_DISABLENOSCROLL | SIF_TRACKPOS;
		[DllImport("User32", EntryPoint="GetScrollInfo")]
		private static extern bool GetScrollInfo (IntPtr hWnd, int fnBar, ref SCROLLINFO info);
		[Browsable(true)]
		[Description("Indicates that the control has been scrolled horizontally.")]
		[Category("Panel")]
		public new event MozScrollEventHandler HorizontalScroll;
		[Browsable(true)]
		[Description("Indicates that the control has been scrolled vertically.")]
		[Category("Panel")]
		public new event MozScrollEventHandler VerticalScroll;
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams p = base.CreateParams;
				return p; 
			}
		}
		protected override void WndProc(ref Message m)
		{
			base.WndProc (ref m);
			if ( m.Msg == WM_HSCROLL ) 
			{
				if ( HorizontalScroll != null ) 
				{
					uint wParam = (uint)m.WParam.ToInt32();
					SCROLLINFO si = new SCROLLINFO();
					si.cbSize = Marshal.SizeOf(si);
					si.fMask = SIF_ALL;
					bool ret = GetScrollInfo(this.Handle,SB_HORZ,ref si);
					HorizontalScroll( this, 
						new MozScrollEventArgs( 
							GetEventType( wParam & 0xffff), (int)(wParam >> 16),si ) );
				}
			} 
			else if ( m.Msg == WM_VSCROLL )
			{
				if ( VerticalScroll != null )
				{
					uint wParam = (uint)m.WParam.ToInt32();
					SCROLLINFO si = new SCROLLINFO();
					si.cbSize = Marshal.SizeOf(si);
					si.fMask = SIF_ALL;
					bool ret = GetScrollInfo(this.Handle,SB_VERT,ref si);
					VerticalScroll( this, 
						new MozScrollEventArgs( 
						GetEventType( wParam & 0xffff), (int)(wParam >> 16),si ) );
				}
			}
		}
		private static ScrollEventType [] _events =
			new ScrollEventType[] {
									  ScrollEventType.SmallDecrement,
									  ScrollEventType.SmallIncrement,
									  ScrollEventType.LargeDecrement,
									  ScrollEventType.LargeIncrement,
									  ScrollEventType.ThumbPosition,
									  ScrollEventType.ThumbTrack,
									  ScrollEventType.First,
									  ScrollEventType.Last,
									  ScrollEventType.EndScroll
								  };
		private ScrollEventType GetEventType( uint wParam )
		{
			if ( wParam < _events.Length )
				return _events[wParam];
			else
				return ScrollEventType.EndScroll;
		}
	}
	public class MozScrollEventArgs
	{
		private ScrollEventType m_type;
		private int m_newValue;
		private SCROLLINFO m_info;
		public MozScrollEventArgs(ScrollEventType type , int newValue, SCROLLINFO info)
		{
			m_type = type;
			m_newValue = newValue; 
			m_info = info;
		}
		public SCROLLINFO ScrollInfo
		{
			get
			{
				return this.m_info;
			}
		}
		public ScrollEventType Type
		{
			get
			{
				return this.m_type;
			}
		}
		public int NewValue
		{
			get
			{
				return this.m_newValue;
			}
		}
	}
}
