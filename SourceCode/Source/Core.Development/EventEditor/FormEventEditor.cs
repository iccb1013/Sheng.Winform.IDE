using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Core.Development.Localisation;

namespace Sheng.SailingEase.Core.Development
{
    /// <summary>
    /// 只管编辑，不管checkwarning
    /// 如果编辑完需要checkwarning，应另外调用checkwarning方法
    /// 在编辑事件时，确定后，返回原事件对象（被编辑的对象实现，而不是新的对象实例）
    /// </summary>
    partial class FormEventEditor : FormViewBase
    {
        #region 私有字段

        private static FormEventEditor _instance = null;
        /// <summary>
        /// 当前事件设置窗体实例
        /// 在formload中会为_instance赋值
        /// 这里没有把FormEventSet做成单实例，
        /// 1因为Instance这个属性是后来加的，做单实例改动大
        /// 2.本窗体初例化的时候带有参数，处理改动大
        /// 
        /// 本属性用于调用对象方法事件时，能引到本窗体为树节点加事件设置相关节点
        /// </summary>
        public static FormEventEditor Instance
        {
            get
            {
                return _instance;
            }
        }

        private bool isEdit = false;

        private IEventEditorSupport _eventEditorSupport;

        private EntityBase _entity;

        private BindingList<EventProvideAttribute> _eventProvideAttribute = new BindingList<EventProvideAttribute>();
        private BindingList<EventTimeAbstract> _eventTime = new BindingList<EventTimeAbstract>();

        #endregion

        #region 公开属性

        /// <summary>
        /// 动作对象
        /// </summary>
        private EventBase _event;
        /// <summary>
        /// 新增或正在编辑的动作
        /// </summary>
        public EventBase Event
        {
            get
            {
                return this._event;
            }
            private set
            {
                this._event = value;
            }
        }

        public IEventSupport EventSupport
        {
            get
            {
                return this._entity as IEventSupport;
            }
        }

        public int EventTimeCode
        {
            get
            {
                object selectedValue = availableEventTimes.GetSelectedValue();

                if (selectedValue == null)
                {
                    Debug.Assert(false, "没有取到触发时机");
                    return -1;
                }

                return ((EventTimeAbstract)selectedValue).Code;
            }
        }

        public string EventTimeName
        {
            get
            {
                return EventSupport.GetEventTimeName(EventTimeCode);
            }
        }

        private WindowEntity formEntity;
        /// <summary>
        /// 窗体的数据实体
        /// 有些事件在设置过程中需要用到
        /// 
        /// 另外在事件设置完毕后，也要告诉事件其所在的FormEntity，尤其是新增事件时
        /// 
        /// 原FormEntityDev
        /// </summary>
        public WindowEntity FormEntity
        {
            get
            {
                return this.formEntity;
            }
            set
            {
                this.formEntity = value;
            }
        }

        private EventCollection eventList;
        /// <summary>
        /// 当前动作操作所在的动作序列,用于查找代码是否被占用
        /// </summary>
        public EventCollection EventList
        {
            get
            {
                return this.eventList;
            }
            set
            {
                this.eventList = value;
            }
        }

        #endregion

        #region 窗体构造及窗体事件

