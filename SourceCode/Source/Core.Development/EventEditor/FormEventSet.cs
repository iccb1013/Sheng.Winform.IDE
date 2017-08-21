using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Sheng.SIMBE.Core;
using Sheng.SIMBE.Core.Event;
using Sheng.SIMBE.IDE.EntityDev;
using Sheng.SIMBE.IDE.Sys;
using Sheng.SIMBE.IDE.EventDev;
using System.Reflection;
using Sheng.SIMBE.Languages.IDE;
using Sheng.SIMBE.Core.Entity;

namespace Sheng.SIMBE.IDE.UI.EventSet
{
    //否绝
    //删除的时候注意资源文件处理
    internal partial class FormEventSet : FormBase
    {
        #region 私有字段

        private static FormEventSet _instance = null;
        /// <summary>
        /// 当前事件设置窗体实例
        /// 在formload中会为_instance赋值
        /// 这里没有把FormEventSet做成单实例，
        /// 1因为Instance这个属性是后来加的，做单实例改动大
        /// 2.本窗体初例化的时候带有参数，处理改动大
        /// 
        /// 本属性用于调用对象方法事件时，能引到本窗体为树节点加事件设置相关节点
        /// </summary>
        public static FormEventSet Instance
        {
            get
            {
                return _instance;
            }
        }

        private IEventParameterSet iEventParameterSet;

        /// <summary>
        /// 可选的动作列表
        /// </summary>
        DataTable dtEvent;
        /// <summary>
        /// 传进来的动作适用类型
        /// </summary>
        EnumEventType eventType;

        #endregion

        #region 公开属性

        /// <summary>
        /// 动作对象
        /// </summary>
        EventBase even;
        /// <summary>
        /// 获取动作
        /// </summary>
        public EventBase Event
        {
            get
            {
                return this.even;
            }
            private set
            {
                this.even = value;
            }
        }

        private bool isEdit = false;
        private EventBase editEvent;
        /// <summary>
        /// 要编辑的动作
        /// </summary>
        public EventBase EditEvent
        {
            get
            {
                return this.editEvent;
            }
            set
            {
                this.editEvent = value;
            }
        }

        private FormEntityDev formEntity;
        /// <summary>
        /// 窗体的数据实体
        /// 有些事件在设置过程中需要用到
        /// 
        /// 另外在事件设置完毕后，也要告诉事件其所在的FormEntity，尤其是新增事件时
        /// </summary>
        public FormEntityDev FormEntity
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

        private EnumEventTime eventTime;
        /// <summary>
        /// 选择的事件触发时机
        /// </summary>
        public EnumEventTime EventTime
        {
            get
            {
                return (EnumEventTime)Convert.ToInt32(this.ddlEventTime.SelectedValue.ToString());
            }
        }

        #endregion

        #region 窗体构造及窗体事件

        
        public FormEventSet(EnumEventType eventType)
        {
            InitializeComponent();

            base.ApplyDefLanguageResource();

            //初始化imageList
            this.imageListParameter.Images.Add(IconResource.EmptyIcon);
            this.imageListParameter.Images.Add(IconResource.Property);
            this.imageListParameter.Images.Add(IconResource.Right);
            this.imageListParameter.Images.Add(IconResource.Method);            

            this.eventType = eventType;
        }

