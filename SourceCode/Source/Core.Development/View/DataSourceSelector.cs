/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Sheng.SailingEase.Core.Development.View
{
    public partial class DataSourceSelector : UserControl
    {
        private DataSource2 _dataSource = null;
        private WindowEntity _windowEntity;
        public WindowEntity WindowEntity
        {
            get { return _windowEntity; }
            set { _windowEntity = value; }
        }
        public string DataSource
        {
            get
            {
                if (_dataSource == null)
                    return String.Empty;
                else
                    return _dataSource.ToString();
            }
        }
        private string _dataSourceName;
        public string DataSourceName
        {
            get
            {
                return _dataSourceName;
            }
        }
        public DataSourceSelector()
        {
            InitializeComponent();
        }
        private void OnDataSourceChanged()
        {
            if (DataSourceChanged != null)
            {
                DataSourceChangedEventArgs e = new DataSourceChangedEventArgs(this._dataSource.String);
                DataSourceChanged(this, e);
            }
        }
        private void linkLabelDataSource_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (DataSourceEditView view = new DataSourceEditView())
            {
                view.WindowEntity = _windowEntity;
                if (view.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_dataSource == null || _dataSource.String != view.DataSource.String)
                    {
                        _dataSource = view.DataSource;
                        DataSourceProvideArgs args = new DataSourceProvideArgs()
                        {
                            WindowEntity = _windowEntity
                        };
                        _dataSourceName = DataSourceProvideFactory.Instance.GetDisplayString(_dataSource.ToString(), args);
                        linkLabelDataSource.Text = _dataSourceName;
                        OnDataSourceChanged();
                    }
                }
            }
        }
        public delegate void OnDataSourceChangedHandler(object sender, DataSourceChangedEventArgs e);
        public event OnDataSourceChangedHandler DataSourceChanged;
        public class DataSourceChangedEventArgs : EventArgs
        {
            private string _dataSource;
            public string DataSource
            {
                get { return _dataSource; }
            }
            public DataSourceChangedEventArgs(string dataSource)
            {
                _dataSource = dataSource;
            }
        }
    }
}
