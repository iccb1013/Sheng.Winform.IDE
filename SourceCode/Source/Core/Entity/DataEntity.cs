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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    public class DataEntity:EntityBase
    {
        private DataItemEntityFactoryAbstract _itemFactory = DataItemEntityFactory.Instance;
        protected DataItemEntityFactoryAbstract ItemFactory
        {
            get { return _itemFactory; }
            set { _itemFactory = value; }
        }
        public const string Property_Items = "Items";
        private DataItemEntityCollection _items = new DataItemEntityCollection();
        public DataItemEntityCollection Items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }
        public DataEntity()
            : this(true)
        {
        }
        public DataEntity(bool addId)
        {
            this.XmlRootName = "Entity";
            if (addId)
            {
                CreateIdField();
            }
        }
        protected void CreateIdField()
        {
            DataItemEntity dataItemEntity = ItemFactory.CreateDataItemEntity(this);
            dataItemEntity.Id = Guid.NewGuid().ToString();
            dataItemEntity.Name = "Id"; 
            dataItemEntity.Code = "Id";
            dataItemEntity.Sys = true;
            dataItemEntity.Field = DataBaseProvide.Current.CreateIdField();
            dataItemEntity.AllowEmpty = false;
            dataItemEntity.DefaultValue = DataBaseProvide.Current.IdFieldDefaultValue;
            this.Items.Add(dataItemEntity);
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXml);
            this.Items.Clear();
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(XmlRootName + "/Items/Item");
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                DataItemEntity dataItemEntity = ItemFactory.CreateDataItemEntity(this);
                dataItemEntity.FromXml(xmlNodeList[i].OuterXml);
                this.Items.Add(dataItemEntity);
            }
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild("Items");
            foreach (DataItemEntity dataItemEntity in this.Items)
            {
                xmlDoc.AppendInnerXml("/Items", dataItemEntity.ToXml());
            }
            return xmlDoc.ToString();
        }
    }
}
