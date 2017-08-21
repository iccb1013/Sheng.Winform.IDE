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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Components.Window.DesignComponent;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Components.Window.DesignComponent.Localisation;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    partial class FormToolbox : PadViewBase
    {
        private FormHostingContainer _container;
        private Toolbox _toolbox;
        public Toolbox Toolbox
        {
            get
            {
                return this._toolbox;
            }
            set
            {
                this.panelToolbox.Controls.Clear();
                if (_toolbox != null)
                {
                    _toolbox.Dispose();
                }
                this._toolbox = value;
                this._toolbox.Dock = DockStyle.Fill;
                this.panelToolbox.Controls.Add(this._toolbox);
            }
        }
        public FormToolbox(FormHostingContainer container)
        {
            InitializeComponent();
            _container = container;
            _container.ActiveHostingChanged += new FormHostingContainer.OnActiveHostingChangedHandler(_container_ActiveHostingChanged);
            this.Icon = DrawingTool.ImageToIcon(IconsLibrary.Toolbox);
            this.TabText = Language.Current.FormToolbox_TabText;
        }
        private void FormToolbox_Load(object sender, EventArgs e)
        {
        }
        private void _container_ActiveHostingChanged(FormDesignSurfaceHosting hosting)
        {
            if (hosting != null)
                this.Toolbox.DesignerHost = hosting.DesignerHost;
            else
                this.Toolbox.DesignerHost = null;
        }
    }
}
