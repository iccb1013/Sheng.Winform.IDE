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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    public partial class UserControlEventEditorPanelBase : UserControlViewBase, IEventEditorPanel
    {
        public UserControlEventEditorPanelBase()
        {
            InitializeComponent();
        }
        public UserControlEventEditorPanelBase(EventEditorAdapterAbstract hostAdapter)
            : this()
        {
            this.HostAdapter = hostAdapter;
            this.lblTitle.Text = this.PanelTitle;
            this.Dock = DockStyle.Fill;
        }
        public override bool SEValidate(out string validateMsg)
        {
            return this.ValidateParameter(out validateMsg);
        }
        public virtual string PanelTitle
        {
            get
            {
                return String.Empty;
            }
        }
        public virtual bool DefaultPanel
        {
            get { return false; }
        }
        private EventEditorAdapterAbstract _hostAdapter;
        public EventEditorAdapterAbstract HostAdapter
        {
            get { return _hostAdapter; }
            set { _hostAdapter = value; }
        }
        public virtual List<System.Xml.Linq.XObject> GetXml()
        {
            throw new NotImplementedException();
        }
        public virtual void SetParameter(EventBase even)
        {
            throw new NotImplementedException();
        }
        public virtual bool ValidateParameter(out string validateMsg)
        {
            return base.SEValidate(out validateMsg);
        }
    }
}
