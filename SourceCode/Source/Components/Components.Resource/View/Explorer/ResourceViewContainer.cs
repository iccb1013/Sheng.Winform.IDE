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
using Sheng.SailingEase.Controls.MozBar;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.ResourceComponent.View
{
    public partial class ResourceViewContainer : UserControl
    {
        private Dictionary<MozItem, Control> _views = new Dictionary<MozItem, Control>();
        public ResourceViewContainer()
        {
            InitializeComponent();
            this.viewNavigation.ItemSelected += new MozItemEventHandler(viewNavigation_ItemSelected);
        }
        private void ResourceViewContainer_Load(object sender, EventArgs e)
        {
            InitNavigation();
        }
        void viewNavigation_ItemSelected(object sender, MozItemEventArgs e)
        {
            MozItem item = e.MozItem;
            Control view = _views[item];
            view.Dock = DockStyle.Fill;
            this.viewContainer.Controls.Clear();
            this.viewContainer.Controls.Add(view);
        }
        private void InitNavigation()
        {
            AddMenu("Image", new ImageResourceView());
            this.viewNavigation.SelectItem(0);
        }
        private void AddMenu(string name, Control view)
        {
            MozItem item = new MozItem();
            item.Name = name;
            item.Text = name;
            item.Images.NormalImage = IconsLibrary.Image2;
            item.ItemStyle = MozItemStyle.TextAndPicture;
            item.TextAlign = MozTextAlign.Right;
            item.ImageWidth = 16;
            item.ImageHeight = 16;
            this.viewNavigation.Items.Add(item);
            _views.Add(item, view);
        }
    }
}
