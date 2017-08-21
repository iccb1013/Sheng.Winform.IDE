/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Localisation {
    public interface ILanguage {
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Resources.ResourceManager ResourceManager {
            get;
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Globalization.CultureInfo Culture {
            get;
        }
        string BooleanFalse {
            get;
        }
        string BooleanTrue {
            get;
        }
        string ConfirmDelete {
            get;
        }
        string FolderNotExist {
            get;
        }
        string MessageBoxButton_Cancel {
            get;
        }
        string MessageBoxButton_No {
            get;
        }
        string MessageBoxButton_Yes {
            get;
        }
        string MessageCaption_Error {
            get;
        }
        string MessageCaption_Infomation {
            get;
        }
        string MessageCaption_Notice {
            get;
        }
        string MessageCaption_Warning {
            get;
        }
        string RegexMsg_EntityCode {
            get;
        }
        string SystemItemNoDeleteAllowed {
            get;
        }
        string SystemItemNoEditAllowed {
            get;
        }
        string ValueInefficacyUseKeywords {
            get;
        }
        bool IsDefault {
            get;
        }
    }
}
