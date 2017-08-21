using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Sheng.SailingEase.Components.Window.DesignComponent;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Controls.PopupControl;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Components.Window.DesignComponent.Localisation;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Controls.Docking;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Core.Development;

namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    /// <summary>
    /// 窗体设计器主窗体
    /// </summary>
    partial class FormHostingContainer : WorkbenchViewBase
    {
        #region 私有成员

        /// <summary>
        /// 是否处在属性更新状态,通过属性面板更改对象属性时将此置为true
        /// 如果为true,componentChangeService_ComponentChanged事件不会执行
        /// 避免反复执行,但是有一个小问题,就是当updateview后控件实际接受的属性和entity中的值不同时
        /// 不在能componentChangeService_ComponentChanged中返过来同步了
        /// </summary>
        private bool _propertyChanging = false;
        public bool PropertyChanging
        {
            get { return _propertyChanging; }
        }

        private Popup _undoEnginePop;
        private StepList _stepList = new StepList();

        private SEDesignSurfaceManager _designSurfaceManager = new SEDesignSurfaceManager();

        private IWorkbenchService _workbenchService = ServiceUnity.WorkbenchService;

        #region 从属窗体声明

        //Hosting要用到，为了向DesignSurface中添加工具箱服务
        public Toolbox Toolbox
        {
            get { return _formToolbox.Toolbox; }
        }

        /// <summary>
        /// 属性窗体
        /// </summary>
        private FormPropertyGrid _formPropertyGrid;
        public FormPropertyGrid FormPropertyGrid
        {
            get { return _formPropertyGrid; }
        }

        /// <summary>
        /// 工具盒窗体
        /// </summary>
        FormToolbox _formToolbox;

        /// <summary>
        /// 事件树窗体
        /// </summary>
        FormDesignerEventTree _formEvent;

        #endregion

        #endregion

        #region 公开属性

        private static object objLock = new object();
        private static FormHostingContainer _instance;
        //TODO:否决，不要到处用这个属性来访问，容易乱，失控
        //要解决显示的问题，定义一个静态的Show方法来解决
        public static FormHostingContainer Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                {
                    lock (objLock)
                    {
                        if (_instance == null || _instance.IsDisposed)
                        {
                            _instance = new FormHostingContainer();
                            _instance.HideOnClose = false;
                            _instance.TabText = Language.Current.FormHostingContainer;
                            _instance.Icon = DrawingTool.ImageToIcon(IconsLibrary.Form);
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 当前活动的FormDesignSurfaceHosting
        /// public因为要在ContextMenuCommands中调用
        /// </summary>
        public FormDesignSurfaceHosting ActiveHosting
        {
            get
            {
                return dockPanel.ActiveDocument as FormDesignSurfaceHosting;
            }
        }

        public List<FormDesignSurfaceHosting> Hostings
        {
            get
            {
                List<FormDesignSurfaceHosting> _hostings = new List<FormDesignSurfaceHosting>();
                foreach (IDockContent dock in dockPanel.Documents)
                {
                    _hostings.Add(dock as FormDesignSurfaceHosting);
                }
                return _hostings;
            }
        }

        //原来是Dev
        public WindowEntity ActiveFormEntity
        {
            get
            {
                if (ActiveHosting != null)
                    return ActiveHosting.WindowEntity;
                else
                    return null;
            }
        }

        #endregion

        #region 构造及窗体事件

        private FormHostingContainer()
        {
            InitializeComponent();

            //Handler键盘操作
            if (FormKeyHandler.Inserted == false)
            {
                FormKeyHandler.Insert();
            }
        }

        private void FormDesinger_Load(object sender, EventArgs e)
        {
            #region 初始化各子窗体必须的对象 并显示各子窗体

            //属性窗体
            _formPropertyGrid = new FormPropertyGrid(this);
            //_formPropertyGrid.PropertyGrid = new SEPropertyGrid();
            _formPropertyGrid.DockAreas = DockAreas.DockLeft;
            _formPropertyGrid.HideOnClose = true;
            _formPropertyGrid.Show(this.dockPanel);

            //工具窗体
            _formToolbox = new FormToolbox(this);
            _formToolbox.Toolbox = new Toolbox();
            _formToolbox.Toolbox.ToolboxManager = DesignerToolboxManager.Instance;
            _formToolbox.DockAreas = DockAreas.DockLeft;
            _formToolbox.HideOnClose = true;
            _formToolbox.Show(this.dockPanel);

            //事件树窗体
            _formEvent = new FormDesignerEventTree(this);
            _formEvent.DockAreas = DockAreas.DockRight;
            _formEvent.HideOnClose = true;
            _formEvent.Show(this.dockPanel);

            #endregion

            #region 初始化撤销重做引擎相关对象

            this._undoEnginePop = new Popup(this._stepList);
            this._undoEnginePop.Size = new Size(300, 280);
            this._stepList.Popup = this._undoEnginePop;
            this._stepList.OnClick += new StepList.OnClickEventHandler(_stepList_OnClick);

            #endregion

            _formPropertyGrid.PropertyGrid.PropertyChanged += new PropertyGridPad.OnPropertyChangeHandler(PropertyGrid_PropertyChanged);
            _formPropertyGrid.PropertyGrid.ObjectPropertyValueChanged += new PropertyGridPad.OnObjectPropertyValueChangedHandler(PropertyGrid_ObjectPropertyValueChanged);

            dockPanel.ActiveDocumentChanged += new EventHandler(dockPanel_ActiveDocumentChanged);
        }

        private void FormHostingContainer_FormClosing(object sender, FormClosingEventArgs e)
        {
            

            //判断有没有做了修改没有保存的设计窗体(dirty=true)，一把列出来，让用户选择是否全部保存或取消
            List<ICanSaveForm> saveForms = new List<ICanSaveForm>();
            foreach (FormDesignSurfaceHosting hosting in this.Hostings)
            {
                if (hosting.Dirty)
                    saveForms.Add(hosting);
            }
            if (saveForms.Count > 0)
            {
                using (FormMultiSave multiSave = new FormMultiSave())
                {
                    multiSave.SaveForms.AddRange(saveForms);
                    DialogResult result = multiSave.ShowDialog();
                    if (result == DialogResult.Yes)
                    {
                        foreach (FormDesignSurfaceHosting hosting in this.Hostings)
                        {
                            hosting.Save();
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取撤销步长列表框弹出的位置
        /// </summary>
        /// <param name="toolStripItem"></param>
        /// <returns></returns>
        private Point GetUndoEnginePopShowLocation(System.Windows.Forms.ToolStripItem toolStripItem)
        {
            Point point, toolStripPanelPoint;

            //point = new Point(toolStripItem.Owner.Location.X + toolStripItem.Bounds.Location.X,
            //    toolStripItem.Owner.Location.Y + toolStripItem.Bounds.Location.Y + toolStripItem.Height);
            //return this.PointToScreen(point);

            //撤销重做按钮是显示在主界面工具条上的，所以要用主界面的PointToScreen

            //要先把toolStripItem.Owner.Location属性PointToScreen后，再参与计算
            //对于主界面，还存在菜单栏，所以在计算坐标时也要考虑
            //注意：工具栏的Y坐标是0，是因为它在Workbench.Instance.ToolStripPanel中！Y轴要取工具栏主容器的坐标+工具栏本身的Y坐标
            //X轴也一样，要取ToolStripPanel的X坐标+工具栏的X坐标
            toolStripPanelPoint = _workbenchService.PointToScreen(_workbenchService.ToolStripPanelLocation);
            point = new Point(toolStripPanelPoint.X + toolStripItem.Owner.Location.X + toolStripItem.Bounds.Location.X,
                toolStripPanelPoint.Y + toolStripItem.Owner.Location.Y + toolStripItem.Bounds.Location.Y + toolStripItem.Height);

            return point;
        }

        #endregion

        #region 公开方法

        public void Create(WindowEntity formEntity)
        {
            //不能在FormDesignSurfaceHosting的VisibleChanged事件中，设置当前活动设计器或撤销重做引擎这些当前对象
            //在多个设计窗体切换时，要显示的窗体的Visible=true,然后被切掉的窗体才Visible=false，永远走到false里去

            if (OnDesign(formEntity.Id))
            {
                //激活
                foreach (IDockContent dockContent in this.dockPanel.Documents)
                {
                    FormDesignSurfaceHosting hosting = dockContent as FormDesignSurfaceHosting;
                    if (hosting.WindowEntity.Id == formEntity.Id)
                        dockContent.DockHandler.Activate();
                }
            }
            else
            {
                //创建新的设计器容器
                FormDesignSurfaceHosting hosting = new FormDesignSurfaceHosting(formEntity, this._designSurfaceManager.NewDesignSurface(), this);
                hosting.SelectionService.SelectionChanged += new EventHandler(SelectionService_SelectionChanged);
                hosting.InitFormElementsComponentComplete += new FormDesignSurfaceHosting.OnInitFormElementsComponentCompleteHandler(hosting_InitFormElementsComponentComplete);
                hosting.DockAreas = DockAreas.Document;
                hosting.Show(this.dockPanel);
            }
        }

        /// <summary>
        /// 显示属性面板
        /// public因为要在ContextMenuCommands中调用
        /// </summary>
        public void ShowProperties()
        {
            this._formPropertyGrid.Show();
        }

        /// <summary>
        /// 显示工具箱面板
        /// public因为要在ContextMenuCommands中调用
        /// </summary>
        public void ShowToolbox()
        {
            this._formToolbox.Show();
        }

        public static void ShowView()
        {
            ServiceUnity.WorkbenchService.Show(Instance);
        }

        /// <summary>
        /// 更新属性网格（非属性面板）
        /// PropertyGrid指向的对象的属性(Property)已改变,指示立即更新属性行中的值
        /// 
        ///  默认在调用UpdateProperty方法时，更新选中的对象的可视状态，以及触发PropertyChanged事件
        ///  等价 UpdatePropertyGrid(true)
        /// </summary>
        public void UpdatePropertyGrid()
        {
            UpdatePropertyGrid(true);
        }

        /// <summary>
        /// 更新属性网格（非属性面板）
        /// 
        /// PropertyGrid指向的对象的属性(Property)已改变,指示立即更新属性行中的值
        /// 通过参数指定更新属性网格后，是否让属性网格去同步组件的可视状态
        /// </summary>
        /// <param name="updateSelectedObject"> 在调用UpdateProperty方法时，是否更新选中的对象的可视状态，以及是否触发PropertyChanged事件</param>
        public void UpdatePropertyGrid(bool updateSelectedObject)
        {
            this._formPropertyGrid.PropertyGrid.UpdateProperty(updateSelectedObject);
        }

        /// <summary>
        /// 判断指定ID的窗体实体对象是否处于设计中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool OnDesign(string id)
        {
            foreach (IDockContent dockContent in this.dockPanel.Documents)
            {
                FormDesignSurfaceHosting hosting = dockContent as FormDesignSurfaceHosting;
                if (hosting.WindowEntity.Id == id)
                    return true;
            }

            return false;
        }

        public void MakeDirty()
        {
            if (this.ActiveHosting != null)
                this.ActiveHosting.MakeDirty();
        }

        /// <summary>
        /// 添加可撤销的工作单元
        /// </summary>
        /// <param name="undoUnits"></param>
        public void AddUndoUnit(SEUndoUnitCollection undoUnits)
        {
            Debug.Assert(undoUnits != null);

            if (this.ActiveHosting != null)
                this.ActiveHosting.AddUndoUnit(undoUnits);
        }

        public void AddUndoUnit(SEUndoUnitAbstract undoUnit)
        {
            Debug.Assert(undoUnit != null);

            if (this.ActiveHosting != null)
                this.ActiveHosting.AddUndoUnit(undoUnit);
        }

        public void ExecuteCommand(CommandAbstract command)
        {
            ExecuteCommand(command, null);
        }

        /// <summary>
        /// 执行一个命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="action"></param>
        public void ExecuteCommand(CommandAbstract command, Action<SEUndoUnitAbstract, SEUndoEngine.Type> action)
        {
            command.Execute();

            Debug.Assert(command.UndoUnit != null);

            if (command.UndoUnit != null)
            {
                if (action != null)
                    command.UndoUnit.Action = action;

                AddUndoUnit(command.UndoUnit);
            }
        }

        #endregion

        #region 事件处理

        private void SelectionService_SelectionChanged(object sender, EventArgs e)
        {
            if (HostingSelectionChanged != null)
            {
                HostingSelectionChanged(this.ActiveHosting);
            }
        }

        private void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
           

            if (this.ActiveHosting != null)
            {
                _designSurfaceManager.ActiveDesignSurface = this.ActiveHosting.DesignSurface;
                _stepList.UndoEngine = this.ActiveHosting.UndoEngine;
            }
            else
            {
                _designSurfaceManager.ActiveDesignSurface = null;
                _stepList.UndoEngine = null;
            }

            if (ActiveHostingChanged != null)
            {
                ActiveHostingChanged(this.ActiveHosting);
            }
        }

        private void hosting_InitFormElementsComponentComplete(FormDesignSurfaceHosting hosting)
        {
            if (HostingInitFormElementsComponentComplete != null)
            {
                HostingInitFormElementsComponentComplete(hosting);
            }
        }

        #endregion

        #region StepList　事件

        private void _stepList_OnClick(object sender, StepList.OnClickEventArgs e)
        {
            if (this.ActiveHosting != null)
            {
                if (e.ActionType == SEUndoEngine.Type.Undo)
                    this.ActiveHosting.Undo(e.SelectedItemCount);
                else
                    this.ActiveHosting.Redo(e.SelectedItemCount);
            }
        }

        #endregion

        #region PropertyGrid 事件

        private void PropertyGrid_PropertyChanged(object sender, PropertyChangeEventArgs e)
        {
            //如果是在设计中拖动操作组件的，此处直接返回
            //无需UpdateView，也不能在这里封装撤销单元，因为在这种情况下这里拿不到旧值
            if (ActiveHosting.ComponentChanging)
                return;

            _propertyChanging = true;

            #region 提交可撤销的工作单元集合

            //提交可撤销的工作单元集合（如果需要），提交之后清空临时可撤销工作单元集合
            ActiveHosting.CommitUndoUnitList();

            #endregion

            #region 更新当前设计器中选定的对象

            //TODO:这部分代码可考虑整个移到FormDesignSurfaceHosting
            ICollection selectedComponents = ActiveHosting.SelectionService.GetSelectedComponents();

            //释放时会走到这里 ,control为null
            if (selectedComponents == null)
            {
                return;
            }

            IComponent[] selArray = new IComponent[selectedComponents.Count];
            selectedComponents.CopyTo(selArray, 0);

            ActiveHosting.UpdateView(selArray);

            #endregion

            _propertyChanging = false;

            ActiveHosting.MakeDirty();
        }

        private void PropertyGrid_ObjectPropertyValueChanged(object sender, ObjectPropertyValueChangedEventArgs e)
        {
            #region 封装可撤销的工作单元

            if (ActiveHosting.Loaded)
            {
                SEUndoUnitFormDesigner undoUnit = new SEUndoUnitFormDesigner(e.PropertyName);
                undoUnit.Type = SEUndoUnitFormDesigner.UndoUnitType.ComponentChanged;
                undoUnit.Entity = (EntityBase)e.RootObject;
                undoUnit.Value = e.TargetObject;
                undoUnit.Members.Add(e.Row.PropertyName, e.OldValue, e.NewValue);
                ActiveHosting.AddUndoUnitList(undoUnit);

                Debug.WriteLine("封装可撤销的工作单元:" + undoUnit.ToString());
            }

            #endregion
        }

        #endregion

        #region IWorkbenchWindow 成员

        private List<ToolStripCodon> _toolStripList;
        public override List<ToolStripCodon> ToolStripList
        {
            get
            {
                if (_toolStripList == null)
                {
                    _toolStripList = new List<ToolStripCodon>();

                    #region toolStripCommand

                    ToolStripCodon toolStripCommand = new ToolStripCodon("FormDesignerCommand");
                    toolStripCommand.Items.Add(new ToolStripButtonCodon("AlignLeft", Language.Current.FormHostingContainer_ToolStripButtonAlignLeft, IconsLibrary.AlignObjectsLeftHS,
                        delegate(object sender, ToolStripItemCodonEventArgs codon)
                        {
                            if (this.ActiveHosting != null)
                                this.ActiveHosting.DesignSurface.DoAction(StandardCommands.AlignLeft);
                        })
                        {
                            IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null; }
                        });
                    toolStripCommand.Items.Add(new ToolStripButtonCodon("AlignVerticalCenters", Language.Current.FormHostingContainer_ToolStripButtonAlignVerticalCenters, IconsLibrary.AlignObjectsCenteredHorizontalHS,
                        delegate(object sender, ToolStripItemCodonEventArgs codon)
                        {
                            if (this.ActiveHosting != null)
                                this.ActiveHosting.DesignSurface.DoAction(StandardCommands.AlignVerticalCenters);
                        })
                        {
                            IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null; }
                        });
                    toolStripCommand.Items.Add(new ToolStripButtonCodon("AlignRight", Language.Current.FormHostingContainer_ToolStripButtonAlignRight, IconsLibrary.AlignObjectsRightHS,
                       delegate(object sender, ToolStripItemCodonEventArgs codon)
                       {
                           if (this.ActiveHosting != null)
                               this.ActiveHosting.DesignSurface.DoAction(StandardCommands.AlignRight);
                       })
                       {
                           IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null; }
                       });
                    toolStripCommand.Items.Add(new ToolStripSeparatorCodon());
                    toolStripCommand.Items.Add(new ToolStripButtonCodon("AlignTop", Language.Current.FormHostingContainer_ToolStripButtonAlignTop, IconsLibrary.AlignObjectsTopHS,
                      delegate(object sender, ToolStripItemCodonEventArgs codon)
                      {
                          if (this.ActiveHosting != null)
                              this.ActiveHosting.DesignSurface.DoAction(StandardCommands.AlignTop);
                      })
                      {
                          IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null; }
                      });
                    toolStripCommand.Items.Add(new ToolStripButtonCodon("AlignHorizontalCenters", Language.Current.FormHostingContainer_ToolStripButtonAlignHorizontalCenters, IconsLibrary.AlignObjectsCenteredVerticalHS,
                     delegate(object sender, ToolStripItemCodonEventArgs codon)
                     {
                         if (this.ActiveHosting != null)
                             this.ActiveHosting.DesignSurface.DoAction(StandardCommands.AlignHorizontalCenters);
                     })
                     {
                         IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null; }
                     });
                    toolStripCommand.Items.Add(new ToolStripButtonCodon("AlignBottom", Language.Current.FormHostingContainer_ToolStripButtonAlignBottom, IconsLibrary.AlignObjectsBottomHS,
                     delegate(object sender, ToolStripItemCodonEventArgs codon)
                     {
                         if (this.ActiveHosting != null)
                             this.ActiveHosting.DesignSurface.DoAction(StandardCommands.AlignBottom);
                     })
                     {
                         IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null; }
                     });
                    toolStripCommand.Items.Add(new ToolStripSeparatorCodon());
                    toolStripCommand.Items.Add(new ToolStripButtonCodon("SizeToControlWidth", Language.Current.FormHostingContainer_ToolStripButtonSizeToControlWidth, IconsLibrary.ObjectWidthEqual,
                     delegate(object sender, ToolStripItemCodonEventArgs codon)
                     {
                         if (this.ActiveHosting != null)
                             this.ActiveHosting.DesignSurface.DoAction(StandardCommands.SizeToControlWidth);
                     })
                     {
                         IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null; }
                     });
                    toolStripCommand.Items.Add(new ToolStripButtonCodon("SizeToControlHeight", Language.Current.FormHostingContainer_ToolStripButtonSizeToControlHeight, IconsLibrary.ObjectHeightEqual,
                   delegate(object sender, ToolStripItemCodonEventArgs codon)
                   {
                       if (this.ActiveHosting != null)
                           this.ActiveHosting.DesignSurface.DoAction(StandardCommands.SizeToControlHeight);
                   })
                   {
                       IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null; }
                   });
                    toolStripCommand.Items.Add(new ToolStripButtonCodon("SizeToControl", Language.Current.FormHostingContainer_ToolStripButtonSizeToControl, IconsLibrary.ObjectSizeEqual,
                   delegate(object sender, ToolStripItemCodonEventArgs codon)
                   {
                       if (this.ActiveHosting != null)
                           this.ActiveHosting.DesignSurface.DoAction(StandardCommands.SizeToControl);
                   })
                   {
                       IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null; }
                   });

                    _toolStripList.Add(toolStripCommand);

                    #endregion

                    #region toolStripFunction

                    ToolStripCodon toolStripFunction = new ToolStripCodon("FormDesignerFunction");
                    toolStripFunction.Items.Add(new ToolStripButtonCodon("Save", Language.Current.FormHostingContainer_ToolStripButtonSave, IconsLibrary.Save,
                        delegate(object sender, ToolStripItemCodonEventArgs codon)
                        {
                            if (this.ActiveHosting != null)
                                this.ActiveHosting.Save();
                        })
                        {
                            IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null && this.ActiveHosting.Saving == false; }
                        });
                    toolStripFunction.Items.Add(new ToolStripSeparatorCodon());
                    toolStripFunction.Items.Add(new ToolStripButtonCodon("Cut", Language.Current.FormHostingContainer_ToolStripButtonCut, IconsLibrary.Cut,
                      delegate(object sender, ToolStripItemCodonEventArgs codon)
                      {
                          if (this.ActiveHosting != null)
                          {
                              this.ActiveHosting.DesignSurface.DoAction(StandardCommands.Cut);
                          }
                      })
                      {
                          IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null && this.ActiveHosting.EnableCut; }
                      });
                    toolStripFunction.Items.Add(new ToolStripButtonCodon("Copy", Language.Current.FormHostingContainer_ToolStripButtonCopy, IconsLibrary.Copy,
                     delegate(object sender, ToolStripItemCodonEventArgs codon)
                     {
                         if (this.ActiveHosting != null)
                         {
                             this.ActiveHosting.DesignSurface.DoAction(StandardCommands.Copy);
                         }
                     })
                     {
                         IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null && this.ActiveHosting.EnableCopy; }
                     });
                    toolStripFunction.Items.Add(new ToolStripButtonCodon("Paste", Language.Current.FormHostingContainer_ToolStripButtonPaste, IconsLibrary.Paste,
                     delegate(object sender, ToolStripItemCodonEventArgs codon)
                     {
                         if (this.ActiveHosting != null)
                             this.ActiveHosting.DesignSurface.DoAction(StandardCommands.Paste);
                     })
                     {
                         IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null && this.ActiveHosting.EnablePaste; }
                     });
                    toolStripFunction.Items.Add(new ToolStripSplitButtonCodon("Undo", Language.Current.FormHostingContainer_ToolStripButtonUndo, IconsLibrary.Undo,
                        delegate(object sender, ToolStripItemCodonEventArgs codon)
                        {
                            if (this.ActiveHosting != null)
                                ActiveHosting.Undo();
                        })
                        {
                            IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null && this.ActiveHosting.EnableUndo; },

                            DropDownOpeningAction = delegate(object sender, ToolStripItemCodonEventArgs codon)
                            {
                                if (_stepList.UndoEngine != null)
                                {
                                    _stepList.ActionType = SEUndoEngine.Type.Undo;
                                    _undoEnginePop.Show(GetUndoEnginePopShowLocation(sender as System.Windows.Forms.ToolStripItem));
                                }
                            }
                        });
                    toolStripFunction.Items.Add(new ToolStripSplitButtonCodon("Redo", Language.Current.FormHostingContainer_ToolStripButtonRedo, IconsLibrary.Redo,
                        delegate(object sender, ToolStripItemCodonEventArgs codon)
                        {
                            if (this.ActiveHosting != null)
                                ActiveHosting.Redo();
                        })
                        {
                            IsEnabled = delegate(IToolStripItemCodon codon) { return this.ActiveHosting != null && this.ActiveHosting.EnableRedo; },

                            DropDownOpeningAction = delegate(object sender, ToolStripItemCodonEventArgs codon)
                            {
                                if (_stepList.UndoEngine != null)
                                {
                                    _stepList.ActionType = SEUndoEngine.Type.Redo;
                                    _undoEnginePop.Show(GetUndoEnginePopShowLocation(sender as System.Windows.Forms.ToolStripItem));
                                }
                            }
                        });
                    toolStripFunction.Items.Add(new ToolStripSeparatorCodon());
                    toolStripFunction.Items.Add(new ToolStripButtonCodon("Toolbox", Language.Current.FormHostingContainer_ToolStripButtonToolbox, IconsLibrary.Toolbox,
                        delegate(object sender, ToolStripItemCodonEventArgs codon)
                        {
                            ShowToolbox();
                        }));
                    toolStripFunction.Items.Add(new ToolStripButtonCodon("Property", Language.Current.FormHostingContainer_ToolStripButtonProperty, IconsLibrary.Property,
                       delegate(object sender, ToolStripItemCodonEventArgs codon)
                       {
                           ShowProperties();
                       }));
                    ToolStripSplitButtonCodon toolStripItemWarning = new ToolStripSplitButtonCodon("Warning", Language.Current.FormHostingContainer_ToolStripSplitButtonWarning, IconsLibrary.Warning,
                        ToolStripItemDisplayStyle.ImageAndText,
                        delegate(object sender, ToolStripItemCodonEventArgs codon)
                        {
                            #region 显示警告树

                            //显示警告树
                            if (this.ActiveHosting != null)
                            {
                                IWarningable warningable = this.ActiveHosting.WindowEntity as IWarningable;
                                if (warningable != null)
                                {
                                    warningable.CheckWarning();
                                    using (FormWarning formWarning = new FormWarning())
                                    {
                                        formWarning.WarningSign = warningable.Warning;
                                        formWarning.ShowDialog();
                                    }
                                }
                            }

                            #endregion
                        })
                        {
                            #region GetText

                            //在文本中显示警告的数量
                            GetText = delegate()
                            {
                                if (this.ActiveHosting != null)
                                {
                                    IWarningable warning = this.ActiveHosting.WindowEntity as IWarningable;
                                    if (warning != null)
                                    {
                                        return String.Format(Language.Current.FormHostingContainer_ToolStripSplitButtonWarning,
                                            warning.Warning.WarningCount);
                                    }
                                }

                                return String.Format(Language.Current.FormHostingContainer_ToolStripSplitButtonWarning, 0);

                            }

                            #endregion
                        };
                    toolStripItemWarning.Items.Add(new ToolStripMenuItemCodon("Refresh", Language.Current.FormHostingContainer_ToolStripMenuItemRefreshWarning,
                        delegate(object sender, ToolStripItemCodonEventArgs codon)
                        {
                            if (this.ActiveHosting != null)
                            {
                                ActiveHosting.CheckWarning();
                            }
                        }));
                    toolStripFunction.Items.Add(toolStripItemWarning);

                    _toolStripList.Add(toolStripFunction);

                    #endregion
                }

                return _toolStripList;
            }
        }

        #endregion

        #region 事件

        public delegate void OnActiveHostingChangedHandler(FormDesignSurfaceHosting hosting);
        /// <summary>
        /// 切换当前活动的窗体设计容器
        /// </summary>
        public event OnActiveHostingChangedHandler ActiveHostingChanged;

        public delegate void OnHostingSelectionChangedHandler(FormDesignSurfaceHosting hosting);
        /// <summary>
        /// 活动窗体设计容器当前选择发生更改时发生
        /// 此事件在 Hosting的 SelectionService_SelectionChanged 中调用
        /// </summary>
        public event OnHostingSelectionChangedHandler HostingSelectionChanged;

        public delegate void OnHostingInitFormElementsComponentCompleteHandler(FormDesignSurfaceHosting hosting);
        /// <summary>
        /// Hosting初始化窗体元素完毕后触发该事件
        /// 以供FormPropertyGrid载入可供选择的对象的下拉列表
        /// </summary>
        public event OnHostingInitFormElementsComponentCompleteHandler HostingInitFormElementsComponentComplete;

        #endregion
    }
}
