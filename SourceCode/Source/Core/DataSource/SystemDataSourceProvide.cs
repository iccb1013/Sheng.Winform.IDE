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
using Sheng.SailingEase.Core.Localisation;
using System.Diagnostics;
namespace Sheng.SailingEase.Core
{
    public class SystemDataSourceProvide : IDataSourceProvide
    {
        public const string PREFIX = "System";
        ILanguage _language = Language.Current;
        DataSourceCollection _systemDataSource = new DataSourceCollection();
        public string Name
        {
            get { return _language.SystemDataSoureProvide; }
        }
        public SystemDataSourceProvide()
        {
            _systemDataSource.Add(new SystemDataSoure(_language.EnumSystemDataSource_UserId, "0"));
            _systemDataSource.Add(new SystemDataSoure(_language.EnumSystemDataSource_UserName, "1"));
            _systemDataSource.Add(new SystemDataSoure(_language.EnumSystemDataSource_UserLoginName, "2"));
            _systemDataSource.Add(new SystemDataSoure(_language.EnumSystemDataSource_Date, "5"));
            _systemDataSource.Add(new SystemDataSoure(_language.EnumSystemDataSource_DateTime, "6"));
        }
        public DataSourceCollection GetAvailableDataSource(DataSourceProvideArgs args)
        {
            return _systemDataSource;
        }
        public string GetEditorString(string dataSourceString, DataSourceProvideArgs args)
        {
            return String.Empty;
        }
        public string RestoreEditorString(string editorString, DataSourceProvideArgs args)
        {
            return String.Empty;
        }
        public string GetDataSourceString(object sourceItem, DataSourceProvideArgs args)
        {
            foreach (DataSource2 item in _systemDataSource)
            {
                if (item.SourceItem.Equals(sourceItem))
                    return item.String;
            }
            return String.Empty;
        }
        public string GetDisplayString(string dataSourceString, DataSourceProvideArgs args)
        {
            foreach (SystemDataSoure item in _systemDataSource)
            {
                if (item.SourceItem.Equals(dataSourceString))
                    return _language.SystemDataSoureDisplayString + " " + item.Name;
            }
            Debug.Assert(false, "GetDisplayString 没有找到对应的 DataSource");
            return String.Empty;
        }
        public bool Validate(string dataSourceString, DataSourceProvideArgs args)
        {
            bool result = false;
            foreach (SystemDataSoure item in _systemDataSource)
            {
                if (item.SourceItem.Equals(dataSourceString))
                {
                    result = true;
                }
            }
            return result;
        }
    }
    public class SystemDataSoure : DataSource2
    {
        private string _name;
        public override string Name
        {
            get { return _name; }
        }
        private string _sourceItem;
        public override object SourceItem
        {
            get { return _sourceItem; }
        }
        public override string String
        {
            get { return String.Format("{0}:{1}", SystemDataSourceProvide.PREFIX, _sourceItem); }
        }
        public SystemDataSoure(string name, string sourceItem)
        {
            _name = name;
            _sourceItem = sourceItem;
        }
    }
}
