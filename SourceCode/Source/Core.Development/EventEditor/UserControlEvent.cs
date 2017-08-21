using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;

namespace Sheng.SailingEase.Core.Development
{
    /// <summary>
    /// 动作操作用户控件
    /// 可以用到需要设置动作的对象操作界面上
    /// 使用EventList来获取或设置动作列表
    /// </summary>
    [ToolboxItem(false)]
    public partial class UserControlEvent : UserControlViewBase
    {
        #region 事件

        /// <summary>
        /// 编辑或删除动作之后执行的外部动作委托
        /// 用于通知父窗体checkwarning
        /// </summary>
        public delegate void OnEditedEventHandler(object sender,CollectionEditEventArgs e);

        public event OnEditedEventHandler OnEdited;

        #endregion

        #region 私有字段

        /// <summary>
        /// 呈现在DataGrid中的事件列表
        /// </summary>
        BindingList<EventBase> _eventBindingList;

        #endregion

        #region 公开属性

        private EntityBase _hostEntity;
        /// <summary>
        /// 事件的宿主实体对象
        /// </summary>
        public EntityBase HostEntity
        {
            get { return this._hostEntity; }
            set
            {
                //改变所关联的实体对象
                this._hostEntity = value;

                IEventSupport eventSupport = value as IEventSupport;
                if (eventSupport == null || eventSupport.EventProvide.Count == 0)
                {
                    this.toolStrip.Enabled = false;
                    this.dataGridViewEvent.WaterText = Language.Current.UserControlEvent_EventNotSupport;
                    this.dataGridViewEvent.Enabled = false;
                    this.EventList = null;
                }
                else
                {
                    this.toolStrip.Enabled = true;
                    this.dataGridViewEvent.WaterText = String.Empty;
                    this.dataGridViewEvent.Enabled = true;
                    this.EventList = eventSupport.Events;
                }
            }
        }

        private WindowEntity _formEntity;
        /// <summary>
        /// 所属的窗口实体
        /// 可能为null，例如在为主菜单设置动作时
        /// 
        /// 原 FormEntityDev
        /// </summary>
        public WindowEntity FormEntity
        {
            get
            {
                return this._formEntity;
            }
            set
            {
                this._formEntity = value;
            }
        }

        private EventCollection _eventList = new EventCollection(null, null);
        /// <summary>
        /// 事件列表
        /// </summary>
        public EventCollection EventList
        {
            get
            {
                return this._eventList;
            }
            set
            {
                this._eventList = value;

                //设置BindingList
                if (value != null)
                {
                    _eventBindingList = new BindingList<EventBase>(value);
                }
                else
                {
                    _eventBindingList = new BindingList<EventBase>();
                }

                dataGridViewEvent.DataSource = _eventBindingList;
            }
        }

        #endregion

        #region 用户控件构造,事件,及资源应用

        public UserControlEvent(EntityBase entity)
        {
            InitializeComponent();         

            UIHelper.ProcessDataGridView(this.dataGridViewEvent);
            this.dataGridViewEvent.AutoGenerateColumns = false;

            this.toolStrip.Renderer = ToolStripRenders.ControlToControlLight;
            this.contextMenuStrip.Renderer = ToolStripRenders.Default;

            Unity.ApplyResource(this);
            ApplyIconResource();

            this.HostEntity = entity;
        }

        /// <summary>
        /// 控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlEvent_Load(object sender, EventArgs e)
        {
            this.dataGridViewEvent.Select();
        }

        /// <summary>
        /// 应用图标资源
        /// </summary>
        private void ApplyIconResource()
        {
            this.toolStripButtonAddEvent.Image = IconsLibrary.New2;
            this.toolStripButtonEditEvent.Image = IconsLibrary.Edit;
            this.toolStripButtonDeleteEvent.Image = IconsLibrary.Delete;

            this.toolStripButtonUpEvent.Image = IconsLibrary.Up;
            this.toolStripButtonDownEvent.Image = IconsLibrary.Down;

            this.toolStripMenuItemUp.Image = IconsLibrary.Up;
            this.toolStripMenuItemDown.Image = IconsLibrary.Down;
        }

        #endregion

        #region 工具栏事件

        /// <summary>
        /// 添加动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonAddEvent_Click(object sender, EventArgs e)
        {
            Add();
        }

        /// <summary>
        /// 编辑动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonEditEvent_Click(object sender, EventArgs e)
        {
            Edit();
        }

        /// <summary>
        /// 删除动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonDeleteEvent_Click(object sender, EventArgs e)
        {
            Delete();
        }