        public FormEventEditor(EntityBase hostEntity)
            : this(hostEntity, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity">实体对象,将从此对象中提取其所支持的事件</param>
        /// <param name="eventBase">要编辑的事件</param>
        public FormEventEditor(EntityBase hostEntity, EventBase eventBase)
        {
            InitializeComponent();

            Unity.ApplyResource(this);
            ApplyLanguageResource();

            availableEvents.DisplayMember = EventProvideAttribute.Property_Name;
            availableEvents.DescriptionMember = EventProvideAttribute.Property_Description;

            availableEventTimes.DisplayMember = EventTimeAbstract.Property_Name;
            availableEventTimes.LayoutMode = ListViewLayoutMode.Standard;

            //初始化imageList
            //如果修改这里，注意修改 EditorTreeNodeIcons
            this.imageListParameter.Images.Add(IconsLibrary.EmptyIcon);
            this.imageListParameter.Images.Add(IconsLibrary.Property);
            this.imageListParameter.Images.Add(IconsLibrary.Right);
            this.imageListParameter.Images.Add(IconsLibrary.Method);
            this.imageListParameter.Images.Add(IconsLibrary.Error2);

            this._entity = hostEntity;
            this.Event = eventBase;

            //CheckWarning
            if (this.Event != null)
            {
                IWarningable warningable = this.Event as IWarningable;
                if (warningable != null)
                {
                    warningable.CheckWarning();
                }
            }
        }

        private void FormEventSet_Load(object sender, EventArgs e)
        {
            _instance = this;

            //提取实体对象所支持的事件
            BindingSupportEventList();

            //如果EditEvent不为空，则当前窗体是在编辑模式下，编辑EditEvent
            if (this.Event != null)
                this.isEdit = true;

            //如果不是编辑状态，返回
            if (this.isEdit)
            {
                //加载编辑状态信息

                availableEvents.Enabled = false;

                //选择对应的动作
                availableEvents.SetSelectedValue(EventSupport.EventProvide.GetProvideAttribute(this.Event));

                List<EventTimeAbstract> eventTime = (from c in EventSupport.EventTimeProvide
                                                     where c.Code == this.Event.EventTime
                                                     select c).ToList<EventTimeAbstract>();
                Debug.Assert(eventTime.Count == 1, "触发时机未能正确获得");
                if (eventTime.Count == 1)
                {
                    //选择对应的触发时机
                    availableEventTimes.SetSelectedValue(eventTime[0]);
                }

                //把动作传到用户控件里去,设置相应信息
                CreateEventEditorSupport();

                this._eventEditorSupport.EditorAdapter.BeginEdit();
            }
            else
            {
                availableEvents.Enabled = true;

                availableEvents.SelectedValueChanged += new EventHandler<SEComboSelector2.OnSelectedValueChangedEventArgs>(availableEvents_SelectedValueChanged);
            }
        }

        private void FormEventSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.treeViewParameter.Nodes.Clear();
        }

        #endregion

        #region 应用本地化资源

        public void ApplyLanguageResource()
        {
            this.availableEvents.Title = Language.Current.FormEventEditor_LabelEvent;
            this.availableEventTimes.Title = Language.Current.FormEventEditor_LabelEventTime;


        }

        #endregion

        #region 验证输入

