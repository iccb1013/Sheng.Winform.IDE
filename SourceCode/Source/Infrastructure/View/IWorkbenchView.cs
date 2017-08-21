/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Controls.Docking;
namespace Sheng.SailingEase.Infrastructure
{
    public interface IWorkbenchView : IView
    {
        List<ToolStripCodon> ToolStripList { get; }
        ContextMenuStrip TabPageContextMenuStrip { get; set; }
    }
}
