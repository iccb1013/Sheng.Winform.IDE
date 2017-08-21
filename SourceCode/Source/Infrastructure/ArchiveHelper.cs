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
using System.Xml.Linq;
using Sheng.SailingEase.Core;
using System.Diagnostics;
namespace Sheng.SailingEase.Infrastructure
{
    public static class ArchiveHelper
    {
        public static XElement GetEntityArchiveIndex(EntityBase entity)
        {
            Debug.Assert(entity != null, "entity 为 null");
            XElement ele = new XElement("Entity",
                new XAttribute("Id", entity.Id),
                new XAttribute("Name", entity.Name),
                new XAttribute("Code", entity.Code),
                new XAttribute("Sys", entity.Sys));
            return ele;
        }
    }
}
