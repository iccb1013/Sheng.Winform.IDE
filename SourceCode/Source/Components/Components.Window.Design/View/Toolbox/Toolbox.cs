using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    public partial class Toolbox : UserControl, IToolboxService
    {
        private IDesignerHost _designerHost = null;
        public IDesignerHost DesignerHost
        {
            set
            {
                _designerHost = value;
            }
            get
            {
                return _designerHost;
            }
        }

        private System.Drawing.Design.ToolboxItem _selectedToolboxItem;
        /// <summary>
        /// 当前选中的工具栏项目
        /// </summary>
        public System.Drawing.Design.ToolboxItem SelectedToolboxItem
        {
            get
            {
                return this._selectedToolboxItem;
            }
            set
            {
                this._selectedToolboxItem = value;
            }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private ToolboxTabCollection m_ToolboxTabCollection = null;
        /// <summary>
        /// 工具盒分组集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolboxTabCollection Tabs
        {
            get
            {
                return m_ToolboxTabCollection;
            }
            set
            {
                m_ToolboxTabCollection = value;
            }
        }

        //private string m_filePath = null;

        private ToolboxTabButton[] m_tabPageArray = null;
        /// <summary>
        /// 工具盒项目分组按钮数组
        /// </summary>
        internal ToolboxTabButton[] TabPageArray
        {
            get
            {
                return m_tabPageArray;
            }
            set
            {
                m_tabPageArray = value;
            }
        }

        private ToolboxItemContainer _activeToolboxItemContainer = null;
        /// <summary>
        /// 当前活动工具盒项目容器
        /// </summary>
        public ToolboxItemContainer ActiveToolboxItemContainer
        {
            get
            {
                return this._activeToolboxItemContainer;
            }
            set
            {
                this._activeToolboxItemContainer = value;
            }
        }

        private ToolboxItemContainer[] _toolboxItemContainer = null;
        /// <summary>
        /// 工具盒项目容器数组
        /// </summary>
        internal ToolboxItemContainer[] ToolboxItemContainer
        {
            get
            {
                return _toolboxItemContainer;
            }
            set
            {
                _toolboxItemContainer = value;
            }
        }

        public Toolbox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置 ToolboxManager
        /// </summary>
        public ToolboxManager ToolboxManager
        {
            set
            {
                Tabs = value.ToolboxTabCollection;

                ToolboxUIManager toolboxUIManagerVS = new ToolboxUIManager(this);
                toolboxUIManagerVS.FillToolbox();
            }
        }

        public void ToolBoxItemButtonMouseDown(System.Windows.Forms.MouseEventArgs e, System.Drawing.Design.ToolboxItem toolboxItem)
        {
            if (toolboxItem == null)
                return;

            if (this.DesignerHost == null)
                return;

            if (e.Clicks == 2)
            {
                IDesignerHost idh = (IDesignerHost)this.DesignerHost.GetService(typeof(IDesignerHost));
                IToolboxUser tbu = idh.GetDesigner(idh.RootComponent as IComponent) as IToolboxUser;

                if (tbu != null)
                {
                    tbu.ToolPicked(toolboxItem);
                }
            }
            else if (e.Clicks < 2)
            {
                IToolboxService tbs = this;

                // The IToolboxService serializes ToolboxItems by packaging them in DataObjects.
                DataObject d = tbs.SerializeToolboxItem(toolboxItem) as DataObject;

                try
                {
                    this.DoDragDrop(d, DragDropEffects.Copy);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void ToolBoxItemButtonKeyDown(System.Windows.Forms.KeyEventArgs e, System.Drawing.Design.ToolboxItem toolboxItem)
        {
            if (toolboxItem == null)
            {
                return;
            }


            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (DesignerHost == null)
                        MessageBox.Show("idh Null");

                    IToolboxUser tbu = DesignerHost.GetDesigner(DesignerHost.RootComponent as IComponent) as IToolboxUser;

                    if (tbu != null)
                    {
                        // Enter means place the tool with default location and default size.
                        tbu.ToolPicked(toolboxItem);
                    }
                    break;

                default:
                    {
                        Console.WriteLine("Error: Not able to add");
                        break;
                    }
            }
        }


        #region IToolboxService Members

        // We only implement what is really essential for ToolboxService

        /// <summary>
        /// 选取当前选中的工具盒项目
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public System.Drawing.Design.ToolboxItem GetSelectedToolboxItem(IDesignerHost host)
        {
            /*
            ListBox list = this.ToolsListBox;
            System.Drawing.Design.ToolboxItem tbi = (System.Drawing.Design.ToolboxItem)list.Items[selectedIndex];
            if (tbi.DisplayName != "<Pointer>")
                return tbi;
            else
                return null;
             */

            //如果TypeName为空说明选中的是"指针"
            if (this.SelectedToolboxItem != null &&
                this.SelectedToolboxItem.TypeName != String.Empty)
            {
                return this.SelectedToolboxItem;
            }
            else
            {
                return null;
            }
        }

        public System.Drawing.Design.ToolboxItem GetSelectedToolboxItem()
        {
            return this.GetSelectedToolboxItem(null);
        }

        public void AddToolboxItem(System.Drawing.Design.ToolboxItem toolboxItem, string category)
        {
        }

        public void AddToolboxItem(System.Drawing.Design.ToolboxItem toolboxItem)
        {
        }

        public bool IsToolboxItem(object serializedObject, IDesignerHost host)
        {
            return false;
        }

        public bool IsToolboxItem(object serializedObject)
        {
            return false;
        }

        public void SetSelectedToolboxItem(System.Drawing.Design.ToolboxItem toolboxItem)
        {
        }

        /// <summary>
        /// 工具盒项目已被使用
        /// </summary>
        public void SelectedToolboxItemUsed()
        {
            //ListBox list = this.ToolsListBox;

            //list.Invalidate(list.GetItemRectangle(selectedIndex));
            //selectedIndex = 0;
            //list.SelectedIndex = 0;
            //list.Invalidate(list.GetItemRectangle(selectedIndex));

            //将选中的项重新置于"指针"上
            this._activeToolboxItemContainer.SelectedToolboxItemUsed();

        }

        public CategoryNameCollection CategoryNames
        {
            get
            {
                return null;
            }
        }

        void IToolboxService.Refresh()
        {
        }

        public void AddLinkedToolboxItem(System.Drawing.Design.ToolboxItem toolboxItem, string category, IDesignerHost host)
        {
        }

        public void AddLinkedToolboxItem(System.Drawing.Design.ToolboxItem toolboxItem, IDesignerHost host)
        {
        }

        public bool IsSupported(object serializedObject, ICollection filterAttributes)
        {
            return false;
        }

        public bool IsSupported(object serializedObject, IDesignerHost host)
        {
            return false;
        }

        public string SelectedCategory
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public System.Drawing.Design.ToolboxItem DeserializeToolboxItem(object serializedObject, IDesignerHost host)
        {
           

            DataObject dataObject = serializedObject as DataObject;
            if (dataObject != null)
            {
                return (System.Drawing.Design.ToolboxItem)dataObject.GetData(typeof(System.Drawing.Design.ToolboxItem));
            }
            else
            {
                return null;
            }
        }

        public System.Drawing.Design.ToolboxItem DeserializeToolboxItem(object serializedObject)
        {
            return this.DeserializeToolboxItem(serializedObject, this.DesignerHost);
        }

        public System.Drawing.Design.ToolboxItemCollection GetToolboxItems(string category, IDesignerHost host)
        {
            return null;
        }

        public System.Drawing.Design.ToolboxItemCollection GetToolboxItems(string category)
        {
            return null;
        }

        public System.Drawing.Design.ToolboxItemCollection GetToolboxItems(IDesignerHost host)
        {
            return null;
        }

        public System.Drawing.Design.ToolboxItemCollection GetToolboxItems()
        {
            return null;
        }

        public void AddCreator(ToolboxItemCreatorCallback creator, string format, IDesignerHost host)
        {
        }

        public void AddCreator(ToolboxItemCreatorCallback creator, string format)
        {
        }

        public bool SetCursor()
        {
            return false;
        }

        public void RemoveToolboxItem(System.Drawing.Design.ToolboxItem toolboxItem, string category)
        {
        }

        public void RemoveToolboxItem(System.Drawing.Design.ToolboxItem toolboxItem)
        {
        }

        public object SerializeToolboxItem(System.Drawing.Design.ToolboxItem toolboxItem)
        {
            return new DataObject(toolboxItem);
        }

        public void RemoveCreator(string format, IDesignerHost host)
        {
        }

        public void RemoveCreator(string format)
        {
        }

        #endregion



    }
}
