using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace Sheng.SailingEase.Controls.Extensions
{
   

    /// <summary>
    /// 支持按类型绑定的 TreeView 控制器
    /// 通过描述类型与类型之间的从属关系来呈现树
    /// </summary>
    public class TypeBinderTreeViewController
    {
        #region 私有成员

        /// <summary>
        /// 指示是否忽略AfterSelected事件
        /// 这发生在内部调整节点选中状态时
        /// </summary>
        private bool _ignoreAfterSelectedEvent = false;

        private TreeView _treeView;

        private ContextMenuStrip ContextMenuStrip
        {
            get { return this._treeView.ContextMenuStrip; }
            set { this._treeView.ContextMenuStrip = value; }
        }

        private List<TypeBinderTreeViewNodeCodon> _binderTreeViewNodeCodons = new List<TypeBinderTreeViewNodeCodon>();

        private IList _ownerList;

        #endregion

        #region 公开属性

        private Type _nodeType = typeof(TypeBinderTreeViewNode);
        /// <summary>
        /// 用于创建树节点的对象类型，继承自 TreeNode 对象
        /// </summary>
        public Type NodeType
        {
            get { return _nodeType; }
            set
            {
                if (value.GetType().IsSubclassOf(typeof(TreeNode)) == false ||
                    value.GetType().IsSubclassOf(typeof(ITypeBinderTreeViewNode)) == false)
                {
                    Debug.Assert(false, "NodeType 不对");
                    _nodeType = typeof(TypeBinderTreeViewNode);
                    return;
                }

                _nodeType = value;
            }
        }

        public ITypeBinderTreeViewNode SelectedNode
        {
            get
            {
                ITypeBinderTreeViewNode selectedNode = this._treeView.SelectedNode as ITypeBinderTreeViewNode;
                if (selectedNode == null)
                    return null;
                else
                    return selectedNode;
            }
        }

        /// <summary>
        /// 当前选中的节点的类型
        /// </summary>
        public Type SelectedDataBoundItemType
        {
            get
            {
                ITypeBinderTreeViewNode selectedNode = this._treeView.SelectedNode as ITypeBinderTreeViewNode;
                if (selectedNode == null)
                    return null;
                else
                    return selectedNode.DataBoundItem.GetType();
            }
        }

        /// <summary>
        /// 当前选定的对象
        /// </summary>
        public object SelectedDataBoundItem
        {
            get
            {
                ITypeBinderTreeViewNode selectedNode = this._treeView.SelectedNode as ITypeBinderTreeViewNode;
                if (selectedNode == null)
                    return null;
                else
                    return selectedNode.DataBoundItem;
            }
        }

        public ImageList ImageList
        {
            get { return _treeView.ImageList; }
            set { _treeView.ImageList = value; }
        }

        #endregion

        #region 构造

        public TypeBinderTreeViewController(TreeView treeView)
        {
            _treeView = treeView;
            _treeView.HideSelection = false;

            _treeView.AfterSelect += (sender, e) =>
            {
                ITypeBinderTreeViewNode node = (ITypeBinderTreeViewNode)e.Node;

                //设置当前右键菜单
                this.ContextMenuStrip = node.Codon.ContextMenuStrip;

                //触发事件
                //这里把_ignoreAfterSelectedEvent放在这里判断，而不是连同上面的代码一起判断
                //是因为在内部调整节点选中状态时，有可能会在中间切换到其它节点时，变更了上述代码中的一些状态，如ContextMenuStrip
                if (_ignoreAfterSelectedEvent == false && AfterSelect != null)
                {
                    AfterSelect(this, new AfterSelectEventArgs(node));
                }
            };

            //_treeView.CanDragFunc = (node) =>
            //{
            //    return true;
            //};

            //_treeView.CanDropFunc = (dragNode, dropNode) =>
            //{
            //    return true;
            //};

            //_treeView.DragDropAction = (dragNode, dropNode) =>
            //{

            //};
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 清除当前节点，上下文菜单等
        /// </summary>
        private void Clear()
        {
            _ownerList = null;
            _treeView.Nodes.Clear();
            ContextMenuStrip = null;
        }

        private TypeBinderTreeViewNodeCodon GetSelectedNodeCodon()
        {
            ITypeBinderTreeViewNode selectedNode = SelectedNode;
            if (selectedNode == null)
                return null;
            else
                return selectedNode.Codon;
        }

        private TypeBinderTreeViewNodeCodon GetNodeCodon(Type type)
        {
            foreach (var item in _binderTreeViewNodeCodons)
            {
                if (item.DataBoundType == null)
                    continue;

                //if (item.DataBoundType == type || (item.ActOnSubClass && type.IsSubclassOf(item.DataBoundType)))
                if (item.Compatible(type))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// 创建一个树节点
        /// </summary>
        /// <param name="dataBoundItem"></param>
        /// <returns></returns>
        private ITypeBinderTreeViewNode CreateNode(object dataBoundItem)
        {
            ITypeBinderTreeViewNode node = (ITypeBinderTreeViewNode)Activator.CreateInstance(this.NodeType);
            node.DataBoundItem = dataBoundItem;
            TypeBinderTreeViewNodeCodon codon = GetNodeCodon(dataBoundItem.GetType());
            if (codon != null)
            {
                node.Codon = codon;
                //这里有个漏洞，一旦把绑定到 dataBoundItem 的节点移动到另外一个节点下，其实际 OwnerList 就已经发生了改变
                //所以不能在创建节点时一次写死
                //node.OwnerList = ownerList;
                DataBind(node);
            }
            return node;
        }

        /// <summary>
        /// 绑定指定节点的所有子节点（递归）
        /// </summary>
        /// <param name="node"></param>
        private void DataBind(ITypeBinderTreeViewNode node)
        {
            IList list = node.Items;

            if (list == null)
                return;

            foreach (var item in list)
            {
                ITypeBinderTreeViewNode childNode = CreateNode(item);
                if (childNode.Codon == null)
                {
                    Debug.Assert(false, "childNode.Codon 为空");
                    continue;
                }

                node.Node.Nodes.Add(childNode.Node);
            }
        }

        public ITypeBinderTreeViewNode Find(object obj)
        {
            return Find(_treeView.Nodes, obj);
        }

        /// <summary>
        /// 通过递归节点及所有子节点查找所绑定对象为 obj 的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private ITypeBinderTreeViewNode Find(TreeNodeCollection nodes, object obj)
        {
            foreach (ITypeBinderTreeViewNode item in nodes)
            {
                if (item.DataBoundItem == obj)
                    return item;

                ITypeBinderTreeViewNode childNode = Find(item.Node.Nodes, obj);
                if (childNode != null)
                    return childNode;
            }

            return null;
        }

        public ITypeBinderTreeViewNode Find(Filter filter)
        {
            return Find(_treeView.Nodes, filter);
        }

        /// <summary>
        /// 通过Filter过滤器递归节点及所有子节点，查找符合条件的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private ITypeBinderTreeViewNode Find(TreeNodeCollection nodes, Filter filter)
        {
            Debug.Assert(filter != null, "Filter 为 null");

            if (filter == null)
                return null;

            foreach (ITypeBinderTreeViewNode item in nodes)
            {
                if (filter(item))
                    return item;

                ITypeBinderTreeViewNode childNode = Find(item.Node.Nodes, filter);
                if (childNode != null)
                    return childNode;
            }

            return null;
        }

        /// <summary>
        /// 从指定节点，开如冒泡查询指定类型T的节点
        /// 若查询到顶都没有，则返回null，否则返回遇到的第一个匹配的节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        private ITypeBinderTreeViewNode BubblingFind<T>(ITypeBinderTreeViewNode node)
        {
            if (node == null)
                return null;

            //if (node.DataBoundItem.GetType() == typeof(T) || node.DataBoundItem.GetType().IsSubclassOf(typeof(T)))
            //    return node;

            if (node.Codon.UpwardCompatible(typeof(T)))
                return node;

            if (node.Parent == null)
                return null;

            return BubblingFind<T>(node.Parent);
        }

        #endregion

        #region 公开方法

        #region Codon

        public void AddNodeCodon(TypeBinderTreeViewNodeCodon codon)
        {
            if (_binderTreeViewNodeCodons.Contains(codon))
            {
                Debug.Assert(false, "_binderTreeViewNodeCodons 重复添加:" + codon.ToString());
                return;
            }

            Debug.Assert(GetNodeCodon(codon.DataBoundType) == null,
                "_binderTreeViewNodeCodons 重复添加类型:" + codon.ToString());

            _binderTreeViewNodeCodons.Add(codon);
        }

        #endregion

        #region DataBind

        public void DataBind(object obj)
        {
            if (obj == null)
            {
                Debug.Assert(false, "obj 为 null");
                Clear();
                return;
            }

            _treeView.SuspendLayout();
            _treeView.Nodes.Clear();

            ITypeBinderTreeViewNode node = CreateNode(obj);

            _treeView.Nodes.Add(node.Node);

            _treeView.ResumeLayout();

        }

        public void DataBind(IList list)
        {
            if (list == null)
            {
                Debug.Assert(false, "list 为空");
                Clear();
                return;
            }

            _ownerList = list;

            _treeView.SuspendLayout();
            _treeView.Nodes.Clear();

            foreach (var item in list)
            {
                ITypeBinderTreeViewNode node = CreateNode(item);
                this._treeView.Nodes.Add(node.Node);
            }

            _treeView.ResumeLayout();
        }

        #endregion

        #region GetData...

        public T GetSelectedData<T>()
        {
            ITypeBinderTreeViewNode selectedNode = this._treeView.SelectedNode as ITypeBinderTreeViewNode;
            if (selectedNode == null)
                return default(T);

            object obj = selectedNode.DataBoundItem;
            if (obj == null)
                return default(T);

            //if (obj.GetType().Equals(typeof(T)) == false && obj.GetType().IsSubclassOf(typeof(T)) == false)
            //    return default(T);

            if (selectedNode.Codon.UpwardCompatible(typeof(T)))
                return (T)obj;
            else
                return default(T);
        }

        /// <summary>
        /// 如果bubbling为true则从当前选中节点开始使用冒泡查询向上查询
        /// 直一遇到第一个绑定对象类型为T的节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bubbling"></param>
        /// <returns></returns>
        public T GetSelectedData<T>(bool bubbling)
        {
            if (bubbling)
            {
                ITypeBinderTreeViewNode node = BubblingFind<T>(SelectedNode);
                if (node == null)
                    return default(T);

                return (T)node.DataBoundItem;
            }
            else
            {
                return GetSelectedData<T>();
            }
        }

        /// <summary>
        /// 获取当前选定节点的前一个同级树节点的绑定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPreSelectedData<T>()
        {
            ITypeBinderTreeViewNode selectedNode = this._treeView.SelectedNode as ITypeBinderTreeViewNode;
            if (selectedNode == null)
                return default(T);

            ITypeBinderTreeViewNode prevNode = selectedNode.PrevNode;

            if (prevNode == null)
                return default(T);

            object obj = prevNode.DataBoundItem;
            if (obj == null)
                return default(T);

            //if (obj.GetType().Equals(typeof(T)) == false && obj.GetType().IsSubclassOf(typeof(T)) == false)
            //    return default(T);

            if (prevNode.Codon.UpwardCompatible(typeof(T)))
                return (T)obj;
            else
                return default(T);
        }

        /// <summary>
        /// 获取当前选定节点的下一个同级树节点的绑定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetNextSelectedData<T>()
        {
            ITypeBinderTreeViewNode selectedNode = this._treeView.SelectedNode as ITypeBinderTreeViewNode;
            if (selectedNode == null)
                return default(T);

            ITypeBinderTreeViewNode nextNode = selectedNode.NextNode;

            if (nextNode == null)
                return default(T);

            object obj = nextNode.DataBoundItem;
            if (obj == null)
                return default(T);

            //if (obj.GetType().Equals(typeof(T)) == false && obj.GetType().IsSubclassOf(typeof(T)) == false)
            //    return default(T);

            if (nextNode.Codon.UpwardCompatible(typeof(T)))
                return (T)obj;
            else
                return default(T);
        }

        public IList GetOwnerList(ITypeBinderTreeViewNode node)
        {
            ITypeBinderTreeViewNode parentNode = node.Parent;
            if (parentNode == null)
                return _ownerList;

            return parentNode.Items;
        }
        
        #endregion

        #region Select

        /// <summary>
        /// 选中树中所绑定对象为obj的节点
        /// </summary>
        /// <param name="obj"></param>
        public void Select(object obj)
        {
            ITypeBinderTreeViewNode node = Find(obj);
            Select(node);
        }

        public void Select(Filter filter)
        {
            ITypeBinderTreeViewNode node = Find(filter);
            Select(node);
        }

        public void Select(ITypeBinderTreeViewNode node)
        {
            if (node != null)
            {
                if (node.Node.Parent != null && node.Node.Parent.IsExpanded == false)
                    node.Node.Parent.Expand();

                _treeView.SelectedNode = node.Node;
            }
        }

        #endregion

        #region Add,Update,Remove

        public void Add(object obj)
        {
            Debug.Assert(obj != null, "obj 为 null");

            if (_ownerList != null && _ownerList.Contains(obj) == false)
                _ownerList.Add(obj);

            ITypeBinderTreeViewNode node = CreateNode(obj);
            this._treeView.Nodes.Add(node.Node);
        }

        public void Add(object obj, Filter filter)
        {
            Debug.Assert(obj != null, "obj 为 null");

            ITypeBinderTreeViewNode parentNode = Find(filter);
            if (parentNode == null)
                return;

            ITypeBinderTreeViewNode node = CreateNode(obj);
            parentNode.AddChild(node);
        }

        /// <summary>
        /// Update 方法只能更新节点本身的状态，如其显示的文本
        /// 是无法改变树结构的
        /// </summary>
        /// <param name="obj"></param>
        public void Update(object obj)
        {
            ITypeBinderTreeViewNode node = Find(obj);
            if (node != null)
            {
                node.Update();
                UpdateRelation(node);
            }
        }

        public void Update(object obj, Filter filter)
        {
            ITypeBinderTreeViewNode node = Find(filter);
            if (node != null)
            {
                node.Update(obj);
                UpdateRelation(node);
            }
        }

        public void Update(Filter filter)
        {
            ITypeBinderTreeViewNode node = Find(filter);
            if (node != null)
            {
                node.Update();
                UpdateRelation(node);
            }
        }

        /// <summary>
        /// 判断并更新（如果需要）指定节点与父节点的关联关系
        /// </summary>
        /// <param name="node"></param>
        private void UpdateRelation(ITypeBinderTreeViewNode node)
        {
            if (node == null)
            {
                Debug.Assert(false, "node 为空");
                return;
            }

            ITypeBinderTreeViewNode parentNode = node.Parent;
            if (parentNode == null)
                return;

            object nodeValue = node.RelationParentMemberValue;
            object parentValue = parentNode.RelationChildMemberValue;

            if (nodeValue == null || nodeValue.Equals(parentValue))
                return;

            //如果关联值不相等，为传进来的 node 查找新的父节点

            ITypeBinderTreeViewNode newParentNode = Find(
                (filterNode) =>
                {
                    object relationChildMemberValue = filterNode.RelationChildMemberValue;
                    if (relationChildMemberValue == null)
                        return false;
                    else
                        return relationChildMemberValue.Equals(nodeValue);
                });

            //如果找不到新的父节点，删除该节点
            if (newParentNode == null)
            {
                Remove(node.DataBoundItem);
            }

            Move(node, newParentNode);
        }

        public void Remove(object obj)
        {
            ITypeBinderTreeViewNode node = Find(obj);
            if (node != null)
            {
                node.Remove();
            }
        }

        public void Remove(Filter filter)
        {
            ITypeBinderTreeViewNode node = Find(filter);
            if (node != null)
                Remove(node.DataBoundItem);
        }

        #endregion

        #region Move

        /// <summary>
        /// 移动源节点到目标节点的下面，成为目标节点的子节点
        /// </summary>
        /// <param name="sourceFilter"></param>
        /// <param name="targetFilter"></param>
        public void Move(Filter sourceFilter, Filter targetFilter)
        {
            ITypeBinderTreeViewNode sourceNode = Find(sourceFilter);
            ITypeBinderTreeViewNode targetNode = Find(targetFilter);

            Move(sourceNode, targetNode);
        }

        public void Move(ITypeBinderTreeViewNode sourceNode, ITypeBinderTreeViewNode targetNode)
        {
            Debug.Assert(sourceNode != null || targetNode != null, "目标树节点为null");

            if (sourceNode == null || targetNode == null)
                return;

            Debug.Assert(sourceNode != targetNode, "指定的两个节点是同一个节点");

            if (sourceNode == targetNode)
                return;

            Debug.Assert(sourceNode.Parent != targetNode, "sourceNode 已经是 targetNode 的子节点了");

            if (sourceNode.Parent == targetNode)
                return;

            TreeNodeCollection sourceOwnerNodes;
            if (sourceNode.Parent == null)
                sourceOwnerNodes = _treeView.Nodes;
            else
                sourceOwnerNodes = sourceNode.Parent.Node.Nodes;

            //调整所绑定对象在所在的IList内的顺利
            IList sourceOwnerList = GetOwnerList(sourceNode);
            IList targetList = targetNode.Items;

            //如果原节点原来是选定状态，在移动之后继续保持其选中状态
            //但是不能触发AfterSelect事件，否则会引发外部一些不必要的处理逻辑
            bool willBeSelected = sourceNode.Node.IsSelected;
            _ignoreAfterSelectedEvent = true;

            //调整树节点的顺序
            sourceOwnerNodes.Remove(sourceNode.Node);
            targetNode.Node.Nodes.Add(sourceNode.Node);

            if (willBeSelected)
                _treeView.SelectedNode = sourceNode.Node;

            _ignoreAfterSelectedEvent = false;

            if (sourceOwnerList != null && sourceOwnerList.Contains(sourceNode.DataBoundItem))
                sourceOwnerList.Remove(sourceNode.DataBoundItem);

            if (targetList != null && targetList.Contains(sourceNode.DataBoundItem) == false)
                targetList.Add(sourceNode.DataBoundItem);
        }

        /// <summary>
        /// 移动一个树节点到目标节点之前，并同步所绑定的对象在集合中的位置
        /// 能够跨级别移动
        /// </summary>
        /// <param name="sourceFilter"></param>
        /// <param name="targetFilter"></param>
        public void MoveBefore(Filter sourceFilter, Filter targetFilter)
        {
            ITypeBinderTreeViewNode sourceNode = Find(sourceFilter);
            ITypeBinderTreeViewNode targetNode = Find(targetFilter);

            Debug.Assert(sourceNode != null || targetNode != null, "目标树节点为null");

            if (sourceNode == null || targetNode == null)
                return;

            Debug.Assert(sourceNode != targetNode, "指定的两个节点是同一个节点");

            if (sourceNode == targetNode)
                return;

            TreeNodeCollection sourceOwnerNodes;
            if (sourceNode.Parent == null)
                sourceOwnerNodes = _treeView.Nodes;
            else
                sourceOwnerNodes = sourceNode.Parent.Node.Nodes;

            TreeNodeCollection targetOwnerNodes;
            if (targetNode.Parent == null)
                targetOwnerNodes = _treeView.Nodes;
            else
                targetOwnerNodes = targetNode.Parent.Node.Nodes;

            int sourceIndex = sourceOwnerNodes.IndexOf(sourceNode.Node);
            int targetIndex = targetOwnerNodes.IndexOf(targetNode.Node);

            //如果targetObj刚好在beforeObj之前，就无需移动了
            if (sourceOwnerNodes == targetOwnerNodes && targetIndex - sourceIndex == 1)
                return;

            //调整所绑定对象在所在的IList内的顺利
            IList sourceOwnerList = GetOwnerList(sourceNode);
            IList targetOwnerList = GetOwnerList(targetNode);

            //如果原节点原来是选定状态，在移动之后继续保持其选中状态
            //但是不能触发AfterSelect事件，否则会引发外部一些不必要的处理逻辑
            bool willBeSelected = sourceNode.Node.IsSelected;
            _ignoreAfterSelectedEvent = true;

            //调整树节点的顺序
            sourceOwnerNodes.Remove(sourceNode.Node);
            //要重新 IndexOf(beforeObj) ，因为targetObj被remove之后，beforeObj的下标就可能被改变了
            targetOwnerNodes.Insert(targetOwnerNodes.IndexOf(targetNode.Node), sourceNode.Node);

            if (willBeSelected)
                _treeView.SelectedNode = sourceNode.Node;

            _ignoreAfterSelectedEvent = false;

            if (sourceOwnerList != null && sourceOwnerList.Contains(sourceNode.DataBoundItem))
                sourceOwnerList.Remove(sourceNode.DataBoundItem);

            if (targetOwnerList != null && targetOwnerList.Contains(targetNode.DataBoundItem))
                targetOwnerList.Insert(targetOwnerList.IndexOf(targetNode.DataBoundItem), sourceNode.DataBoundItem);
        }

        public void MoveAfter(Filter sourceFilter, Filter targetFilter)
        {
            ITypeBinderTreeViewNode sourceNode = Find(sourceFilter);
            ITypeBinderTreeViewNode targetNode = Find(targetFilter);

            Debug.Assert(sourceNode != null || targetNode != null, "目标树节点为null");

            if (sourceNode == null || targetNode == null)
                return;

            Debug.Assert(sourceNode != targetNode, "指定的两个节点是同一个节点");

            if (sourceNode == targetNode)
                return;

            TreeNodeCollection sourceOwnerNodes;
            if (sourceNode.Parent == null)
                sourceOwnerNodes = _treeView.Nodes;
            else
                sourceOwnerNodes = sourceNode.Parent.Node.Nodes;

            TreeNodeCollection targetOwnerNodes;
            if (targetNode.Parent == null)
                targetOwnerNodes = _treeView.Nodes;
            else
                targetOwnerNodes = targetNode.Parent.Node.Nodes;

            int sourceIndex = sourceOwnerNodes.IndexOf(sourceNode.Node);
            int targetIndex = targetOwnerNodes.IndexOf(targetNode.Node);

            //如果targetObj刚好在beforeObj之后，就无需移动了
            if (sourceOwnerNodes == targetOwnerNodes && sourceIndex - targetIndex == 1)
                return;

            //要在移动节点之前取
            //调整所绑定对象在所在的IList内的顺利
            IList sourceOwnerList = GetOwnerList(sourceNode);
            IList targetOwnerList = GetOwnerList(targetNode);

            //如果原节点原来是选定状态，在移动之后继续保持其选中状态
            //但是不能触发AfterSelect事件，否则会引发外部一些不必要的处理逻辑
            bool willBeSelected = sourceNode.Node.IsSelected;
            _ignoreAfterSelectedEvent = true;

            //调整树节点的顺序
            sourceOwnerNodes.Remove(sourceNode.Node);
            //要重新 IndexOf(beforeObj) ，因为targetObj被remove之后，beforeObj的下标就可能被改变了
            targetOwnerNodes.Insert(targetOwnerNodes.IndexOf(targetNode.Node) + 1, sourceNode.Node);

            if (willBeSelected)             
                _treeView.SelectedNode = sourceNode.Node;

            _ignoreAfterSelectedEvent = false;

            if (sourceOwnerList != null && sourceOwnerList.Contains(sourceNode.DataBoundItem))
                sourceOwnerList.Remove(sourceNode.DataBoundItem);

            if (targetOwnerList != null && targetOwnerList.Contains(targetNode.DataBoundItem))
                targetOwnerList.Insert(targetOwnerList.IndexOf(targetNode.DataBoundItem) + 1, sourceNode.DataBoundItem);
        }

        #endregion

        #region Expand

        /// <summary>
        /// 展开当前节点
        /// </summary>
        public void Expand()
        {
            TreeNode node = _treeView.SelectedNode;
            if (node != null)
                node.Expand();
        }

        #endregion

        #endregion

        #region 事件

        /// <summary>
        /// 用于过滤节点的过滤器方法委托
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public delegate bool Filter(ITypeBinderTreeViewNode node);

        public delegate void OnAfterSelectHandler(object sender, AfterSelectEventArgs e);
        public event OnAfterSelectHandler AfterSelect;

        public class AfterSelectEventArgs
        {
            public ITypeBinderTreeViewNode Node { get; set; }

            public AfterSelectEventArgs(ITypeBinderTreeViewNode node)
            {
                Node = node;
            }
        }

        #endregion
    }
}
