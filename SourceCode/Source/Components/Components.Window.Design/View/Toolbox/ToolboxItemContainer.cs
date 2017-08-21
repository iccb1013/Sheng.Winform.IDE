/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    public partial class ToolboxItemContainer : UserControl
    {
        private ToolboxItemButton _selectedToolboxItemButton = null;
        private System.Drawing.Design.ToolboxItem pointer;
        private System.Windows.Forms.ToolTip toolTip = new ToolTip();
        private List<ToolboxItemButton> toolboxItemButtonCollection = new List<ToolboxItemButton>();
        public List<ToolboxItemButton> ToolboxItemButtonCollection
        {
            get
            {
                return this.toolboxItemButtonCollection;
            }
        }
        private Toolbox _toolbox;
        public Toolbox Toolbox
        {
            get
            {
                return this._toolbox;
            }
            set
            {
                this._toolbox = value;
            }
        }
        private int _ItemHeight = 20;
        public int ItemHeight
        {
            get
            {
                return this._ItemHeight;
            }
            set
            {
                this._ItemHeight = value;
            }
        }
        public int Count
        {
            get
            {
                return this.ToolboxItems.Count;
            }
        }
        private ToolboxItemCollection m_toolboxItemCollection = null;
        public ToolboxItemCollection ToolboxItems
        {
            get
            {
                return m_toolboxItemCollection;
            }
            set
            {
                m_toolboxItemCollection = value;
                this.Height = (value.Count + 1) * this.ItemHeight; 
                DrawToolBoxItem();
                DrawToolBoxItem(pointer);
            }
        }
        public ToolboxItemContainer(Toolbox toolbox)
        {
            InitializeComponent();
            this.Toolbox = toolbox;
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            pointer = new System.Drawing.Design.ToolboxItem();
            pointer.DisplayName = "<指针>"; 
            pointer.Bitmap = IconsLibrary.Arrow;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.BackColor = System.Drawing.Color.White;
            this.toolTip.ForeColor = System.Drawing.Color.Black;
            this.toolTip.InitialDelay = 300;
            this.toolTip.ReshowDelay = 300;
        }
        private void ToolboxItemContainer_Load(object sender, EventArgs e)
        {
        }
        public void ClearToolBoxItem()
        {
            this.SuspendLayout();
            this.Controls.Clear();
            DrawToolBoxItem(pointer);
            this.ResumeLayout();
        }
        private void DrawToolBoxItem()
        {
            if (this.ToolboxItems == null)
                return;
            if (this.ToolboxItems.Count == 0)
            {
                return;
            }
            this.SuspendLayout();
            for (int i = this.ToolboxItems.Count - 1; i >= 0; i--)
            {
                Type type = this.ToolboxItems[i].Type;
                System.Drawing.Design.ToolboxItem tbi = new System.Drawing.Design.ToolboxItem(type);
                System.Drawing.ToolboxBitmapAttribute tba =
                    TypeDescriptor.GetAttributes(type)[typeof(System.Drawing.ToolboxBitmapAttribute)] as System.Drawing.ToolboxBitmapAttribute;
                if (tba != null)
                {
                    tbi.Bitmap = (System.Drawing.Bitmap)tba.GetImage(type);
                }
                DrawToolBoxItem(tbi, this.ToolboxItems[i].Name);
            }
            this.ResumeLayout();
        }
        private void DrawToolBoxItem(System.Drawing.Design.ToolboxItem toolboxItem)
        {
            DrawToolBoxItem(toolboxItem, String.Empty);
        }
        private void DrawToolBoxItem(System.Drawing.Design.ToolboxItem toolboxItem,string displayName)
        {
            if (displayName == String.Empty)
            {
                displayName = toolboxItem.DisplayName;
            }
            ToolboxItemButton toolboxButton = new ToolboxItemButton(toolboxItem, displayName);
            toolboxButton.Index = this.toolboxItemButtonCollection.Count;
            toolboxButton.Height = this.ItemHeight;
            toolboxButton.Dock = DockStyle.Top;
            this.toolTip.SetToolTip(toolboxButton, toolboxItem.Description);
            toolboxButton.MouseDown += new MouseEventHandler(toolboxButton_MouseDown);
            toolboxButton.KeyUp += new KeyEventHandler(toolboxButton_KeyUp);
            this.toolboxItemButtonCollection.Add(toolboxButton);
            this.Controls.Add(toolboxButton);
        }
        private void toolboxButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ToolboxItemButton toolboxItemButton = sender as ToolboxItemButton;
            this._selectedToolboxItemButton = toolboxItemButton;
            this.Toolbox.SelectedToolboxItem = toolboxItemButton.ToolboxItem;
            if (toolboxItemButton.ToolboxItem.TypeName != String.Empty)
            {
                this.Toolbox.ToolBoxItemButtonMouseDown(e, toolboxItemButton.ToolboxItem);
            }
        }
        private void toolboxButton_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void SelectedToolboxItemUsed()
        {
            
            foreach (ToolboxItemButton btn in this.toolboxItemButtonCollection)
            {
                if (btn.ToolboxItem == this.pointer)
                {
                    this.Toolbox.SelectedToolboxItem = pointer;
                    break;
                }
            }
        }
        private void ToolboxItemContainer_Enter(object sender, EventArgs e)
        {
            if (this.Toolbox != null)
            {
                this.Toolbox.ActiveToolboxItemContainer = this;
            }
        }
    }
}
