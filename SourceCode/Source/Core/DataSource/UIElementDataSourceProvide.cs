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
using System.Diagnostics;
using Sheng.SailingEase.Core.Localisation;
namespace Sheng.SailingEase.Core
{
    public class UIElementDataSourceProvide : IDataSourceProvide
    {
        public const string PREFIX = "UIElement";
        ILanguage _language = Language.Current;
        public string Name
        {
            get { return _language.UIElementDataSourceProvide; }
        }
        public UIElementDataSourceProvide()
        {
        }
        public DataSourceCollection GetAvailableDataSource(DataSourceProvideArgs args)
        {
            DataSourceCollection collection = new DataSourceCollection();
            if (args == null || args.WindowEntity == null)
            {
                Debug.Assert(false, "args.WindowEntity 为空");
                return collection;
            }
            WindowEntity window = args.WindowEntity;
            foreach (var item in window.GetFormElement())
            {
                if (item.DataSourceUseable)
                {
                    UIElementDataSoure dataSource = new UIElementDataSoure(item);
                    collection.Add(dataSource);
                }
            }
            return collection;
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
            UIElement element = sourceItem as UIElement;
            if (element == null)
            {
                return String.Empty;
            }
            return UIElementDataSoure.Parse(element);
        }
        public string GetDisplayString(string dataSourceString, DataSourceProvideArgs args)
        {
            if (args == null || args.WindowEntity == null)
            {
                Debug.Assert(false, "args.WindowEntity 为空");
                return String.Empty;
            }
            WindowEntity window = args.WindowEntity;
            UIElement element = window.FindFormElementById(dataSourceString);
            if (element != null)
                return _language.UIElementDataSoureDisplayString + " " + element.FullName;
            else
                return String.Empty;
        }
        public bool Validate(string dataSourceString, DataSourceProvideArgs args)
        {
            if (args == null || args.WindowEntity == null)
            {
                Debug.Assert(false, "args.WindowEntity 为空");
                return false;
            }
            WindowEntity window = args.WindowEntity;
            return window.FindFormElementById(dataSourceString) != null;
        }
    }
    public class UIElementDataSoure : DataSource2
    {
        public override string Name
        {
            get { return _sourceItem.FullName; }
        }
        private UIElement _sourceItem;
        public override object SourceItem
        {
            get { return _sourceItem; }
        }
        public override string String
        {
            get { return Parse(_sourceItem); }
        }
        public UIElementDataSoure(UIElement source)
        {
            _sourceItem = source;
        }
        public static string Parse(UIElement source)
        {
            return String.Format("{0}:{1}", UIElementDataSourceProvide.PREFIX, source.Id);
        }
    }
}
