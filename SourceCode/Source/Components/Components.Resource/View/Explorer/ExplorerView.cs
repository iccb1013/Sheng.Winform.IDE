/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Controls.Docking;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Components.ResourceComponent.Localisation;
using Sheng.SailingEase.Core;
using System.Collections;
namespace Sheng.SailingEase.Components.ResourceComponent.View
{
    public partial class ExplorerView : WorkbenchViewBase
    {
        public const string SINGLEKEY = "ResourceExplorerView";
        public ExplorerView()
        {
            InitializeComponent();
            this.Single = true;
            this.SingleKey = SINGLEKEY;
            this.HideOnClose = true;
            this.Icon = DrawingTool.ImageToIcon(IconsLibrary.Image2);
            this.TabText = Language.Current.ExplorerView_TabText;
        }
        private void ExplorerView_Load(object sender, EventArgs e)
        {
            ResourceViewContainer resourceViews = new ResourceViewContainer();
            resourceViews.Dock = DockStyle.Fill;
            this.Controls.Add(resourceViews);
        }
    }
}
