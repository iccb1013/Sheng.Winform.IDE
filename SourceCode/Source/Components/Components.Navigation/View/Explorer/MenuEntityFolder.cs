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
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    class MenuEntityFolder
    {
        public const string Property_Text = "Text";
        public string Text
        {
            get { return Language.Current.Explorer_MenuEntityFolder; }
        }
        public const string Property_Items = "Items";
        private List<MenuEntity> _items;
        public List<MenuEntity> Items
        {
            get { return _items; }
        }
        public MenuEntityFolder(List<MenuEntity> items)
        {
            _items = items;
        }
    }
}
