/*
 * TreeListView - A listview that can show a tree of objects in a column
 *
 * Author: Phillip Piper
 * Date: 23/09/2008 11:15 AM
 *
 * Change log:
 * 2009-09-01  JPP  - Fixed off-by-one error that was messing up hit detection
 * 2009-08-27  JPP  - Fixed bug when dragging a node from one place to another in the tree
 * v2.2.1
 * 2009-07-14  JPP  - Clicks to the left of the expander in tree cells are now ignored.
 * v2.2
 * 2009-05-12  JPP  - Added tree traverse operations: GetParent and GetChildren.
 *                  - Added DiscardAllState() to completely reset the TreeListView.
 * 2009-05-10  JPP  - Removed all unsafe code
 * 2009-05-09  JPP  - Fixed bug where any command (Expand/Collapse/Refresh) on a model
 *                    object that was once visible but that is currently in a collapsed branch
 *                    would cause the control to crash.
 * 2009-05-07  JPP  - Fixed bug where RefreshObjects() would fail when none of the given
 *                    objects were present/visible.
 * 2009-04-20  JPP  - Fixed bug where calling Expand() on an already expanded branch confused
 *                    the display of the children (SF#2499313)
 * 2009-03-06  JPP  - Calculate edit rectangle on column 0 more accurately
 * v2.1
 * 2009-02-24  JPP  - All commands now work when the list is empty (SF #2631054)
 *                  - TreeListViews can now be printed with ListViewPrinter
 * 2009-01-27  JPP  - Changed to use new Renderer and HitTest scheme
 * 2009-01-22  JPP  - Added RevealAfterExpand property. If this is true (the default),
 *                    after expanding a branch, the control scrolls to reveal as much of the
 *                    expanded branch as possible.
 * 2009-01-13  JPP  - Changed TreeRenderer to work with visual styles are disabled
 * v2.0.1
 * 2009-01-07  JPP  - Made all public and protected methods virtual 
 *                  - Changed some classes from 'internal' to 'protected' so that they
 *                    can be accessed by subclasses of TreeListView.
 * 2008-12-22  JPP  - Added UseWaitCursorWhenExpanding property
 *                  - Made TreeRenderer public so that it can be subclassed
 *                  - Added LinePen property to TreeRenderer to allow the connection drawing 
 *                    pen to be changed 
 *                  - Fixed some rendering issues where the text highlight rect was miscalculated
 *                  - Fixed connection line problem when there is only a single root
 * v2.0
 * 2008-12-10  JPP  - Expand/collapse with mouse now works when there is no SmallImageList.
 * 2008-12-01  JPP  - Search-by-typing now works.
 * 2008-11-26  JPP  - Corrected calculation of expand/collapse icon (SF#2338819)
 *                  - Fixed ugliness with dotted lines in renderer (SF#2332889)
 *                  - Fixed problem with custom selection colors (SF#2338805)
 * 2008-11-19  JPP  - Expand/collapse now preserve the selection -- more or less :)
 *                  - Overrode RefreshObjects() to rebuild the given objects and their children
 * 2008-11-05  JPP  - Added ExpandAll() and CollapseAll() commands
 *                  - CanExpand is no longer cached
 *                  - Renamed InitialBranches to RootModels since it deals with model objects
 * 2008-09-23  JPP  Initial version
 *
 * TO DO:
 * 2008-12-10  If the TreeListView doesn't have a small image list, checkboxes do not work.
 *             [Is this still the case? 2009/01/27]
 * 2008-10-19  Can we remove the need to ownerdraw the tree view?
 *             If tree does not have checkboxes, we could use the state image
 *             to show the expand/collapse icon. If the tree has check boxes,
 *             it has to be owner drawn.
 * 
 * Copyright (C) 2006-2008 Phillip Piper
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * If you wish to use this code in a closed source application, please contact phillip_piper@bigfoot.com.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Sheng.SailingEase.Controls.ObjectListView
{
    /// <summary>
    /// A TreeListView combines an expandable tree structure with list view columns.
    /// </summary>
    /// <remarks>
    /// <para>To support tree operations, two delegates must be provided:</para>
    /// <list>
    /// <item>CanExpandGetter. This delegate must accept a model object and return a boolean indicating
    /// if that model should be expandable. </item>
    /// <item>ChildrenGetter. This delegate must accept a model object and return an IEnumerable of model
    /// objects that will be displayed as children of the parent model. This delegate will only be called
    /// for a model object if the CanExpandGetter has already returned true for that model.</item>
    /// </list>
    /// <para>
    /// The top level branches of the tree are set via the Roots property. SetObjects(), AddObjects() 
    /// and RemoveObjects() are interpreted as operations on this collection of roots.
    /// </para>
    /// <para>
    /// To add new children to an existing branch, make changes to your model objects and then
    /// call RefreshObject() on the parent.
    /// </para>
    /// <para>The tree must be a directed acyclic graph -- no cycles are allowed.</para>
    /// <para>More generally, each model object must appear only once in the tree. If the same model object appears in two
    /// places in the tree, the control will become confused.</para>
    /// </remarks>
    public class TreeListView : VirtualObjectListView
    {
        /// <summary>
        /// Make a default TreeListView
        /// </summary>
        public TreeListView() {
            this.TreeModel = new Tree(this);
            this.OwnerDraw = true;
            this.View = View.Details;

            this.DataSource = this.TreeModel;
            this.TreeColumnRenderer = new TreeRenderer();

            // This improves hit detection even if we don't have any state image
            this.StateImageList = new ImageList();
        }

        //------------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// This is the delegate that will be used to decide if a model object can be expanded.
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual CanExpandGetterDelegate CanExpandGetter {
            get { return this.TreeModel.CanExpandGetter; }
            set { this.TreeModel.CanExpandGetter = value; }
        }

        /// <summary>
        /// Gets whether or not this listview is capabale of showing groups
        /// </summary>
        [Browsable(false)]
        public override bool CanShowGroups {
            get {
                return false;
            }
        }

        /// <summary>
        /// This is the delegate that will be used to fetch the children of a model object
        /// </summary>
        /// <remarks>This delegate will only be called if the CanExpand delegate has 
        /// returned true for the model object.</remarks>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ChildrenGetterDelegate ChildrenGetter {
            get { return this.TreeModel.ChildrenGetter; }
            set { this.TreeModel.ChildrenGetter = value; }
        }

        /// <summary>
        /// After expanding a branch, should the TreeListView attempts to show as much of the 
        /// revealed descendents as possible.
        /// </summary>
        [Category("Behavior - ObjectListView"),
         Description("Should a wait cursor be shown when a branch is being expaned?"),
         DefaultValue(true)]
        public bool RevealAfterExpand {
            get { return revealAfterExpand; }
            set { revealAfterExpand = value; }
        }
        private bool revealAfterExpand = true;

        /// <summary>
        /// The model objects that form the top level branches of the tree.
        /// </summary>
        /// <remarks>Setting this does <b>NOT</b> reset the state of the control.
        /// In particular, it does not collapse branches.</remarks>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnumerable Roots {
            get { return this.TreeModel.RootObjects; }
            set {
                // Make sure that column 0 is showing a tree
                if (this.GetColumn(0).Renderer == null)
                    this.GetColumn(0).Renderer = this.TreeColumnRenderer;
                if (value == null)
                    this.TreeModel.RootObjects = new ArrayList();
                else
                    this.TreeModel.RootObjects = value;
                this.UpdateVirtualListSize();
            }
        }

        /// <summary>
        /// The renderer that will be used to draw the tree structure
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual BaseRenderer TreeColumnRenderer {
            get { return treeRenderer; }
            set {
                treeRenderer = value;
                if (this.Columns.Count > 0)
                    this.GetColumn(0).Renderer = value;
            }
        }
        private BaseRenderer treeRenderer;

        /// <summary>
        /// Should a wait cursor be shown when a branch is being expanded?
        /// </summary>
        /// <remarks>When this is true, the wait cursor will be shown whilst the children of the 
        /// branch are being fetched. If the children of the branch have already been cached, 
        /// the cursor will not change.</remarks>
        [Category("Behavior - ObjectListView"),
        Description("Should a wait cursor be shown when a branch is being expaned?"),
        DefaultValue(true)]
        public virtual bool UseWaitCursorWhenExpanding {
            get { return useWaitCursorWhenExpanding; }
            set { useWaitCursorWhenExpanding = value; }
        }
        private bool useWaitCursorWhenExpanding = true;

        /// <summary>
        /// The model that is used to manage the tree structure
        /// </summary>
        protected Tree TreeModel;

        //------------------------------------------------------------------------------------------
        // Accessing

        /// <summary>
        /// Return true if the branch at the given model is expanded
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool IsExpanded(Object model) {
            Branch br = this.TreeModel.GetBranch(model);
            return (br != null && br.IsExpanded);
        }

        //------------------------------------------------------------------------------------------
        // Commands

        /// <summary>
        /// Collapse the subtree underneath the given model
        /// </summary>
        /// <param name="model"></param>
        public virtual void Collapse(Object model) {
            if (this.GetItemCount() == 0)
                return;
            IList selection = this.SelectedObjects;
            int idx = this.TreeModel.Collapse(model);
            if (idx >= 0) {
                this.UpdateVirtualListSize();
                this.SelectedObjects = selection;
                this.RedrawItems(idx, this.GetItemCount() - 1, false);
            }
        }

        /// <summary>
        /// Collapse all subtrees within this control
        /// </summary>
        public virtual void CollapseAll() {
            if (this.GetItemCount() == 0)
                return;
            IList selection = this.SelectedObjects;
            int idx = this.TreeModel.CollapseAll();
            if (idx >= 0) {
                this.UpdateVirtualListSize();
                this.SelectedObjects = selection;
                this.RedrawItems(idx, this.GetItemCount() - 1, false);
            }
        }

        /// <summary>
        /// Collapse all roots and forget everything we know about all models
        /// </summary>
        public virtual void DiscardAllState() {
            // Remember the bits of info we don't want to forget (anyone ever see Memento?)
            IEnumerable roots = this.Roots;
            CanExpandGetterDelegate canExpand = this.CanExpandGetter;
            ChildrenGetterDelegate childrenGetter = this.ChildrenGetter;

            // Give ourselves a new data structure
            this.TreeModel = new Tree(this);
            this.DataSource = this.TreeModel;

            // Put back the bits we didn't want to forget
            this.CanExpandGetter = canExpand;
            this.ChildrenGetter = childrenGetter;
            this.Roots = roots;
        }

        /// <summary>
        /// Expand the subtree underneath the given model object
        /// </summary>
        /// <param name="model"></param>
        public virtual void Expand(Object model) {
            if (this.GetItemCount() == 0)
                return;

            // Remember the selection so we can put it back later
            IList selection = this.SelectedObjects;

            // Expand the model first
            int idx = this.TreeModel.Expand(model);
            if (idx < 0)
                return;

            // Update the size of the list and restore the selection
            this.UpdateVirtualListSize();
            this.SelectedObjects = selection;

            // Redraw the items that were changed by the expand operation
            this.RedrawItems(idx, this.GetItemCount() - 1, false);

            if (this.RevealAfterExpand && idx > 0) {
                // TODO: This should be a separate method
                this.BeginUpdate();
                try {
                    int countPerPage = NativeMethods.GetCountPerPage(this);
                    int descedentCount = this.TreeModel.GetVisibleDescendentCount(model);
                    if (descedentCount < countPerPage)
                        this.EnsureVisible(idx + descedentCount);
                    else
                        this.TopItemIndex = idx;
                }
                finally {
                    this.EndUpdate();
                }
            }

        }

        /// <summary>
        /// Expand all the branches within this tree recursively.
        /// </summary>
        /// <remarks>Be careful: this method could take a long time for large trees.</remarks>
        public virtual void ExpandAll() {
            if (this.GetItemCount() == 0)
                return;
            IList selection = this.SelectedObjects;
            int idx = this.TreeModel.ExpandAll();
            if (idx >= 0) {
                this.UpdateVirtualListSize();
                this.SelectedObjects = selection;
                this.RedrawItems(idx, this.GetItemCount() - 1, false);
            }
        }

        /// <summary>
        /// Update the rows that are showing the given objects
        /// </summary>
        public override void RefreshObjects(IList modelObjects) {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { this.RefreshObjects(modelObjects); });
                return;
            }
            // There is no point in refreshing anything if the list is empty
            if (this.GetItemCount() == 0)
                return;

            // Remember the selection so we can put it back later
            IList selection = this.SelectedObjects;

            // Refresh each object, remembering where the first update occured
            int firstChange = Int32.MaxValue;
            foreach (Object model in modelObjects) {
                if (model != null) {
                    int idx = this.TreeModel.RebuildChildren(model);
                    if (idx >= 0)
                        firstChange = Math.Min(firstChange, idx);
                }
            }

            // If we didn't refresh any objects, don't do anything else
            if (firstChange >= this.GetItemCount())
                return;

            this.UpdateVirtualListSize();
            this.SelectedObjects = selection;

            // Redraw everything from the first update to the end of the list
            this.RedrawItems(firstChange, this.GetItemCount() - 1, false);
        }

        /// <summary>
        /// Toggle the expanded state of the branch at the given model object
        /// </summary>
        /// <param name="model"></param>
        public virtual void ToggleExpansion(Object model) {
            if (this.IsExpanded(model))
                this.Collapse(model);
            else
                this.Expand(model);
        }

        //------------------------------------------------------------------------------------------
        // Commands - Tree traversal

        /// <summary>
        /// Return the model object that is the parent of the given model object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>The given model must have already been seen in the tree.</remarks>
        public virtual Object GetParent(Object model) {
            Branch br = this.TreeModel.GetBranch(model);
            if (br == null || br.ParentBranch == null)
                return null;
            else
                return br.ParentBranch.Model;
        }

        /// <summary>
        /// Return the collection of model objects that are the children of the 
        /// given model.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>The given model must have already been seen in the tree and
        /// must be expandable</remarks>
        public virtual IEnumerable GetChildren(Object model) {
            Branch br = this.TreeModel.GetBranch(model);
            if (br == null || !br.CanExpand)
                return new ArrayList();
            else
                return br.Children;
        }

        //------------------------------------------------------------------------------------------
        // Delegates

        /// <summary>
        /// Delegates of this type are use to decide if the given model object can be expanded
        /// </summary>
        /// <param name="model">The model under consideration</param>
        /// <returns>Can the given model be expanded?</returns>
        public delegate bool CanExpandGetterDelegate(Object model);

        /// <summary>
        /// Delegates of this type are used to fetch the children of the given model object
        /// </summary>
        /// <param name="model">The parent whose children should be fetched</param>
        /// <returns>An enumerable over the children</returns>
        public delegate IEnumerable ChildrenGetterDelegate(Object model);

        //------------------------------------------------------------------------------------------
        // Implementation

        /// <summary>
        /// Intercept the basic message pump to customise the mouse down and hit testing.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                //case 0x1012: // LVM_HITTEST = (LVM_FIRST + 18)
                //    this.HandleHitTest(ref m);
                //    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// Handle a hit test to account for the indent of the branch
        /// </summary>
        /// <param name="m"></param>
        //protected virtual void HandleHitTest(ref Message m) {
        //    //THINK: Do we need to do this, since we are using the build-in Level ability of
        //    // of ListCtrl, which should take the indent into account

        //    // We want to change our base behavior by taking the indentation of tree into account
        //    // when performing a hit test. So we figure out which row is at the test point,
        //    // then calculate the indentation for that row, and modify the hit test *inplace*
        //    // so that the normal hittest is done, but indented by the correct amount.

        //    this.DefWndProc(ref m);
        //    //NativeMethods.LVHITTESTINFO* hittest = (NativeMethods.LVHITTESTINFO*)m.LParam;
        //    NativeMethods.LVHITTESTINFO hittest = (NativeMethods.LVHITTESTINFO)m.GetLParam(typeof(NativeMethods.LVHITTESTINFO));
        //    // Find which row was hit...
        //    int row = hittest.iItem;
        //    if (row < 0)
        //        return;

        //    // ...from that decide the model object...
        //    Object model = this.TreeModel.GetNthObject(row);
        //    if (model == null)
        //        return;

        //    // ...and from that, the branch of the tree showing that model...
        //    Branch br = this.TreeModel.GetBranch(model);
        //    if (br == null)
        //        return;

        //    // ...use the indentation on that branch to modify the hittest
        //    hittest.pt_x += (br.Level * TreeRenderer.PIXELS_PER_LEVEL);
        //    System.Runtime.InteropServices.Marshal.StructureToPtr(hittest, m.LParam, false);
        //    this.DefWndProc(ref m);
        //}

        /// <summary>
        /// Handle a left button down event
        /// </summary>
        /// <param name="hti"></param>
        /// <returns></returns>
        protected override bool ProcessLButtonDown(OlvListViewHitTestInfo hti) {
            // Did they click in the expander?
            if (hti.HitTestLocation == HitTestLocation.ExpandButton) {
                this.PossibleFinishCellEditing();
                this.ToggleExpansion(hti.RowObject);
                return true;
            }

            return base.ProcessLButtonDown(hti);
        }

        /// <summary>
        /// Create a OLVListItem for given row index
        /// </summary>
        /// <param name="itemIndex">The index of the row that is needed</param>
        /// <returns>An OLVListItem</returns>
        /// <remarks>This differs from the base method by also setting up the IndentCount property.</remarks>
        public override OLVListItem MakeListViewItem(int itemIndex) {
            OLVListItem olvItem = base.MakeListViewItem(itemIndex);
            Branch br = this.TreeModel.GetBranch(olvItem.RowObject);
            if (br != null)
                olvItem.IndentCount = br.Level - 1;
            return olvItem;
        }

        #region Event handlers

        /// <summary>
        /// Decide if the given key event should be handled as a normal key input to the control?
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData) {
            // We want to handle Left and Right keys within the control
            if (((keyData & Keys.KeyCode) == Keys.Left) || ((keyData & Keys.KeyCode) == Keys.Right)) {
                return true;
            } else
                return base.IsInputKey(keyData);
        }

        /// <summary>
        /// Handle the keyboard input to mimic a TreeView.
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns>Was the key press handled?</returns>
        protected override void OnKeyDown(KeyEventArgs e) {
            OLVListItem focused = this.FocusedItem as OLVListItem;
            if (focused == null) {
                base.OnKeyDown(e);
                return;
            }

            Object modelObject = focused.RowObject;
            Branch br = this.TreeModel.GetBranch(modelObject);

            switch (e.KeyCode) {
                case Keys.Left:
                    // If the branch is expanded, collapse it. If it's collapsed,
                    // select the parent of the branch.
                    if (br.IsExpanded)
                        this.Collapse(modelObject);
                    else {
                        if (br.ParentBranch != null && br.ParentBranch.Model != null)
                            this.SelectObject(br.ParentBranch.Model, true);
                    }
                    e.Handled = true;
                    break;

                case Keys.Right:
                    // If the branch is expanded, select the first child.
                    // If it isn't expanded and can be, expand it.
                    if (br.IsExpanded) {
                        if (br.ChildBranches.Count > 0)
                            this.SelectObject(br.ChildBranches[0].Model, true);
                    } else {
                        if (br.CanExpand)
                            this.Expand(modelObject);
                    }
                    e.Handled = true;
                    break;
            }

            base.OnKeyDown(e);
        }

        #endregion

        //------------------------------------------------------------------------------------------
        // Support classes

        /// <summary>
        /// A Tree object represents a tree structure data model that supports both 
        /// tree and flat list operations as well as fast access to branches.
        /// </summary>
        protected class Tree : IVirtualListDataSource
        {
            public Tree(TreeListView treeView) {
                this.treeView = treeView;
                this.trunk = new Branch(null, this, null);
                this.trunk.IsExpanded = true;
            }

            //------------------------------------------------------------------------------------------
            // Properties

            /// <summary>
            /// This is the delegate that will be used to decide if a model object can be expanded.
            /// </summary>
            public CanExpandGetterDelegate CanExpandGetter {
                get { return canExpandGetter; }
                set { canExpandGetter = value; }
            }
            private CanExpandGetterDelegate canExpandGetter;

            /// <summary>
            /// This is the delegate that will be used to fetch the children of a model object
            /// </summary>
            /// <remarks>This delegate will only be called if the CanExpand delegate has 
            /// returned true for the model object.</remarks>
            public ChildrenGetterDelegate ChildrenGetter {
                get { return childrenGetter; }
                set { childrenGetter = value; }
            }
            private ChildrenGetterDelegate childrenGetter;

            /// <summary>
            /// Get or return the top level model objects in the tree
            /// </summary>
            public IEnumerable RootObjects {
                get { return this.trunk.Children; }
                set {
                    this.trunk.Children = value;
                    this.RebuildList();
                }
            }

            /// <summary>
            /// What tree view is this Tree the model for?
            /// </summary>
            public TreeListView TreeView {
                get { return this.treeView; }
            }

            //------------------------------------------------------------------------------------------
            // Commands

            /// <summary>
            /// Collapse the subtree underneath the given model
            /// </summary>
            /// <param name="model">The model to be collapsed. If the model isn't in the tree,
            /// or if it is already collapsed, the command does nothing.</param>
            /// <returns>The index of the model in flat list version of the tree</returns>
            public virtual int Collapse(Object model) {
                Branch br = this.GetBranch(model);
                if (br == null || !br.IsExpanded || !br.Visible)
                    return -1;

                int count = br.NumberVisibleDescendents;
                br.Collapse();

                // Remove the visible descendents from after the branch itself
                int idx = this.GetObjectIndex(model);
                this.objectList.RemoveRange(idx + 1, count);
                this.RebuildObjectMap(idx + 1);
                return idx;
            }

            /// <summary>
            /// Collapse all branches in this tree
            /// </summary>
            /// <returns>Return the index of the first root that was not collapsed</returns>
            public virtual int CollapseAll() {
                foreach (Branch br in this.trunk.ChildBranches) {
                    if (br.IsExpanded)
                        br.Collapse();
                }
                this.RebuildList();
                return 0;
            }

            /// <summary>
            /// Expand the subtree underneath the given model object
            /// </summary>
            /// <param name="model">The model to be expanded.</param> 
            /// <returns>The index of the model in flat list version of the tree</returns>
            /// <remarks>
            /// If the model isn't in the tree,
            /// if it cannot be expanded or if it is already expanded, the command does nothing.
            /// </remarks>
            public virtual int Expand(Object model) {
                Branch br = this.GetBranch(model);
                if (br == null || !br.CanExpand || br.IsExpanded|| !br.Visible)
                    return -1;

                int idx = this.GetObjectIndex(model);
                this.InsertChildren(br, idx + 1);
                return idx;
            }

            /// <summary>
            /// Expand all branches in this tree
            /// </summary>
            /// <returns>Return the index of the first branch that was expanded</returns>
            public virtual int ExpandAll() {
                this.trunk.ExpandAll();
                this.Sort(this.lastSortColumn, this.lastSortOrder);
                return 0;
            }

            /// <summary>
            /// Return the Branch object that represents the given model in the tree
            /// </summary>
            /// <param name="model">The model whose branches is to be returned</param>
            /// <returns>The branch that represents the given model, or null if the model
            /// isn't in the tree.</returns>
            public virtual Branch GetBranch(object model) {
                if (model == null)
                    return null;

                Branch br;

                if (this.mapObjectToBranch.TryGetValue(model, out br))
                    return br;
                else
                    return null;
            }

            /// <summary>
            /// Return the number of visible descendents that are below the given model.
            /// </summary>
            /// <param name="model">The model whose descendent count is to be returned</param>
            /// <returns>The number of visible descendents. 0 if the model doesn't exist or is collapsed</returns>
            public virtual int GetVisibleDescendentCount(object model) {
                Branch br = this.GetBranch(model);
                if (br == null || !br.IsExpanded)
                    return 0;
                else
                    return br.NumberVisibleDescendents;
            }

            /// <summary>
            /// Rebuild the children of the given model, refreshing any cached information held about the given object
            /// </summary>
            /// <param name="model"></param>
            /// <returns>The index of the model in flat list version of the tree</returns>
            public virtual int RebuildChildren(Object model) {
                Branch br = this.GetBranch(model);
                if (br == null || !br.Visible)
                    return -1;

                int count = br.NumberVisibleDescendents;
                br.ClearCachedInfo();

                // Remove the visible descendents from after the branch itself
                int idx = this.GetObjectIndex(model);
                if (count > 0)
                    this.objectList.RemoveRange(idx + 1, count);
                if (br.IsExpanded)
                    this.InsertChildren(br, idx + 1);
                return idx;
            }

            //------------------------------------------------------------------------------------------
            // Implementation

            /// <summary>
            /// Insert the children of the given branch into the given position
            /// </summary>
            /// <param name="br">The branch whose children should be inserted</param>
            /// <param name="idx">The index where the children should be inserted</param>
            protected virtual void InsertChildren(Branch br, int idx) {
                // Expand the branch
                br.Expand();
                br.Sort(this.GetBranchComparer());

                // Insert the branch's visible descendents after the branch itself
                this.objectList.InsertRange(idx, br.Flatten());
                this.RebuildObjectMap(idx);
            }

            /// <summary>
            /// Rebuild our flat internal list of objects.
            /// </summary>
            protected virtual void RebuildList() {
                this.objectList = ArrayList.Adapter(this.trunk.Flatten());
                if (this.trunk.ChildBranches.Count > 0) {
                    this.trunk.ChildBranches[0].IsFirstBranch = true;
                    this.trunk.ChildBranches[0].IsOnlyBranch = (this.trunk.ChildBranches.Count == 1);
                }
                this.RebuildObjectMap(0);
            }

            /// <summary>
            /// Rebuild our reverse index that maps an object to its location
            /// in the objectList array.
            /// </summary>
            /// <param name="startIndex"></param>
            protected virtual void RebuildObjectMap(int startIndex) {
                for (int i = startIndex; i < this.objectList.Count; i++)
                    this.mapObjectToIndex[this.objectList[i]] = i;
            }

            /// <summary>
            /// Remember that the given branch is part of this tree.
            /// </summary>
            /// <param name="br"></param>
            public virtual void RegisterBranch(Branch br) {
                this.mapObjectToBranch[br.Model] = br;
            }

            //------------------------------------------------------------------------------------------

            #region IVirtualListDataSource Members

            public virtual object GetNthObject(int n) {
                return this.objectList[n];
            }

            public virtual int GetObjectCount() {
                return this.trunk.NumberVisibleDescendents;
            }

            public virtual int GetObjectIndex(object model) {
                int idx;

                if (model != null && this.mapObjectToIndex.TryGetValue(model, out idx))
                    return idx;
                else
                    return -1;
            }

            public virtual void PrepareCache(int first, int last) {
            }

            public virtual int SearchText(string value, int first, int last, OLVColumn column) {
                return AbstractVirtualListDataSource.DefaultSearchText(value, first, last, column, this);
            }

            public virtual void Sort(OLVColumn column, SortOrder order) {
                this.lastSortColumn = column;
                this.lastSortOrder = order;

                // Sorting is going to change the order of the branches so clear
                // the "first branch" flag
                if (this.trunk.ChildBranches.Count > 0)
                    this.trunk.ChildBranches[0].IsFirstBranch = false;

                this.trunk.Sort(this.GetBranchComparer());
                this.RebuildList();
            }

            protected virtual BranchComparer GetBranchComparer() {
                if (this.lastSortColumn == null)
                    return null;
                else
                    return new BranchComparer(new ModelObjectComparer(this.lastSortColumn, this.lastSortOrder,
                        this.treeView.GetColumn(0), this.lastSortOrder));
            }

            public virtual void AddObjects(ICollection modelObjects) {
                ArrayList newRoots = new ArrayList();
                foreach (Object x in this.treeView.Roots)
                    newRoots.Add(x);
                foreach (Object x in modelObjects)
                    newRoots.Add(x);
                this.SetObjects(newRoots);
            }

            public virtual void RemoveObjects(ICollection modelObjects) {
                ArrayList newRoots = new ArrayList();
                foreach (Object x in this.treeView.Roots)
                    newRoots.Add(x);
                foreach (Object x in modelObjects)
                    newRoots.Remove(x);
                this.SetObjects(newRoots);
            }

            public virtual void SetObjects(IEnumerable collection) {
                // We interpret a SetObjects() call as setting the roots of the tree
                this.treeView.Roots = collection;
            }

            #endregion

            //------------------------------------------------------------------------------------------
            // Private instance variables

            private OLVColumn lastSortColumn;
            private SortOrder lastSortOrder;
            private Dictionary<Object, Branch> mapObjectToBranch = new Dictionary<object, Branch>();
            private Dictionary<Object, int> mapObjectToIndex = new Dictionary<object, int>();
            private ArrayList objectList = new ArrayList();
            private TreeListView treeView;
            private Branch trunk;
        }

        /// <summary>
        /// A Branch represents a sub-tree within a tree
        /// </summary>
        protected class Branch
        {
            [Flags]
            public enum BranchFlags
            {
                FirstBranch = 1,
                LastChild = 2,
                OnlyBranch = 4
            }

            public Branch(Branch parent, Tree tree, Object model) {
                this.ParentBranch = parent;
                this.Tree = tree;
                this.Model = model;

                //if (parent != null)
                //    this.Level = parent.Level + 1;
            }

            //------------------------------------------------------------------------------------------
            // Properties

            /// <summary>
            /// Get the ancestor branches of this branch, with the 'oldest' ancestor first.
            /// </summary>
            public virtual IList<Branch> Ancestors {
                get {
                    List<Branch> ancestors = new List<Branch>();
                    if (this.ParentBranch != null)
                        this.ParentBranch.PushAncestors(ancestors);
                    return ancestors;
                }
            }

            private void PushAncestors(IList<Branch> list) {
                // This is designed to ignore the trunk (which has no parent)
                if (this.ParentBranch != null) {
                    this.ParentBranch.PushAncestors(list);
                    list.Add(this);
                }
            }

            /// <summary>
            /// Can this branch be expanded?
            /// </summary>
            public virtual bool CanExpand {
                get {
                    if (this.Tree.CanExpandGetter == null || this.Model == null)
                        return false;
                    else
                        return this.Tree.CanExpandGetter(this.Model);
                }
            }

            /// <summary>
            /// Get/set the model objects that are beneath this branch
            /// </summary>
            public virtual IEnumerable Children {
                get {
                    ArrayList children = new ArrayList();
                    foreach (Branch x in this.ChildBranches)
                        children.Add(x.Model);
                    return children;
                }
                set {
                    if (this.ChildBranches.Count > 0)
                        this.ChildBranches[this.ChildBranches.Count - 1].IsLastChild = false;

                    this.ChildBranches.Clear();
                    foreach (Object x in value)
                        this.AddChild(x);
                    if (this.ChildBranches.Count > 0)
                        this.ChildBranches[this.ChildBranches.Count - 1].IsLastChild = true;
                }
            }

            private void AddChild(object model) {
                Branch br = this.Tree.GetBranch(model);
                if (br == null)
                    br = this.MakeBranch(model);
                else
                    br.ParentBranch = this;
                this.ChildBranches.Add(br);
            }

            private Branch MakeBranch(object model) {
                Branch br = new Branch(this, this.Tree, model);
                this.Tree.RegisterBranch(br);
                return br;
            }

            /// <summary>
            /// Return the number of descendents of this branch that are currently visible
            /// </summary>
            /// <returns></returns>
            public virtual int NumberVisibleDescendents {
                get {
                    if (!this.IsExpanded)
                        return 0;

                    int count = this.ChildBranches.Count;
                    foreach (Branch br in this.ChildBranches)
                        count += br.NumberVisibleDescendents;
                    return count;
                }
            }

            /// <summary>
            /// Return true if this branch is the first branch of the entire tree
            /// </summary>
            public virtual bool IsFirstBranch {
                get {
                    return ((this.flags & Branch.BranchFlags.FirstBranch) != 0);
                }
                set {
                    if (value)
                        this.flags |= Branch.BranchFlags.FirstBranch;
                    else
                        this.flags &= ~Branch.BranchFlags.FirstBranch;
                }
            }

            /// <summary>
            /// Return true if this branch is the last child of its parent
            /// </summary>
            public virtual bool IsLastChild {
                get {
                    return ((this.flags & Branch.BranchFlags.LastChild) != 0);
                }
                set {
                    if (value)
                        this.flags |= Branch.BranchFlags.LastChild;
                    else
                        this.flags &= ~Branch.BranchFlags.LastChild;
                }
            }

            /// <summary>
            /// Return true if this branch is the only top level branch
            /// </summary>
            public virtual bool IsOnlyBranch {
                get {
                    return ((this.flags & Branch.BranchFlags.OnlyBranch) != 0);
                }
                set {
                    if (value)
                        this.flags |= Branch.BranchFlags.OnlyBranch;
                    else
                        this.flags &= ~Branch.BranchFlags.OnlyBranch;
                }
            }

            /// <summary>
            /// Is this branch currently visible? A branch is visible
            /// if it has no parent (i.e. it's a root), or its parent
            /// is visible and expanded.
            /// </summary>
            public virtual bool Visible {
                get {
                    if (this.ParentBranch == null)
                        return true;
                    else
                        return this.ParentBranch.IsExpanded && this.ParentBranch.Visible;
                }
            }

            //------------------------------------------------------------------------------------------
            // Commands

            /// <summary>
            /// Clear any cached information that this branch is holding
            /// </summary>
            public virtual void ClearCachedInfo() {
                this.Children = new ArrayList();
                this.alreadyHasChildren = false;
            }

            /// <summary>
            /// Collapse this branch
            /// </summary>
            public virtual void Collapse() {
                this.IsExpanded = false;
            }

            /// <summary>
            /// Expand this branch
            /// </summary>
            public virtual void Expand() {
                if (this.CanExpand) {
                    this.IsExpanded = true;
                    this.FetchChildren();
                }
            }

            /// <summary>
            /// Expand this branch recursively
            /// </summary>
            public virtual void ExpandAll() {
                this.Expand();
                foreach (Branch br in this.ChildBranches)
                    br.ExpandAll();
            }

            /// <summary>
            /// Fetch the children of this branch.
            /// </summary>
            /// <remarks>This should only be called when CanExpand is true.</remarks>
            public virtual void FetchChildren() {
                if (this.alreadyHasChildren)
                    return;

                this.alreadyHasChildren = true;

                if (this.Tree.ChildrenGetter == null)
                    return;

                if (this.Tree.TreeView.UseWaitCursorWhenExpanding) {
                    Cursor previous = Cursor.Current;
                    try {
                        Cursor.Current = Cursors.WaitCursor;
                        this.Children = this.Tree.ChildrenGetter(this.Model);
                    }
                    finally {
                        Cursor.Current = previous;
                    }
                } else {
                    this.Children = this.Tree.ChildrenGetter(this.Model);
                }
            }

            /// <summary>
            /// Collapse the visible descendents of this branch into list of model objects
            /// </summary>
            /// <returns></returns>
            public virtual IList Flatten() {
                ArrayList flatList = new ArrayList();
                if (this.IsExpanded)
                    this.FlattenOnto(flatList);
                return flatList;
            }

            /// <summary>
            /// Flatten this branch's visible descendents onto the given list.
            /// </summary>
            /// <param name="flatList"></param>
            /// <remarks>The branch itself is <b>not</b> included in the list.</remarks>
            public virtual void FlattenOnto(IList flatList) {
                foreach (Branch br in this.ChildBranches) {
                    flatList.Add(br.Model);
                    if (br.IsExpanded)
                        br.FlattenOnto(flatList);
                }
            }

            /// <summary>
            /// Sort the sub-branches and their descendents so they are ordered according
            /// to the given comparer.
            /// </summary>
            /// <param name="comparer">The comparer that orders the branches</param>
            public virtual void Sort(BranchComparer comparer) {
                if (this.ChildBranches.Count == 0)
                    return;

                // We're about to sort the children, so clear the last child flag
                this.ChildBranches[this.ChildBranches.Count - 1].IsLastChild = false;
                if (comparer != null)
                    this.ChildBranches.Sort(comparer);
                this.ChildBranches[this.ChildBranches.Count - 1].IsLastChild = true;

                foreach (Branch br in this.ChildBranches)
                    br.Sort(comparer);
            }

            //------------------------------------------------------------------------------------------
            // Public instance variables

            public Object Model;
            public Tree Tree;
            public Branch ParentBranch;
            public List<Branch> ChildBranches = new List<Branch>();
            //public bool CanExpand = false;
            public bool IsExpanded = false;
            public int Level {
                get {
                    if (this.ParentBranch == null)
                        return 0;
                    else
                        return this.ParentBranch.Level + 1;
                }
            }

            //------------------------------------------------------------------------------------------
            // Private instance variables

            private bool alreadyHasChildren = false;
            private BranchFlags flags;
        }

        /// <summary>
        /// This class sorts branches according to how their respective model objects are sorted
        /// </summary>
        protected class BranchComparer : IComparer<Branch>
        {
            public BranchComparer(IComparer actualComparer) {
                this.actualComparer = actualComparer;
            }

            public int Compare(Branch x, Branch y) {
                return this.actualComparer.Compare(x.Model, y.Model);
            }

            private IComparer actualComparer;
        }

        /// <summary>
        /// This class handles drawing the tree structure of the primary column.
        /// </summary>
        public class TreeRenderer : BaseRenderer
        {
            public TreeRenderer() {
                this.LinePen = new Pen(Color.Blue, 1.0f);
                this.LinePen.DashStyle = DashStyle.Dot;
            }

            /// <summary>
            /// Return the branch that the renderer is currently drawing.
            /// </summary>
            private Branch Branch {
                get {
                    return this.TreeListView.TreeModel.GetBranch(this.RowObject);
                }
            }

            /// <summary>
            /// Return the pen that will be used to draw the lines between branches
            /// </summary>
            public Pen LinePen {
                get { return linePen; }
                set { linePen = value; }
            }
            private Pen linePen;

            /// <summary>
            /// Return the TreeListView for which the renderer is being used.
            /// </summary>
            public TreeListView TreeListView {
                get {
                    return (TreeListView)this.ListView;
                }
            }

            /// <summary>
            /// Should the renderer draw lines connecting siblings?
            /// </summary>
            public bool IsShowLines = true;

            /// <summary>
            /// How many pixels will be reserved for each level of indentation?
            /// </summary>
            public static int PIXELS_PER_LEVEL = 16 + 1;

            /// <summary>
            /// The real work of drawing the tree is done in this method
            /// </summary>
            /// <param name="g"></param>
            /// <param name="r"></param>
            public override void Render(System.Drawing.Graphics g, System.Drawing.Rectangle r) {
                this.DrawBackground(g, r);

                Branch br = this.Branch;

                if (this.IsShowLines)
                    this.DrawLines(g, r, this.LinePen, br);

                if (br.CanExpand) {
                    Rectangle r2 = r;
                    r2.Offset((br.Level - 1) * PIXELS_PER_LEVEL, 0);
                    r2.Width = PIXELS_PER_LEVEL;

                    if (!this.IsPrinting && Application.RenderWithVisualStyles) {
                        VisualStyleElement element = VisualStyleElement.TreeView.Glyph.Closed;
                        if (br.IsExpanded)
                            element = VisualStyleElement.TreeView.Glyph.Opened;
                        VisualStyleRenderer renderer = new VisualStyleRenderer(element);
                        renderer.DrawBackground(g, r2);
                    } else {
                        int h = 8;
                        int w = 8;
                        int x = r2.X + 4;
                        int y = r2.Y + (r2.Height / 2) - 4;

                        g.DrawRectangle(new Pen(SystemBrushes.ControlDark), x, y, w, h);
                        g.FillRectangle(Brushes.White, x + 1, y + 1, w - 1, h - 1);
                        g.DrawLine(Pens.Black, x + 2, y + 4, x + w - 2, y + 4);

                        if (!br.IsExpanded)
                            g.DrawLine(Pens.Black, x + 4, y + 2, x + 4, y + h - 2);
                    }
                }

                int indent = br.Level * PIXELS_PER_LEVEL;
                r.Offset(indent, 0);
                r.Width -= indent;

                this.DrawImageAndText(g, r);
            }

            private void DrawLines(Graphics g, Rectangle r, Pen p, Branch br) {
                Rectangle r2 = r;
                r2.Width = PIXELS_PER_LEVEL;

                // Vertical lines have to start on even points, otherwise the dotted line looks wrong.
                // This isn't need if pen isn't dotted.
                int top = r2.Top;
                if (p.DashStyle == DashStyle.Dot && (top & 1) == 1)
                    top += 1;

                // Draw lines for ancestors
                int midX;
                IList<Branch> ancestors = br.Ancestors;
                foreach (Branch ancestor in ancestors) {
                    if (!ancestor.IsLastChild) {
                        midX = r2.Left + r2.Width / 2;
                        g.DrawLine(p, midX, top, midX, r2.Bottom);
                    }
                    r2.Offset(PIXELS_PER_LEVEL, 0);
                }

                // Draw lines for this branch
                midX = r2.Left + r2.Width / 2;
                int midY = r2.Top + r2.Height / 2;
                // Horizontal line first
                g.DrawLine(p, midX, midY, r2.Right, midY);
                // Vertical line second
                if (br.IsFirstBranch) {
                    if (!br.IsOnlyBranch)
                        g.DrawLine(p, midX, midY, midX, r2.Bottom);
                } else {
                    if (br.IsLastChild)
                        g.DrawLine(p, midX, top, midX, midY);
                    else
                        g.DrawLine(p, midX, top, midX, r2.Bottom);
                }
            }

            protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y) {
                Branch br = this.Branch;

                Rectangle r = this.Bounds;
                if (br.CanExpand) {
                    r.Offset((br.Level - 1) * PIXELS_PER_LEVEL, 0);
                    r.Width = PIXELS_PER_LEVEL;
                    if (r.Contains(x, y)) {
                        hti.HitTestLocation = HitTestLocation.ExpandButton;
                        return;
                    }
                }

                r = this.Bounds;
                int indent = br.Level * PIXELS_PER_LEVEL;
                r.X += indent;
                r.Width -= indent;

                // Ignore events in the indent zone
                if (x < r.Left) {
                    hti.HitTestLocation = HitTestLocation.Nothing;
                } else {
                    this.StandardHitTest(g, hti, r, x, y);
                }
            }

            protected override Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds,
                OLVListItem item, int subItemIndex) {
                return this.StandardGetEditRectangle(g, cellBounds);
            }
        }
    }
}
