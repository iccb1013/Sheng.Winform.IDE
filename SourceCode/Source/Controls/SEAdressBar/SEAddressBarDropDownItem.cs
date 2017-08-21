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
    class SEAddressBarDropDownItem : ToolStripMenuItem
    {
        private IAddressNode _addressNode;
        public IAddressNode AddressNode
        {
            get { return _addressNode; }
            set { _addressNode = value; }
        }
        public SEAddressBarDropDownItem(string text, Image image, EventHandler onClick) :
            base(text, image, onClick)
        {
        }
    }
}
