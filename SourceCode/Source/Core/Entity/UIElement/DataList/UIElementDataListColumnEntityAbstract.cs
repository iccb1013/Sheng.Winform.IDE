/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Diagnostics;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    public abstract class UIElementDataListColumnEntityAbstract:UIElement
    {
        /*
         *  ������ɳ������Է����鷳���𲻵���֤Shell��IDE��ʵ�ִ����Ե�Ŀ��
         *  ��Ϊ��Core����Ӧ��ʵ�����ͱ���ʵ�ִ������ˣ��ܲ���SHELL��IDE�У�������һ��CORE�е����Ի����������ԣ��̳��ߵ�ȥoverride
         *  ���鷳
         */
        [NonSerialized]
        private UIElementDataListColumnDataRuleTypesAbstract _dataRuleTypesAdapter = UIElementDataListColumnDataRuleTypes.Instance;
        protected UIElementDataListColumnDataRuleTypesAbstract DataRuleTypesAdapter
        {
            get { return this._dataRuleTypesAdapter; }
            set { this._dataRuleTypesAdapter = value; }
        }
        public override string FullCode
        {
            get
            {
                return this.DataList.Code + "." + this.Code;
            }
        }
        private bool _isBind ;
        public bool IsBind
        {
            get
            {
                return this._isBind;
            }
            set
            {
                this._isBind = value;
            }
        }
        private string _dataItemId ;
        public string DataItemId
        {
            get
            {
                return this._dataItemId;
            }
            set
            {
                this._dataItemId = value;
            }
        }
        private string _dataPropertyName;
        public string DataPropertyName
        {
            get
            {
                return this._dataPropertyName;
            }
            set
            {
                this._dataPropertyName = value;
            }
        }
        [NonSerialized]
        private UIElementDataListEntity _dataList;
        public UIElementDataListEntity DataList
        {
            get { return this._dataList; }
            set { this._dataList = value; }
        }
        private UIElementDataListColumnDataRuleAbstract _dataRule = new UIElementDataListColumnDataRules.Normal();
        public UIElementDataListColumnDataRuleAbstract DataRule
        {
            get { return this._dataRule; }
            set { this._dataRule = value; }
        }
        public UIElementDataListColumnEntityAbstract()
            : base()
        {
            this.XmlRootName = "Column";
        }
        public override string ToXml()
        {
            /*
             * �����Ǽ̳е�FormElement
             * �ۺϿ������������ǲ�Ҫ����base.ToXml()���ܶ��������࣬�ò���
             * ��FormElement�ﴴ��ControlType�ڵ�ʱ��ҪFormElementEntityTypes.Instance.GetProvideAttribute
             * �������赥��һ��Types������������������FormElementEntityTypes��ע��
             * ͬʱ��Ϊ��ʵ��ĳЩ�������ܣ�����XML�д���Name,Code���ֱ���̳�EntityBase��������FormElement�ϼ�һ������
             */
            XElement xmlDoc = new XElement(
                new XElement("Column", 
                    new XAttribute("Id", this.Id), 
                    new XAttribute("Name", this.Name), 
                    new XAttribute("Code", this.Code),
                    new XAttribute("ColumnType", UIElementDataListColumnEntityTypes.Instance.GetProvideAttribute(this).Code),
                    new XElement("IsBind", this.IsBind),
                    new XElement("DataItemId", this.DataItemId),
                    new XElement("DataPropertyName", this.DataPropertyName),
                    new XElement("Visible", this.Visible),
                    new XElement("Text", this.Text),
                    new XElement("Width", this.Width),
                    new XElement("Remark", this.Remark)
                    ));
            xmlDoc.Add(XElement.Parse(this.DataRule.ToXml()));
            return xmlDoc.ToString();
        }
        public override void FromXml(string strXml)
        {
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Id = xmlDoc.GetAttributeObject("Id");
            this.Name = xmlDoc.GetAttributeObject("Name");
            this.Code = xmlDoc.GetAttributeObject("Code");
            this.Text = xmlDoc.GetInnerObject("/Text");
            this.Width = xmlDoc.GetInnerObject<int>("/Width", 100);
            this.Remark = xmlDoc.GetInnerObject("/Remark");
            this.IsBind = xmlDoc.GetInnerObject<bool>("/IsBind", false);
            this.DataItemId = xmlDoc.GetInnerObject("/DataItemId");
            this.DataPropertyName = xmlDoc.GetInnerObject("/DataPropertyName");
            this.Visible = xmlDoc.GetInnerObject<bool>("/Visible", true);
            this.DataRule = DataRuleTypesAdapter.CreateInstance(xmlDoc.GetAttributeObject<int>("/DataRule", "Type", 0));
            Debug.Assert(this.DataRule != null, "������DataRuleδ�ܳ�ʼ��");
            if (this.DataRule != null)
            {
                this.DataRule.FromXml(xmlDoc.SelectSingleNode("/DataRule").ToString());
            }
        }
    }
}
