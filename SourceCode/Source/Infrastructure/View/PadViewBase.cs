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
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Controls.Docking;
namespace Sheng.SailingEase.Infrastructure
{
    public partial class PadViewBase : SEFormDock, IPadView
    {
        public PadViewBase()
        {
            InitializeComponent();
            this.DockAreas = (DockAreas)Enum.Parse(typeof(DockAreas), _padAreas.ToString());
        }
        private PadAreas _padAreas = PadAreas.Float;
        public PadAreas PadAreas
        {
            get
            {
                return _padAreas;
            }
            set
            {
                _padAreas = value;
                this.DockAreas = (DockAreas)Enum.Parse(typeof(DockAreas), _padAreas.ToString());
            }
        }
        public virtual string Title
        {
            get { return this.TabText; }
        }
        private bool _single = false;
        public virtual bool Single
        {
            get { return _single; }
            set { _single = value; }
        }
        private object _singleKey = null;
        public virtual object SingleKey
        {
            get { return _singleKey; }
            set { _singleKey = value; }
        }
        public virtual bool CompareSingleKey(object singleKey)
        {
            if (_singleKey != null)
                return _singleKey.Equals(singleKey);
            else
                return false;
        }
    }
}
