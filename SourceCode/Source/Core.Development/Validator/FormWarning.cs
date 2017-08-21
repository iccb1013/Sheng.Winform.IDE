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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Controls;
namespace Sheng.SailingEase.Core.Development
{
    public partial class FormWarning : FormViewBase
    {
        private WarningSign _warningSign;
        public WarningSign WarningSign
        {
            get { return _warningSign; }
            set
            {
                _warningSign = value;
                userControlWarningView.ShowWarning(_warningSign);
                this.toolStripButtonWarning.Text = String.Format(Language.Current.FormWarning_ToolStripButtonWarning, _warningSign.WarningCount);
            }
        }
        public FormWarning()
        {
            InitializeComponent();
            this.toolStrip1.Renderer = ToolStripRenders.Default;
            this.toolStripButtonWarning.Image = IconsLibrary.Warning;
            Unity.ApplyResource(this);
        }
    }
}
