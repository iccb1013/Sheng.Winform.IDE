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
using System.Drawing;
namespace Sheng.SailingEase.Controls.SEAdressBar
{
    public interface IAddressNode
    {
        IAddressNode Parent
        {
            get;
            set;
        }
        String DisplayName
        {
            get;
        }
        Bitmap Icon
        {
            get;
        }
        string UniqueID
        {
            get;
        }
        SEAddressBarDropDown DropDownMenu
        {
            get;
            set;
        }
        IAddressNode[] Children
        {
            get;
        }
        void CreateChildNodes();
        void UpdateChildNodes();
        IAddressNode GetChild(string uniqueID);
        IAddressNode Clone();
    }
}
