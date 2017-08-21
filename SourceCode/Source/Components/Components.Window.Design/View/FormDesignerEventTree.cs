/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Components.Window.DesignComponent.Localisation;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    partial class FormDesignerEventTree : PadViewBase
    {
        private FormHostingContainer _container;
        private ToolStripCodon _toolStrip;
        private EventTreeView _eventTreeView = new EventTreeView()
        {
            Dock = DockStyle.Fill,
            AllowDrop = true,
            BorderStyle = BorderStyle.None
        };
        private Action<SEUndoUnitAbstract, SEUndoEngine.Type> _undounitAction;
        public EntityBase Entity
        {
            get { return this._eventTreeView.Entity; }
            set
            {
                this._eventTreeView.FormEntity = FormHostingContainer.Instance.ActiveFormEntity;
                this._eventTreeView.Entity = value;
                _toolStrip.View.UpdateStatus();
            }
        }
        public FormDesignerEventTree(FormHostingContainer container)
        {
            InitializeComponent();
            _container = container;
            _container.ActiveHostingChanged += new FormHostingContainer.OnActiveHostingChangedHandler(_container_ActiveHostingChanged);
            _container.HostingSelectionChanged += new FormHostingContainer.OnHostingSelectionChangedHandler(_container_HostingSelectionChanged);
            this.Icon = DrawingTool.ImageToIcon(IconsLibrary.Event);
            this.TabText = Language.Current.FormDesignerEventTree_TabText;
            Unity.ApplyResource(this);
            _eventTreeView.AllowDrop = true;
            _eventTreeView.AfterSelect += new TreeViewEventHandler(_eventTreeView_AfterSelect);
            _eventTreeView.EventChanged += new EventTreeView.OnEventChangedHandler(_eventTreeView_EventChanged);
            _eventTreeView.EventOrderChanged += new EventTreeView.OnEventOrderChangedHandler(_eventTreeView_EventOrderChanged);
            this.Controls.Add(_eventTreeView);
            InitToolStrip();
            this.Controls.Add(_toolStrip.View);
            _toolStrip.View.UpdateStatus();
            _undounitAction = new Action<SEUndoUnitAbstract, SEUndoEngine.Type>(
                delegate(SEUndoUnitAbstract unit, SEUndoEngine.Type type)
                {
                   
                    if (unit is SEUndoUnitEventEdit)
                    {
                        SEUndoUnitEventEdit undoUnit = unit as SEUndoUnitEventEdit;
                        if (undoUnit != null && ((EventBase)undoUnit.Value).HostEntity == this._eventTreeView.Entity)
                        {
                            _eventTreeView.ReBuild();
                        }
                    }
                    else if (unit is SEUndoTransaction)
                    {
                        SEUndoTransaction undoUnit = unit as SEUndoTransaction;
                        if (undoUnit != null && undoUnit.GetAllUnits().Length > 0)
                        {
                            SEUndoUnitEventEdit eventUnit = undoUnit.GetAllUnits()[0] as SEUndoUnitEventEdit;
                            if (eventUnit != null && ((EventBase)eventUnit.Value).HostEntity == this._eventTreeView.Entity)
                            {
                                _eventTreeView.ReBuild();
                            }
                        }
                    }
                });
        }
        private void _container_ActiveHostingChanged(FormDesignSurfaceHosting hosting)
        {
            UpdateSelection(hosting);
        }
        private void _container_HostingSelectionChanged(FormDesignSurfaceHosting hosting)
        {
            UpdateSelection(hosting);
        }
        private void _eventTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _toolStrip.View.UpdateStatus();
        }
        private void _eventTreeView_EventChanged(CollectionEditEventArgs e)
        {
            SEUndoUnitEventEdit undoUnit = new SEUndoUnitEventEdit(e);
            undoUnit.Action = _undounitAction;
            FormHostingContainer.Instance.ActiveHosting.AddUndoUnit(undoUnit);
        }
        private void _eventTreeView_EventOrderChanged(CollectionEditEventArgs[] e)
        {
            SEUndoUnitCollection undoUnitCollection = new SEUndoUnitCollection();
            foreach (CollectionEditEventArgs args in e)
            {
                SEUndoUnitEventEdit undoUnit = new SEUndoUnitEventEdit(args);
                undoUnitCollection.Add(undoUnit);
            }
            undoUnitCollection.Action = _undounitAction;
            FormHostingContainer.Instance.ActiveHosting.AddUndoUnit(undoUnitCollection);
        }
        private void UpdateSelection(FormDesignSurfaceHosting hosting)
        {
            if (hosting == null || hosting.SelectionService == null)
            {
                this.Entity = null;
                return;
            }
            ICollection selection = hosting.SelectionService.GetSelectedComponents();
            if (selection != null && selection.Count == 1)
            {
                object[] selArray = new object[selection.Count];
                selection.CopyTo(selArray, 0);
                IShellControlDev shellControlDev = selArray[0] as IShellControlDev;
                if (shellControlDev != null)
                {
                    this.Entity = shellControlDev.Entity;
                }
            }
            else
            {
                this.Entity = null;
            }
        }
        private void InitToolStrip()
        {
            Func<IToolStripItemCodon, bool> selectedEventNotNull = delegate(IToolStripItemCodon codon)
            {
                return this._eventTreeView.SelectedEvent != null;
            };
            _toolStrip = new ToolStripCodon("ToolStrip");
            _toolStrip.Items.Add(new ToolStripButtonCodon("Add", Language.Current.FormDesignerEventTree_ToolStripButtonAddEvent, IconsLibrary.New2,
                delegate(object sender, ToolStripItemCodonEventArgs args) { _eventTreeView.Add(); })
                {
                    IsEnabled = delegate(IToolStripItemCodon codon)
                    {
                        return this.Entity != null;
                    }
                });
            _toolStrip.Items.Add(new ToolStripButtonCodon("Edit", Language.Current.FormDesignerEventTree_ToolStripButtonEditEvent, IconsLibrary.Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args) { _eventTreeView.Edit(); })
                {
                    IsEnabled = selectedEventNotNull
                });
            _toolStrip.Items.Add(new ToolStripButtonCodon("Delete", Language.Current.FormDesignerEventTree_ToolStripButtonDeleteEvent, IconsLibrary.Delete,
               delegate(object sender, ToolStripItemCodonEventArgs args) { _eventTreeView.Delete(); })
               {
                   IsEnabled = selectedEventNotNull
               });
            _toolStrip.Items.Add(new ToolStripSeparatorCodon());
            _toolStrip.Items.Add(new ToolStripButtonCodon("Up", Language.Current.FormDesignerEventTree_ToolStripButtonUpEvent, IconsLibrary.Up,
                     delegate(object sender, ToolStripItemCodonEventArgs args) { _eventTreeView.Up(); })
            {
                IsEnabled = delegate(IToolStripItemCodon codon)
                {
                    return this._eventTreeView.SelectedEvent != null && this._eventTreeView.SelectedNode.PrevNode != null;
                }
            });
            _toolStrip.Items.Add(new ToolStripButtonCodon("Down", Language.Current.FormDesignerEventTree_ToolStripButtonDownEvent, IconsLibrary.Down,
              delegate(object sender, ToolStripItemCodonEventArgs args) { _eventTreeView.Down(); })
            {
                IsEnabled = delegate(IToolStripItemCodon codon)
                {
                    return this._eventTreeView.SelectedEvent != null && this._eventTreeView.SelectedNode.NextNode != null;
                }
            });
            _toolStrip.View.Dock = DockStyle.Top;
        }
    }
}
