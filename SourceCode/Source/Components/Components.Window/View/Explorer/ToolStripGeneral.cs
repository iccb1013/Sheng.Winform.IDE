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
using Sheng.SailingEase.Components.WindowComponent.Localisation;
namespace Sheng.SailingEase.Components.WindowComponent.View
{
    class ToolStripGeneral : ToolStripCodon
    {
       
        private ToolStripButtonCodon _addCodon;
        private ToolStripButtonCodon _addFolderCodon;
        private ToolStripButtonCodon _editCodon;
        private ToolStripButtonCodon _deleteCodon;
        public Action<object, ToolStripItemCodonEventArgs> AddAction { get; set; }
        public Func<IToolStripItemCodon, bool> AddActionIsEnabled
        {
            get { return _addCodon.IsEnabled; }
            set { _addCodon.IsEnabled = value; }
        }
        public Action<object, ToolStripItemCodonEventArgs> AddFolderAction { get; set; }
        public Func<IToolStripItemCodon, bool> AddFolderActionIsEnabled
        {
            get { return _addFolderCodon.IsEnabled; }
            set { _addFolderCodon.IsEnabled = value; }
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
        public ToolStripGeneral()
            : base("ToolStripGeneral")
        {
            _addFolderCodon = new ToolStripButtonCodon("AddFolder", Language.Current.Explorer_ToolStripGeneral_AddFolder, IconsLibrary.FolderNew,
               delegate(object sender, ToolStripItemCodonEventArgs args)
               {
                   if (this.AddFolderAction != null) AddFolderAction(sender, args);
               });
            this.Items.Add(_addFolderCodon);
            this.Items.Add(new ToolStripSeparatorCodon());
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
        }
    }
}
