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
namespace Sheng.SailingEase.Core
{
    public interface IDataSourceProvide
    {
        string Name { get; }
        DataSourceCollection GetAvailableDataSource(DataSourceProvideArgs args);
        string GetEditorString(string dataSourceString, DataSourceProvideArgs args);
        string RestoreEditorString(string editorString, DataSourceProvideArgs args);
        string GetDataSourceString(object sourceItem, DataSourceProvideArgs args);
        string GetDisplayString(string dataSourceString, DataSourceProvideArgs args);
        bool Validate(string dataSourceString, DataSourceProvideArgs args);
    }
}
