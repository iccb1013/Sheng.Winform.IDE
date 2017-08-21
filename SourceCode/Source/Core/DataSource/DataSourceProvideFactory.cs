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
using Sheng.SailingEase.Kernal;
using System.Diagnostics;
namespace Sheng.SailingEase.Core
{
   
    public class DataSourceProvideFactory
    {
        Dictionary<string, IDataSourceProvide> _provides = new Dictionary<string, IDataSourceProvide>();
        private static InstanceLazy<DataSourceProvideFactory> _instance =
          new InstanceLazy<DataSourceProvideFactory>(() => new DataSourceProvideFactory());
        public static DataSourceProvideFactory Instance
        {
            get { return _instance.Value; }
        }
        private DataSourceProvideFactory()
        {
            _provides.Add(SystemDataSourceProvide.PREFIX, new SystemDataSourceProvide());
            _provides.Add(UIElementDataSourceProvide.PREFIX, new UIElementDataSourceProvide());
        }
        public IList<IDataSourceProvide> GetProvideList()
        {
            return _provides.Values.ToList();
        }
        public string GetEditorString(string dataSourceString, DataSourceProvideArgs args)
        {
            if (DataSourceFormatValidate(dataSourceString) == false)
            {
                Debug.Assert(false, "dataSourceString 格式不对");
                return String.Empty;
            }
            string[] strs = dataSourceString.Split(':');
            if (strs.Length != 2)
            {
                Debug.Assert(false, "dataSourceString 格式不对");
                return String.Empty;
            }
            string prefix = strs[0];
            string sourceString = strs[1];
            IDataSourceProvide dataSoureProvide = GetDataSoureProvide(prefix);
            if (dataSoureProvide == null)
                return String.Empty;
            return dataSoureProvide.GetEditorString(sourceString, args);
        }
        public string RestoreEditorString(string editorString, DataSourceProvideArgs args)
        {
            return string.Empty;
        }
        public string GetDisplayString(string dataSourceString, DataSourceProvideArgs args)
        {
            if (DataSourceFormatValidate(dataSourceString) == false)
            {
                Debug.Assert(false, "dataSourceString 格式不对");
                return String.Empty;
            }
            string[] strs = dataSourceString.Split(':');
            if (strs.Length != 2)
            {
                Debug.Assert(false, "dataSourceString 格式不对");
                return String.Empty;
            }
            string prefix = strs[0];
            string sourceString = strs[1];
            IDataSourceProvide dataSoureProvide = GetDataSoureProvide(prefix);
            if (dataSoureProvide == null)
                return String.Empty;
            return dataSoureProvide.GetDisplayString(sourceString, args);
        }
        public string GetDataSourceString(object sourceItem, DataSourceProvideArgs args)
        {
            Debug.Assert(sourceItem != null, "sourceItem 为 null");
            if (sourceItem == null)
                return String.Empty;
            string dataSourceString = String.Empty;
            foreach (IDataSourceProvide provide in _provides.Values)
            {
                dataSourceString = provide.GetDataSourceString(sourceItem, args);
                if (String.IsNullOrEmpty(dataSourceString) == false)
                    continue;
            }
            return dataSourceString;
        }
        public bool Validate(string dataSourceString, DataSourceProvideArgs args)
        {
            if (DataSourceFormatValidate(dataSourceString) == false)
            {
                Debug.Assert(false, "dataSourceString 格式不对");
                return false;
            }
            string[] strs = dataSourceString.Split(':');
            if (strs.Length != 2)
            {
                Debug.Assert(false, "dataSourceString 格式不对");
                return false;
            }
            string prefix = strs[0];
            string sourceString = strs[1];
            IDataSourceProvide dataSoureProvide = GetDataSoureProvide(prefix);
            if (dataSoureProvide == null)
                return false;
            return dataSoureProvide.Validate(sourceString, args);
        }
        public bool DataSourceFormatValidate(string dataSourceString)
        {
            if (String.IsNullOrEmpty(dataSourceString))
            {
                Debug.Assert(false, "dataSourceString 为空");
                return false;
            }
            if (dataSourceString.Contains(':') == false)
            {
                Debug.Assert(false, "dataSourceString 格式不对");
                return false;
            }
            return true;
        }
        private IDataSourceProvide GetDataSoureProvide(string prefix)
        {
            if (String.IsNullOrEmpty(prefix))
            {
                Debug.Assert(false, "prefix 为空");
                return null;
            }
            if (_provides.ContainsKey(prefix) == false)
            {
                Debug.Assert(false, "没有找到指定的 IDataSoureProvide");
                return null;
            }
            return _provides[prefix];
        }
    }
}
