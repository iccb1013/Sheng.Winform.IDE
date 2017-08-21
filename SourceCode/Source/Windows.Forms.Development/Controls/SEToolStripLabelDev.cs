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
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    class SEToolStripLabelDev : SEToolStripLabel
    {
        public SEToolStripLabelDev(ToolStripItemAbstract item)
        {
            ToolStripLabelEntity entity = (ToolStripLabelEntity)item;
            this.Text = entity.Text;
            this.Alignment = (ToolStripItemAlignment)Enum.Parse(typeof(ToolStripItemAlignment), ((int)entity.Aligment).ToString());
            this.ToolTipText = entity.ToolTipText;
        }
    }
}
