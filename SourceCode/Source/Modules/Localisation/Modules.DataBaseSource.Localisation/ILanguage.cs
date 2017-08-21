/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Modules.DataBaseSourceModule.Localisation {
    public interface ILanguage {
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Resources.ResourceManager ResourceManager {
            get;
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Globalization.CultureInfo Culture {
            get;
        }
        string DataSourceCreateView {
            get;
        }
        string DataSourceCreateView_ButtonCancel {
            get;
        }
        string DataSourceCreateView_ButtonOK {
            get;
        }
        string DataSourceCreateView_ButtonRefreshDataSource {
            get;
        }
        string DataSourceCreateView_GroupBoxLoginOption {
            get;
        }
        string DataSourceCreateView_LabelDataSourceName {
            get;
        }
        string DataSourceCreateView_LabelDataSourceType {
            get;
        }
        string DataSourceCreateView_LabelNotice {
            get;
        }
        string DataSourceCreateView_LabelPassword {
            get;
        }
        string DataSourceCreateView_LabelUserId {
            get;
        }
        string DataSourceCreateView_RadioButtonIntegratedSecurity {
            get;
        }
        string DataSourceCreateView_RadioButtonNoIntegratedSecurity {
            get;
        }
        string DataSourceSetView {
            get;
        }
        string DataSourceSetView_ButtonCancel {
            get;
        }
        string DataSourceSetView_ButtonOK {
            get;
        }
        string DataSourceSetView_LabelConnectionString {
            get;
        }
        string DataSourceSetView_LabelTitle {
            get;
        }
        string DataSourceSetView_LinkLabelCreateDataSource {
            get;
        }
        string Navigation_Menu_DataSourceSet {
            get;
        }
        bool IsDefault {
            get;
        }
    }
}
