

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Components.Window.DesignComponent.Localisation;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;

namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    partial class FormDesignSurfaceHosting : WorkbenchViewBase, ICanSaveForm, IDesignSurfaceHosting
    {
        #region 遮挡窗体设计器的提示Label

        Label _cloakMessage = new Label()
        {
            Text = String.Empty,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = false,
            Dock = DockStyle.Fill
        };

        #endregion

        #region 私有成员

        private IWindowCompontsContainer _windowCompontsContainer = ServiceUnity.WindowCompontsContainer;

        private IWindowComponentService _windowComponentService = ServiceUnity.WindowComponentService;

        private IWorkbenchService _workbenchService = ServiceUnity.WorkbenchService;

        /// <summary>
        /// 正在保存
        /// </summary>
        private bool _saving = false;
        /// <summary>
        /// 用于刷新工具条中的保存按钮是否可用状态
        /// 如果正在保存，把按钮不可用
        /// </summary>
        public bool Saving
        {
            get { return _saving; }
        }

        /// <summary>
        /// 组件是否正在更新中
        /// 这是由用户直接在设计器中拖动组件而引发的
        /// 目的是使PropertyGrid_PropertyChanged事件中能够判断是否需要UpdateView
        /// </summary>
        bool _componentChanging = false;
        public bool ComponentChanging
        {
            get { return _componentChanging; }
        }

        ///// <summary>
        ///// 是否处在属性更新状态,通过属性面板更改对象属性时将此置为true
        ///// 如果为true,componentChangeService_ComponentChanged事件不会执行
        ///// 避免反复执行,但是有一个小问题,就是当updateview后控件实际接受的属性和entity中的值不同时
        ///// 不在能componentChangeService_ComponentChanged中返过来同步了
        ///// </summary>
        //bool _propertyChanging = false;

        /// <summary>
        /// 确定designSurface是否处于卸载状态
        /// </summary>
        bool _designSurfaceUnloading = false;

        /// <summary>
        /// 设计器根容器
        /// </summary>
        IWindowDesignerRootComponent _rootComponentForm;

        private bool _componentListChanged = false;
        /// <summary>
        /// 是否执行了添加或删除了组件的操作
        /// Host_TransactionClosed事件将根据这个决定是否更新属性面板上的对象下拉列表
        /// 使用属性而不是直接操作_componentListChanged是为了方便跟踪调试代码
        /// </summary>
        private bool ComponentListChanged
        {
            get { return _componentListChanged; }
            set { _componentListChanged = value; }
        }

        /// <summary>
        /// 为设计器提供用于选择组件的接口
        /// </summary>
        ISelectionService _selectionService;

        /// <summary>
        /// 为设计器提供用于菜单服务的接口
        /// </summary>
        IMenuCommandService _menuCommandService;

        /// <summary>
        /// 可撤销工作单元集合
        /// 此集合只用在一处：用户直接操作设计器中的对象，如拖动改变位置，大小时
        /// 封装这些操作的可撤销工作单元，因为用户可能同时选择多个对象一操作，所以这里需要一个集合
        /// 这一集合将在每次提交后被清空
        /// </summary>
        SEUndoUnitCollectionFormDesigner _undoUnitList = new SEUndoUnitCollectionFormDesigner();

        #endregion

        #region 公开属性

        private bool _dirty = false;
        public bool Dirty
        {
            get { return _dirty; }
        }

        private bool _loaded = false;
        /// <summary>
        /// DesignSurface 是否已经载入完毕
        /// </summary>
        public bool Loaded
        {
            get { return this._loaded; }
            set { this._loaded = value; }
        }

        private SEDesignSurface _designSurface;
        public SEDesignSurface DesignSurface
        {
            get { return _designSurface; }
        }

        private WindowEntity _windowEntity;
        /// <summary>
        /// 窗体实体对象
        /// </summary>
        public WindowEntity WindowEntity
        {
            get
            {
                return this._windowEntity;
            }
        }

        public ISelectionService SelectionService
        {
            get
            {
                return this._selectionService;
            }
        }

        public IDesignerHost DesignerHost
        {
            get
            {
                if (this._designSurface != null)
                {
                    return this._designSurface.Host;
                }
                else
                {
                    return null;
                }
            }
        }

        private SEUndoEngine _undoEngine;
        /// <summary>
        /// 撤销/重做引擎
        /// </summary>
        public SEUndoEngine UndoEngine
        {
            get { return this._undoEngine; }
        }

        public FormHostingContainer HostingContainer
        {
            get;
            set;
        }

        #region 命令标识符是否可用

        /// <summary>
        /// 显示事件是否可用
        /// </summary>
        public bool EnableEvent
        {
            get
            {
                //只有当前选定的对像个数是一个的时候，才可用
                return this.SelectionService.GetSelectedComponents().Count == 1;
            }
        }

        public bool EnableCut
        {
            get
            {
                return IsMenuCommandEnabled(StandardCommands.Cut);
            }
        }

        public bool EnableCopy
        {
            get
            {
                return IsMenuCommandEnabled(StandardCommands.Copy);
            }
        }

        public bool EnablePaste
        {
            get
            {
                return IsMenuCommandEnabled(StandardCommands.Paste);
            }
        }

        public bool EnableDelete
        {
            get
            {
                return IsMenuCommandEnabled(StandardCommands.Delete);
            }
        }

        public bool EnableSelectAll
        {
            get
            {
                return _designSurface != null;
            }
        }

        #endregion

        #endregion

        #region 构造及窗体事件

        public FormDesignSurfaceHosting(WindowEntity windowEntity, SEDesignSurface designSurface, FormHostingContainer hostingContainer)
        {
            InitializeComponent();

            if (DesignMode)
                return;

            this._windowEntity = windowEntity;

            this.Icon = DrawingTool.ImageToIcon(IconsLibrary.Form);
            this.SetTabText();
            this.HideOnClose = false;

            this._undoEngine = new SEUndoEngineFormDesigner(this);

            this._designSurface = designSurface;

            this.HostingContainer = hostingContainer;

            this.TabPageContextMenuStrip = HostingTabPageContextMenuStrip.Instance.MenuStrip;

            //初始化DesignSurface
            //在此初始化，保证FormHostingContainer中的dockPanel_ActiveDocumentChanged执行时
            //能拿到DesignSurface中的服务
            this._designSurface = InitialseDesignSurface();
        }

        private void FormDesignSurfaceHosting_Load(object sender, EventArgs e)
        {
            #region 绑定撤销/重做引擎事件

            _undoEngine.OnAddUndoUnit += new SEUndoEngine.OnAddUndoUnitHandler(UndoEngineAddUndoUnitEvent);
            _undoEngine.OnRedo += new SEUndoEngine.OnRedoHandler(UndoEngineUndoEvent);
            _undoEngine.OnUndo += new SEUndoEngine.OnUndoHandler(UndoEngineUndoEvent);
            _undoEngine.OnStepRedo += new SEUndoEngine.OnStepRedoHandler(UndoEngineStepUndoEvent);
            _undoEngine.OnStepUndo += new SEUndoEngine.OnStepUndoHandler(UndoEngineStepUndoEvent);

            #endregion
        }

        private void FormDesignSurfaceHosting_Shown(object sender, EventArgs e)
        {
            //显示出“正在加载”这个文本，而不是整个UI卡住
            Application.DoEvents();

            //实始化窗体元素
            InitialseUIElementsComponent();

            #region 绑定事件处理程序

            //在调用 InitialseFormElementsComponent 创建了已有的窗体元素之后
            //为添加、更改、移除或重命名组件的事件添加和移除事件处理程序
            //因为创建已有的窗体元素不需要(也不能)执行ComponentAdded事件中的逻辑
            IComponentChangeService componentChangeService =
                _designSurface.Host.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            componentChangeService.ComponentChanging += new ComponentChangingEventHandler(componentChangeService_ComponentChanging);
            componentChangeService.ComponentChanged += new ComponentChangedEventHandler(componentChangeService_ComponentChanged);
            componentChangeService.ComponentAdded += new ComponentEventHandler(componentChangeService_ComponentAdded);
            componentChangeService.ComponentRemoved += new ComponentEventHandler(componentChangeService_ComponentRemoved);

            //_selectionService.SelectionChanged += new EventHandler(_selectionService_SelectionChanged);

            #endregion

            //调用初始窗体元素完毕的事件
            if (InitFormElementsComponentComplete != null)
                InitFormElementsComponentComplete(this);

            //对FormEntity进行一次CheckWarning操作
            //以工具条上显示出警告信息
            this.CheckWarning();

            HideCloakMessage();

            this.Loaded = true;
        }

        private void FormDesignSurfaceHosting_FormClosing(object sender, FormClosingEventArgs e)
        {
            //判断是否做了更改还没保存 
            //但是需要注意，如果是直接关FormHostingContainer窗体，这些Hosting子窗体的FormClosing，FormClosed都不会触发
            if (_dirty)
            {
                this.Activate();

                SEMessageBox msgBox = new SEMessageBox(SEMessageBoxStyle.CmdLink);
                msgBox.Icon = SEMessageBoxIcon.Question;
                msgBox.Caption = CommonLanguage.Current.MessageCaption_Notice;
                msgBox.Text = String.Format(Language.Current.FormDesignSurfaceHosting_MessageIsSave, this.WindowEntity.Name);
                msgBox.AddButton(
                    new SEMessageBoxButton(CommonLanguage.Current.MessageBoxButton_Yes,
                        Language.Current.FormDesignSurfaceHosting_MessageIsSave_YesButtonDescription) { Result = DialogResult.Yes },
                    new SEMessageBoxButton(CommonLanguage.Current.MessageBoxButton_No,
                        Language.Current.FormDesignSurfaceHosting_MessageIsSave_NoButtonDescription) { Result = DialogResult.No },
                    new SEMessageBoxButton(CommonLanguage.Current.MessageBoxButton_Cancel) { Result = DialogResult.Cancel, IsCancelButton = true }
                    );
                DialogResult result = msgBox.Show(this).Result;

                if (result == DialogResult.Yes)
                {
                    _windowComponentService.Save(this.WindowEntity);
                    _workbenchService.SetStatusMessage(Language.Current.StatusBarMessage_Saved);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        //此事件先于 dockPanel_ActiveDocumentChanged 执行
        private void FormDesignSurfaceHosting_FormClosed(object sender, FormClosedEventArgs e)
        {
            //释放 DesignSurface 之前，移除工具箱服务，否则会连同工具箱一直释放
            this._designSurface.Host.RemoveService((typeof(IToolboxService)));

            //释放 DesignSurface
            //这个必须放在FormClosed事件中,如果放在Closing中,会报一个InvalidOperationException,集合已修改,可能无法执行枚举
            this._designSurface.Dispose();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 显示遮挡窗体设计器的提示文本
        /// </summary>
        /// <param name="msg"></param>
        private void ShowCloakMessage(string msg)
        {
            this._cloakMessage.Text = msg;
            this.panel.Controls.Clear();
            this.panel.Controls.Add(this._cloakMessage);
            Application.DoEvents();
        }

        /// <summary>
        /// 隐藏遮挡窗体设计器的提示文本
        /// 重新显示窗体设计器
        /// </summary>
        private void HideCloakMessage()
        {
            this.panel.Controls.Clear();
            this.panel.Controls.Add(_designSurface.GetView());
            Application.DoEvents();
        }

        /// <summary>
        /// 设置选项卡文本
        /// </summary>
        /// <param name="unSave"></param>
        private void SetTabText()
        {
            this.TabText = String.Format(Language.Current.FormDesignSurfaceHosting_TabText, this._windowEntity.Name);
            if (_dirty)
                this.TabText += " *";
        }

        /// <summary>
        /// 初始化设计界面
        /// </summary>
        /// <returns></returns>
        private SEDesignSurface InitialseDesignSurface()
        {
            //绑定事件
            this._designSurface.Loading += new EventHandler(_designSurface_Loading);
            this._designSurface.Unloading += new EventHandler(_designSurface_Unloading);
            this._designSurface.Host.TransactionClosed += new DesignerTransactionCloseEventHandler(Host_TransactionClosed);

            #region 加载服务

            //共享的服务，如工具箱，在释放DesignSurface前，必须移除
            //否则会连同DesignSurface一起被释放掉

            CodeDomComponentSerializationService codeDomComponentSerializationService =
             new CodeDomComponentSerializationService(this._designSurface.GetServiceContainer());
            this._designSurface.Host.AddService(typeof(ComponentSerializationService), codeDomComponentSerializationService);

            //供一个可调用序列化和反序列化的接口
            this._designSurface.Host.AddService(typeof(IDesignerSerializationService),
                new SEDesignerSerializationService(this._designSurface.GetServiceContainer()));

            //工具盒服务
            this._designSurface.Host.AddService(typeof(IToolboxService), HostingContainer.Toolbox);

            //提供可以生成对象的唯一名称的服务
            this._designSurface.Host.AddService(typeof(INameCreationService), new SENameCreationService());

            //撤销/重复功能
            //_undoEngine= new SEUndoEngine(this._designSurface.GetServiceContainer());
            //this._designSurface.Host.AddService(typeof(UndoEngine), _undoEngine);

            //选择服务
            this._selectionService = (ISelectionService)(this._designSurface.GetService(typeof(ISelectionService)));

            //快捷菜单服务
            this._designSurface.Host.AddService(typeof(IMenuCommandService),
                new DesignerMenuCommandService(this._designSurface.GetServiceContainer()));

            this._menuCommandService = (IMenuCommandService)_designSurface.GetService(typeof(IMenuCommandService));

            //事件服务
            DesignerEventBindingService designerEventBindingService = new DesignerEventBindingService(this._designSurface.GetServiceContainer());
            this._designSurface.Host.AddService(typeof(IEventBindingService), designerEventBindingService);

            #endregion

            //创建窗体根对象
            //必须在获取View前创建,此函数完毕后需要使用View呈现
            this._rootComponentForm = this._designSurface.CreateRootComponent(
                _windowCompontsContainer.GetWindowDesignerRootComponent(), this._windowEntity.Size)
                as IWindowDesignerRootComponent;

            _rootComponentForm.Entity = _windowEntity;
            _rootComponentForm.UpdateView();

            this._designSurface.GetView().BackColor = Color.White;
            return this._designSurface;
        }

        /// <summary>
        /// 更新属性面板中的元素列表
        /// 目前需要在撤销引擎中调用
        /// 不在这里调用是因为撤销，重做时可能是一个事务处理，避免不断的更新属性面板中的元素下拉列表
        /// 此方法会忽略 _componentListChanged 直接更新下拉列表，但是完成后会将 _componentListChanged 设置为 false
        /// </summary>
        private void UpdatePropertyPadComponentList()
        {
            //更新当前可选控件
            this.HostingContainer.FormPropertyGrid.SelectableObjects = this._designSurface.Host.Container.Components;

            ComponentListChanged = false;
        }

        /// <summary>
        /// 根据实体对象初始化设计器中的组件
        /// </summary>
        private void InitialseUIElementsComponent()
        {
            Debug.WriteLine("加载当前窗体元素");

            //创建窗体元素
            foreach (UIElement element in this._windowEntity.Elements)
            {
                CreateControl(element);
            }
        }

        /// <summary>
        /// 为控件创建并绑定实体对象
        /// </summary>
        /// <param name="shellControlDev"></param>
        private void CreateEntity(IShellControlDev shellControlDev)
        {
            Type type = shellControlDev.GetType();
            object [] attributes = type.GetCustomAttributes(typeof(RuntimeControlDesignSupportAttribute),false);
            if (attributes.Length == 1)
            {
                RuntimeControlDesignSupportAttribute attr = (RuntimeControlDesignSupportAttribute)attributes[0];
                UIElement entity = Activator.CreateInstance(attr.EntityType) as UIElement;
                if (entity == null)
                {
                    Debug.Assert(false, "无法创建实体对象，RuntimeControlDesignSupportAttribute.EntityType 不是 UIElement");
                    return;
                }
                entity.Code = _windowEntity.CreationElementCode(attr.CodeCreationPrefix);
                entity.Name = entity.Code;
                shellControlDev.InitializationEntity(entity);
                shellControlDev.Entity = entity;
            }
            else
            {
                Debug.Assert(false, "无法创建实体对象，没有找到 RuntimeControlDesignSupportAttribute");
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 根据实体对象在窗体根窗口中创建对象
        /// 公开方法是撤销重做引擎需要用到
        /// 需要public出来给撤销重做引擎调用
        /// </summary>
        /// <param name="element"></param>
        public void CreateControl(UIElement element)
        {
            //Control control = ControlFactory.InitControl(element, this._designSurface);

            IFormElementEntityDev entityDev = element as IFormElementEntityDev;
            if (entityDev == null)
            {
                throw new NotImplementedException();
            }

            //TODO:考虑在IDE中继承DesignSurface，并重写CreateControl，在调用基类的CreateControl后，直接把Entity对象放进去
            Control control = this._designSurface.CreateControl(entityDev.DesignerControlType, element.Size, element.Location);

            IShellControlDev shellControlDev = control as IShellControlDev;

            element.HostFormEntity = this._windowEntity;
            shellControlDev.Entity = element;

            Debug.WriteLine("挂上Entity : " + element.GetType().Name);

            shellControlDev.UpdateView();

            //这种情况发生在撤销重做引擎中
            //初始化的时候，element是在formEntity中的，但是在撤销时，肯定不在，已经被移除了
            //if (!_formEntity.Elements.Contains(element))
            //{
            //    _formEntity.Elements.Add(element);
            //}
        }

        /// <summary>
        /// 找到实体对象所关联的组件，将其销毁
        /// 需要public出来给撤销重做引擎调用
        /// </summary>
        /// <param name="element"></param>
        public void DestroyControl(UIElement element)
        {
            IFormElementEntityDev entityDev = element as IFormElementEntityDev;
            if (entityDev == null)
            {
                throw new NotImplementedException();
            }

            this._designSurface.DestroyComponent(entityDev.Component);
        }

        /// <summary>
        /// 更新控件，组件的可视状态
        /// 通过获取组件的IShellControlDev接口，调用UpdateView方法
        /// </summary>
        /// <param name="components"></param>
        public void UpdateView(IComponent[] components)
        {
            for (int i = 0; i < components.Length; i++)
            {
                IShellControlDev shellControlDev = components[i] as IShellControlDev;

                if (shellControlDev == null)
                {
                    Debug.WriteLine("component 没有实现 IShellControlDev 接口");
                    continue;
                }

                //这里try catch目前是为了兼容设置字体时可能出现的异常
                //见 ElementFont 中的说明
                try
                {
                    shellControlDev.UpdateView();
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, "shellControlDev.UpdateView()失败", ex.Message);

                    //UpdateView不成功,根据控件状态同步实体对象
                    //暂时不更新属性行中的显示了
                    //对于主要要解决的字体设置的问题,update时字体对象会clone一个新的
                    //这里调e.Row.UpdateProperty是没用的,因为行上还是旧的对象,重新设置新的对象也比较麻烦,要考虑多选的问题
                    //字体出错的问题比较少,这里只要对象同步了也没有大问题,暂不过多处理了
                    shellControlDev.UpdateEntity();
                    PropertyGridDataErrorView formDataError = new PropertyGridDataErrorView(ex.Message);
                    formDataError.ShowDialog();
                }
            }
        }

        public void UpdateView(EntityBase entity)
        {
            UpdateView(new EntityCollection(new EntityBase[] { entity }));
        }

        /// <summary>
        /// 更新控件，组件的可视状态
        /// </summary>
        /// <param name="entitys"></param>
        public void UpdateView(EntityCollection entitys)
        {
            IComponent[] components = new IComponent[entitys.Count];
            for (int i = 0; i < entitys.Count; i++)
            {
                IFormElementEntityDev entityDev = entitys[i] as IFormElementEntityDev;
                if (entityDev == null)
                {
                    Debug.WriteLine("UpdateView 时指定的实体对象没有实现 IFormElementEntityDev 接口");
                    continue;
                }

                components[i] = entityDev.Component;
            }

            UpdateView(components);
        }

        public void Cut()
        {
            _menuCommandService.GlobalInvoke(StandardCommands.Cut);
        }

        public void Copy()
        {
            _menuCommandService.GlobalInvoke(StandardCommands.Copy);
        }

        public void Paste()
        {
            _menuCommandService.GlobalInvoke(StandardCommands.Paste);
        }

        public void Delete()
        {
            _menuCommandService.GlobalInvoke(StandardCommands.Delete);
        }

        public void SelectAll()
        {
            _menuCommandService.GlobalInvoke(StandardCommands.SelectAll);
        }

        public void MakeDirty()
        {
            this._dirty = true;

            SetTabText();
        }

        public void CleanDirty()
        {
            this._dirty = false;

            SetTabText();
        }

        /// <summary>
        /// 对FormEntity 进行CheckWarning,并将警告数量显示在警告按钮上
        /// </summary>
        public void CheckWarning()
        {
            //this.FormEntity.CheckWarning();
            IWarningable warningable = this.WindowEntity as IWarningable;
            if (warningable != null)
                warningable.CheckWarning();
        }

        /// <summary>
        /// 保存窗体
        /// </summary>
        public void Save()
        {
            if (_saving)
                return;

            //TODO:在保存时灰掉保存按钮
            //TODO:增加saving和saved事件
            _saving = true;
            //this.toolStripButtonSave.Enabled = false;
            //FormOperator.Instance.Update(FormDesignerManager.ActiveFormEntity);
            _windowComponentService.Save(this.WindowEntity);
            CheckWarning();
            this.CleanDirty();
            //toolStripButtonSave.Enabled = true;
            _workbenchService.SetStatusMessage(Language.Current.StatusBarMessage_Saved);
            //FormForm.MakeDirty();
            _saving = false;
        }

        /// <summary>
        /// 提交可撤销的工作单元集合（如果需要），提交之后清空临时可撤销工作单元集合
        /// </summary>
        public void CommitUndoUnitList()
        {
            if (this._undoUnitList.Count > 0)
            {
                this._undoUnitList.Clearup();
                this._undoEngine.AddUndoUnit(_undoUnitList);
                this._undoUnitList.Clear();
            }
        }

        /// <summary>
        /// 向临时可撤销工作单元添加ISEUndoUnit
        /// </summary>
        /// <param name="undoUnit"></param>
        public void AddUndoUnitList(SEUndoUnitAbstract undoUnit)
        {
            _undoUnitList.Add(undoUnit);
        }

        /// <summary>
        /// 判断一个命令标识符是否可用
        /// </summary>
        /// <param name="commandID"></param>
        /// <returns></returns>
        public bool IsMenuCommandEnabled(CommandID commandID)
        {
            if (_designSurface == null)
            {
                return false;
            }

            if (_menuCommandService == null)
            {
                return false;
            }

            System.ComponentModel.Design.MenuCommand menuCommand = _menuCommandService.FindCommand(commandID);
            if (menuCommand == null)
            {
                return false;
            }

            //int status = menuCommand.OleStatus;
            return menuCommand.Enabled;
        }

        /////////////

        #endregion

        #region 事件处理

        #region DesignSurface 事件

        private void _designSurface_Loading(object sender, EventArgs e)
        {
            ShowCloakMessage(Language.Current.FormDesignSurfaceHosting_LabelLoadingMsg);
        }

        /// <summary>
        /// 设计器容器卸载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _designSurface_Unloading(object sender, EventArgs e)
        {
            this._designSurfaceUnloading = true;

            ShowCloakMessage(Language.Current.FormDesignSurfaceHosting_LabelUnloadingMsg);
        }

        /// <summary>
        /// 此事件在关闭事务后发生
        /// 在设计器中操作对象后引发，通过属性面板更新对象不会引发此事件
        /// 注意：设计器中，添加组件不会进这个事件，只有变更组件，删除组件才会进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Host_TransactionClosed(object sender, DesignerTransactionCloseEventArgs e)
        {
            if (!e.LastTransaction)
                return;

            #region 提交可撤销的工作单元集合

            //提交可撤销的工作单元集合（如果需要），提交之后清空临时可撤销工作单元集合
            if (this._undoUnitList.Count > 0)
            {
                this._undoUnitList.Clearup();
                this._undoEngine.AddUndoUnit(_undoUnitList);
                this._undoUnitList.Clear();
            }

            #endregion

            #region 如果执行了添加或删除组件操作，更新属性面板上面下拉框

            //如果执行了添加或删除组件操作，更新属性面板上面下拉框
            //放在 TransactionClosed 事件里而不是 Add 或 Remove 事件里，是因为在批量剪切粘贴时
            //Add 或 Remove 会多次执行，而 TransactionClosed 只会在完成操作时触发
            //需判断是否是撤销重做引擎引发的事件，如果是，不在这里更新，在撤销重做引擎的OnTransactionClosed事件中更新
            if (ComponentListChanged && !_undoEngine.Working)
            {
                UpdatePropertyPadComponentList();
            }

            #endregion

            #region 同步组件属性到实体对象

            //这段代码要处理的典型情况是粘贴组件时，同步组件实体对象的坐标属性值
            //在 ComponentChanged 事件中同步还不行，ComponentChanged事件中跟不到组件新的位置坐标
            //只能跟到组件左上角坐标为15,15的时候
            //估计是设计器在使用复制的组件初始化新组件后，不再引发ComponentChanged事件
            //初始化完毕后进TransactionClosed

            ICollection selectedComponents = this._selectionService.GetSelectedComponents();

            if (selectedComponents != null)
            {
                //ICollection不支持下标访问
                object[] selArray = new object[selectedComponents.Count];
                selectedComponents.CopyTo(selArray, 0);

                for (int i = 0; i < selectedComponents.Count; i++)
                {
                    IShellControlDev shellControlDev = selArray[i] as IShellControlDev;
                    if (shellControlDev != null)
                        shellControlDev.UpdateEntity();
                }
            }

            #endregion

        }

        #endregion

        #region DesignSurface 服务事件

    

        private void componentChangeService_ComponentChanging(object sender, ComponentChangingEventArgs e)
        {
            // this._componentChanging = true;
        }

        /// <summary>
        /// 在组件属性发生更改时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void componentChangeService_ComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            if (_designSurfaceUnloading)
                return;

           

            if (this.HostingContainer.PropertyChanging)
                return;

            //同步到控件所关联的Entity中

            Control control = e.Component as Control;
            if (control == null)
                return;

            IShellControlDev shellControlDev = control as IShellControlDev;

            if (shellControlDev == null)
                throw new NotImplementedException();

            if (shellControlDev.Entity == null)
                return;

            if (shellControlDev.ViewUpdating)
                return;

            this._componentChanging = true;

            //在ComponentAdded之后,ComponentChanged会进来多次
            //虽然ComponentAdded事件中已经处理了控件的显示文本,但是最后一次进入ComponentChanged时
            //其显示文本又会是类型名开头的文本,不知道为什么,看调用堆栈是从Toolbox过来的
            //这里做个判断,若不同强制其更新使用Entity中的文本
            if (control.Text != shellControlDev.GetText())
                control.Text = shellControlDev.GetText();


            //start:这两行代码拿到Host_TransactionClosed里还不行
            //添加控件后触发Host_TransactionClosed事件中，
            //this._selectionService.GetSelectedComponents()取下来的不是添加的那个控件，而是根容器窗体，
            //或者说添加控件的父控件（暂没实现多级，这一说法未测，目前的情况就是窗体）
            //而如果是改变控件的ZOrder，进到这里时，取到的 e.Component是被改变的控件的父控件
            //需要到Host_TransactionClosed事件中，去同步实体对象以更新ZOrder
            //注意的是，撤销重做操作均不引发ComponentChanged和Host_TransactionClosed事件，需在Undo,Redo操作中同步ZOrder
            shellControlDev.UpdateEntity();

            //通知属性网格属性已更改
            //不能在这里判断_componentChanging，因为更新属性行上的属性值还是必须的
            //需要在属性值更新后引发的PropertyGrid_PropertyChanged事件中判断
            this.HostingContainer.UpdatePropertyGrid();
            //end

            #region 封装可撤销的工作单元

           
            if (!_undoEngine.Working)// && !e.NewValue.Equals(e.OldValue))
            {
               
                if (e.Member.Name != "Controls")
                {
                    SEUndoUnitFormDesigner undoUnit = new SEUndoUnitFormDesigner(e.Member.Name);
                    undoUnit.Type = SEUndoUnitFormDesigner.UndoUnitType.ComponentChanged;
                    undoUnit.Components = e.Component as IComponent;
                    undoUnit.Entity = shellControlDev.Entity;
                    undoUnit.Members.Add(e.Member.Name, e.OldValue, e.NewValue);
                    this._undoUnitList.Add(undoUnit);
                }
            }

            #endregion

            this._componentChanging = false;

            MakeDirty();

            Debug.WriteLine("对象属性已更新:" + shellControlDev.Entity.Code);
        }

        /// <summary>
        /// 在组件已添加时发生
        /// 此事件在 FormFormDesinger_Load 执行 InitialseFormElementsComponent() 后挂接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void componentChangeService_ComponentAdded(object sender, ComponentEventArgs e)
        {
            //这个事件里的代码主要是为添加的组件添加实体对象

            Control control = e.Component as Control;

            //如果是窗体,返回
            //这里不能和_rootComponentForm比较,_designSurface.CreateRootComponent执行后结果才会赋值到_rootComponentForm
            //另外窗体所对应的实体对象不在这里设置
            if (control is IWindowDesignerRootComponent)
                return;

            //为新增的组件绑定一个实体对象
            IShellControlDev shellControlDev = control as IShellControlDev;

           
            if (shellControlDev.Entity == null)
            {
                //shellControlDev.CreateEntity(this._formEntity);
                CreateEntity(shellControlDev);
                _windowEntity.Elements.Add((UIElement)shellControlDev.Entity);
            }
            else
            {
                
                if (this.WindowEntity.Elements.Contains(shellControlDev.Entity as UIElement) == false)
                {
                    UIElement formElementEntity = shellControlDev.Entity.Clone() as UIElement;

                    //要先清除，不能直接赋值
                    //因为IShellControlDev.Entity的set中有逻辑处理.会把set过来的Entity对象放到原Entity对象一样的FormEntity中
                    shellControlDev.ClearEntity();

                    shellControlDev.Entity = formElementEntity;

                    //添加实体对象到当前FormEntity.Elements中
                    this.WindowEntity.Elements.Add(formElementEntity);
                }
            }

            
            shellControlDev.UpdateView();

            ComponentListChanged = true;

            #region 封装可撤销的工作单元

            //这里也要考虑同时添加多个组件的可能，比如粘贴操作，需要把同时添加的多个组件封装为一个事务
            if (!_undoEngine.Working)
            {
                SEUndoUnitFormDesigner undoUnit = new SEUndoUnitFormDesigner("ComponentAdded");
                undoUnit.Type = SEUndoUnitFormDesigner.UndoUnitType.ComponentAdded;
                undoUnit.Components = e.Component as IComponent;
               
                this._undoUnitList.Add(undoUnit);
            }

            #endregion

            Debug.WriteLine("增加了窗体对象:" + shellControlDev.Entity.Code + "( " + shellControlDev.Entity.GetType().Name + " )");
            Debug.WriteLine("当前窗体元素个数:" + this._windowEntity.Elements.Count);
        }

        /// <summary>
        /// 在组件已移除时发生
        /// 注意:在 DesignSurface Dispose 时,所有组件都会触发此事件
        /// 所以在 DesignSurface 的 Unloading 事件里注销了此事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void componentChangeService_ComponentRemoved(object sender, ComponentEventArgs e)
        {
            if (_designSurfaceUnloading)
                return;

            Control control = e.Component as Control;
            IShellControlDev shellControlDev = control as IShellControlDev;

            //将关联的实体对象从formEntity中删除
            this._windowEntity.Elements.Remove((UIElement)shellControlDev.Entity);

            ComponentListChanged = true;

            #region 封装可撤销的工作单元

            //这里也要考虑同时删除多个组件的可能，比如粘贴操作，需要把同时添加的多个组件封装为一个事务
            if (!_undoEngine.Working)
            {
                SEUndoUnitFormDesigner undoUnit = new SEUndoUnitFormDesigner("ComponentRemoved");
                undoUnit.Type = SEUndoUnitFormDesigner.UndoUnitType.ComponentRemoved;
                undoUnit.Components = e.Component as IComponent;
                undoUnit.Entity = shellControlDev.Entity;
                this._undoUnitList.Add(undoUnit);
            }

            #endregion

            Debug.WriteLine("删除了窗体对象:" + shellControlDev.Entity.Code);
            Debug.WriteLine("当前窗体元素个数:" + this._windowEntity.Elements.Count);
        }

        #endregion

        #endregion

        #region 与撤销，重做引擎有关的方法和属性 UndoEngine

        #region 公开属性

        /// <summary>
        /// 当前是否可撤销
        /// </summary>
        public bool EnableUndo
        {
            get
            {
                if (_undoEngine != null)
                {
                    return _undoEngine.EnableUndo;
                }
                return false;
            }
        }

        /// <summary>
        /// 当前是否可重做
        /// </summary>
        public bool EnableRedo
        {
            get
            {
                if (_undoEngine != null)
                {
                    return _undoEngine.EnableRedo;
                }
                return false;
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        public void Undo()
        {
            if (EnableUndo)
            {
                _undoEngine.Undo();
            }
        }

        public void Undo(int step)
        {
            //if (EnableUndo)
            //{
            //    _undoEngine.Undo(step);
            //}

            if (EnableUndo == false)
                return;

            if (step > 1)
            {
                ShowCloakMessage(Language.Current.FormDesignSurfaceHosting_LabelWorkingMsg);

                //取消当前选中的对象，避免鼠标闪烁
                this.SelectionService.SetSelectedComponents(new object[] { this._designSurface.Host.RootComponent });
            }

            _undoEngine.Undo(step);

            if (step > 1)
            {
                HideCloakMessage();
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public void Redo()
        {
            if (EnableRedo)
            {
                _undoEngine.Redo();
            }
        }

        public void Redo(int step)
        {
            //if (EnableRedo)
            //{
            //    _undoEngine.Redo(step);
            //}

            if (EnableRedo == false)
                return;

            if (step > 1)
            {
                ShowCloakMessage(Language.Current.FormDesignSurfaceHosting_LabelWorkingMsg);

                //取消当前选中的对象，避免鼠标闪烁
                this.SelectionService.SetSelectedComponents(new object[] { this._designSurface.Host.RootComponent });
            }

            _undoEngine.Redo(step);

            if (step > 1)
            {
                HideCloakMessage();
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 添加可撤销的工作单元
        /// </summary>
        /// <param name="undoUnits"></param>
        public void AddUndoUnit(SEUndoUnitCollection undoUnits)
        {
            this._undoEngine.AddUndoUnit(undoUnits);
        }

        public void AddUndoUnit(SEUndoUnitAbstract undoUnit)
        {
            this._undoEngine.AddUndoUnit(undoUnit);
        }

        #endregion

        #region 事件处理

        private void UndoEngineUndoEvent(SEUndoUnitAbstract unit)
        {
            //事务结束后，需要判断此事务中的工作单元是否涉及到组件的添加，删除
            //如果涉及，需要更新属性面板中的组件下拉列表
            if (this.ComponentListChanged)
            {
                UpdatePropertyPadComponentList();
            }
        }

        private void UndoEngineStepUndoEvent(int step)
        {

            if (this.ComponentListChanged)
            {
                UpdatePropertyPadComponentList();
            }
        }

        private void UndoEngineAddUndoUnitEvent(SEUndoUnitAbstract undoUnit)
        {
        }

        #endregion

        #endregion

        #region ICanSaveForm 成员

        public string SaveFormTitle
        {
            get { return String.Format("{0}({1})", this.WindowEntity.Name, this.WindowEntity.Code); }
        }

        #endregion

        #region 事件

        public delegate void OnInitFormElementsComponentCompleteHandler(FormDesignSurfaceHosting hosting);
        /// <summary>
        /// 初始化窗体元素完毕后触发该事件
        /// 以供FormPropertyGrid载入可供选择的对象的下拉列表
        /// </summary>
        public event OnInitFormElementsComponentCompleteHandler InitFormElementsComponentComplete;

        #endregion

        #region IDesignSurfaceHosting 成员

        //目前只有一个地方会调用，SEUndoEngineFormDesigner
        public void UpdatePropertyGrid(bool updateSelectedObject)
        {
            this.HostingContainer.UpdatePropertyGrid(updateSelectedObject);
        }

        #endregion
    }
}
