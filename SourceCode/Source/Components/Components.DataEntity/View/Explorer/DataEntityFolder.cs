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
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    class DataEntityFolder
    {
        public const string Property_Text = "Text";
        public string Text
        {
            get { return Language.Current.NavigationTreeController_DataEntity; }
        }
        public const string Property_Items = "Items";
        private List<DataEntity> _items;
        public List<DataEntity> Items
        {
            get { return _items; }
        }
        public DataEntityFolder(List<DataEntity> items)
        {
            _items = items;
        }
    }
}
