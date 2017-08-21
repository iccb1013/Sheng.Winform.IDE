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
namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    public class StepListItem
    {
        private bool _selected = false;
        public bool Selected
        {
            get { return this._selected; }
            set
            {
                if (this._selected != value)
                {
                    this._selected = value;
                    if (this.List != null)
                    {
                        if (this.List.InDisplayRange(this.Index))
                            this.List.DrawItem(this.Index);
                    }
                }
            }
        }
        public int Index
        {
            get
            {
                if (this.List == null)
                    return -1;
                return this.List.Items.IndexOf(this);
            }
        }
        public StepList List { get; set; }
        public SEUndoUnitAbstract UndoUnit { get; set; }
        public StepListItem()
        {
        }
        public StepListItem(SEUndoUnitAbstract obj)
        {
            this.UndoUnit = obj;
        }
        public override string ToString()
        {
            return UndoUnit.Name;
        }
    }
}
