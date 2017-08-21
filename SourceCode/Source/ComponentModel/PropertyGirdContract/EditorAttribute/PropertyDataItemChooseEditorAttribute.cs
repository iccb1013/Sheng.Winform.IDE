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
namespace Sheng.SailingEase.ComponentModel
{
    [Serializable]
    public class PropertyDataItemChooseEditorAttribute : PropertyEditorAttribute
    {
        private bool _showDataItem = true;
        public bool ShowDataItem
        {
            get
            {
                return this._showDataItem;
            }
            set
            {
                this._showDataItem = value;
            }
        }
    }
}
