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
using System.Drawing.Drawing2D;
using System.ComponentModel;
namespace Sheng.SIMBE.SEControl.Docking
{
    partial class DockPanel
    {
        private abstract class DragHandlerBase : NativeWindow, IMessageFilter
        {
            protected DragHandlerBase()
            {
            }
            protected abstract Control DragControl
            {
                get;
            }
            private Point m_startMousePosition = Point.Empty;
            protected Point StartMousePosition
            {
                get { return m_startMousePosition; }
                private set { m_startMousePosition = value; }
            }
            protected bool BeginDrag()
            {
                lock (this)
                {
                    if (DragControl == null)
                        return false;
                    StartMousePosition = Control.MousePosition;
                    if (!NativeMethods.DragDetect(DragControl.Handle, StartMousePosition))
                        return false;
                    DragControl.FindForm().Capture = true;
                    AssignHandle(DragControl.FindForm().Handle);
                    Application.AddMessageFilter(this);
                    return true;
                }
            }
            protected abstract void OnDragging();
            protected abstract void OnEndDrag(bool abort);
            private void EndDrag(bool abort)
            {
                ReleaseHandle();
                Application.RemoveMessageFilter(this);
                DragControl.FindForm().Capture = false;
                OnEndDrag(abort);
            }
            bool IMessageFilter.PreFilterMessage(ref Message m)
            {
                if (m.Msg == (int)Win32.Msgs.WM_MOUSEMOVE)
                    OnDragging();
                else if (m.Msg == (int)Win32.Msgs.WM_LBUTTONUP)
                    EndDrag(false);
                else if (m.Msg == (int)Win32.Msgs.WM_CAPTURECHANGED)
                    EndDrag(true);
                else if (m.Msg == (int)Win32.Msgs.WM_KEYDOWN && (int)m.WParam == (int)Keys.Escape)
                    EndDrag(true);
                return OnPreFilterMessage(ref m);
            }
            protected virtual bool OnPreFilterMessage(ref Message m)
            {
                return false;
            }
            protected sealed override void WndProc(ref Message m)
            {
                if (m.Msg == (int)Win32.Msgs.WM_CANCELMODE || m.Msg == (int)Win32.Msgs.WM_CAPTURECHANGED)
                    EndDrag(true);
                base.WndProc(ref m);
            }
        }
        private abstract class DragHandler : DragHandlerBase
        {
            private DockPanel m_dockPanel;
            protected DragHandler(DockPanel dockPanel)
            {
                m_dockPanel = dockPanel;
            }
            public DockPanel DockPanel
            {
                get { return m_dockPanel; }
            }
            private IDragSource m_dragSource;
            protected IDragSource DragSource
            {
                get { return m_dragSource; }
                set { m_dragSource = value; }
            }
            protected sealed override Control DragControl
            {
                get { return DragSource == null ? null : DragSource.DragControl; }
            }
            protected sealed override bool OnPreFilterMessage(ref Message m)
            {
                if ((m.Msg == (int)Win32.Msgs.WM_KEYDOWN || m.Msg == (int)Win32.Msgs.WM_KEYUP) &&
                    ((int)m.WParam == (int)Keys.ControlKey || (int)m.WParam == (int)Keys.ShiftKey))
                    OnDragging();
                return base.OnPreFilterMessage(ref m);
            }
        }
    }
}
