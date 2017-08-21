/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.SEAdressBar
{
    /*
     * 加用户控件有几个原因
     * 1.如果不加用户控件，从ToolStrip继承下来的控件，要么Dock，如果不Dock，不能调整宽度
     * 2.没有边框，不能设置边框
     */
    public partial class SEAddressBar : UserControl
    {
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.addressBarStrip.BackColor = value;
            }
        }
        public ToolStripRenderer Renderer
        {
            get { return this.addressBarStrip.Renderer; }
            set
            {
                this.addressBarStrip.Renderer = value;
            }
        }
        public ToolStripRenderer DropDownRenderer
        {
            get { return this.addressBarStrip.DropDownRenderer; }
            set { this.addressBarStrip.DropDownRenderer = value; }
        }
        public IAddressNode CurrentNode
        {
            get { return addressBarStrip.CurrentNode; ; }
            set { addressBarStrip.CurrentNode = value; }
        }
        public SEAddressBar()
        {
            InitializeComponent();
        }
        public void InitializeRoot(IAddressNode rootNode)
        {
            addressBarStrip.InitializeRoot(rootNode);
        }
        public void SetAddress(string path)
        {
            addressBarStrip.SetAddress(path);
        }
        public void SetAddress(IAddressNode addressNode)
        {
            addressBarStrip.SetAddress(addressNode);
        }
        public void UpdateNode()
        {
            addressBarStrip.UpdateNode();
        }
        public event SEAddressBarStrip.SelectionChanged SelectionChange
        {
            add { addressBarStrip.SelectionChange += value; }
            remove { addressBarStrip.SelectionChange -= value; }
        }
    }
}
