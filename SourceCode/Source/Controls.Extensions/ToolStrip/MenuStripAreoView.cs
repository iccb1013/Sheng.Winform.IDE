/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;
namespace Sheng.SailingEase.Controls.Extensions
{
    public class MenuStripAreoView : SEAreoMainMenuStrip
    {
        public MenuStripCodon Codon
        {
            get;
            private set;
        }
        public MenuStripAreoView(MenuStripCodon codon)
        {
            Codon = codon;
        }
        public void RegisterItem(string path, IToolStripItemCodon toolStripItem)
        {
            RegisterItem(new ToolStripPath(path), toolStripItem);
        }
        public void RegisterItem(ToolStripPath path, IToolStripItemCodon toolStripItem)
        {
            ToolStripItem item = (ToolStripItem)toolStripItem.View;
            if (path.PathPoints.Count == 1)
            {
                if (path.PathPoints[0].Index.HasValue)
                {
                    int index = path.PathPoints[0].Index.Value;
                    if (index >= this.Items.Count)
                    {
                        index = this.Items.Count;
                    }
                    this.Items.Insert(index, item);
                }
                else
                {
                    this.Items.Add(item);
                }
            }
            else
            {
                ToolStripMenuItemView targetItem = null;
                ToolStripItemCollection targetItemCollection = this.Items;
                for (int i = 1; i < path.PathPoints.Count; i++)
                {
                    foreach (ToolStripMenuItemView targetItemDoozer in targetItemCollection)
                    {
                        IToolStripItemView doozer = targetItemDoozer as IToolStripItemView;
                        if (doozer == null)
                            continue;
                        if (doozer.Codon.PathPoint == path.PathPoints[i].Name)
                        {
                            targetItem = targetItemDoozer;
                            targetItemCollection = targetItemDoozer.DropDownItems;
                            break;
                        }
                    }
                }
                if (targetItem != null)
                {
                    if (path.PathPoints[path.PathPoints.Count - 1].Index.HasValue)
                    {
                        int index = path.PathPoints[path.PathPoints.Count - 1].Index.Value;
                        if (index >= targetItem.DropDownItems.Count)
                        {
                            index = targetItem.DropDownItems.Count;
                        }
                        targetItem.DropDownItems.Insert(index, item);
                    }
                    else
                    {
                        targetItem.DropDownItems.Add(item);
                    }
                }
            }
            if (ItemRegister != null)
            {
                ItemRegister(new ItemRegisterEventArgs(path,toolStripItem,item));
            }
        }
        public void UpdateStatus()
        {
            IToolStripItemView view;
            foreach (System.Windows.Forms.ToolStripItem item in this.Items)
            {
                view = item as IToolStripItemView;
                if (view != null)
                    view.UpdateSataus();
            }
        }
        public delegate void OnItemRegisterHandler(ItemRegisterEventArgs e);
        public event OnItemRegisterHandler ItemRegister;
        public class ItemRegisterEventArgs
        {
            public ToolStripPath Path { get; private set; }
            public IToolStripItemCodon Codon { get; private set; }
            public ToolStripItem Item { get; private set; }
            public ItemRegisterEventArgs(ToolStripPath path, IToolStripItemCodon codon, ToolStripItem item)
            {
                Path = path;
                Codon = codon;
                Item = item;
            }
        }
    }
}
