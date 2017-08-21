using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Sheng.SailingEase.Controls.PopupControl;
using Sheng.SailingEase.ComponentModel.Design.Localisation;

namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    public partial class StepList : UserControl
    {
        #region 私有成员

        private int _selectedToIndex = 0;
        private int _tempSelectedToIndex = 0;

        private int _displayRangeMin = 0;
        private int _displayRangeMax = 0;

        private int ScrollValue
        {
            get { return this.vScrollBar.Value; }
            set { this.vScrollBar.Value = value; }
        }

        private int ScrollMaximum
        {
            get { return this.vScrollBar.Maximum; }
            set
            {
                if (value < 0)
                    value = 0;

                this.vScrollBar.Maximum = value;
            }
        }

        private int ScrollMinimum
        {
            get { return this.vScrollBar.Minimum; }
            set { this.vScrollBar.Minimum = value; }
        }

        //private SEUndoUnit [] _stack;

        #endregion

        #region 公开属性

        public Popup Popup { get; set; }

        private int _itemHeight = 14;
        public int ItemHeight
        {
            get { return this._itemHeight; }
            set { this._itemHeight = value; }
        }

        /// <summary>
        /// 当前选中了多少个条目
        /// </summary>
        public int SelectedItemCount
        {
            get
            {
                int count = 0;

                foreach (StepListItem item in this.Items)
                {
                    if (item.Selected)
                        count++;
                    else
                        break;
                }

                return count;
            }
        }

        private StepListItemCollection _items = new StepListItemCollection();
        public StepListItemCollection Items
        {
            get { return this._items; }
            set { this._items = value; }
        }

        private SEUndoEngine _undoEngine;
        public SEUndoEngine UndoEngine
        {
            get { return this._undoEngine; }
            set { this._undoEngine = value; }
        }

        private SEUndoEngine.Type _actionType = SEUndoEngine.Type.Undo;
        public SEUndoEngine.Type ActionType
        {
            get { return this._actionType; }
            set
            {
                this._actionType = value;

                if (value == SEUndoEngine.Type.Undo)
                {
                    this.SetItems(this.UndoEngine.GetUndoUnits());
                }
                else
                {
                    this.SetItems(this.UndoEngine.GetRedoUnits());
                }
            }
        }

        #endregion

        #region 构造

        public StepList()
        {
            InitializeComponent();

            this.BorderStyle = BorderStyle.FixedSingle;

            this.Items.StepList = this;

            Reset();

            //this.Font = new Font(DefaultFont.FontFamily, ItemHeight -2 );

            this.MouseWheel += new MouseEventHandler(StepList_MouseWheel);
            this.panel.Paint += new PaintEventHandler(panel_Paint);
            this.panel.MouseClick+=new MouseEventHandler(panel_MouseClick);
            this.panel.MouseMove += new MouseEventHandler(panel_MouseMove);
            this.vScrollBar.ValueChanged+=new EventHandler(vScrollBar_ValueChanged);
        }
       
        #endregion

        #region 事件

        private void StepList_MouseWheel(object sender, MouseEventArgs e)
        {
            int value = this.ScrollValue;

            if (e.Delta > 0)
                value += -this.vScrollBar.LargeChange;
            else
                value += this.vScrollBar.LargeChange;

            if (value >= this.ScrollMinimum && value <= this.ScrollMaximum)
            {
                this.ScrollValue = value;
            }
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            DrawItems();
        }

        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.Popup != null)
                this.Popup.Close();

            if (this.OnClick != null)
            {
                OnClick(this, new OnClickEventArgs(this.SelectedItemCount, this.ActionType));
            }
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            this._tempSelectedToIndex = GetMouseHotIndex(e.Y);

            if (this._selectedToIndex != this._tempSelectedToIndex)
            {
                this._selectedToIndex = GetMouseHotIndex(e.Y);
                SetSelectedItems(this._selectedToIndex);

                //显示提示
                if (this.ActionType == SEUndoEngine.Type.Undo)
                {
                    this.lblNotice.Text = String.Format(Language.Current.UndoEngine_StepList_UndoNotice, this.SelectedItemCount);
                }
                else
                {
                    this.lblNotice.Text = String.Format(Language.Current.UndoEngine_StepList_RedoNotice, this.SelectedItemCount);
                }

                //调用事件
                if (OnSelectedItemChanage != null)
                    OnSelectedItemChanage(this, new EventArgs()); 
            }
        }

        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            DrawItems();
        }

        #endregion

        #region 内部方法

        private void SetSelectedItems(int selectedToIndex)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (i <= selectedToIndex )
                {
                    this.Items[i].Selected = true;
                }
                else
                {
                    this.Items[i].Selected = false;
                }
            }
        }

        private int GetMouseHotIndex(int mouseY)
        {
            return mouseY / this.ItemHeight + this.ScrollValue;
        }

        private void DrawBackGround()
        {
            Graphics g = this.panel.CreateGraphics();
            g.Clear(SystemColors.Window);
        }

        private void DrawItems()
        {
            DrawBackGround();

            if (this.Items == null || this.Items.Count == 0)
                return;

            this.ScrollMaximum = _items.Count - this.panel.Height / this.ItemHeight;

            if (this.ScrollMaximum == 0)
                this.vScrollBar.Visible = false;
            else
                this.vScrollBar.Visible = true;

            _displayRangeMin = this.ScrollValue;
            _displayRangeMax = this.panel.Height / this.ItemHeight +this.ScrollValue - 1;

            if(_displayRangeMax>=this.Items.Count)
                _displayRangeMax = this.Items.Count - 1;           

            for (int i = _displayRangeMin; i <= _displayRangeMax; i++)
            {
                DrawItem(i);
            }
        }

        private void Reset()
        {
            this._selectedToIndex = 0;
           
            this.ScrollMaximum = 0;
            this.ScrollMinimum = 0;
            this.ScrollValue = 0;
            this.vScrollBar.LargeChange = 1;

            if (this.Items == null)
            {
                this.ScrollMaximum = 0;
            }
            else
            {
                this.ScrollMaximum = _items.Count - this.panel.Height / this.ItemHeight;
            }

            this.lblNotice.Text = String.Empty;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 判断指定下标的项是否在当前需要显示的范围
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool InDisplayRange(int index)
        {
            if (index < _displayRangeMin || index > _displayRangeMax)
                return false;
            else
                return true;
        }

        public void SetItems(SEUndoUnitAbstract [] units)
        {
            this.Items.SetRange(units);
            this.Reset();
            DrawItems();
        }

        internal void DrawItem(int index)
        {
            if (InDisplayRange(index) == false)
                return;

            int displayIndex = index - this.ScrollValue;

            Graphics g = panel.CreateGraphics();

            if (this.Items[index].Selected)
            {
                g.FillRectangle(SystemBrushes.Highlight, new Rectangle(0, displayIndex * this.ItemHeight, this.panel.Width, this.ItemHeight));
                g.DrawString(this.Items[index].ToString(), this.Font, SystemBrushes.HighlightText, new PointF(0F, displayIndex * this.ItemHeight));
            }
            else
            {
                g.FillRectangle(SystemBrushes.Window, new Rectangle(0, displayIndex * this.ItemHeight, this.panel.Width, this.ItemHeight));
                g.DrawString(this.Items[index].ToString(), this.Font, SystemBrushes.ControlText, new PointF(0F, displayIndex * this.ItemHeight));
            }
        }

        #endregion

        #region 公开事件

        public delegate void OnSelectedItemChangeEventHandler(object sender ,EventArgs e);
        /// <summary>
        /// 选择的项发生变更
        /// </summary>
        public event OnSelectedItemChangeEventHandler OnSelectedItemChanage;

        public delegate void OnClickEventHandler(object sender, OnClickEventArgs e);

        /// <summary>
        /// 单击
        /// </summary>
        public new event  OnClickEventHandler OnClick;

        #endregion

        public class OnClickEventArgs : EventArgs
        {
            /// <summary>
            /// 选择的项数目
            /// </summary>
            public int SelectedItemCount { get; set; }

            public SEUndoEngine.Type ActionType { get; set; }

            public OnClickEventArgs(int selectedItemCount, SEUndoEngine.Type actionType)
            {
                this.SelectedItemCount = selectedItemCount;
                this.ActionType = actionType;
            }
        }
    }
}