        private void FormEventSet_Load(object sender, EventArgs e)
        {
            _instance = this;

            //加载动作列表
            dtEvent = EnumMember.GetEventList(eventType);

            DataRow dr = dtEvent.NewRow();
            dr["Text"] = "";
            dr["Value"] = "";
            dtEvent.Rows.InsertAt(dr, 0);

            ddlEvent.DataSource = dtEvent;
            ddlEvent.DisplayMember = "Text";
            ddlEvent.ValueMember = "Value";

            //加载触发时机列表
            ddlEventTime.DataSource = EnumMember.GetEventTimeList(this.eventType);

            //如果EditEvent不为空，则当前窗体是在编辑模式下，编辑EditEvent
            if (this.EditEvent != null)
            {
                this.isEdit = true;
            }

            //如果不是编辑状态，返回
            if (!this.isEdit)
            {
                return;
            }

            //加载编辑状态信息

            //选择对应的动作
            ddlEvent.SelectedValue = this.EditEvent.EventCode;

            //选择对应的触发时机
            this.ddlEventTime.SelectedValue = ((int)this.EditEvent.EventTime).ToString();

            //如果有警告,显示警告信息
            if (this.EditEvent.Warning)
            {
                MessageBox.Show(this.EditEvent.WarningMsg,
                       Language.Current.MessageBoxCaptiton_Warning,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //把动作传到用户控件里去,设置相应信息
            this.iEventParameterSet.SetParameter(this.EditEvent);

        }

        private void FormEventSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.treeViewParameter.Nodes.Clear();
        }

        #endregion

        #region 验证输入

        /// <summary>
        /// 验证控件输入
        /// </summary>
        /// <returns></returns>
        public override bool DoValidate()
        {
            bool validateResult = true;
            string validateMsg = String.Empty;
            foreach (Control ctrl in this.Controls)
            {
                Type t = ctrl.GetType();

                if (!ValidateControl(ctrl, t, ref validateMsg))
                {
                    validateResult = false;
                }

            }

            //验证未被显示的参数设置面板
            if (this.iEventParameterSet != null)
            {
                //所有参数面板
                foreach (UserControlEventSetParameter userControlEventSetParameter 
                    in iEventParameterSet.UserControlEventSetParameterList)
                {
                    //未被显示出
                    if (userControlEventSetParameter.Parent == null)
                    {
                        if (!userControlEventSetParameter.SEValidate(ref validateMsg))
                        {
                            validateResult = false;
                        }
                    }
                }
            }

            if (!validateResult)
            {
                //去掉多余的换行符
                while (true)
                {
                    if (validateMsg.IndexOf("\r\n\r\n") > 0)
                    {
                        validateMsg = validateMsg.Replace("\r\n\r\n", "\r\n");
                    }
                    else
                    {
                        break;
                    }
                }

                MessageBox.Show(validateMsg, Language.Current.MessageBoxCaptiton_Notice,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return validateResult;
        }

        private bool ValidateControl(Control ctrl, Type t, ref string validateMsg)
        {
            MethodInfo mi = t.GetMethod("SEValidate");
            PropertyInfo pi = t.GetProperty("HighLight");
            if (mi == null)
            {
                return true;
            }
            string[] msg = new string[1];
            object result = mi.Invoke(ctrl, msg);
            if ((bool)result == false)
            {
                validateMsg += msg[0] + "\r\n";
                //获取属性判断是否需要改变背景色
                if ((bool)pi.GetValue(ctrl, null))
                {
                    ctrl.BackColor = Color.Pink;
                }
                return false;
            }
            else
            {
                if ((bool)pi.GetValue(ctrl, null))
                {
                    ctrl.BackColor = SystemColors.Window;
                }
                return true;
            }
        }

        #endregion

        #region 窗体控件事件

        /// <summary>
        /// 点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.DoValidate())
            {
                return;
            }

            //检查指定的动作代码是否已存在
            if (this.EditEvent == null || this.EditEvent.Code != this.iEventParameterSet.EnteredCode)
            {
                //检查指定代码的窗体元素是否已存在
                if (this.EventList.GetEventByCode(this.iEventParameterSet.EnteredCode) != null)
                {
                    MessageBox.Show(Language.Current.FormEventSet_MessageEventCodeExist,
                           Language.Current.MessageBoxCaptiton_Notice,
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            this.Event.FromXml(
                iEventParameterSet.GetParameterSetXml());

            this.Event.FormEntity = this.FormEntity;

            this.Event.CheckWarning();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEvent.SelectedValue.GetType().Name == "DataRowView")
            {
                ddlEvent.SelectedValue = dtEvent.Rows[0]["Value"].ToString();
                return;
            }            
          
            if (ddlEvent.SelectedValue.ToString() != String.Empty)
            {
                EnumEvent enumEvent = (EnumEvent)(Convert.ToInt32(ddlEvent.SelectedValue));

                //根据选择的动作类型，初始化不同的参数设置面板
                this.Event = EventFactory.GetEventDev(enumEvent);
            }
            else
            {
                this.Event = null;
            }

            if (this.Event != null)
            {
                this.iEventParameterSet = (IEventParameterSet)this.Event;

                this.treeViewParameter.Nodes.Clear();
                this.treeViewParameter.Nodes.Add(iEventParameterSet.GetParameterSetNode(this.FormEntity));
                this.treeViewParameter.ExpandAll();
                this.treeViewParameter.SelectedNode = iEventParameterSet.DefaultSelectedNode;
            }
            else
            {
                this.iEventParameterSet = null;
                this.treeViewParameter.Nodes.Clear();
                this.panelParameter.Controls.Clear();
            }
        }

        /// <summary>
        /// 更新参数树选定的节点后发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewParameter_AfterSelect(object sender, TreeViewEventArgs e)
        {
            EventParameterTreeNode eventParameterTreeNode = e.Node as EventParameterTreeNode;
            this.panelParameter.Controls.Clear();
            this.panelParameter.Controls.Add(eventParameterTreeNode.ParameterSetPlan);
        }

        #endregion

        #region 树节点操作

        /// <summary>
        /// 向参数中添加节点
        /// 用于调用对象方法事件，选定对象方法后，所需参数设置的有关面板挂过来
        /// </summary>
        /// <param name="treeNode"></param>
        public void AddParameterTreeNode(EventParameterTreeNode treeNode)
        {
            this.treeViewParameter.Nodes.Add(treeNode);
            treeNode.ExpandAll();
        }

        /// <summary>
        /// 删除参数面板中的节点
        /// 用于调用对象方法事件，选定对象方法后，把之前的方法面板删除
        /// </summary>
        /// <param name="treeNode"></param>
        public void RemoveParameterTreeNode(EventParameterTreeNode treeNode)
        {
            this.treeViewParameter.Nodes.Remove(treeNode);
        }

        #endregion

    }
}
