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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Components.NavigationComponent
{
    class ToolStripPageEntityDev : ToolStripPageEntity, IPersistence
    {
        public void Save()
        {
            ToolStripArchive.Instance.Commit(this);
        }
        public void Delete()
        {
            ToolStripArchive.Instance.RemovePage(this);
        }
    }
}
