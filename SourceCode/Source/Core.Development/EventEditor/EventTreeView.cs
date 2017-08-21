/*
 *EventTreeViewNode的父节点必须是 EventTreeViewTimeNode.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;

namespace Sheng.SailingEase.Core.Development
{
    public class EventTreeView : SETreeView
    {
        #region 私有成员

        //节点的小图标ImageList
        private ImageList _imageList = new ImageList();

        #endregion

        #region 公开属性

        //还是要用到Entity，不能直接用IEventSupport
        //因为添加事件需要用到Entity
        private EntityBase _entity;
        public EntityBase Entity
        {
            get { return _entity; }
            set
            {
                //先解除现有的事件注册
                if (EventSupport != null)
                {
                    EventSupport.EventUpdated -= EventSupport_EventUpdated;
                }

                _entity = value;

                //注册事件
                if (EventSupport != null)
                {
                    EventSupport.EventUpdated += EventSupport_EventUpdated;
                }

                BuildTree();
            }
        }

        /// <summary>
        /// 因为EntityBase中是没有HostFormEntity这个属性的，只有到了FormElement一层才有
        /// 所以这里额外开一个属性
        /// 原 FormEntityDev
        /// </summary>
        public WindowEntity FormEntity
        {
            get;
            set;
        }

        /// <summary>
        /// 从此提取事件加载到树中
        /// </summary>
        private IEventSupport EventSupport
        {
            get { return _entity as IEventSupport; }
        }

        public EventBase SelectedEvent
        {
            get
            {
                EventTreeNode node = this.SelectedNode as EventTreeNode;
                if (node != null)
                {
                    return node.Event;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region 构造

        public EventTreeView()
        {
            _imageList.Images.Add(IconsLibrary.Event);  //0
            _imageList.Images.Add(IconsLibrary.Method);  //1

            this.ImageList = _imageList;

            //用事件，而不是override，是考虑到继承者再次重写的问题，还是对base的调用是必须调用的
            //那么继承者重写时可能引发意外的问题
            this.MouseDoubleClick += new MouseEventHandler(EventTreeView_MouseDoubleClick);
        }

        #endregion

        #region 私有方法

        private void BuildTree()
        {
            this.SuspendLayout();

            this.Nodes.Clear();

            if (EventSupport == null)
                return;

            foreach (EventTimeAbstract eventTime in EventSupport.EventTimeProvide)
            {
                EventTimeTreeNode timeNode = new EventTimeTreeNode(eventTime);

                //加载该触发时机下的事件
                foreach (EventBase eventBase in EventSupport.Events.GetEventsByTime(eventTime.Code))
                {
                    timeNode.Nodes.Add(new EventTreeNode(eventBase));
                }

                this.Nodes.Add(timeNode);
            }

            this.ExpandAll();

            this.ResumeLayout();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 重新加载事件树
        /// </summary>
        public void ReBuild()
        {
            BuildTree();
        }

        //添加，编辑，删除，上下移动方法公开，是为了让外部按钮调用

        /// <summary>
        /// 添加事件
        /// </summary>
        public void Add()
        {
            if (EventSupport == null)
                return;

            FormEventEditor formEventSet = new FormEventEditor(this.Entity);
            formEventSet.FormEntity = this.FormEntity;
            formEventSet.EventList = EventSupport.Events;

            if (formEventSet.ShowDialog() != DialogResult.OK)
                return;

            EventSupport.Events.Add(formEventSet.Event);

            EventSupport.EventUpdate(this);

            BuildTree();

            if (EventChanged != null)
            {
                EventChanged(new CollectionEditEventArgs(this.EventSupport.Events, CollectionEditType.Add,
                    this.EventSupport.Events.IndexOf(formEventSet.Event), formEventSet.Event));
            }
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        public void Edit()
        {
            EventTreeNode node = this.SelectedNode as EventTreeNode;
            //如果选择的不是事件，直接返回
            if (node == null)
                return;

            //保持当前事件对象的副本，用于在编辑事件之后，进行Compare提取差异
            EventBase oldEvent = null;
            if (EventChanged != null)
            {
                oldEvent = node.Event.Clone() as EventBase;
            }

            //FormEventEditor.Event编辑后还是原对象的引用，在原对象基础上修改的
            FormEventEditor formEventSet = new FormEventEditor(node.Event.HostEntity, node.Event);
            formEventSet.FormEntity = node.Event.HostFormEntity;
            formEventSet.EventList = EventSupport.Events;

            if (formEventSet.ShowDialog() == DialogResult.OK)
            {
                EventSupport.EventUpdate(this);

                //直接重建树
                //如果想根据改变的触发时机来修改当前事件的父节点，涉及到事件的顺序问题
                //因为编辑的事件在移到新的触发时机下面时，不一定是最后一个
                BuildTree();

                //触发事件（如果有）
                if (EventChanged != null)
                {
                    CollectionEditEventArgs args = new CollectionEditEventArgs(this.EventSupport.Events, CollectionEditType.Edit, 
                        this.EventSupport.Events.IndexOf(node.Event), node.Event);
                    args.Members.Inject(ObjectCompare.Compare(oldEvent, node.Event));
                    EventChanged(args);
                }
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        public void Delete()
        {
            EventTreeNode node = this.SelectedNode as EventTreeNode;
            //如果双击的是事件，删除
            if (node != null)
            {
                if (MessageBox.Show(
                    CommonLanguage.Current.ConfirmDelete
                      + Environment.NewLine + Environment.NewLine + node.Name,
                      CommonLanguage.Current.MessageCaption_Notice,
                      MessageBoxButtons.OKCancel,
                      MessageBoxIcon.Question
                      ) != DialogResult.OK)
                {
                    return;
                }

                int index= this.EventSupport.Events.IndexOf(node.Event);

                this.EventSupport.Events.Remove(node.Event);

                EventSupport.EventUpdate(this);

                node.Remove();

                if (EventChanged != null)
                {
                    Dictionary<int, object> values = new Dictionary<int, object>();
                    values.Add(index, node.Event);
                    EventChanged(new CollectionEditEventArgs(this.EventSupport.Events, values));
                }
            }
        }

        /// <summary>
        /// 上移事件
        /// </summary>
        public void Up()
        {
            EventTreeNode node = this.SelectedNode as EventTreeNode;

            if (node == null)
                return;

            EventTreeNode prevNodex = node.PrevNode as EventTreeNode;
            if (prevNodex == null)
                return;

            int nodeOldIndex = this.EventSupport.Events.IndexOf(node.Event);

            this.EventSupport.Events.PreTo(node.Event, prevNodex.Event);

            EventSupport.EventUpdate(this);

            //BuildTree();
            this.SwapNode(node, prevNodex);
            this.SelectedNode = node;

            if (EventOrderChanged != null)
            {
                CollectionEditEventArgs[] eventArgs = new CollectionEditEventArgs[1];
                eventArgs[0] = new CollectionEditEventArgs(this.EventSupport.Events, CollectionEditType.Move,
                    this.EventSupport.Events.IndexOf(node.Event), nodeOldIndex, node.Event);
                EventOrderChanged(eventArgs);
            }
        }

        /// <summary>
        /// 下移事件
        /// </summary>
        public void Down()
        {
            EventTreeNode node = this.SelectedNode as EventTreeNode;

            if (node == null)
                return;

            EventTreeNode nextNodex = node.NextNode as EventTreeNode;
            if (nextNodex == null)
                return;

            int nodeOldIndex = this.EventSupport.Events.IndexOf(node.Event);

            this.EventSupport.Events.NextTo(node.Event, nextNodex.Event);

            EventSupport.EventUpdate(this);

            //BuildTree();
            this.SwapNode(node, nextNodex);
            this.SelectedNode = node;

            if (EventOrderChanged != null)
            {
                CollectionEditEventArgs[] eventArgs = new CollectionEditEventArgs[1];
                eventArgs[0] = new CollectionEditEventArgs(this.EventSupport.Events, CollectionEditType.Move,
                    this.EventSupport.Events.IndexOf(node.Event), nodeOldIndex, node.Event);
                EventOrderChanged(eventArgs);
            }
        }

        #endregion

        #region 处理拖放

      

        protected override bool CanDrag(TreeNode treeNode)
        {
            if (treeNode.GetType().Equals(typeof(EventTreeNode)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

      

        protected override void ProcessDragDrop(TreeNode dragNode, TreeNode dropNode)
        {
            CollectionEditEventArgs[] eventArgs =  new CollectionEditEventArgs[2];

            EventTreeNode node = dragNode as EventTreeNode;
            int nodeOldIndex = this.EventSupport.Events.IndexOf(node.Event);
            EventBase oldEvent = null;

            if (EventOrderChanged != null)
            {
                oldEvent = node.Event.Clone() as EventBase;
            }

            #region 如果目标节点是触发时机

            //如果目标节点是触发时机，把拖动的节点放到这个触发时机的最后面
            //需要调用一次EventCollection.Down方法，同步事件序列的顺序
            //并把拖动的事件的触发时机修改为目标节点同样的触发时机
            if (dropNode.GetType().Equals(typeof(EventTimeTreeNode)))
            {
                //直接调用基类方法完成节点移动
                base.MoveNode(dragNode, dropNode);

                //同步触发事件顺序
                //拿到dragNode的前一个节点（如果有），调用一次Down方法

                //改变触发时机
                EventTimeTreeNode eventTimeNode = dragNode.Parent as EventTimeTreeNode;
                node.Event.EventTime = eventTimeNode.EventTime.Code;

                //确保拖动的事件在事件序列中处于当前触发时机中所有事件的最后
                EventTreeNode eventNode = dragNode.PrevNode as EventTreeNode;
                if (eventNode != null)
                {
                    this.EventSupport.Events.NextTo(node.Event, eventNode.Event);
                }
            }

            #endregion

            #region 如果目标节点是事件

            //如果目标节点是事件，把拖动的事件放到这个事件的前面，需调用Up方法同步事件顺序
            //并把拖动的事件的触发时机修改为目标节点同样的触发时机
            else if (dropNode.GetType().Equals(typeof(EventTreeNode)))
            {
                //如果放在拖动节点的同级下一个节点，do nothing
                if (dropNode.NextNode == dragNode)
                    return;

                //移动树节点
                // Remove drag node from parent
                dragNode.Parent.Nodes.Remove(dragNode);
                // Add drag node to drop node
                dropNode.Parent.Nodes.Insert(dropNode.Index + 1, dragNode);

                EventTreeNode prevNode = dragNode.PrevNode as EventTreeNode;

                //改变触发时机
                node.Event.EventTime = prevNode.Event.EventTime;

                //nextNode不可能为null，就是dragNode
                this.EventSupport.Events.NextTo(node.Event, prevNode.Event);
            }

            #endregion

            if (EventOrderChanged != null)
            {
                CollectionEditEventArgs args0 = new CollectionEditEventArgs(this.EventSupport.Events, 
                    CollectionEditType.Edit, this.EventSupport.Events.IndexOf(node.Event), node.Event);
                args0.Members.Inject(ObjectCompare.Compare(oldEvent, node.Event));
                eventArgs[0] = args0;

                eventArgs[1] = new CollectionEditEventArgs(this.EventSupport.Events, CollectionEditType.Move,
                    this.EventSupport.Events.IndexOf(node.Event), nodeOldIndex, node.Event);

                EventOrderChanged(eventArgs);
            }
        }

        #endregion

        #region 事件处理

        private void EventTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Edit();
        }

        private void EventSupport_EventUpdated(object sender, IEventSupport eventSupport)
        {
            if (sender != this)
                BuildTree();
        }

        #endregion

        #region 事件

        //这里事件的参数直接用CollectionEditEventArgs，因为在树中对事件做的操作，本质都是对事件集合的操作

        //编辑事件也使用 CollectionEditEventArgs ，而不是单独记录新事件，旧事件
        //比较方便，CollectionEditEventArgs直接就能处理这种情况了，还能直接和撤销重做单元结合
        //对事件的编辑是不记录到Member级别的，直接用新旧事件对象互相替换

        public delegate void OnEventChangedHandler(CollectionEditEventArgs e);
        /// <summary>
        /// 新增，编辑，删除事件
        /// </summary>
        public event OnEventChangedHandler EventChanged;

        public delegate void OnEventOrderChangedHandler(CollectionEditEventArgs [] e);
        /// <summary>
        /// 改变Event在事件序列中的位置
        /// 事件数组有两个元素，第一个是改变触发时机，第二个是移动事件的位置
        /// 此事件参数之所以用数组，是因为拖动事件改变事件的触发时机时
        /// 除了更改事件本身的触发时机，还可能会移动事件在整个事件序列中的位置
        /// 如果分两次引发EventChanged事件，外部接收者（主要是考虑到撤销重做引擎）
        /// 不方便判断是否等待下一次事件，是否要封装为事务
        /// </summary>
        public event OnEventOrderChangedHandler EventOrderChanged;

        #endregion

        public struct Images
        {
            public const int Event = 0;
            public const int Method = 1;
        }
    }
}
