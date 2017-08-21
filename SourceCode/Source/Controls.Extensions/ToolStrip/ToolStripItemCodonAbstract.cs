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
using System.Drawing;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.Extensions
{
    public abstract class ToolStripItemCodonAbstract<TView> : IToolStripItemCodon where TView : IToolStripItemView
    {
        private IToolStripItemView _view;
        public IToolStripItemView View
        {
            get
            {
                if (_view == null)
                {
                    CreateView();
                }
                return _view;
            }
        }
        private string _text;
        public string Text
        {
            get
            {
                if (GetText == null)
                {
                    if (_text == null)
                        return String.Empty;
                    else
                        return _text;
                }
                else
                {
                    return GetText();
                }
            }
        }
        public string Description
        {
            get;
            set;
        }
        private Image _image;
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }
        public virtual ToolStripCodonBase Owner
        {
            get;
            set;
        }
        public string PathPoint
        {
            get;
            protected set;
        }
        private ToolStripItemAlignment _alignment = ToolStripItemAlignment.Left;
        public ToolStripItemAlignment Alignment
        {
            get { return _alignment; }
            set { _alignment = value; }
        }
        public int? Width
        {
            get;
            set;
        }
        private bool _autoSize = true;
        public bool AutoSize
        {
            get { return _autoSize; }
            set { _autoSize = value; }
        }
        private bool _autoToolTip = false;
        public bool AutoToolTip
        {
            get { return _autoToolTip; }
            set { _autoToolTip = value; }
        }
        private bool _available = true;
        public bool Available
        {
            get { return _available; }
            set { _available = value; }
        }
        private ToolStripItemDisplayStyle _displayStyle = ToolStripItemDisplayStyle.Image;
        public ToolStripItemDisplayStyle DisplayStyle
        {
            get { return _displayStyle; }
            set { _displayStyle = value; }
        }
        public OnToolStripItemCodonActionHandler Action
        {
            get;
            set;
        }
        public Func<IToolStripItemCodon, bool> IsVisible
        {
            get;
            set;
        }
        public Func<IToolStripItemCodon, bool> IsEnabled
        {
            get;
            set;
        }
        public Func<string> GetText
        {
            get;
            set;
        }
        public bool Visible
        {
            get
            {
                if (IsVisible == null)
                    return true;
                else
                    return IsVisible(this);
            }
        }
        public bool Enabled
        {
            get
            {
                if (IsEnabled == null)
                    return true;
                else
                    return IsEnabled(this);
            }
        }
        public ToolStripItemCodonAbstract()
        {
        }
        public ToolStripItemCodonAbstract(string pathPoint)
        {
            PathPoint = pathPoint;
        }
        public ToolStripItemCodonAbstract(string pathPoint, OnToolStripItemCodonActionHandler action)
            : this(pathPoint, null, null, ToolStripItemDisplayStyle.Image, action)
        {
        }
        public ToolStripItemCodonAbstract(string pathPoint, string text, ToolStripItemDisplayStyle displayStyle)
            : this(pathPoint, text, null, displayStyle, null)
        {
        }
        public ToolStripItemCodonAbstract(string pathPoint, string text)
            : this(pathPoint, text, null, ToolStripItemDisplayStyle.Image, null)
        {
        }
        public ToolStripItemCodonAbstract(string pathPoint, string text, Image image)
            : this(pathPoint, text, image, ToolStripItemDisplayStyle.Image, null)
        {
        }
        public ToolStripItemCodonAbstract(string pathPoint, string text, Image image, OnToolStripItemCodonActionHandler action)
            : this(pathPoint, text, image, ToolStripItemDisplayStyle.Image, action)
        {
        }
        public ToolStripItemCodonAbstract(string pathPoint, string text, Image image, ToolStripItemDisplayStyle displayStyle, OnToolStripItemCodonActionHandler action)
        {
            PathPoint = pathPoint;
            if (text != null)
                this._text = text;
            if (image != null)
                this.Image = image;
            this.DisplayStyle = displayStyle;
            if (action != null)
                Action = action;
        }
        private void CreateView()
        {
            _view = Activator.CreateInstance<TView>();
            _view.Codon = this;
        }
        public TView GetView()
        {
            return (TView)this.View;
        }
    }
}
