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
using System.Xml.Linq;
using Sheng.SailingEase.Kernal;
using System.Diagnostics;
namespace Sheng.SailingEase.Core
{
    
    public interface IEntityIndex
    {
        string Id { get; }
        string Name { get; }
        string Code { get; }
        bool System { get; }
        EntityBase Entity { get; }
        Type EntityType { get; }
    }
    public class EntityIndex<T> : IEntityIndex where T : EntityBase
    {
        protected bool EntityIsNull
        {
            get { return _entity == null; }
        }
        private string _id;
        public string Id
        {
            get
            {
                if (_entity != null)
                    return _entity.Id;
                else
                    return _id;
            }
            protected set
            {
                _id = value;
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                if (_entity != null)
                    return _entity.Name;
                else
                    return _name;
            }
            protected set
            {
                _name = value;
            }
        }
        private string _code;
        public string Code
        {
            get
            {
                if (_entity != null)
                    return _entity.Code;
                else
                    return _code;
            }
            protected set
            {
                _code = value;
            }
        }
        private bool _system;
        public bool System
        {
            get
            {
                if (_entity != null)
                    return _entity.Sys;
                else
                    return _system;
            }
            protected set
            {
                _system = value;
            }
        }
        public Func<IEntityIndex, T> CreateEntity
        {
            get;
            set;
        }
        private EntityBase _entity;
        public EntityBase Entity
        {
            get
            {
                Debug.Assert(CreateEntity != null, "CreateEntity 为 null");
                if (_entity == null && CreateEntity != null)
                {
                    _entity = CreateEntity(this);
                }
                return _entity;
            }
        }
        public T FactEntity
        {
            get { return Entity as T; }
        }
        public Type EntityType
        {
            get { return typeof(T); }
        }
        public EntityIndex()
        {
        }
        public virtual void FromXml(string xml)
        {
            Debug.Assert(String.IsNullOrEmpty(xml) == false, "xml 为 空");
            if (String.IsNullOrEmpty(xml)) { return; }
            SEXElement element = SEXElement.Parse(xml);
            this.Id = element.Attribute("Id").Value;
            this.Name = element.Attribute("Name").Value;
            this.Code = element.Attribute("Code").Value;
            this.System = element.GetAttributeObject<bool>("Sys", false);
        }
    }
}
