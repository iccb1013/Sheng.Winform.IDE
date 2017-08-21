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
using System.Windows.Forms;
using System.Drawing;
namespace Sheng.SailingEase.Controls.SEAdressBar
{
    class SEAddressBarButton : ToolStripButton
    {
        public IAddressNode AddressNode
        {
            get;
            set;
        }
        public SEAddressBarDropDownButton DropDownButton
        {
            get;
            set;
        }
        public SEAddressBarButton(string text, Image image, EventHandler onClick)
            : base(text, image, onClick)
        {
        }
    }
}
