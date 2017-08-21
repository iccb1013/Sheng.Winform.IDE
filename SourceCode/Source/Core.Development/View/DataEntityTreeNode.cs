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
using System.Windows.Forms;
namespace Sheng.SailingEase.Core.Development
{
    interface IDataEntityTreeNode
    {
        bool IsDataEntity { get; }
        string IdPath { get; }
        string FullName { get; }
        string Id { get; }
        string Name { get; }
        TreeNode Node { get; }
    }
    class DataEntityTreeNode : TreeNode, IDataEntityTreeNode
    {
        private DataEntity _entity;
        public DataEntity Entity
        {
            get { return _entity; }
        }
        public DataEntityTreeNode(DataEntity entity, bool showItems)
            : base(entity.Name)
        {
            _entity = entity;
            this.ImageIndex = this.SelectedImageIndex = 0;
            if (showItems)
            {
                foreach (DataItemEntity item in _entity.Items)
                {
                    this.Nodes.Add(new DataEntityItemTreeNode(item));
                }
            }
        }
        public bool IsDataEntity
        {
            get { return true; }
        }
        public string IdPath
        {
            get { return _entity.Id; }
        }
        public string FullName
        {
            get { return _entity.Name; }
        }
        public string Id
        {
            get { return _entity.Id; }
        }
        public TreeNode Node
        {
            get { return this; }
        }
    }
    class DataEntityItemTreeNode : TreeNode, IDataEntityTreeNode
    {
        private DataItemEntity _entity;
        public DataItemEntity Entity
        {
            get { return _entity; }
        }
        public DataEntityItemTreeNode(DataItemEntity entity)
            : base(entity.Name)
        {
            _entity = entity;
            this.ImageIndex = this.SelectedImageIndex = 1;
        }
        public bool IsDataEntity
        {
            get { return false; }
        }
        public string IdPath
        {
            get { return String.Format("{0}.{1}", _entity.Owner.Id, _entity.Id); }
        }
        public string FullName
        {
            get { return String.Format("{0}.{1}", _entity.Owner.Name, _entity.Name); }
        }
        public string Id
        {
            get { return _entity.Id; }
        }
        public TreeNode Node
        {
            get { return this; }
        }
    }
}
