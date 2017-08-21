/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    partial class DataSourceEditView : FormViewBase
    {
        private DataSourceProvideFactory _provideFactory = DataSourceProvideFactory.Instance;
        public WindowEntity WindowEntity
        {
            get;
            set;
        }
        private DataSource2 _dataSource;
        public DataSource2 DataSource
        {
            get { return _dataSource; }
        }
        public DataSourceEditView()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            this.dataSourceSelector.Title = Language.Current.DataSourceEditView_LabelDataSource;
            this.provideSelector.Title = Language.Current.DataSourceEditView_LabelProvide;
        }
        private void DataSourceEditView_Load(object sender, EventArgs e)
        {
            provideSelector.DataBind(_provideFactory.GetProvideList().ToList());
        }
        private void provideSelector_SelectedValueChanged(object sender, Controls.SEComboSelector2.OnSelectedValueChangedEventArgs e)
        {
            IDataSourceProvide dataSourceProvide = e.Value as IDataSourceProvide;
            if (dataSourceProvide == null)
            {
                dataSourceSelector.Clear();
            }
            else
            {
                DataSourceProvideArgs args = new DataSourceProvideArgs()
                {
                    WindowEntity = this.WindowEntity
                };
                dataSourceSelector.DataBind(dataSourceProvide.GetAvailableDataSource(args));
            }
        }
        private void dataSourceSelector_ItemTextGetting(object sender, Controls.SEComboSelector2.ItemTextGettingEventArgs e)
        {
            DataSource2 dataSource = (DataSource2)e.Item;
            if (dataSource.SourceItem is UIElement)
            {
                UIElement element = (UIElement)dataSource.SourceItem;
                string text = String.Format("{0} ({1})", element.Name, FormElementEntityDevTypes.Instance.GetName(element));
                e.Text = text;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
                return;
            _dataSource = dataSourceSelector.GetSelectedValue() as DataSource2;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
