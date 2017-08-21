/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    partial class FormDesignerEvents : FormViewBase
    {
        UserControlEvent _userControlEvent;
        public FormDesignerEvents()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
        }
        private void FormEvents_Load(object sender, EventArgs e)
        {
            if (FormHostingContainer.Instance.ActiveHosting == null)
                return;
            if (FormHostingContainer.Instance.ActiveHosting.SelectionService.PrimarySelection == null)
                return;
            object primarySelection = FormHostingContainer.Instance.ActiveHosting.SelectionService.PrimarySelection;
            IShellControlDev shellControlDev = primarySelection as IShellControlDev;
            if (shellControlDev == null)
                throw new NotImplementedException();
            this._userControlEvent = new UserControlEvent(shellControlDev.Entity);
            this._userControlEvent.FormEntity = FormHostingContainer.Instance.ActiveFormEntity;
            this._userControlEvent.Dock = DockStyle.Fill;
            this._userControlEvent.OnEdited += new UserControlEvent.OnEditedEventHandler(_userControlEvent_OnAfterEdit);
            this.Controls.Add(this._userControlEvent);
        }
        void _userControlEvent_OnAfterEdit(object sender, CollectionEditEventArgs e)
        {
            FormHostingContainer.Instance.ActiveHosting.MakeDirty();
            SEUndoUnitEventEdit undoUnit = new SEUndoUnitEventEdit(e);
            undoUnit.Action = new Action<SEUndoUnitAbstract, SEUndoEngine.Type>(
                 delegate(SEUndoUnitAbstract unit, SEUndoEngine.Type type)
                 {
                     EventBase even = undoUnit.Value as EventBase;
                     if (even == null)
                     {
                         if (undoUnit.Values.Count > 0)
                         {
                             even = undoUnit.Values[0] as EventBase;
                         }
                     }
                     if (even != null)
                     {
                         IEventSupport eventSupport = (IEventSupport)even.HostEntity;
                         eventSupport.EventUpdate(this);
                     }
                 }
                );
            FormHostingContainer.Instance.ActiveHosting.UndoEngine.AddUndoUnit(undoUnit);
        }
    }
}
