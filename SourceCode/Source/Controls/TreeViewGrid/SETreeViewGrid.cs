using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Collections;
using System.Drawing.Design;
using Sheng.SailingEase.Controls.TreeViewGrid.NodeControls;

namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	public partial class SETreeViewGrid : Control
	{
		#region Inner Classes

		private struct NodeControlInfo
		{
			public static readonly NodeControlInfo Empty = new NodeControlInfo();

			private NodeControl _control;
			public NodeControl Control
			{
				get { return _control; }
			}

			private Rectangle _bounds;
			public Rectangle Bounds
			{
				get { return _bounds; }
			}

			public NodeControlInfo(NodeControl control, Rectangle bounds)
			{
				_control = control;
				_bounds = bounds;
			}
		}

		private class ExpandedNode
		{
			private object _tag;
			public object Tag
			{
				get { return _tag; }
				set { _tag = value; }
			}

			private Collection<ExpandedNode> _children = new Collection<ExpandedNode>();
			public Collection<ExpandedNode> Children
			{
				get { return _children; }
				set { _children = value; }
			}
		}

		#endregion

		private const int TopMargin = 0;
		private const int LeftMargin = 7;
		private const int ItemDragSensivity = 4;
		private readonly int _columnHeaderHeight;
		private const int DividerWidth = 9; 

		private int _offsetX;
		private int _firstVisibleRow;
		private ReadOnlyCollection<TreeNodeAdv> _readonlySelection;
		private Pen _linePen;
		private Pen _markPen;
		private bool _dragMode;
		private bool _suspendUpdate;
		private bool _structureUpdating;
		private bool _needFullUpdate;
		private bool _fireSelectionEvent;
		private NodePlusMinus _plusMinus;
		private Control _currentEditor;
		private EditableControl _currentEditorOwner;
		private ToolTip _toolTip;

		#region Internal Properties

		private int ColumnHeaderHeight
		{
			get
			{
				if (UseColumns)
					return _columnHeaderHeight;
				else
					return 0;
			}
		}

		/// <summary>
		/// returns all nodes, which parent is expanded
		/// </summary>
		private IEnumerable<TreeNodeAdv> ExpandedNodes
		{
			get
			{
				if (_root.Nodes.Count > 0)
				{
					TreeNodeAdv node = _root.Nodes[0];
					while (node != null)
					{
						yield return node;
						if (node.IsExpanded && node.Nodes.Count > 0)
							node = node.Nodes[0];
						else if (node.NextNode != null)
							node = node.NextNode;
						else
							node = node.BottomNode;
					}
				}
			}
		}

		private bool _suspendSelectionEvent;
		internal bool SuspendSelectionEvent
		{
			get { return _suspendSelectionEvent; }
			set 
			{ 
				_suspendSelectionEvent = value;
				if (!_suspendSelectionEvent && _fireSelectionEvent)
					OnSelectionChanged();
			}
		}

		private List<TreeNodeAdv> _rowMap;
		internal List<TreeNodeAdv> RowMap
		{
			get { return _rowMap; }
		}

		private TreeNodeAdv _selectionStart;
		internal TreeNodeAdv SelectionStart
		{
			get { return _selectionStart; }
			set { _selectionStart = value; }
		}

		private InputState _input;
		internal InputState Input
		{
			get { return _input; }
			set 
			{ 
				_input = value;
			}
		}

		private bool _itemDragMode;
		internal bool ItemDragMode
		{
			get { return _itemDragMode; }
			set { _itemDragMode = value; }
		}

		private Point _itemDragStart;
		internal Point ItemDragStart
		{
			get { return _itemDragStart; }
			set { _itemDragStart = value; }
		}


		/// <summary>
		/// Number of rows fits to the screen
		/// </summary>
		internal int PageRowCount
		{
			get
			{
				return Math.Max((DisplayRectangle.Height - ColumnHeaderHeight) / RowHeight, 0);
			}
		}

		/// <summary>
		/// Number of all visible nodes (which parent is expanded)
		/// </summary>
		internal int RowCount
		{
			get
			{
				return _rowMap.Count;
			}
		}

		private int _contentWidth = 0;
		private int ContentWidth
		{
			get
			{
				return _contentWidth;
			}
		}

		internal int FirstVisibleRow
		{
			get { return _firstVisibleRow; }
			set
			{
				HideEditor();
				_firstVisibleRow = value;
				UpdateView();
			}
		}

		private int OffsetX
		{
			get { return _offsetX; }
			set
			{
				HideEditor();
				_offsetX = value;
				UpdateView();
			}
		}

		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle r = ClientRectangle;
				//r.Y += ColumnHeaderHeight;
				//r.Height -= ColumnHeaderHeight;
				int w = _vScrollBar.Visible ? _vScrollBar.Width : 0;
				int h = _hScrollBar.Visible ? _hScrollBar.Height : 0;
				return new Rectangle(r.X, r.Y, r.Width - w, r.Height - h);
			}
		}

		private List<TreeNodeAdv> _selection;
		internal List<TreeNodeAdv> Selection
		{
			get { return _selection; }
		}

		#endregion

		#region Public Properties

		#region DesignTime

		private bool _fullRowSelect;
		[DefaultValue(false), Category("Behavior")]
		public bool FullRowSelect
		{
			get { return _fullRowSelect; }
			set 
			{ 
				_fullRowSelect = value;
				UpdateView();
			}
		}

		private bool _useColumns;
		[DefaultValue(false), Category("Behavior")]
		public bool UseColumns
		{
			get { return _useColumns; }
			set 
			{ 
				_useColumns = value;
				FullUpdate();
			}
		}

		private bool _showLines = true;
		[DefaultValue(true), Category("Behavior")]
		public bool ShowLines
		{
			get { return _showLines; }
			set 
			{ 
				_showLines = value;
				UpdateView();
			}
		}

		private bool _showNodeToolTips = false;
		[DefaultValue(false), Category("Behavior")]
		public bool ShowNodeToolTips
		{
			get { return _showNodeToolTips; }
			set { _showNodeToolTips = value; }
		}

		private bool _keepNodesExpanded;
		[DefaultValue(false), Category("Behavior")]
		public bool KeepNodesExpanded
		{
			get { return _keepNodesExpanded; }
			set { _keepNodesExpanded = value; }
		}

		private ITreeModel _model;
		[Category("Data")]
		public ITreeModel Model
		{
			get { return _model; }
			set
			{
				if (_model != value)
				{
					if (_model != null)
						UnbindModelEvents();
					_model = value;
					CreateNodes();
					FullUpdate();
					if (_model != null)
						BindModelEvents();
				}
			}
		}

		private BorderStyle _borderStyle;
		[DefaultValue(BorderStyle.Fixed3D), Category("Appearance")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this._borderStyle;
			}
			set
			{
				if (_borderStyle != value)
				{
					_borderStyle = value;
					base.UpdateStyles();
				}
			}
		}

		private int _rowHeight = 16;
		[DefaultValue(16), Category("Appearance")]
		public int RowHeight
		{
			get
			{
				return _rowHeight;
			}
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException();

				_rowHeight = value;
				FullUpdate();
			}
		}

		private TreeSelectionMode _selectionMode = TreeSelectionMode.Single;
		[DefaultValue(TreeSelectionMode.Single), Category("Behavior")]
		public TreeSelectionMode SelectionMode
		{
			get { return _selectionMode; }
			set { _selectionMode = value; }
		}

		private bool _hideSelection;
		[DefaultValue(false), Category("Behavior")]
		public bool HideSelection
		{
			get { return _hideSelection; }
			set 
			{ 
				_hideSelection = value;
				UpdateView();
			}
		}

		private float _topEdgeSensivity = 0.3f;
		[DefaultValue(0.3f), Category("Behavior")]
		public float TopEdgeSensivity
		{
			get { return _topEdgeSensivity; }
			set
			{
				if (value < 0 || value > 1)
					throw new ArgumentOutOfRangeException();
				_topEdgeSensivity = value;
			}
		}

		private float _bottomEdgeSensivity = 0.3f;
		[DefaultValue(0.3f), Category("Behavior")]
		public float BottomEdgeSensivity
		{
			get { return _bottomEdgeSensivity; }
			set
			{
				if (value < 0 || value > 1)
					throw new ArgumentOutOfRangeException("value should be from 0 to 1");
				_bottomEdgeSensivity = value;
			}
		}

		private bool _loadOnDemand;
		[DefaultValue(false), Category("Behavior")]
		public bool LoadOnDemand
		{
			get { return _loadOnDemand; }
			set { _loadOnDemand = value; }
		}

		private int _indent = 19;
		[DefaultValue(19), Category("Behavior")]
		public int Indent
		{
			get { return _indent; }
			set 
			{ 
				_indent = value;
				UpdateView();
			}
		}

		private Color _lineColor = SystemColors.ControlDark;
		[Category("Behavior")]
		public Color LineColor
		{
			get { return _lineColor; }
			set 
			{ 
				_lineColor = value;
				CreateLinePen();
				UpdateView();
			}
		}

		private Color _dragDropMarkColor = Color.Black;
		[Category("Behavior")]
		public Color DragDropMarkColor
		{
			get { return _dragDropMarkColor; }
			set 
			{ 
				_dragDropMarkColor = value;
				CreateMarkPen();
			}
		}

		private float _dragDropMarkWidth = 3.0f;
		[DefaultValue(3.0f), Category("Behavior")]
		public float DragDropMarkWidth
		{
			get { return _dragDropMarkWidth; }
			set 
			{ 
				_dragDropMarkWidth = value; 
				CreateMarkPen();
			}
		}

		private TreeColumnCollection _columns;
		[Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Collection<TreeColumn> Columns
		{
			get { return _columns; }
		}

		private NodeControlsCollection _controls;
		[Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(NodeControlCollectionEditor), typeof(UITypeEditor))]
		public Collection<NodeControl> NodeControls
		{
			get
			{
				return _controls;
			}
		}

		#endregion

		#region RunTime

		[Browsable(false)]
		public IEnumerable<TreeNodeAdv> AllNodes
		{
			get
			{
				if (_root.Nodes.Count > 0)
				{
					TreeNodeAdv node = _root.Nodes[0];
					while (node != null)
					{
						yield return node;
						if (node.Nodes.Count > 0)
							node = node.Nodes[0];
						else if (node.NextNode != null)
							node = node.NextNode;
						else
							node = node.BottomNode;
					}
				}
			}
		}

		private DropPosition _dropPosition;
		[Browsable(false)]
		public DropPosition DropPosition
		{
			get { return _dropPosition; }
			set { _dropPosition = value; }
		}

		private TreeNodeAdv _root;
		[Browsable(false)]
		public TreeNodeAdv Root
		{
			get { return _root; }
		}

		[Browsable(false)]
		public ReadOnlyCollection<TreeNodeAdv> SelectedNodes
		{
			get
			{
				return _readonlySelection;
			}
		}

		[Browsable(false)]
		public TreeNodeAdv SelectedNode
		{
			get
			{
				if (Selection.Count > 0)
				{
					if (CurrentNode != null && CurrentNode.IsSelected)
						return CurrentNode;
					else
						return Selection[0];
				}
				else
					return null;
			}
			set
			{
				if (SelectedNode == value)
					return;

				BeginUpdate();
				try
				{
					if (value == null)
					{
						ClearSelection();
					}
					else
					{
						if (!IsMyNode(value))
							throw new ArgumentException();

						ClearSelection();
						value.IsSelected = true;
						CurrentNode = value;
						EnsureVisible(value);
					}
				}
				finally
				{
					EndUpdate();
				}
			}
		}

		private TreeNodeAdv _currentNode;
		[Browsable(false)]
		public TreeNodeAdv CurrentNode
		{
			get { return _currentNode; }
			internal set { _currentNode = value; }
		}


		#endregion

		#endregion

		#region Public Events
	
		[Category("Action")]
		public event ItemDragEventHandler ItemDrag;
		private void OnItemDrag(MouseButtons buttons, object item)
		{
			if (ItemDrag != null)
				ItemDrag(this, new ItemDragEventArgs(buttons, item));
		}

		[Category("Behavior")]
		public event EventHandler<TreeNodeAdvMouseEventArgs> NodeMouseDoubleClick;
		private void OnNodeMouseDoubleClick(TreeNodeAdvMouseEventArgs args)
		{
			if (NodeMouseDoubleClick != null)
				NodeMouseDoubleClick(this, args);
		}

		[Category("Behavior")]
		public event EventHandler<TreeColumnEventArgs> ColumnWidthChanged;
		internal void OnColumnWidthChanged(TreeColumn column)
		{
			if (ColumnWidthChanged != null)
				ColumnWidthChanged(this, new TreeColumnEventArgs(column));
		}

		[Category("Behavior")]
		public event EventHandler SelectionChanged;
		internal void OnSelectionChanged()
		{
			if (SuspendSelectionEvent)
				_fireSelectionEvent = true;
			else
			{
				_fireSelectionEvent = false;
				if (SelectionChanged != null)
					SelectionChanged(this, EventArgs.Empty);
			}
		}

		[Category("Behavior")]
		public event EventHandler<TreeViewAdvEventArgs> Collapsing;
		internal void OnCollapsing(TreeNodeAdv node)
		{
			if (Collapsing != null)
				Collapsing(this, new TreeViewAdvEventArgs(node));
		}

		[Category("Behavior")]
		public event EventHandler<TreeViewAdvEventArgs> Collapsed;
		internal void OnCollapsed(TreeNodeAdv node)
		{
			if (Collapsed != null)
				Collapsed(this, new TreeViewAdvEventArgs(node));
		}

		[Category("Behavior")]
		public event EventHandler<TreeViewAdvEventArgs> Expanding;
		internal void OnExpanding(TreeNodeAdv node)
		{
			if (Expanding != null)
				Expanding(this, new TreeViewAdvEventArgs(node));
		}

		[Category("Behavior")]
		public event EventHandler<TreeViewAdvEventArgs> Expanded;
		internal void OnExpanded(TreeNodeAdv node)
		{
			if (Expanded != null)
				Expanded(this, new TreeViewAdvEventArgs(node));
		}

		#endregion

		public SETreeViewGrid()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint
				| ControlStyles.UserPaint
				| ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.ResizeRedraw
				| ControlStyles.Selectable
				, true);


			if (Application.RenderWithVisualStyles)
				_columnHeaderHeight = 20;
			else
				_columnHeaderHeight = 17;

			BorderStyle = BorderStyle.Fixed3D;
			_hScrollBar.Height = SystemInformation.HorizontalScrollBarHeight;
			_vScrollBar.Width = SystemInformation.VerticalScrollBarWidth;
			_rowMap = new List<TreeNodeAdv>();
			_selection = new List<TreeNodeAdv>();
			_readonlySelection = new ReadOnlyCollection<TreeNodeAdv>(_selection);
			_columns = new TreeColumnCollection(this);
			_toolTip = new ToolTip();

			Input = new NormalInputState(this);
			CreateNodes();
			CreatePens();

			ArrangeControls();

			_plusMinus = new NodePlusMinus();
			_controls = new NodeControlsCollection(this);
		}


		#region Public Methods

		public TreePath GetPath(TreeNodeAdv node)
		{
			if (node == _root)
				return TreePath.Empty;
			else
			{
				Stack<object> stack = new Stack<object>();
				while (node != _root)
				{
					stack.Push(node.Tag);
					node = node.Parent;
				}
				return new TreePath(stack.ToArray());
			}
		}

		public TreeNodeAdv GetNodeAt(Point point)
		{
			if (point.X < 0 || point.Y < 0)
				return null;
			
			point = ToAbsoluteLocation(point);
			int row = point.Y / RowHeight;
			if (row < RowCount && row >= 0)
			{
				NodeControlInfo info = GetNodeControlInfoAt(_rowMap[row], point);
				if (info.Control != null)
					return _rowMap[row];
			}
			return null;
		}

		public void BeginUpdate()
		{
			_suspendUpdate = true;
		}

		public void EndUpdate()
		{
			_suspendUpdate = false;
			if (_needFullUpdate)
				FullUpdate();
			else
				UpdateView();
		}

		public void ExpandAll()
		{
			BeginUpdate();
			SetIsExpanded(_root, true);
			EndUpdate();
		}

		public void CollapseAll()
		{
			BeginUpdate();
			SetIsExpanded(_root, false);
			EndUpdate();
		}


		/// <summary>
		/// Expand all parent nodes, andd scroll to the specified node
		/// </summary>
		public void EnsureVisible(TreeNodeAdv node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			if (!IsMyNode(node))
				throw new ArgumentException();

			TreeNodeAdv parent = node.Parent;
			while (parent != _root)
			{
				parent.IsExpanded = true;
				parent = parent.Parent;
			}
			ScrollTo(node);
		}

		/// <summary>
		/// Make node visible, scroll if needed. All parent nodes of the specified node must be expanded
		/// </summary>
		/// <param name="node"></param>
		public void ScrollTo(TreeNodeAdv node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			if (!IsMyNode(node))
				throw new ArgumentException();

			if (node.Row < 0)
				CreateRowMap();

			int row = 0;
			if (node.Row < FirstVisibleRow)
				row = node.Row;
			else if (node.Row >= FirstVisibleRow + (PageRowCount - 1))
				row = node.Row - (PageRowCount - 1);

			if (row >= _vScrollBar.Minimum && row <= _vScrollBar.Maximum)
				_vScrollBar.Value = row;
		}

		#endregion

		private Point ToAbsoluteLocation(Point point)
		{
			return new Point(point.X + _offsetX, point.Y + (FirstVisibleRow * RowHeight) - ColumnHeaderHeight);
		}

		private Point ToViewLocation(Point point)
		{
			return new Point(point.X - _offsetX, point.Y - (FirstVisibleRow * RowHeight) + ColumnHeaderHeight);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			ArrangeControls();
			SafeUpdateScrollBars();
			base.OnSizeChanged(e);
		}

		private void ArrangeControls()
		{
			int hBarSize = _hScrollBar.Height;
			int vBarSize = _vScrollBar.Width;
			Rectangle clientRect = ClientRectangle;
			
			_hScrollBar.SetBounds(clientRect.X, clientRect.Bottom - hBarSize,
				clientRect.Width - vBarSize, hBarSize);

			_vScrollBar.SetBounds(clientRect.Right - vBarSize, clientRect.Y,
				vBarSize, clientRect.Height - hBarSize);
		}

		private void SafeUpdateScrollBars()
		{
			if (InvokeRequired)
				Invoke(new MethodInvoker(UpdateScrollBars));
			else
				UpdateScrollBars();
		}

		private void UpdateScrollBars()
		{
			UpdateVScrollBar();
			UpdateHScrollBar();
			UpdateVScrollBar();
			UpdateHScrollBar();
			_hScrollBar.Width = DisplayRectangle.Width;
			_vScrollBar.Height = DisplayRectangle.Height;
		}

		private void UpdateHScrollBar()
		{
			_hScrollBar.Maximum = ContentWidth;
			_hScrollBar.LargeChange = Math.Max(DisplayRectangle.Width, 0);
			_hScrollBar.SmallChange = 5;
			_hScrollBar.Visible = _hScrollBar.LargeChange < _hScrollBar.Maximum;
			_hScrollBar.Value = Math.Min(_hScrollBar.Value, _hScrollBar.Maximum - _hScrollBar.LargeChange + 1);
		}

		private void UpdateVScrollBar()
		{
			_vScrollBar.Maximum = Math.Max(RowCount - 1, 0);
			_vScrollBar.LargeChange = PageRowCount;
			_vScrollBar.Visible = _vScrollBar.LargeChange <= _vScrollBar.Maximum;
			_vScrollBar.Value = Math.Min(_vScrollBar.Value, _vScrollBar.Maximum - _vScrollBar.LargeChange + 1);
		}

		private void CreatePens()
		{
			CreateLinePen();
			CreateMarkPen();
		}

		private void CreateMarkPen()
		{
			GraphicsPath path = new GraphicsPath();
			path.AddLines(new Point[] { new Point(0, 0), new Point(1, 1), new Point(-1, 1), new Point(0, 0) });
			CustomLineCap cap = new CustomLineCap(null, path);
			cap.WidthScale = 1.0f;

			_markPen = new Pen(_dragDropMarkColor, _dragDropMarkWidth);
			_markPen.CustomStartCap = cap;
			_markPen.CustomEndCap = cap;
		}

		private void CreateLinePen()
		{
			_linePen = new Pen(_lineColor);
			_linePen.DashStyle = DashStyle.Dot;
		}

		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams res = base.CreateParams;
				switch (BorderStyle)
				{
					case BorderStyle.FixedSingle:
							res.Style |= 0x800000;
							break;
					case BorderStyle.Fixed3D:
							res.ExStyle |= 0x200;
						break;
				}
				return res;
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			DisposeEditor();
			UpdateView();
			ChangeInput();
			base.OnGotFocus(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			DisposeEditor();
			UpdateView();
			base.OnLeave(e);
		}

		#region Keys

		protected override bool IsInputKey(Keys keyData)
		{
			if (((keyData & Keys.Up) == Keys.Up)
				|| ((keyData & Keys.Down) == Keys.Down)
				|| ((keyData & Keys.Left) == Keys.Left)
				|| ((keyData & Keys.Right) == Keys.Right))
				return true;
			else
				return base.IsInputKey(keyData);
		}

		internal void ChangeInput()
		{
			if ((ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				if (!(Input is InputWithShift))
					Input = new InputWithShift(this);
			}
			else if ((ModifierKeys & Keys.Control) == Keys.Control)
			{
				if (!(Input is InputWithControl))
					Input = new InputWithControl(this);
			}
			else 
			{
				if (!(Input.GetType() == typeof(NormalInputState)))
					Input = new NormalInputState(this);
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!e.Handled)
			{
				if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey)
					ChangeInput();
				Input.KeyDown(e);
				if (!e.Handled)
				{
					foreach (NodeControlInfo item in GetNodeControls(CurrentNode))
					{
						item.Control.KeyDown(e);
						if (e.Handled)
							return;
					}
				}
			}
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (!e.Handled)
			{
				if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey)
					ChangeInput();
				if (!e.Handled)
				{
					foreach (NodeControlInfo item in GetNodeControls(CurrentNode))
					{
						item.Control.KeyUp(e);
						if (e.Handled)
							return;
					}
				}
			}
		}

		#endregion 

		#region Mouse

		private TreeNodeAdvMouseEventArgs CreateMouseArgs(MouseEventArgs e)
		{
			TreeNodeAdvMouseEventArgs args = new TreeNodeAdvMouseEventArgs(e);
			args.ViewLocation = e.Location;
			args.AbsoluteLocation = ToAbsoluteLocation(e.Location);
			args.ModifierKeys = ModifierKeys;
			args.Node = GetNodeAt(e.Location);
			NodeControlInfo info = GetNodeControlInfoAt(args.Node, args.AbsoluteLocation);
			args.ControlBounds = info.Bounds;
			args.Control = info.Control;
			return args;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (SystemInformation.MouseWheelScrollLines > 0)
			{
				int lines = e.Delta / 120 * SystemInformation.MouseWheelScrollLines;
				int newValue = _vScrollBar.Value - lines;
				_vScrollBar.Value = Math.Max(_vScrollBar.Minimum,
					Math.Min(_vScrollBar.Maximum - _vScrollBar.LargeChange + 1, newValue));
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!Focused)
				Focus();

			if (e.Button == MouseButtons.Left)
			{
				TreeColumn c = GetColumnDividerAt(e.Location);
				if (c != null)
				{
					Input = new ResizeColumnState(this, c, e.Location);
					return;
				}
			}

			ChangeInput();
			TreeNodeAdvMouseEventArgs args = CreateMouseArgs(e);

			if (args.Node != null && args.Control != null)
				args.Control.MouseDown(args);

			if (!args.Handled)
				Input.MouseDown(args);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			TreeNodeAdvMouseEventArgs args = CreateMouseArgs(e);
			if (Input is ResizeColumnState)
				Input.MouseUp(args);
			else
			{
				base.OnMouseUp(e);
				if (args.Node != null && args.Control != null)
					args.Control.MouseUp(args);
				if (!args.Handled)
					Input.MouseUp(args);
			}
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			TreeNodeAdvMouseEventArgs args = CreateMouseArgs(e);
			if (args.Node != null)
			{
				OnNodeMouseDoubleClick(args);
				if (args.Handled)
					return;
			}

			if (args.Node != null && args.Control != null)
				args.Control.MouseDoubleClick(args);
			if (!args.Handled)
			{
				if (args.Node != null && args.Button == MouseButtons.Left)
					args.Node.IsExpanded = !args.Node.IsExpanded;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (Input.MouseMove(e))
				return;

			base.OnMouseMove(e);
			SetCursor(e);
			if (e.Location.Y <= ColumnHeaderHeight)
			{
				_toolTip.Active = false;
			}
			else
			{
				UpdateToolTip(e);
				if (ItemDragMode && Dist(e.Location, ItemDragStart) > ItemDragSensivity
					&& CurrentNode != null && CurrentNode.IsSelected)
				{
					ItemDragMode = false;
					_toolTip.Active = false;
					OnItemDrag(e.Button, Selection.ToArray());
				}
			}
		}

		private void SetCursor(MouseEventArgs e)
		{
			if (GetColumnDividerAt(e.Location) == null)
				this.Cursor = Cursors.Default;
			else
				this.Cursor = Cursors.VSplit;
		}

		private TreeColumn GetColumnDividerAt(Point p)
		{
			if (p.Y > ColumnHeaderHeight)
				return null;

			int x = -OffsetX;
			foreach (TreeColumn c in Columns)
			{
				if (c.IsVisible)
				{
					x += c.Width;
					Rectangle rect = new Rectangle(x - DividerWidth / 2, 0, DividerWidth, ColumnHeaderHeight);
					if (rect.Contains(p))
						return c;
				}
			}
			return null;
		}

		TreeNodeAdv _hoverNode;
		NodeControl _hoverControl;
		private void UpdateToolTip(MouseEventArgs e)
		{
			if (_showNodeToolTips)
			{
				TreeNodeAdvMouseEventArgs args = CreateMouseArgs(e);
				if (args.Node != null)
				{
					if (args.Node != _hoverNode || args.Control != _hoverControl)
					{
						string msg = args.Control.GetToolTip(args.Node);
						if (!String.IsNullOrEmpty(msg))
						{
							_toolTip.SetToolTip(this, msg);
							_toolTip.Active = true;
						}
						else
							_toolTip.SetToolTip(this, null);
					}
				}
				else
					_toolTip.SetToolTip(this, null);

				_hoverControl = args.Control;
				_hoverNode = args.Node;

			}
			else
				_toolTip.SetToolTip(this, null);
		}
		#endregion

		#region DragDrop
		protected override void OnDragOver(DragEventArgs drgevent)
		{
			ItemDragMode = false;
			_dragMode = true;
			Point pt = PointToClient(new Point(drgevent.X, drgevent.Y));
			SetDropPosition(pt);
			UpdateView();
			base.OnDragOver(drgevent);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			_dragMode = false;
			UpdateView();
			base.OnDragLeave(e);
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			_dragMode = false;
			UpdateView();
			base.OnDragDrop(drgevent);
		}
		#endregion

		private IEnumerable<NodeControlInfo> GetNodeControls(TreeNodeAdv node)
		{
			if (node == null)
				yield break;

			int y = node.Row * RowHeight + TopMargin;
			int x = (node.Level - 1) * _indent + LeftMargin;
			int width = _plusMinus.MeasureSize(node).Width;

			Rectangle rect = new Rectangle(x, y, width, RowHeight);
			if (UseColumns && Columns.Count > 0 && Columns[0].Width < rect.Right)
				rect.Width = Columns[0].Width - x;
			yield return new NodeControlInfo(_plusMinus, rect);

			x += (width + 1);
			if (!UseColumns)
			{
				foreach (NodeControl c in NodeControls)
				{
					width = c.MeasureSize(node).Width; 
					rect = new Rectangle(x, y, width, RowHeight);
					x += (width + 1);
					yield return new NodeControlInfo(c, rect);
				}
			}
			else
			{
				int right = 0;
				foreach (TreeColumn col in Columns)
				{
					if (col.IsVisible)
					{
						right += col.Width;
						for (int i = 0; i < NodeControls.Count; i++)
						{
							NodeControl nc = NodeControls[i];
							if (nc.Column == col.Index)
							{
								bool isLastControl = true;
								for (int k = i + 1; k < NodeControls.Count; k++)
									if (NodeControls[k].Column == col.Index)
									{
										isLastControl = false;
										break;
									}

								width = right - x;
								if (!isLastControl)
									width = nc.MeasureSize(node).Width;
								int maxWidth = Math.Max(0, right - x);
								rect = new Rectangle(x, y, Math.Min(maxWidth, width), RowHeight);
								x += (width + 1);
								yield return new NodeControlInfo(nc, rect);
							}
						}
						x = right;
					}
				}
			}
		}

		private static double Dist(Point p1, Point p2)
		{
			return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
		}

		private void SetDropPosition(Point pt)
		{
			TreeNodeAdv node = GetNodeAt(pt);
			_dropPosition.Node = node;
			if (node != null)
			{
				float pos = (pt.Y - ColumnHeaderHeight - ((node.Row - FirstVisibleRow) * RowHeight)) / (float)RowHeight;
				if (pos < TopEdgeSensivity)
					_dropPosition.Position = NodePosition.Before;
				else if (pos > (1 - BottomEdgeSensivity))
					_dropPosition.Position = NodePosition.After;
				else
					_dropPosition.Position = NodePosition.Inside;
			}
		}

		internal void FullUpdate()
		{
			CreateRowMap();
			SafeUpdateScrollBars();
			UpdateView();
			_needFullUpdate = false;
		}

		internal void UpdateView()
		{
			if (!_suspendUpdate)
				Invalidate(false);
		}

		private void CreateNodes()
		{
			Selection.Clear();
			SelectionStart = null;
			_root = new TreeNodeAdv(this, null);
			_root.IsExpanded = true;
			if (_root.Nodes.Count > 0)
				CurrentNode = _root.Nodes[0];
			else
				CurrentNode = null;
		}

		internal void ReadChilds(TreeNodeAdv parentNode)
		{
			ReadChilds(parentNode, null);
		}

		private void ReadChilds(TreeNodeAdv parentNode, Collection<ExpandedNode> expandedNodes)
		{
			if (!parentNode.IsLeaf)
			{
				parentNode.IsExpandedOnce = true;
				foreach (TreeNodeAdv n in parentNode.Nodes)
				{
					n.Parent = null;
				}
				parentNode.Nodes.Clear();
				if (Model != null)
				{
					IEnumerable items = Model.GetChildren(GetPath(parentNode));
					if (items != null)
					{
						foreach (object obj in items)
						{
							Collection<ExpandedNode> expandedChildren = null;
							if (expandedNodes != null)
								foreach(ExpandedNode str in expandedNodes)
								{
									if (str.Tag == obj)
									{
										expandedChildren = str.Children;
										break;
									}
								}
							AddNode(parentNode, obj, -1, expandedChildren);
						}
					}
				}
			}
		}

		private void AddNode(TreeNodeAdv parent, object tag, int index, Collection<ExpandedNode> expandedChildren)
		{
			TreeNodeAdv node = new TreeNodeAdv(this, tag);
			node.Parent = parent;

			if (index >= 0 && index < parent.Nodes.Count)
				parent.Nodes.Insert(index, node);
			else
				parent.Nodes.Add(node);

			node.IsLeaf = Model.IsLeaf(GetPath(node));
			if (!LoadOnDemand)
				ReadChilds(node);
			else if (expandedChildren != null)
			{
				ReadChilds(node, expandedChildren);
				node.IsExpanded = true;
			}
		}

		private void AddNode(TreeNodeAdv parent, object tag, int index)
		{
			AddNode(parent, tag, index, null);
		}

		private void CreateRowMap()
		{
			_rowMap.Clear();
			int row = 0;
			_contentWidth = 0;
			foreach (TreeNodeAdv node in ExpandedNodes)
			{
				node.Row = row;
				_rowMap.Add(node);
				if (!UseColumns)
				{
					Rectangle rect = GetNodeBounds(node);
					_contentWidth = Math.Max(_contentWidth, rect.Right);
				}
				row++;
			}
			if (UseColumns)
			{
				_contentWidth = 0;
				foreach (TreeColumn col in _columns)
					if (col.IsVisible)
						_contentWidth += col.Width;
			}
		}

		private NodeControlInfo GetNodeControlInfoAt(TreeNodeAdv node, Point point)
		{
			foreach (NodeControlInfo info in GetNodeControls(node))
				if (info.Bounds.Contains(point))
					return info;

			return NodeControlInfo.Empty;
		}

		private Rectangle GetNodeBounds(TreeNodeAdv node)
		{
			Rectangle res = Rectangle.Empty;
			foreach (NodeControlInfo info in GetNodeControls(node))
			{
				if (res == Rectangle.Empty)
					res = info.Bounds;
				else
					res = Rectangle.Union(res, info.Bounds);
			}
			return res;
		}

		private void _vScrollBar_ValueChanged(object sender, EventArgs e)
		{
			FirstVisibleRow = _vScrollBar.Value;
		}

		private void _hScrollBar_ValueChanged(object sender, EventArgs e)
		{
			OffsetX = _hScrollBar.Value;
		}

		private void SetIsExpanded(TreeNodeAdv root, bool value)
		{
			foreach (TreeNodeAdv node in root.Nodes)
			{
				node.IsExpanded = value;
				SetIsExpanded(node, value);
			}
		}

		public void ClearSelection()
		{
			while (Selection.Count > 0)
				Selection[0].IsSelected = false;
		}

		internal void SmartFullUpdate()
		{
			if (_suspendUpdate || _structureUpdating)
				_needFullUpdate = true;
			else
				FullUpdate();
		}

		internal bool IsMyNode(TreeNodeAdv node)
		{
			if (node == null)
				return false;

			if (node.Tree != this)
				return false;

			while (node.Parent != null)
				node = node.Parent;

			return node == _root;
		}

		private void UpdateSelection()
		{
			bool flag = false;

			if (!IsMyNode(CurrentNode))
				CurrentNode = null;
			if (!IsMyNode(_selectionStart))
				_selectionStart = null;

			for (int i = Selection.Count - 1; i >= 0; i--)
				if (!IsMyNode(Selection[i]))
				{
					flag = true;
					Selection.RemoveAt(i);
				}

			if (flag)
				OnSelectionChanged();
		}

		internal void UpdateHeaders()
		{
			UpdateView();
		}

		internal void UpdateColumns()
		{
			FullUpdate();
		}

		internal void ChangeColumnWidth(TreeColumn column)
		{
			if (!(_input is ResizeColumnState))
			{
				FullUpdate();
				OnColumnWidthChanged(column);
			}
		}

		#region Draw

		protected override void OnPaint(PaintEventArgs e)
		{
			DrawContext context = new DrawContext();
			context.Graphics = e.Graphics;
			context.Font = this.Font;
			context.Enabled = Enabled;

			int y = 0;
			if (UseColumns)
			{
				DrawColumnHeaders(e.Graphics);
				y = ColumnHeaderHeight;
				if (Columns.Count == 0)
					return;
			}

			e.Graphics.ResetTransform();
			e.Graphics.TranslateTransform(-OffsetX, y - (FirstVisibleRow * RowHeight));
			int row = FirstVisibleRow;
			while (row < RowCount && row - FirstVisibleRow <= PageRowCount)
			{
				TreeNodeAdv node = _rowMap[row];
				context.DrawSelection = DrawSelectionMode.None;
				context.CurrentEditorOwner = _currentEditorOwner;
				if (_dragMode)
				{
					if ((_dropPosition.Node == node) && _dropPosition.Position == NodePosition.Inside)
						context.DrawSelection = DrawSelectionMode.Active;
				}
				else
				{
					if (node.IsSelected && Focused)
						context.DrawSelection = DrawSelectionMode.Active;
					else if (node.IsSelected && !Focused && !HideSelection)
						context.DrawSelection = DrawSelectionMode.Inactive;
				}
				context.DrawFocus = Focused && CurrentNode == node;

				if (FullRowSelect)
				{
					context.DrawFocus = false;
					if (context.DrawSelection == DrawSelectionMode.Active || context.DrawSelection == DrawSelectionMode.Inactive)
					{
						Rectangle focusRect = new Rectangle(OffsetX, row * RowHeight, ClientRectangle.Width, RowHeight);
						if (context.DrawSelection == DrawSelectionMode.Active)
						{
							e.Graphics.FillRectangle(SystemBrushes.Highlight, focusRect);
							context.DrawSelection = DrawSelectionMode.FullRowSelect;
						}
						else
						{
							e.Graphics.FillRectangle(SystemBrushes.InactiveBorder, focusRect);
							context.DrawSelection = DrawSelectionMode.None;
						}
					}
				}

				if (ShowLines)
					DrawLines(e.Graphics, node);

				DrawNode(node, context);
				row++;
			}

			if (_dropPosition.Node != null && _dragMode)
				DrawDropMark(e.Graphics);

			e.Graphics.ResetTransform();
			DrawScrollBarsBox(e.Graphics);
		}

		private void DrawColumnHeaders(Graphics gr)
		{
			int x = 0;
			VisualStyleRenderer renderer = null;
			if (Application.RenderWithVisualStyles)
				renderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);

			DrawHeaderBackground(gr, renderer, new Rectangle(0, 0, ClientRectangle.Width + 10, ColumnHeaderHeight));
			gr.TranslateTransform(-OffsetX, 0);
			foreach (TreeColumn c in Columns)
			{
				if (c.IsVisible)
				{
					Rectangle rect = new Rectangle(x, 0, c.Width, ColumnHeaderHeight);
					x += c.Width;
					DrawHeaderBackground(gr, renderer, rect);
					c.Draw(gr, rect, Font);
				}
			}
		}

		private static void DrawHeaderBackground(Graphics gr, VisualStyleRenderer renderer, Rectangle rect)
		{
			if (renderer != null)
				renderer.DrawBackground(gr, rect);
			else
			{
				gr.FillRectangle(SystemBrushes.Control, rect);

				gr.DrawLine(SystemPens.ControlDark, rect.X, rect.Bottom - 2, rect.Right, rect.Bottom - 2);
				gr.DrawLine(SystemPens.ControlLightLight, rect.X, rect.Bottom - 1, rect.Right, rect.Bottom - 1);

				gr.DrawLine(SystemPens.ControlDark, rect.Right - 2, rect.Y, rect.Right - 2, rect.Bottom - 2);
				gr.DrawLine(SystemPens.ControlLightLight, rect.Right - 1, rect.Y, rect.Right - 1, rect.Bottom - 1);
			}
		}

		public void DrawNode(TreeNodeAdv node, DrawContext context)
		{
			foreach (NodeControlInfo item in GetNodeControls(node))
			{
				context.Bounds = item.Bounds;
				context.Graphics.SetClip(item.Bounds);
				item.Control.Draw(node, context);
				context.Graphics.ResetClip();
			}
		}

		private void DrawScrollBarsBox(Graphics gr)
		{
			Rectangle r1 = DisplayRectangle;
			Rectangle r2 = ClientRectangle;
			gr.FillRectangle(SystemBrushes.Control,
				new Rectangle(r1.Right, r1.Bottom, r2.Width - r1.Width, r2.Height - r1.Height));
		}

		private void DrawDropMark(Graphics gr)
		{
			if (_dropPosition.Position == NodePosition.Inside)
				return;

			Rectangle rect = GetNodeBounds(_dropPosition.Node);
			int right = DisplayRectangle.Right - LeftMargin + OffsetX;
			int y = rect.Y;
			if (_dropPosition.Position == NodePosition.After)
				y = rect.Bottom;
			gr.DrawLine(_markPen, rect.X, y, right, y);
		}

		private void DrawLines(Graphics gr, TreeNodeAdv node)
		{
			if (UseColumns && Columns.Count > 0)
				gr.SetClip(new Rectangle(0, 0, Columns[0].Width, RowCount * RowHeight + ColumnHeaderHeight));

			int row = node.Row;
			TreeNodeAdv curNode = node;
			while (curNode != _root)
			{
				int level = curNode.Level;
				int x = (level - 1) * _indent + NodePlusMinus.ImageSize / 2 + LeftMargin;
				int width = NodePlusMinus.Width - NodePlusMinus.ImageSize / 2;
				int y = row * RowHeight + TopMargin;
				int y2 = y + RowHeight;

				if (curNode == node)
				{
					int midy = y + RowHeight / 2;
					gr.DrawLine(_linePen, x, midy, x + width, midy);
					if (curNode.NextNode == null)
						y2 = y + RowHeight / 2;
				}

				if (node.Row == 0)
					y = RowHeight / 2;
				if (curNode.NextNode != null || curNode == node)
					gr.DrawLine(_linePen, x, y, x, y2);

				curNode = curNode.Parent;
			}

			gr.ResetClip();
		}

		#endregion

		#region Editor
		public void DisplayEditor(Control control, EditableControl owner)
		{
			if (control == null || owner == null)
				throw new ArgumentNullException();

			if (CurrentNode != null)
			{
				DisposeEditor();
				EditorContext context = new EditorContext();
				context.Owner = owner;
				context.CurrentNode = CurrentNode;
				context.Editor = control;

				SetEditorBounds(context);

				_currentEditor = control;
				_currentEditorOwner = owner;
				UpdateView();
				control.Parent = this;
				control.Focus();
				owner.UpdateEditor(control);
			}
		}

		private void SetEditorBounds(EditorContext context)
		{
			foreach (NodeControlInfo info in GetNodeControls(context.CurrentNode))
			{
				if (context.Owner == info.Control && info.Control is EditableControl)
				{
					Point p = ToViewLocation(info.Bounds.Location);
					int width = DisplayRectangle.Width - p.X;
					if (UseColumns && info.Control.Column < Columns.Count)
					{
						Rectangle rect = GetColumnBounds(info.Control.Column);
						width = rect.Right - OffsetX - p.X;
					}
					context.Bounds = new Rectangle(p.X, p.Y, width, info.Bounds.Height);
					((EditableControl)info.Control).SetEditorBounds(context);
					return;
				}
			}
		}

		private Rectangle GetColumnBounds(int column)
		{
			int x = 0;
			for (int i = 0; i < Columns.Count; i++)
			{
				if (Columns[i].IsVisible)
				{
					if (i < column)
						x += Columns[i].Width;
					else
						return new Rectangle(x, 0, Columns[i].Width, 0);
				}
			}
			return Rectangle.Empty;
		}

		public void HideEditor()
		{
			this.Focus();
			DisposeEditor();
		}

		public void UpdateEditorBounds()
		{
			if (_currentEditor != null)
			{
				EditorContext context = new EditorContext();
				context.Owner = _currentEditorOwner;
				context.CurrentNode = CurrentNode;
				context.Editor = _currentEditor;
				SetEditorBounds(context);
			}
		}

		private void DisposeEditor()
		{
			if (_currentEditor != null)
				_currentEditor.Parent = null;
			if (_currentEditor != null)
				_currentEditor.Dispose();
			_currentEditor = null;
			_currentEditorOwner = null;
		}
		#endregion

		#region ModelEvents
		private void BindModelEvents()
		{
			_model.NodesChanged += new EventHandler<TreeModelEventArgs>(_model_NodesChanged);
			_model.NodesInserted += new EventHandler<TreeModelEventArgs>(_model_NodesInserted);
			_model.NodesRemoved += new EventHandler<TreeModelEventArgs>(_model_NodesRemoved);
			_model.StructureChanged += new EventHandler<TreePathEventArgs>(_model_StructureChanged);
		}

		private void UnbindModelEvents()
		{
			_model.NodesChanged -= new EventHandler<TreeModelEventArgs>(_model_NodesChanged);
			_model.NodesInserted -= new EventHandler<TreeModelEventArgs>(_model_NodesInserted);
			_model.NodesRemoved -= new EventHandler<TreeModelEventArgs>(_model_NodesRemoved);
			_model.StructureChanged -= new EventHandler<TreePathEventArgs>(_model_StructureChanged);
		}

		private void _model_StructureChanged(object sender, TreePathEventArgs e)
		{
			if (e.Path == null)
				throw new ArgumentNullException();

			TreeNodeAdv node = FindNode(e.Path);
			if (node != null)
			{
				Collection<ExpandedNode> expandedNodes = null;
				if (KeepNodesExpanded && node.IsExpanded)
				{
					expandedNodes = FindExpandedNodes(node);
				}
				_structureUpdating = true;
				try
				{
					ReadChilds(node, expandedNodes);
					UpdateSelection();
				}
				finally
				{
					_structureUpdating = false;
				}
				SmartFullUpdate();
			}
		}

		private Collection<ExpandedNode> FindExpandedNodes(TreeNodeAdv parent)
		{
			Collection<ExpandedNode> expandedNodes = null;
			expandedNodes = new Collection<ExpandedNode>();
			foreach (TreeNodeAdv child in parent.Nodes)
			{
				if (child.IsExpanded)
				{
					ExpandedNode str = new ExpandedNode();
					str.Tag = child.Tag;
					str.Children = FindExpandedNodes(child);
					expandedNodes.Add(str);
				}
			}
			return expandedNodes;
		}

		private void _model_NodesRemoved(object sender, TreeModelEventArgs e)
		{
			TreeNodeAdv parent = FindNode(e.Path);
			if (parent != null)
			{
				if (e.Indices != null)
				{
					List<int> list = new List<int>(e.Indices);
					list.Sort();
					for (int n = list.Count - 1; n >= 0; n--)
					{
						int index = list[n];
						if (index >= 0 && index <= parent.Nodes.Count)
						{
							parent.Nodes[index].Parent = null;
							parent.Nodes.RemoveAt(index);
						}
						else
							throw new ArgumentOutOfRangeException("Index out of range");
					}
				}
				else
				{
					for (int i = parent.Nodes.Count - 1; i >= 0; i--)
					{
						for (int n = 0; n < e.Children.Length; n++)
							if (parent.Nodes[i].Tag == e.Children[n])
							{
								parent.Nodes[i].Parent = null;
								parent.Nodes.RemoveAt(i);
								break;
							}
					}
				}
			}
			UpdateSelection();
			SmartFullUpdate();
		}

		private void _model_NodesInserted(object sender, TreeModelEventArgs e)
		{
			if (e.Indices == null)
				throw new ArgumentNullException("Indices");

			TreeNodeAdv parent = FindNode(e.Path);
			if (parent != null)
			{
				for (int i = 0; i < e.Children.Length; i++)
					AddNode(parent, e.Children[i], e.Indices[i]);
			}
			SmartFullUpdate();
		}

		private void _model_NodesChanged(object sender, TreeModelEventArgs e)
		{
			TreeNodeAdv parent = FindNode(e.Path);
			if (parent != null)
			{
				if (e.Indices != null)
				{
					foreach (int index in e.Indices)
					{
						if (index >= 0 && index < parent.Nodes.Count)
						{
							TreeNodeAdv node = parent.Nodes[index];
							Rectangle rect = GetNodeBounds(node);
							_contentWidth = Math.Max(_contentWidth, rect.Right);
						}
						else
							throw new ArgumentOutOfRangeException("Index out of range");
					}
				}
				else
				{
					foreach (TreeNodeAdv node in parent.Nodes)
					{
						foreach (object obj in e.Children)
							if (node.Tag == obj)
							{
								Rectangle rect = GetNodeBounds(node);
								_contentWidth = Math.Max(_contentWidth, rect.Right);
							}
					}
				}
			}
			SafeUpdateScrollBars();
			UpdateView();
		}

		public TreeNodeAdv FindNode(TreePath path)
		{
			if (path.IsEmpty())
				return _root;
			else
				return FindNode(_root, path, 0);
		}

		private TreeNodeAdv FindNode(TreeNodeAdv root, TreePath path, int level)
		{
			foreach (TreeNodeAdv node in root.Nodes)
				if (node.Tag == path.FullPath[level])
				{
					if (level == path.FullPath.Length - 1)
						return node;
					else
						return FindNode(node, path, level + 1);
				}
			return null;
		}
		#endregion
	}
}
