/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
namespace Sheng.SailingEase.Controls.PopupControl
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ComboBox)), ToolboxItem(true), ToolboxItemFilter("System.Windows.Forms"), Description("Displays an editable text box with a drop-down list of permitted values.")]
    public partial class PopupComboBox : PopupControlComboBoxBase
    {
        public PopupComboBox()
        {
            this.dropDownHideTime = DateTime.UtcNow;
            InitializeComponent();
            base.DropDownHeight = base.DropDownWidth = 1;
            base.IntegralHeight = false;
        }
        private Popup dropDown;
        private Control dropDownControl;
        public Control DropDownControl
        {
            get
            {
                return dropDownControl;
            }
            set
            {
                if (dropDownControl == value)
                {
                    return;
                }
                dropDownControl = value;
                if (dropDown != null)
                {
                    dropDown.Closed -= dropDown_Closed;
                    dropDown.Dispose();
                }
                dropDown = new Popup(value);
                dropDown.Closed += dropDown_Closed;
            }
        }
        private DateTime dropDownHideTime;
        private void dropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            dropDownHideTime = DateTime.UtcNow;
        }
        public new bool DroppedDown
        {
            get
            {
                return dropDown.Visible;
            }
            set
            {
                if (DroppedDown)
                {
                    HideDropDown();
                }
                else
                {
                    ShowDropDown();
                }
            }
        }
        public new event EventHandler DropDown;
        public void ShowDropDown()
        {
            if (dropDown != null)
            {
                if ((DateTime.UtcNow - dropDownHideTime).TotalSeconds > 0.5)
                {
                    if (DropDown != null)
                    {
                        DropDown(this, EventArgs.Empty);
                    }
                    dropDown.Show(this);
                }
                else
                {
                    dropDownHideTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 1));
                    Focus();
                }
            }
        }
        public new event EventHandler DropDownClosed;
        public void HideDropDown()
        {
            if (dropDown != null)
            {
                dropDown.Hide();
                if (DropDownClosed != null)
                {
                    DropDownClosed(this, EventArgs.Empty);
                }
            }
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (NativeMethods.WM_COMMAND + NativeMethods.WM_REFLECT) && NativeMethods.HIWORD(m.WParam) == NativeMethods.CBN_DROPDOWN)
            {
                ShowDropDown();
                return;
            }
            base.WndProc(ref m);
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int DropDownWidth
        {
            get { return base.DropDownWidth; }
            set { base.DropDownWidth = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int DropDownHeight
        {
            get { return base.DropDownHeight; }
            set { base.DropDownHeight = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool IntegralHeight
        {
            get { return base.IntegralHeight; }
            set { base.IntegralHeight = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new ObjectCollection Items
        {
            get { return base.Items; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int ItemHeight
        {
            get { return base.ItemHeight; }
            set { base.ItemHeight = value; }
        }
    }
}
