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
namespace Sheng.SailingEase.Controls.Extensions
{
    public class MenuStripView : MenuStrip
    {
         public MenuStripCodon Codon
        {
            get;
            private set;
        }
         public MenuStripView(MenuStripCodon codon)
        {
            Codon = codon;
        }
        public void RegisterItem(ToolStripPath path, IToolStripItemCodon toolStripItem)
        {
            ToolStripItem view = (ToolStripItem)toolStripItem.View;
            if (path.PathPoints.Count == 1)
            {
                if (path.PathPoints[0].Index.HasValue)
                {
                    int index = path.PathPoints[0].Index.Value;
                    if (index >= this.Items.Count)
                    {
                        index = this.Items.Count;
                    }
                    this.Items.Insert(index, view);
                }
                else
                {
                    this.Items.Add(view);
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
                        targetItem.DropDownItems.Insert(index, view);
                    }
                    else
                    {
                        targetItem.DropDownItems.Add(view);
                    }
                }
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
    }
}
