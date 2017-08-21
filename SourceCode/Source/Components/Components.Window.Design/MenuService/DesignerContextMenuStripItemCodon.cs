/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Controls.Extensions;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class DesignerContextMenuStripItemCodon : ToolStripMenuItemCodon
    {
        private AbstractContextMenuCommand _contextMenuCommand;
        public AbstractContextMenuCommand ContextMenuCommand
        {
            get { return _contextMenuCommand; }
        }
        private DesignerVerb _designerVerb;
        public DesignerContextMenuStripItemCodon(AbstractContextMenuCommand contextMenuCommand)
            : this(contextMenuCommand, null)
        {
        }
        public DesignerContextMenuStripItemCodon(AbstractContextMenuCommand contextMenuCommand, Image image)
            : base(contextMenuCommand.Text, contextMenuCommand.Text, image)
        {
            _contextMenuCommand = contextMenuCommand;
            Init();
        }
        public DesignerContextMenuStripItemCodon(DesignerVerb verb)
            : base(verb.Text, verb.Text)
        {
            _designerVerb = verb;
            Init();
        }
        private void Init()
        {
            this.Action =
                delegate(object sender, ToolStripItemCodonEventArgs codon)
                {
                    if (_contextMenuCommand != null)
                    {
                        _contextMenuCommand.Run();
                    }
                    else if (_designerVerb != null)
                    {
                        _designerVerb.Invoke();
                    }
                };
        }
    }
}