        /// <summary>
        /// 向上调整动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonUpEvent_Click(object sender, EventArgs e)
        {
            Up();
        }

        /// <summary>
        /// 向下调整动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonDownEvent_Click(object sender, EventArgs e)
        {
            Down();
        }

        #endregion

        #region DataGridView事件

        /// <summary>
        /// 双击列表中的单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewEvent_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            Edit();
        }

        private void dataGridViewEvent_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //如果选中了多行，屏蔽编辑按钮和上下移动按钮
            if (this.dataGridViewEvent.SelectedRows.Count > 1)
            {
                this.toolStripButtonEditEvent.Enabled = false;
                this.toolStripButtonUpEvent.Enabled = false;
                this.toolStripButtonDownEvent.Enabled = false;

                this.toolStripMenuItemEdit.Enabled = false;
                this.toolStripMenuItemUp.Enabled = false;
                this.toolStripMenuItemDown.Enabled = false;
            }
            else
            {
                this.toolStripButtonEditEvent.Enabled = true;
                this.toolStripButtonUpEvent.Enabled = true;
                this.toolStripButtonDownEvent.Enabled = true;

                this.toolStripMenuItemEdit.Enabled = true;
                this.toolStripMenuItemUp.Enabled = true;
                this.toolStripMenuItemDown.Enabled = true;
            }
        }

        /// <summary>
        /// 标记出有警告的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewEvent_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewEvent.Rows)
            {
                //dr有可能为null?忘记为什么要判断了
                if (dr != null)
                {
                    IWarningable warningable = dr.DataBoundItem as IWarningable;
                    if (warningable == null)
                        continue;

                    //检查警告
                    warningable.CheckWarning();

                    if (warningable.Warning.ExistWarning)
                    {
                        dr.DefaultCellStyle.BackColor = UIHelper.DataGridViewRowBackColorWarning;
                        dr.DefaultCellStyle.SelectionBackColor = UIHelper.DataGridViewSelectedRowBackColorWarning;
                        dr.DefaultCellStyle.ForeColor = UIHelper.DataGridViewRowForeColorWarning;
                        dr.DefaultCellStyle.SelectionForeColor = UIHelper.DataGridViewSelectedRowForeColorWarning;
                    }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = UIHelper.DataGridViewRowBackColorNormal;
                        dr.DefaultCellStyle.SelectionBackColor = UIHelper.DataGridViewSelectedRowBackColorNormal;
                        dr.DefaultCellStyle.ForeColor = UIHelper.DataGridViewRowForeColorNormal;
                        dr.DefaultCellStyle.SelectionForeColor = UIHelper.DataGridViewSelectedRowForeColorNormal;
                    }
                }
            }
        }

        #endregion

        #region 内部方法

        private void EventUpdate()
        {
            IEventSupport eventSupport = this.HostEntity as IEventSupport;
            if (eventSupport != null)
            {
                eventSupport.EventUpdate(this);
            }
        }

        private void Add()
        {
            FormEventEditor formEventSet = new FormEventEditor(this._hostEntity);
            formEventSet.FormEntity = this.FormEntity;
            formEventSet.EventList = this.EventList;

            if (formEventSet.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.EventList.Add(formEventSet.Event);

            this._eventBindingList.ResetBindings();

            EventUpdate();

            if (this.OnEdited != null)
            {
                OnEdited(this, new CollectionEditEventArgs(this.EventList,
                    CollectionEditType.Add, this.EventList.IndexOf(formEventSet.Event), formEventSet.Event));
            }
        }

        private void Edit()
        {
            if (dataGridViewEvent.SelectedRows.Count != 1)
            {
                return;
            }

            EventBase even = this.dataGridViewEvent.SelectedRows[0].DataBoundItem as EventBase;
            int index = this.EventList.IndexOf(even);

            //保持当前事件对象的副本，用于在编辑事件之后，进行Compare提取差异
            EventBase oldEvent = even.Clone() as EventBase;

            //FormEventEditor.Event编辑后还是原对象的引用，在原对象基础上修改的
            FormEventEditor formEventSet = new FormEventEditor(this._hostEntity, even);
            //这里暂时还能用using，因为窗体释放时，会把事件的编辑面板也释放掉，要另外处理这个问题
            //using (FormEventEditor formEventSet = new FormEventEditor(this._entity, even))
            //{
                formEventSet.FormEntity = this.FormEntity;
                formEventSet.EventList = this.EventList;

                if (formEventSet.ShowDialog() == DialogResult.OK)
                {
                    //CheckWarning
                    IWarningable warningable = formEventSet.Event as IWarningable;
                    if (warningable != null)
                    {
                        warningable.CheckWarning();
                    }

                    _eventBindingList.ResetItem(index);

                    EventUpdate();

                    if (this.OnEdited != null)
                    {
                        CollectionEditEventArgs args = new CollectionEditEventArgs(this.EventList, 
                            CollectionEditType.Edit, index, formEventSet.Event);
                        args.Members.Inject(ObjectCompare.Compare(oldEvent, formEventSet.Event));
                        OnEdited(this, args);
                    }
                }
            //}
        }

        private void Delete()
        {
            if (dataGridViewEvent.SelectedRows.Count == 0)
            {
                return;
            }

            string eventtName = String.Empty;
            string[] eventId = new string[this.dataGridViewEvent.SelectedRows.Count];

            for (int i = 0; i < this.dataGridViewEvent.SelectedRows.Count; i++)
            {
                EventBase even = this.dataGridViewEvent.SelectedRows[i].DataBoundItem as EventBase;

                eventtName += "[" + even.Name + "],";
                eventId[i] = even.Id;
            }

            eventtName = eventtName.TrimEnd(',');

            if (MessageBox.Show(
                Language.Current.UserControlEvent_MessageConfirmDelete
                + Environment.NewLine + Environment.NewLine + eventtName,
                CommonLanguage.Current.MessageCaption_Notice,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question
                ) != DialogResult.OK)
            {
                return;
            }

            List<EventBase> willBeDelete = new List<EventBase>();
            Dictionary<int, object> values = new Dictionary<int, object>();
            for (int i = 0; i < eventId.Length; i++)
            {
                EventBase even =  this.dataGridViewEvent.SelectedRows[i].DataBoundItem as EventBase;
                willBeDelete.Add(even);
                values[this.EventList.IndexOf(even)] = even;
            }

            //删除数据表中对应的行
            //不能通过计数器直接迭代dataGridView.SelectedRows，因为每删除一行，此集合就改变
            foreach (EventBase even in willBeDelete)
            {
                this.EventList.Remove(even);
            }

            this._eventBindingList.ResetBindings();

            EventUpdate();

            if (this.OnEdited != null)
            {
                OnEdited(this, new CollectionEditEventArgs(this.EventList, values));
            }
        }

        private void Up()
        {
            if (dataGridViewEvent.SelectedRows.Count == 0)
            {
                return;
            }

            //向上调整事件的顺序
            int index = dataGridViewEvent.SelectedRows[0].Index - 1;
            if (index < 0)
            {
                return;
            }

            //选中的事件对象
            EventBase even = dataGridViewEvent.SelectedRows[0].DataBoundItem as EventBase;

            int currentColumn = dataGridViewEvent.CurrentCell.ColumnIndex;

            this.EventList.Remove(even);
            this.EventList.Insert(index, even);

            //bindingList会自动与EventList同步

            dataGridViewEvent.SelectedRows[0].Selected = false;
            dataGridViewEvent.Rows[index].Selected = true;
            dataGridViewEvent.CurrentCell = dataGridViewEvent[currentColumn, index];

            EventUpdate();

            if (this.OnEdited != null)
            {
                OnEdited(this, new CollectionEditEventArgs(this.EventList, CollectionEditType.Move, index, index + 1, even));
            }
        }

        private void Down()
        {
            if (dataGridViewEvent.SelectedRows.Count == 0)
            {
                return;
            }

            //向下调整事件的顺序
            int index = dataGridViewEvent.SelectedRows[0].Index + 1;
            if (index >= dataGridViewEvent.Rows.Count)
            {
                return;
            }

            //选中的事件对象
            EventBase even = dataGridViewEvent.SelectedRows[0].DataBoundItem as EventBase;

            int currentColumn = dataGridViewEvent.CurrentCell.ColumnIndex;

            this.EventList.Remove(even);
            this.EventList.Insert(index, even);

            //bindingList会自动与EventList同步

            dataGridViewEvent.SelectedRows[0].Selected = false;
            dataGridViewEvent.Rows[index].Selected = true;
            dataGridViewEvent.CurrentCell = dataGridViewEvent[currentColumn, index];

            EventUpdate();

            if (this.OnEdited != null)
            {
                OnEdited(this, new CollectionEditEventArgs(this.EventList, CollectionEditType.Move, index, index - 1, even));
            }
        }

        #endregion

        #region 右键菜单事件

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void toolStripMenuItemUp_Click(object sender, EventArgs e)
        {
            Up();
        }

        private void toolStripMenuItemDown_Click(object sender, EventArgs e)
        {
            Down();
        }

        #endregion

    }
}
