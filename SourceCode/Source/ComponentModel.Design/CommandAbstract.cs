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
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
namespace Sheng.SailingEase.ComponentModel.Design
{
    public abstract class CommandAbstract
    {
        public SEUndoUnitAbstract UndoUnit { get; protected set; }
        public abstract void Execute();
    }
}
