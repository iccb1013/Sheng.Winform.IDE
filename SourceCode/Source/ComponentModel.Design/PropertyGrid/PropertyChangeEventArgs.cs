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
namespace Sheng.SailingEase.ComponentModel.Design
{
    public class PropertyChangeEventArgs : EventArgs
    {
        private object [] _selectedObjects;
        public object [] SelectedObjects
        {
            get
            {
                return this._selectedObjects;
            }
        }
        private PropertyGridRow _row;
        public PropertyGridRow Row
        {
            get
            {
                return this._row;
            }
        }
        public PropertyChangeEventArgs(object [] targetObject,PropertyGridRow row)
        {
            this._selectedObjects = targetObject;
            this._row = row;
        }
    }
}
