/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Windows.Forms;
using System.Drawing;
namespace Sheng.SailingEase.Controls.Extensions
{
    public delegate void OnToolStripItemCodonActionHandler(object sender, ToolStripItemCodonEventArgs e);
    public interface IToolStripItemCodon
    {
        IToolStripItemView View { get; }
        string Text { get; }
        string Description { get; set; }
        Image Image { get; set; }
        ToolStripCodonBase Owner { get; set; }
        string PathPoint { get; }
        ToolStripItemAlignment Alignment { get; set; }
        int? Width { get; set; }
        bool AutoSize { get; set; }
        bool AutoToolTip { get; set; }
        bool Available { get; set; }
        ToolStripItemDisplayStyle DisplayStyle { get; set; }
        OnToolStripItemCodonActionHandler Action { get; set; }
        Func<IToolStripItemCodon, bool> IsVisible { get; set; }
        Func<IToolStripItemCodon, bool> IsEnabled { get; set; }
        Func<string> GetText { get; set; }
        bool Visible { get; }
        bool Enabled { get; }
    }
}
