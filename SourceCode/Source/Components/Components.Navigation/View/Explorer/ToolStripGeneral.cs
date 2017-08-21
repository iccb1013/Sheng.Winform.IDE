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
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    class ToolStripGeneral : ToolStripCodon
    {
      
        private ToolStripButtonCodon _addCodon;
        private ToolStripButtonCodon _editCodon;
        private ToolStripButtonCodon _deleteCodon;
        private ToolStripButtonCodon _moveBeforeCodon;
        private ToolStripButtonCodon _moveAfterCodon;
        public Action<object, ToolStripItemCodonEventArgs> AddAction { get; set; }
        public Func<IToolStripItemCodon, bool> AddActionIsEnabled
        {
            get { return _addCodon.IsEnabled; }
            set { _addCodon.IsEnabled = value; }
        }
        public Action<object, ToolStripItemCodonEventArgs> EditAction { get; set; }
        public Func<IToolStripItemCodon, bool> EditActionIsEnabled
        {
            get { return _editCodon.IsEnabled; }
            set { _editCodon.IsEnabled = value; }
        }
        public Action<object, ToolStripItemCodonEventArgs> DeleteAction { get; set; }
        public Func<IToolStripItemCodon, bool> DeleteActionIsEnabled
        {
            get { return _deleteCodon.IsEnabled; }
            set { _deleteCodon.IsEnabled = value; }
        }
        public Action<object, ToolStripItemCodonEventArgs> MoveBeforeAction { get; set; }
        public Func<IToolStripItemCodon, bool> MoveBeforeActionIsEnabled
        {
            get { return _moveBeforeCodon.IsEnabled; }
            set { _moveBeforeCodon.IsEnabled = value; }
        }
        public Action<object, ToolStripItemCodonEventArgs> MoveAfterAction { get; set; }
        public Func<IToolStripItemCodon, bool> MoveAfterActionIsEnabled
        {
            get { return _moveAfterCodon.IsEnabled; }
            set { _moveAfterCodon.IsEnabled = value; }
        }
        public ToolStripGeneral()
            : base("ToolStripGeneral")
        {
            _addCodon = new ToolStripButtonCodon("Add", Language.Current.Explorer_ToolStripGeneral_Add, IconsLibrary.New2,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddAction != null) AddAction(sender, args);
                });
            this.Items.Add(_addCodon);
            _editCodon = new ToolStripButtonCodon("Edit", Language.Current.Explorer_ToolStripGeneral_Edit, IconsLibrary.Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditAction != null) EditAction(sender, args);
                });
            this.Items.Add(_editCodon);
            _deleteCodon = new ToolStripButtonCodon("Delete", Language.Current.Explorer_ToolStripGeneral_Delete, IconsLibrary.Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteAction != null) DeleteAction(sender, args);
                });
            this.Items.Add(_deleteCodon);
            this.Items.Add(new ToolStripSeparatorCodon());
            _moveBeforeCodon = new ToolStripButtonCodon("MoveBefore", Language.Current.Explorer_ToolStripGeneral_MoveBefore, IconsLibrary.Up,
              delegate(object sender, ToolStripItemCodonEventArgs args)
              {
                  if (this.MoveBeforeAction != null) MoveBeforeAction(sender, args);
              });
            this.Items.Add(_moveBeforeCodon);
            _moveAfterCodon = new ToolStripButtonCodon("MoveAfter", Language.Current.Explorer_ToolStripGeneral_MoveAfter, IconsLibrary.Down,
              delegate(object sender, ToolStripItemCodonEventArgs args)
              {
                  if (this.MoveAfterAction != null) MoveAfterAction(sender, args);
              });
            this.Items.Add(_moveAfterCodon);
        }
    }
}
