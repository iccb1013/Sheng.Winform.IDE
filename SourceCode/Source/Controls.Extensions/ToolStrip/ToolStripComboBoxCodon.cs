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
namespace Sheng.SailingEase.Controls.Extensions
{
    public delegate void OnToolStripComboBoxCodonSelectedItemChangedHandler(object sender, ToolStripComboBoxCodonSelectedItemChangedEventArgs e);
    public class ToolStripComboBoxCodon<TView> : ToolStripItemCodonAbstract<TView> where TView : IToolStripItemView
    {
        public object DataSource
        {
            get;
            set;
        }
        public string DisplayMember
        {
            get;
            set;
        }
        public string ValueMember
        {
            get;
            set;
        }
        public Func<object> GetSelectedValue
        {
            get;
            set;
        }
        public OnToolStripComboBoxCodonSelectedItemChangedHandler SelectedItemChangedAction
        {
            get;
            set;
        }
        public ToolStripComboBoxCodon(string pathPoint)
            : base(pathPoint)
        {
        }
        public ToolStripComboBoxCodon(string pathPoint, object dataSource,
            OnToolStripComboBoxCodonSelectedItemChangedHandler selectedItemChangedAction)
            : this(pathPoint)
        {
            DataSource = dataSource;
            SelectedItemChangedAction = selectedItemChangedAction;
        }
    }
    public class ToolStripComboBoxCodon : ToolStripComboBoxCodon<ToolStripComboBoxView>
    {
        public ToolStripComboBoxCodon(string pathPoint)
            : base(pathPoint)
        {
        }
        public ToolStripComboBoxCodon(string pathPoint, object dataSource,
            OnToolStripComboBoxCodonSelectedItemChangedHandler selectedItemChangedAction)
            : base(pathPoint, dataSource, selectedItemChangedAction)
        {
        }
    }
}
