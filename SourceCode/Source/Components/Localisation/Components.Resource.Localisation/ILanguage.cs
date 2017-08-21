/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.ResourceComponent.Localisation {
    public interface ILanguage {
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Resources.ResourceManager ResourceManager {
            get;
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Globalization.CultureInfo Culture {
            get;
        }
        string Explorer_ImageListViewContextMenuStrip_Add {
            get;
        }
        string Explorer_ImageListViewContextMenuStrip_Delete {
            get;
        }
        string ExplorerView_TabText {
            get;
        }
        string Navigation_Menu_Resource {
            get;
        }
        string Navigation_ToolStrip_Resource {
            get;
        }
        string RemoveImageResourceCommand_ConfirmDelete {
            get;
        }
        string ResourceComponentService_OverrideConfirm {
            get;
        }
        bool IsDefault {
            get;
        }
    }
}
