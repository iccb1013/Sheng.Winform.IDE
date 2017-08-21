using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;

namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_CallUIElementMethod_General : UserControlEventEditorPanelBase
    {
        #region 私有成员

        /// <summary>
        /// 用于创建 Dev 结尾的动作对象
        /// </summary>
        private EventDevTypes _eventDevTypes;// = EventDevTypes.Instance; 

        /// <summary>
        /// 全局窗体元素选择
        /// </summary>
        private FormGlobalFormElementChoose _formGlobalFormElementChoose;

        /// <summary>
        /// 当前可用的事件的属性(Attribute)BindingList,用于呈现
        /// </summary>
        private BindingList<EventProvideAttribute> _eventProvideAttribute = new BindingList<EventProvideAttribute>();

        /// <summary>
        /// 当前可用的事件TypeCollection
        /// </summary>
        private EventTypeCollection _availabilityMethod;

        /// <summary>
        /// 当前选定的可用事件的编辑面板树节点
        /// </summary>
        private EventEditorNode _treeNode;

        private EventBase _selectedEvent = null;
        /// <summary>
        /// 当前选定的可用事件的事件实例
        /// </summary>
        public EventBase SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                _selectedEvent = value;
                //这个FormEntity是必须设置的，因为一些事件的编辑面板需要用
                if (_selectedEvent != null)
                    _selectedEvent.HostFormEntity = this.HostAdapter.HostFormEntity;
            }
        }

        #endregion

        #region 构造

        public UserControlEventEditorPanel_CallUIElementMethod_General(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();

            _eventDevTypes = EventDevTypes.Instance;

            this.availabilityEvent.SelectedValueChanged += new EventHandler<SEComboSelector2.OnSelectedValueChangedEventArgs>(availabilityEvent_SelectedValueChanged);

            this.txtFormElement.Location = this.ddlFormElement.Location;
            this.txtFormElement.Size = this.ddlFormElement.Size;

            Unity.ApplyResource(this);
            ApplyLanguageResource();

            this.ddlObjectForm.DataSource = EnumDescConverter.Get(typeof(CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm));
            this.ddlFormElement.FormEntity = this.HostAdapter.HostFormEntity;
        }

        #endregion

        #region 应用资源

        private void ApplyLanguageResource()
        {
            //应用本地化语言


            this.txtName.Title = Language.Current.UserControlEventEditorPanel_CallEntityMethod_General_LabelName;
            this.txtCode.Title = Language.Current.UserControlEventEditorPanel_CallEntityMethod_General_LabelCode;

            this.ddlFormElement.Title = Language.Current.UserControlEventEditorPanel_CallEntityMethod_General_LabelFormElement;
            this.availabilityEvent.Title = Language.Current.UserControlEventEditorPanel_CallEntityMethod_General_LabelMethod;
            this.ddlObjectForm.Title = Language.Current.UserControlEventEditorPanel_CallEntityMethod_General_LabelTargetForm;

            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 参入对象方法参数面板
        /// </summary>
        private void LoadMethodPlan()
        {
            //if (this.ddlMethod.SelectedValue == this.SelectedEvent)
            //    return;

            //先移除之前的树节点
            if (this._treeNode != null)
            {
                FormEventEditor.Instance.RemoveParameterTreeNode(_treeNode);
            }

            //EventProvideAttribute attribute = ddlMethod.SelectedValue as EventProvideAttribute;
            //this.SelectedEvent = availabilityMethod.CreateInstance(attribute);

            IEventEditorSupport eventParameterSet = this.SelectedEvent as IEventEditorSupport;
            if (eventParameterSet == null)
                return;

            ICallEntityMethod callEntityMethod = this.SelectedEvent as ICallEntityMethod;
            if (callEntityMethod == null)
                return;

            //获取该事件的设置树节点
            _treeNode = eventParameterSet.EditorAdapter.EditorNode;

            //将窗体元素对象实体作为参数传过去
            CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm objectForm =
                (CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm)Enum.Parse(typeof(CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm), ddlObjectForm.SelectedValue.ToString());

            callEntityMethod.CallEntityMethodObjectForm = objectForm;

            switch (objectForm)
            {
                case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Caller:
                    callEntityMethod.CallEntityMethodFormElement = this.txtFormElement.Text;
                    break;
                case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Current:
                    callEntityMethod.CallEntityMethodFormElement = this.ddlFormElement.SelectedFormElement;
                    break;
            }

            //加入树节点
            FormEventEditor.Instance.AddParameterTreeNode(_treeNode);
        }

        /// <summary>
        /// 初始化当前可用事件
        /// </summary>
        /// <param name="formElementEntityDev"></param>
        /// <param name="objectForm"></param>
        private void InitAvailabilityMethod(ICallEntityMethodSupport formElementEntityDev, CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm objectForm)
        {
            //if (this.treeNode != null)
            //{
            //    FormEventEditor.Instance.RemoveParameterTreeNode(treeNode);
            //    this.treeNode = null;
            //    this.SelectedEvent = null;
            //    this.eventParameterSet = null;
            //}

            _eventProvideAttribute.Clear();
            if(_availabilityMethod !=null)
                _availabilityMethod.Clear();

            if (formElementEntityDev == null)
            {
                this.availabilityEvent.SetSelectedValue(null);
                this.availabilityEvent.Enabled = false;

                return;
            }

            //加个空行
            //_eventProvideAttribute.Add(new EventProvideAttribute(String.Empty, -1));

            _availabilityMethod = formElementEntityDev.GetAvailabilityEntityMethod(objectForm);
            foreach (Type eventType in _availabilityMethod)
            {
                _eventProvideAttribute.Add(_availabilityMethod.GetProvideAttribute(eventType));
            }

            //显示当前可用事件
            availabilityEvent.DataBind(_eventProvideAttribute);

            //如果可有事件为0
            if (_availabilityMethod.Count == 0)
            {
                availabilityEvent.Enabled = false;
            }
            else
            {
                availabilityEvent.Enabled = true;
            }
        }

        #endregion

        #region 公开方法

        #endregion

        #region 事件处理

        void availabilityEvent_SelectedValueChanged(object sender, SEComboSelector2.OnSelectedValueChangedEventArgs e)
        {
            if (e.Value != null)
            {
                EventProvideAttribute attribute = e.Value as EventProvideAttribute;
                this.SelectedEvent = _eventDevTypes.CreateInstance(attribute);
            }
            else
            {
                this.SelectedEvent = null;
            }

            LoadMethodPlan();
        }

        /// <summary>
        /// 切换对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlFormElement_SelectedIndexChanged(object sender, EventArgs e)
        {
            //看所选择的对象是否有可用的对象方法
            ICallEntityMethodSupport formElementEntityDev =
                this.ddlFormElement.SelectedItem as ICallEntityMethodSupport;

            InitAvailabilityMethod(formElementEntityDev, CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Current);
        }

        private void ddlObjectForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm objectForm =
                (CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm)Enum.Parse(typeof(CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm), ddlObjectForm.SelectedValue.ToString());

            switch (objectForm)
            {
                case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Caller:
                    this.txtFormElement.Visible = true;
                    this.ddlFormElement.Visible = false;

                    this.txtFormElement.AllowEmpty = false;
                    this.ddlFormElement.AllowEmpty = true;

                    this.btnBrowse.Enabled = true;

                    this.txtFormElement.Text = String.Empty;

                    break;

                case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Current:
                    this.txtFormElement.Visible = false;
                    this.ddlFormElement.Visible = true;

                    this.txtFormElement.AllowEmpty = true;
                    this.ddlFormElement.AllowEmpty = false;

                    this.btnBrowse.Enabled = false;

                    this.ddlFormElement.SelectedIndex = -1;

                    break;
            }

            availabilityEvent.SetSelectedValue(null);
            availabilityEvent.Enabled = false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (_formGlobalFormElementChoose == null || _formGlobalFormElementChoose.IsDisposed)
            {
                _formGlobalFormElementChoose = new FormGlobalFormElementChoose();
                _formGlobalFormElementChoose.InitFormElementTree();
            }

            if (_formGlobalFormElementChoose.ShowDialog() != DialogResult.OK)
                return;

            this.txtFormElement.Text = _formGlobalFormElementChoose.FormElementFullCode;

            //this.formElementControlType = FormElementEntityDevTypes.Instance.GetProvideAttribute(formGlobalFormElementChoose.SelectedFormElement);

            //根据选择的窗体元素,取可用的对象事件
            ICallEntityMethodSupport formElementEntityDev =
               _formGlobalFormElementChoose.SelectedFormElement as ICallEntityMethodSupport;

            InitAvailabilityMethod(formElementEntityDev, CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Caller);
        }

        #endregion

        #region IEventEditorPanel 成员

        public override string PanelTitle
        {
            get
            {
                return Language.Current.CallEntityMethodDev_EditorPanel_General;
            }
        }

        public override bool DefaultPanel
        {
            get
            {
                return true;
            }
        }

        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();

            _xObject.Add(new XAttribute("Name", txtName.Text));
            _xObject.Add(new XAttribute("Code", txtCode.Text));

            CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm objectForm =
                (CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm)Enum.Parse(typeof(CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm), ddlObjectForm.SelectedValue.ToString());

            _xObject.Add(new XElement("ObjectForm", (int)objectForm));

            switch (objectForm)
            {
                case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Caller:
                    _xObject.Add(new XElement("FormElement", this.txtFormElement.Text));
                    break;
                case CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Current:
                    _xObject.Add(new XElement("FormElement", ((EntityBase)this.ddlFormElement.SelectedItem).Id));
                    break;
            }

            _xObject.Add(new XElement("FormElementControlType", FormElementEntityDevTypes.Instance.GetProvideAttribute((EntityBase)this.ddlFormElement.SelectedItem).Code));
            //EventProvideAttribute attribute = ddlEvent.SelectedValue as EventProvideAttribute;
            EventProvideAttribute attribute = availabilityEvent.GetSelectedValue() as EventProvideAttribute;
            _xObject.Add(new XElement("EventCode", attribute.Code));

            IEventEditorSupport eventParameterSet = this.SelectedEvent as IEventEditorSupport;

            XElement xElement = eventParameterSet.EditorAdapter.GetParameterSetXml();

            xElement.Add(new XAttribute("Name", Guid.NewGuid().ToString()));
            xElement.Add(new XAttribute("Code", Guid.NewGuid().ToString()));

            _xObject.Add(xElement);

            return _xObject;
        }

        public override void SetParameter(EventBase even)
        {
            CallUIElementMethodDev _event = even as CallUIElementMethodDev;

            this.txtName.Text = _event.Name;
            this.txtCode.Text = _event.Code;

            this.ddlObjectForm.SelectedValue = (int)_event.TargetWindow;

            if (_event.TargetWindow == CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Current)
            {
                //先把选择的项置为null
                //因为，当指定的窗体元素不存在时，在编辑面板上选个窗体元素，不保存，再打开编辑功能，上次的选择还存在着。
                this.ddlFormElement.SelectedItem = null;

                foreach (object obj in this.ddlFormElement.Items)
                {
                    UIElement entity = obj as UIElement;
                    if (entity.Id == _event.FormElement)
                    {
                        this.ddlFormElement.SelectedItem = entity;
                        break;
                    }
                }
            }
            else
            {
                txtFormElement.Text = _event.FormElement;

                //调入可用事件列表
                ICallEntityMethodSupport formElementEntityDev =
                    FormElementEntityDevTypes.Instance.CreateInstance(_event.FormElementControlType) as ICallEntityMethodSupport;
                if (formElementEntityDev != null)
                {
                    InitAvailabilityMethod(formElementEntityDev, CallUIElementMethodEvent.EnumCallUIElementMethodTargetForm.Caller);
                }
            }

            this.availabilityEvent.SelectedValueChanged -=new EventHandler<SEComboSelector2.OnSelectedValueChangedEventArgs>(availabilityEvent_SelectedValueChanged);
            //选择对应的动作
            //注意的是，availabilityMethod 是有可能为 null的，如果是当前窗体，但指定的窗体元素已不存在
            //就没有办法拿到 availabilityMethod 此时availabilityMethod 为null
            if (_availabilityMethod != null)
            {
                availabilityEvent.SetSelectedValue(_availabilityMethod.GetProvideAttribute(_event.CallEvent));
                //this.SelectedEvent = _event.CallEvent;
                //LoadMethodPlan();
            }
            this.availabilityEvent.SelectedValueChanged += new EventHandler<SEComboSelector2.OnSelectedValueChangedEventArgs>(availabilityEvent_SelectedValueChanged);

            this.SelectedEvent = _event.CallEvent;
            IEventEditorSupport eventParameterSet = this.SelectedEvent as IEventEditorSupport;

            if (eventParameterSet != null)
            {
                LoadMethodPlan();
                eventParameterSet.EditorAdapter.BeginEdit();
            }
        }

        public override bool ValidateParameter(out string validateMsg)
        {
            bool result = true;
            result = base.ValidateParameter(out validateMsg);
            if (result == false)
            {
                return result;
            }

            result = EditorHelper.ValidateCodeExist(this.HostAdapter, this.txtCode.Text, out validateMsg);

            return result;
        }

        #endregion

      
    }
}