        public override bool DoValidate()
        {
            bool validateResult = true;
            string validateMsg = String.Empty;
            string msg;

            validateResult = SEValidateHelper.ValidateContainerControl(this, out msg);
            if (validateResult == false)
                validateMsg += msg;

            //验证未被显示的参数设置面板，已经显示出来的面板在上面的  ValidateContainerControl 会验证到
            if (this._eventEditorSupport != null)
            {
                //所有参数面板
                foreach (IEventEditorPanel panel in _eventEditorSupport.EditorAdapter.EventEditorPanelList)
                {
                    if ((panel as Control).Parent != null)
                        continue;

                    if (panel.ValidateParameter(out msg) == false)
                    {
                        validateMsg += msg;
                        validateResult = false;
                    }
                }
            }

            if (validateResult == false)
            {
                //去掉多余的换行符
                SEValidateHelper.WipeSpilthSpace(ref validateMsg);

                MessageBox.Show(validateMsg, CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return validateResult;
        }

        #endregion

        #region 事件处理

        void availableEvents_SelectedValueChanged(object sender, SEComboSelector2.OnSelectedValueChangedEventArgs e)
        {
            EventProvideAttribute attribute = e.Value as EventProvideAttribute;

            if (attribute.Code == -1)
            {
                this._eventEditorSupport = null;
                this.treeViewParameter.Nodes.Clear();
                this.panelParameter.Controls.Clear();
                return;
            }

            //EventBase objEvent = EventSupport.EventProvide.CreateInstance(attribute);
            EventBase objEvent = EventDevTypes.Instance.CreateInstance(attribute);
            if (objEvent == null)
                return;

            this.Event = objEvent as EventBase;
            this.Event.HostFormEntity = this.formEntity;

            CreateEventEditorSupport();
        }

        /// <summary>
        /// 点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
            {
                return;
            }

            if (_eventEditorSupport == null)
                return;

            XElement eventXmlElement = _eventEditorSupport.EditorAdapter.GetParameterSetXml();
            eventXmlElement.Attribute("EventTime").Value = EventTimeCode.ToString();

            this.Event.FromXml(eventXmlElement.ToString());
            this.Event.HostFormEntity = this.FormEntity;

            //发出事件已更新的通知
            this.EventSupport.EventUpdate(this);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void treeViewParameter_OnAfterSelectNavigationNode(SENavigationTreeView treeView, SENavigationTreeNode node)
        {
            this.panelParameter.Controls.Clear();
            this.panelParameter.Controls.Add(treeView.AvailabilityNavigationPanel);
        }

        #endregion

        #region 树节点操作

        /// <summary>
        /// 向参数中添加节点
        /// 用于调用对象方法事件，选定对象方法后，所需参数设置的有关面板挂过来
        /// </summary>
        /// <param name="treeNode"></param>
        public void AddParameterTreeNode(EventEditorNode node)
        {
            EventEditorTreeNode treeNode = new EventEditorTreeNode(node);
            this.treeViewParameter.Nodes.Add(treeNode);
            treeNode.ExpandAll();
        }

        /// <summary>
        /// 删除参数面板中的节点
        /// 用于调用对象方法事件，选定对象方法后，把之前的方法面板删除
        /// </summary>
        /// <param name="treeNode"></param>
        public void RemoveParameterTreeNode(EventEditorNode node)
        {
            EventEditorTreeNode treeNode = GetTreeNode(node);
            if (treeNode != null)
                this.treeViewParameter.Nodes.Remove(treeNode);
        }

        /// <summary>
        /// 获取关联到指定对象的TreeNode
        /// </summary>
        /// <param name="editorNode"></param>
        /// <returns></returns>
        private EventEditorTreeNode GetTreeNode(EventEditorNode editorNode)
        {
            return GetTreeNode(editorNode, treeViewParameter.Nodes);
        }

        private EventEditorTreeNode GetTreeNode(EventEditorNode editorNode, TreeNodeCollection nodes)
        {
            foreach (TreeNode treeNode in nodes)
            {
                EventEditorTreeNode eventEditorTreeNode = treeNode as EventEditorTreeNode;
                if (eventEditorTreeNode == null)
                    continue;

                if (eventEditorTreeNode.EditorNode == editorNode)
                    return eventEditorTreeNode;
            }

            foreach (TreeNode treeNode in nodes)
            {
                return GetTreeNode(editorNode, treeNode.Nodes);
            }

            return null;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 绑定对象所支持的事件列表
        /// </summary>
        private void BindingSupportEventList()
        {
            _eventProvideAttribute.Clear();
            _eventTime.Clear();

            //加个空行
            //_eventProvideAttribute.Add(new EventProvideAttribute(String.Empty, -1));

            foreach (Type eventType in EventSupport.EventProvide)
            {
                _eventProvideAttribute.Add(EventSupport.EventProvide.GetProvideAttribute(eventType));
            }

            availableEvents.DataBind(_eventProvideAttribute);

            foreach (EventTimeAbstract eventTime in EventSupport.EventTimeProvide)
            {
                _eventTime.Add(eventTime);
            }

            availableEventTimes.DataBind(_eventTime);
        }

        /// <summary>
        /// 从当前事件对象中提取IEventEditorSupport接口
        /// 然后取出编辑面板节点加到树中
        /// </summary>
        private void CreateEventEditorSupport()
        {
            this._eventEditorSupport = this.Event as IEventEditorSupport;

            this._eventEditorSupport.EditorAdapter.EventCollection = this.EventList;

            this.treeViewParameter.Nodes.Clear();

            #region 如果有警告,显示警告信息

            ////如果有警告,显示警告信息
            IWarningable warningable = this.Event as IWarningable;
            if (warningable != null)
            {
                if (warningable.Warning.ExistWarning)
                {
                    UserControlWarningView warningView = new UserControlWarningView();
                    warningView.ShowWarning(warningable.Warning);

                    TreeNode warningNode = this.treeViewParameter.AddNode(
                        String.Format(Language.Current.FormEventEditor_WarningNode, warningable.Warning.WarningCount), warningView);
                    warningNode.ImageIndex = EditorTreeNodeIcons.Warning;
                    warningNode.SelectedImageIndex = EditorTreeNodeIcons.Warning;
                }
            }

            #endregion

            this.AddParameterTreeNode(_eventEditorSupport.EditorAdapter.EditorNode);

            this.treeViewParameter.ExpandAll();
            this.treeViewParameter.SelectedNode = GetTreeNode(_eventEditorSupport.EditorAdapter.DefaultNode);
        }

        #endregion

        #region EditorTreeNodeIcons

        /// <summary>
        /// 节点树中可显示的图标
        /// </summary>
        public struct EditorTreeNodeIcons
        {
            /// <summary>
            /// 空白 0
            /// </summary>
            public const int EmptyIcon = 0;
            /// <summary>
            /// 属性 1
            /// </summary>
            public const int Property = 1;
            /// <summary>
            /// 向右箭头 2
            /// </summary>
            public const int Right = 2;
            /// <summary>
            /// 方法 3
            /// </summary>
            public const int Method = 3;
            /// <summary>
            /// 警告 4
            /// </summary>
            public const int Warning = 4;
        }

        #endregion
    }
}
