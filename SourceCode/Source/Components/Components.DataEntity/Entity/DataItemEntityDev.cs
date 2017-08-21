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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.IDataBaseProvide;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.DataEntityComponent
{
    class DataItemEntityDev : DataItemEntity, IPersistence, ISnapshot
    {
        public string TypeName
        {
            get
            {
                return DataBaseProvide.Current.FieldFactory.GetName(this.Field);
            }
        }
        public DataItemEntityDev(DataEntity owner)
            : base(owner)
        {
        }
        public DataItemEntityDev(DataEntity owner, string name, string code, bool sys, bool allowEmpty, IField field)
            : this(owner)
        {
            this.Name = name;
            this.Code = code;
            this.Sys = sys;
            this.AllowEmpty = allowEmpty;
            this.Field = field;
        }
        public void Save()
        {
            DataEntityArchive.Instance.CommitDataItem(this, this.Owner.Id);
        }
        public void Delete()
        {
            DataEntityArchive.Instance.DeleteDataItem(this);
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
