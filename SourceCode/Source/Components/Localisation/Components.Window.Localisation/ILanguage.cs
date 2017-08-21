/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.WindowComponent.Localisation {
    public interface ILanguage {
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Resources.ResourceManager ResourceManager {
            get;
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Globalization.CultureInfo Culture {
            get;
        }
        string Explorer_GridColumns_Code {
            get;
        }
        string Explorer_GridColumns_Name {
            get;
        }
        string Explorer_GridViewContextMenu_Add {
            get;
        }
        string Explorer_GridViewContextMenu_AddFolder {
            get;
        }
        string Explorer_GridViewContextMenu_Delete {
            get;
        }
        string Explorer_GridViewContextMenu_Edit {
            get;
        }
        string Explorer_PropertyView_MessageCodeExist {
            get;
        }
        string Explorer_PropertyView_TabText {
            get;
        }
        string Explorer_ToolStripGeneral_Add {
            get;
        }
        string Explorer_ToolStripGeneral_AddFolder {
            get;
        }
        string Explorer_ToolStripGeneral_Delete {
            get;
        }
        string Explorer_ToolStripGeneral_Edit {
            get;
        }
        string ExplorerView_RootFolder {
            get;
        }
        string ExplorerView_TabText {
            get;
        }
        string FolderEditView {
            get;
        }
        string FolderEditView_ButtonCancel {
            get;
        }
        string FolderEditView_ButtonOK {
            get;
        }
        string FolderEditView_LabelName {
            get;
        }
        string Navigation_Menu_Window {
            get;
        }
        string Navigation_ToolStrip_Window {
            get;
        }
        string NavigationTreeView {
            get;
        }
        string TreeMenuFolder_Add {
            get;
        }
        string TreeMenuFolder_AddWindow {
            get;
        }
        string TreeMenuFolder_Delete {
            get;
        }
        string TreeMenuFolder_Edit {
            get;
        }
        string TreeMenuWindowEntity_Add {
            get;
        }
        string TreeMenuWindowEntity_Delete {
            get;
        }
        string TreeMenuWindowEntity_Edit {
            get;
        }
        string WindowEditView {
            get;
        }
        string WindowEditView_ButtonCancel {
            get;
        }
        string WindowEditView_ButtonOK {
            get;
        }
        string WindowEditView_LabelCode {
            get;
        }
        string WindowEditView_LabelName {
            get;
        }
        string WindowEditView_MessageFormEntityCodeExist {
            get;
        }
        bool IsDefault {
            get;
        }
    }
}
