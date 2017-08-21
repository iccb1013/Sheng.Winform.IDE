/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	public class TreeNodeAdv
	{
		private Collection<TreeNodeAdv> _nodes;
		private ReadOnlyCollection<TreeNodeAdv> _children;
		private SETreeViewGrid _tree;
		internal SETreeViewGrid Tree
		{
			get { return _tree; }
		}
		private int _row;
		internal int Row
		{
			get { return _row; }
			set { _row = value; }
		}
		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set 
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					if (Tree.IsMyNode(this))
					{
						if (_isSelected)
						{
							if (!_tree.Selection.Contains(this))
								_tree.Selection.Add(this);
							if (_tree.Selection.Count == 1)
								_tree.CurrentNode = this;
						}
						else
							_tree.Selection.Remove(this);
						_tree.UpdateView();
						_tree.OnSelectionChanged();
					}
				}
			}
		}
		private bool _isLeaf;
		public bool IsLeaf
		{
			get { return _isLeaf; }
			internal set { _isLeaf = value; }
		}
		private bool _isExpandedOnce;
		public bool IsExpandedOnce
		{
			get { return _isExpandedOnce; }
			internal set { _isExpandedOnce = value; }
		}
		private bool _isExpanded;
		public bool IsExpanded
		{
			get { return _isExpanded; }
			set 
			{ 
				if (Tree.IsMyNode(this) && _isExpanded != value)
				{
					if (value)
						Tree.OnExpanding(this);
					else
						Tree.OnCollapsing(this);
					if (value && !_isExpandedOnce)
					{
						Cursor oldCursor = Tree.Cursor;
						Tree.Cursor = Cursors.WaitCursor;
						try
						{
							Tree.ReadChilds(this);
						}
						finally
						{
							Tree.Cursor = oldCursor;
						}
					}
					_isExpanded = value; 
					if (_isExpanded == value)
						Tree.SmartFullUpdate();
					else
						Tree.UpdateView();
					if (value)
						Tree.OnExpanded(this);
					else
						Tree.OnCollapsed(this);
				}
			}
		}
		private TreeNodeAdv _parent;
		public TreeNodeAdv Parent
		{
			get { return _parent; }
			internal set { _parent = value; }
		}
		public int Level
		{
			get
			{
				if (_parent == null)
					return 0;
				else
					return _parent.Level + 1;
			}
		}
		public TreeNodeAdv NextNode
		{
			get
			{
				if (_parent != null)
				{
					int index = _parent.Nodes.IndexOf(this);
					if (index < _parent.Nodes.Count - 1)
						return _parent.Nodes[index + 1];
				}
				return null;
			}
		}
		internal TreeNodeAdv BottomNode
		{
			get
			{
				TreeNodeAdv parent = this.Parent;
				if (parent != null)
				{
					if (parent.NextNode != null)
						return parent.NextNode;
					else
						return parent.BottomNode;
				}
				return null;
			}
		}
		public bool CanExpand
		{
			get
			{
				return (_nodes.Count > 0 || (!IsExpandedOnce && !IsLeaf));
			}
		}
		private object _tag;
		public object Tag
		{
			get { return _tag; }
		}
		internal Collection<TreeNodeAdv> Nodes
		{
			get { return _nodes; }
		}
		public ReadOnlyCollection<TreeNodeAdv> Children
		{
			get
			{
				return _children;
			}
		}
		internal TreeNodeAdv(SETreeViewGrid tree, object tag)
		{
			_row = -1;
			_tree = tree;
			_nodes = new Collection<TreeNodeAdv>();
			_children = new ReadOnlyCollection<TreeNodeAdv>(_nodes);
			_tag = tag;
		}
		public override string ToString()
		{
			if (Tag != null)
				return Tag.ToString();
			else
				return base.ToString();
		}
	}
}
