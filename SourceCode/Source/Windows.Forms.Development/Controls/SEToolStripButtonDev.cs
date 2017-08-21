/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Drawing;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    class SEToolStripButtonDev : SEToolStripButton
    {
        public SEToolStripButtonDev(ToolStripItemAbstract item)
        {
            ToolStripButtonEntity entity = (ToolStripButtonEntity)item;
            this.Text = entity.Text;
            this.DisplayStyle = (ToolStripItemDisplayStyle)Enum.Parse(typeof(ToolStripItemDisplayStyle), ((int)entity.DisplayStyle).ToString());
            this.TextImageRelation = (TextImageRelation)Enum.Parse(typeof(TextImageRelation), ((int)entity.TextImageRelation).ToString()); 
            this.Alignment = (ToolStripItemAlignment)Enum.Parse(typeof(ToolStripItemAlignment), ((int)entity.Aligment).ToString());
            this.ToolTipText = entity.ToolTipText;
            if (entity.Image != null && entity.Image != String.Empty)
            {
                FileInfo fi = new FileInfo(entity.Image);
                Image image;
                if (fi.Exists)
                {
                    image = DrawingTool.GetImage(fi.FullName);
                }
                else
                {
                    image = new Bitmap(16, 16);
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        g.DrawRectangle(Pens.Red, 0, 0, image.Width - 1, image.Height - 1);
                        g.DrawLine(Pens.Red, 0, 0, image.Width, image.Height);
                        g.DrawLine(Pens.Red, image.Width, 0, 0, image.Height);
                    }
                }
                this.Image = image;
            }
        }
    }
}
