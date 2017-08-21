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
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    class ToolStripGeneral : ToolStripCodon
    {
       
        private ToolStripButtonCodon _addCodon;
        private ToolStripButtonCodon _editCodon;
        private ToolStripButtonCodon _deleteCodon;
        private ToolStripButtonCodon _createSqlCodon;
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
        public Action<object, ToolStripItemCodonEventArgs> CreateSqlAction { get; set; }
        public Func<IToolStripItemCodon, bool> CreateSqlActionIsEnabled
        {
            get { return _createSqlCodon.IsEnabled; }
            set { _createSqlCodon.IsEnabled = value; }
        }
        public ToolStripGeneral()
            : base("ToolStripGeneral")
        {
            _addCodon = new ToolStripButtonCodon("Add", Language.Current.ToolStripGeneral_Add, IconsLibrary.New2,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.AddAction != null) AddAction(sender, args);
                });
            this.Items.Add(_addCodon);
            _editCodon = new ToolStripButtonCodon("Edit", Language.Current.ToolStripGeneral_Edit, IconsLibrary.Edit,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.EditAction != null) EditAction(sender, args);
                });
            this.Items.Add(_editCodon);
            _deleteCodon = new ToolStripButtonCodon("Delete", Language.Current.ToolStripGeneral_Delete, IconsLibrary.Delete,
                delegate(object sender, ToolStripItemCodonEventArgs args)
                {
                    if (this.DeleteAction != null) DeleteAction(sender, args);
                });
            this.Items.Add(_deleteCodon);
            this.Items.Add(new ToolStripSeparatorCodon());
            _createSqlCodon = new ToolStripButtonCodon("CreateSql", Language.Current.ToolStripGeneral_CreateSql, IconsLibrary.Script,
              delegate(object sender, ToolStripItemCodonEventArgs args)
              {
                  if (this.CreateSqlAction != null) CreateSqlAction(sender, args);
              });
            this.Items.Add(_createSqlCodon);
        }
    }
}
