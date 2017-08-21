using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    /// <summary>
    /// 工具盒管理器
    /// </summary>
    public class ToolboxManager
    {
        //Toolbox m_toolbox = null;

        ToolboxTabCollection _toolboxTabCollection = new ToolboxTabCollection();

        public ToolboxTabCollection ToolboxTabCollection
        {
            get
            {
                return this._toolboxTabCollection;
            }
        }

        private Type[] windowsFormsToolTypes = new Type[] {
			typeof(System.Windows.Forms.PropertyGrid), typeof(System.Windows.Forms.Label), typeof(System.Windows.Forms.LinkLabel), typeof(System.Windows.Forms.Button), typeof(System.Windows.Forms.TextBox), typeof(System.Windows.Forms.CheckBox), typeof(System.Windows.Forms.RadioButton), typeof(System.Windows.Forms.GroupBox), typeof(System.Windows.Forms.PictureBox), typeof(System.Windows.Forms.Panel), typeof(System.Windows.Forms.DataGrid), typeof(System.Windows.Forms.ListBox), typeof(System.Windows.Forms.CheckedListBox), typeof(System.Windows.Forms.ComboBox), typeof(System.Windows.Forms.ListView), typeof(System.Windows.Forms.TreeView), typeof(System.Windows.Forms.TabControl), typeof(System.Windows.Forms.DateTimePicker), typeof(System.Windows.Forms.MonthCalendar), typeof(System.Windows.Forms.HScrollBar), typeof(System.Windows.Forms.VScrollBar), typeof(System.Windows.Forms.Timer), typeof(System.Windows.Forms.Splitter), typeof(System.Windows.Forms.DomainUpDown), typeof(System.Windows.Forms.NumericUpDown), typeof(System.Windows.Forms.TrackBar), typeof(System.Windows.Forms.ProgressBar), typeof(System.Windows.Forms.RichTextBox), typeof(System.Windows.Forms.ImageList), typeof(System.Windows.Forms.HelpProvider), typeof(System.Windows.Forms.ToolTip), typeof(System.Windows.Forms.ToolBar), typeof(System.Windows.Forms.StatusBar), typeof(System.Windows.Forms.UserControl), typeof(System.Windows.Forms.NotifyIcon), typeof(System.Windows.Forms.OpenFileDialog), typeof(System.Windows.Forms.SaveFileDialog), typeof(System.Windows.Forms.FontDialog), typeof(System.Windows.Forms.ColorDialog), typeof(System.Windows.Forms.PrintDialog), typeof(System.Windows.Forms.PrintPreviewDialog), typeof(System.Windows.Forms.PrintPreviewControl), typeof(System.Windows.Forms.ErrorProvider), typeof(System.Drawing.Printing.PrintDocument), typeof(System.Windows.Forms.PageSetupDialog)
		};
        private Type[] componentsToolTypes = new Type[] {
			typeof(System.IO.FileSystemWatcher), typeof(System.Diagnostics.Process), typeof(System.Timers.Timer)
		};
        private Type[] dataToolTypes = new Type[] {
			typeof(System.Data.OleDb.OleDbCommandBuilder), typeof(System.Data.OleDb.OleDbConnection), typeof(System.Data.SqlClient.SqlCommandBuilder), typeof(System.Data.SqlClient.SqlConnection),
		};
        private Type[] userControlsToolTypes = new Type[] {
			typeof(System.Windows.Forms.UserControl)
		};

        public ToolboxManager()
        {
            //m_toolbox = toolbox;
        }

        //public ToolboxTabCollection PopulateToolboxInfo()
        //{
        //    try
        //    {
        //        if (Toolbox.FilePath == null || Toolbox.FilePath == "" || Toolbox.FilePath == String.Empty)
        //            return PopulateToolboxTabs();

        //        XmlDocument xmlDocument = new XmlDocument();
        //        xmlDocument.Load(Toolbox.FilePath);
        //        return PopulateToolboxTabs(xmlDocument);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error occured in reading Toolbox.xml file.\n" + ex.ToString());
        //        return null;
        //    }
        //}

        //private Toolbox Toolbox
        //{
        //    get
        //    {
        //        return m_toolbox;
        //    }
        //}

        public ToolboxTabCollection PopulateToolboxTabs()
        {
            ToolboxTabCollection toolboxTabs = new ToolboxTabCollection();
            string[] tabNames = { Strings.WindowsForms, Strings.Components, Strings.Data, Strings.UserControls };

            for (int i = 0; i < tabNames.Length; i++)
            {
                ToolboxTab toolboxTab = new ToolboxTab();

                toolboxTab.Name = tabNames[i];
                PopulateToolboxItems(toolboxTab);
                toolboxTabs.Add(toolboxTab);
            }

            return toolboxTabs;
        }

        private void PopulateToolboxItems(ToolboxTab toolboxTab)
        {
            if (toolboxTab == null)
                return;

            Type[] typeArray = null;

            switch (toolboxTab.Name)
            {
                case Strings.WindowsForms:
                    typeArray = windowsFormsToolTypes;
                    break;
                case Strings.Components:
                    typeArray = componentsToolTypes;
                    break;
                case Strings.Data:
                    typeArray = dataToolTypes;
                    break;
                case Strings.UserControls:
                    typeArray = userControlsToolTypes;
                    break;
                default:
                    break;
            }

            ToolboxItemCollection toolboxItems = new ToolboxItemCollection();

            for (int i = 0; i < typeArray.Length; i++)
            {
                ToolboxItem toolboxItem = new ToolboxItem();

                toolboxItem.Type = typeArray[i];
                toolboxItem.Name = typeArray[i].Name;
                toolboxItems.Add(toolboxItem);
            }

            toolboxTab.ToolboxItems = toolboxItems;
        }

        //private ToolboxTabCollection PopulateToolboxTabs(XmlDocument xmlDocument)
        //{
        //    if (xmlDocument == null)
        //        return null;

        //    XmlNode toolboxNode = xmlDocument.FirstChild;
        //    if (toolboxNode == null)
        //        return null;

        //    XmlNode tabCollectionNode = toolboxNode.FirstChild;
        //    if (tabCollectionNode == null)
        //        return null;

        //    XmlNodeList tabsNodeList = tabCollectionNode.ChildNodes;
        //    if (tabsNodeList == null)
        //        return null;

        //    ToolboxTabCollection toolboxTabs = new ToolboxTabCollection();

        //    foreach (XmlNode tabNode in tabsNodeList)
        //    {
        //        if (tabNode == null)
        //            continue;

        //        XmlNode propertiesNode = tabNode.FirstChild;
        //        if (propertiesNode == null)
        //            continue;

        //        XmlNode nameNode = propertiesNode[Strings.Name];
        //        if (nameNode == null)
        //            continue;

        //        ToolboxTab toolboxTab = new ToolboxTab();
        //        toolboxTab.Name = nameNode.InnerXml.ToString();
        //        PopulateToolboxItems(tabNode, toolboxTab);
        //        toolboxTabs.Add(toolboxTab);
        //    }
        //    if (toolboxTabs.Count == 0)
        //        return null;

        //    return toolboxTabs;
        //}

        //private void PopulateToolboxItems(XmlNode tabNode, ToolboxTab toolboxTab)
        //{
        //    if (tabNode == null)
        //        return;

        //    XmlNode toolboxItemCollectionNode = tabNode[Strings.ToolboxItemCollection];
        //    if (toolboxItemCollectionNode == null)
        //        return;

        //    XmlNodeList toolboxItemNodeList = toolboxItemCollectionNode.ChildNodes;
        //    if (toolboxItemNodeList == null)
        //        return;

        //    ToolboxItemCollection toolboxItems = new ToolboxItemCollection();

        //    foreach (XmlNode toolboxItemNode in toolboxItemNodeList)
        //    {
        //        if (toolboxItemNode == null)
        //            continue;

        //        XmlNode typeNode = toolboxItemNode[Strings.Type];
        //        if (typeNode == null)
        //            continue;

        //        bool found = false;
        //        System.Reflection.Assembly[] loadedAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        //        for (int i = 0; i < loadedAssemblies.Length && !found; i++)
        //        {
        //            System.Reflection.Assembly assembly = loadedAssemblies[i];
        //            System.Type[] types = assembly.GetTypes();
        //            for (int j = 0; j < types.Length && !found; j++)
        //            {
        //                System.Type type = types[j];
        //                if (type.FullName == typeNode.InnerXml.ToString())
        //                {
        //                    ToolboxItem toolboxItem = new ToolboxItem();
        //                    toolboxItem.Type = type;
        //                    toolboxItems.Add(toolboxItem);
        //                    found = true;
        //                }
        //            }
        //        }
        //    }
        //    toolboxTab.ToolboxItems = toolboxItems;
        //    return;
        //}

        private class Strings
        {
            public const string Toolbox = "Toolbox";
            public const string TabCollection = "TabCollection";
            public const string Tab = "Tab";
            public const string Properties = "Properties";
            public const string Name = "Name";
            public const string ToolboxItemCollection = "ToolboxItemCollection";
            public const string ToolboxItem = "ToolboxItem";
            public const string Type = "Type";
            public const string WindowsForms = "Windows Forms";
            public const string Components = "Components";
            public const string Data = "Data";
            public const string UserControls = "User Controls";
        }

        /// <summary>
        /// 添加一个工具盒分组
        /// </summary>
        /// <param name="name"></param>
        public void AddToolBoxTab(string name)
        {
            if (_toolboxTabCollection[name] != null)
                return;

            ToolboxTab toolboxTab = new ToolboxTab();
            toolboxTab.Name = name;
            toolboxTab.ToolboxItems = new ToolboxItemCollection();
            // PopulateToolboxItems(toolboxTab);
            this._toolboxTabCollection.Add(toolboxTab);
        }

        /// <summary>
        /// 增加一个工具栏项目
        /// </summary>
        /// <param name="tabIndex"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public void AddToolBoxItem(int tabIndex, Type type,string displayName)
        {
            ToolboxItem toolboxItem = new ToolboxItem();
            toolboxItem.Name = displayName;
            toolboxItem.Type = type;
            this._toolboxTabCollection[tabIndex].ToolboxItems.Add(toolboxItem);
        }

        public void AddToolBoxItem(string tabName, Type type, string displayName)
        {
            if (_toolboxTabCollection[tabName] == null)
            {
                AddToolBoxTab(tabName);
            }

            ToolboxItem toolboxItem = new ToolboxItem();
            toolboxItem.Name = displayName;
            toolboxItem.Type = type;
            this._toolboxTabCollection[tabName].ToolboxItems.Add(toolboxItem);
        }
    }
}
