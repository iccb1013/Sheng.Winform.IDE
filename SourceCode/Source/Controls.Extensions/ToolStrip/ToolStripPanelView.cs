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
using System.Windows.Forms.Layout;
using System.Drawing;
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ToolStripPanelView : ToolStripPanel
    {
        public ToolStripView GetToolStrip(string pathPoint)
        {
            ToolStripView toolStripDoozer;
            foreach (Control control in this.Controls)
            {
                toolStripDoozer = control as ToolStripView;
                if (toolStripDoozer == null)
                    continue;
                if (toolStripDoozer.Codon.PathPoint.Equals(pathPoint))
                {
                    return toolStripDoozer;
                }
            }
            return null;
        }
        public ToolStripView CreateToolStrip(string pathPoint)
        {
            if (String.IsNullOrEmpty(pathPoint))
                throw new ArgumentException("CreateToolStrip Error");
            ToolStripView toolStripDoozer = new ToolStripView(new ToolStripCodon(pathPoint));
            this.Controls.Add(toolStripDoozer);
            return toolStripDoozer;
        }
        public ToolStripView CreateToolStrip(ToolStripCodon toolStripCodon)
        {
            if (toolStripCodon == null)
                throw new ArgumentException("CreateToolStrip Error");
            ToolStripView toolStripDoozer = new ToolStripView(toolStripCodon);
            this.Controls.Add(toolStripDoozer);
            return toolStripDoozer;
        }
        public void UpdateStatus()
        {
            ToolStripView toolStripDoozer;
            foreach (Control control in this.Controls)
            {
                toolStripDoozer = control as ToolStripView;
                if (toolStripDoozer == null)
                    continue;
                toolStripDoozer.UpdateStatus();
            }
        }
        public void RegisterItem(string path, IToolStripItemCodon toolStripItem)
        {
            RegisterItem(new ToolStripPath(path), toolStripItem);
        }
        public void RegisterItem(ToolStripPath path, IToolStripItemCodon toolStripItem)
        {
            ToolStripView toolStrip = this.GetToolStrip(path.PathPoints[0].Name);
            if (toolStrip == null && path.PathPoints.Count == 1)
            {
                toolStrip = this.CreateToolStrip(path.PathPoints[0].Name);
            }
            toolStrip.RegisterItem(path.Cut(0), toolStripItem);
        }
    }
}
