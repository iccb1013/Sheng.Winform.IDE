/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Shell.Localisation {
    public interface ILanguage {
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Resources.ResourceManager ResourceManager {
            get;
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Globalization.CultureInfo Culture {
            get;
        }
        string GlobalTabPageContextMenuStrip_Close {
            get;
        }
        string GlobalTabPageContextMenuStrip_CloseAll {
            get;
        }
        string GlobalTabPageContextMenuStrip_CloseAllButThis {
            get;
        }
        string Navigation_Menu_Build {
            get;
        }
        string Navigation_Menu_Edit {
            get;
        }
        string Navigation_Menu_File {
            get;
        }
        string Navigation_Menu_File_Exit {
            get;
        }
        string Navigation_Menu_Help {
            get;
        }
        string Navigation_Menu_Tool {
            get;
        }
        string Navigation_Menu_View {
            get;
        }
        bool IsDefault {
            get;
        }
    }
}
