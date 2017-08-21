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
using System.Xml.Linq;
namespace Sheng.SailingEase.Core
{
    public class EnumEntity:EntityBase
    {
        private EnumItemEntityFactoryAbstract _itemFactory = EnumItemEntityFactory.Instance;
        protected EnumItemEntityFactoryAbstract ItemFactory
        {
            get { return _itemFactory; }
            set { _itemFactory = value; }
        }
        private List<EnumItemEntity> _items = new List<EnumItemEntity>();
        public List<EnumItemEntity> Items
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
        public EnumEntity()
        {
            this.XmlRootName = "Enum";
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            foreach (XElement element in xmlDoc.SelectNodes("/Items/Item"))
            {
                EnumItemEntity enumItemEntity = ItemFactory.CreateEnumItemEntity(this);
                enumItemEntity.FromXml(element.ToString());
                this.Items.Add(enumItemEntity);
            }
            base.FromXml(strXml);
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild("Items");
            foreach (EnumItemEntity enumItemEntity in this.Items)
            {
                xmlDoc.AppendInnerXml("/Items", enumItemEntity.ToXml());
            }
            return xmlDoc.ToString();
        }
    }
}
