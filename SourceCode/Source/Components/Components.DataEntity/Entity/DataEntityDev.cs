/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.IDataBaseProvide;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Components.DataEntityComponent
{
    class DataEntityDev : DataEntity, IPersistence, ISnapshot
    {
        public DataEntityDev()
            : this(true)
        {
        }
        public DataEntityDev(string name, string code, bool sys)
            : this(true)
        {
            this.Name = name;
            this.Code = code;
            this.Sys = sys;
        }
        public DataEntityDev(bool addId)
            : base(false)
        {
            base.ItemFactory = DataItemEntityDevFactory.Instance;
            if (addId)
            {
                base.CreateIdField();
            }
        }
        public List<DataItemEntityDev> GetDevItems()
        {
            List<DataItemEntityDev> list = new List<DataItemEntityDev>();
            foreach (DataItemEntityDev entity in base.Items)
                list.Add(entity);
            return list;
        }
        public string GetSql()
        {
            List<IField> fields = new List<IField>();
            foreach (DataItemEntityDev item in this.Items)
            {
                fields.Add(item.Field);
            }
            return DataBaseProvide.Current.CreateSql(this.Code, fields);
        }
        public void Save()
        {
            DataEntityArchive.Instance.Commit(this);
        }
        public void Delete()
        {
            DataEntityArchive.Instance.Delete(this);
        }
        private ObjectSnapshot _objectSnapshot;
        public object Copy()
        {
            return MemberwiseClone();
        }
        public void Snapshot()
        {
            if (_objectSnapshot == null)
                _objectSnapshot = new ObjectSnapshot(this);
            _objectSnapshot.Snapshot();
        }
        public void AcceptChange()
        {
            if (_objectSnapshot != null)
                _objectSnapshot.AcceptChange();
        }
        public void Revert()
        {
            if (_objectSnapshot != null)
                _objectSnapshot.Revert();
        }
    }
}
