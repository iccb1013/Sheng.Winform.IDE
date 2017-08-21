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
using Sheng.SailingEase.Controls.Extensions;
namespace Sheng.SailingEase.Shell.View
{
    class MainToolStripPanelPresenter
    {
       
        private MainToolStripPanel _toolStripPanel;
        private Dictionary<ToolStripCodon, bool> _items = new Dictionary<ToolStripCodon, bool>();
        public MainToolStripPanelPresenter(MainToolStripPanel toolStripPanel)
        {
            _toolStripPanel = toolStripPanel;
        }
        public void ActivateToolStrip(ToolStripCodon toolStripCodon)
        {
            if (toolStripCodon == null)
                return;
            if (_items.Keys.Contains(toolStripCodon) == false)
                _items.Add(toolStripCodon, true);
            else
                _items[toolStripCodon] = true;
            toolStripCodon.View.Visible = true;
            ToolStripManager.Merge(toolStripCodon.View, _toolStripPanel.GetToolStrip("Main"));
            UpdateStatus();
        }
        public void DeactiveToolStrip(ToolStripCodon toolStripCodon)
        {
            if (toolStripCodon == null || toolStripCodon.View == null)
                return;
            if (_items.Keys.Contains(toolStripCodon) == false)
                return;
            toolStripCodon.View.Visible = false;
            ToolStripManager.RevertMerge(_toolStripPanel.GetToolStrip("Main"), toolStripCodon.View);
            _items[toolStripCodon] = false;
        }
        public void DestroyToolStrip(List<ToolStripCodon> toolStripCodonList)
        {
            if (toolStripCodonList != null && toolStripCodonList.Count > 0)
            {
                foreach (ToolStripCodon toolStripCodon in toolStripCodonList)
                {
                    DeactiveToolStrip(toolStripCodon);
                    if (_items.Keys.Contains(toolStripCodon))
                        _items.Remove(toolStripCodon);
                    toolStripCodon.View.Dispose();
                }
            }
        }
        public void UpdateStatus()
        {
            _toolStripPanel.UpdateStatus();
            foreach (ToolStripCodon toolStripCodon in _items.Keys)
            {
                if (toolStripCodon.View.IsDisposed)
                    continue;
                toolStripCodon.View.UpdateStatus();
                if (_items[toolStripCodon] == true)
                {
                    if (toolStripCodon.View.Visible == false)
                    {
                        ToolStripManager.RevertMerge(_toolStripPanel.GetToolStrip("Main"), toolStripCodon.View);
                    }
                    else
                    {
                        ToolStripManager.Merge(toolStripCodon.View, _toolStripPanel.GetToolStrip("Main"));
                    }
                }
            }
        }
        public void UpdateToolStrip(List<ToolStripCodon> toolStripCodonList)
        {
            UpdateStatus();
        }
    }
}
