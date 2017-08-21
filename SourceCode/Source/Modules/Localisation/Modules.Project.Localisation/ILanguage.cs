/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Modules.ProjectModule.Localisation {
    public interface ILanguage {
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Resources.ResourceManager ResourceManager {
            get;
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Globalization.CultureInfo Culture {
            get;
        }
        string CreateProjectView {
            get;
        }
        string CreateProjectView_ButtonBrowse {
            get;
        }
        string CreateProjectView_ButtonCancel {
            get;
        }
        string CreateProjectView_ButtonOK {
            get;
        }
        string CreateProjectView_FolderBrowserDialog_Description {
            get;
        }
        string CreateProjectView_LabelFileName {
            get;
        }
        string CreateProjectView_LabelFolder {
            get;
        }
        string CreateProjectView_LabelProjectType {
            get;
        }
        string CreateProjectView_LabelTemplate {
            get;
        }
        string CreateProjectView_MessageFolderNotExist {
            get;
        }
        string CreateProjectView_ProjectType_Empty {
            get;
        }
        string CreateProjectView_ProjectType_Empty_Description {
            get;
        }
        string CreateProjectView_ProjectTypeTreeNode_All {
            get;
        }
        string Navigation_Menu_BuildProject {
            get;
        }
        string Navigation_Menu_CloseProject {
            get;
        }
        string Navigation_Menu_NewProject {
            get;
        }
        string Navigation_Menu_OpenProject {
            get;
        }
        string Navigation_Menu_ProjectProperty {
            get;
        }
        string Navigation_Menu_ProjectStartPage {
            get;
        }
        string Navigation_ToolStrip_NewProject {
            get;
        }
        string Navigation_ToolStrip_OpenProject {
            get;
        }
        string ProjectPropertyView {
            get;
        }
        string ProjectPropertyView_ButtonCancel {
            get;
        }
        string ProjectPropertyView_ButtonOK {
            get;
        }
        string ProjectPropertyView_CheckBoxUserModel {
            get;
        }
        string ProjectPropertyView_CheckBoxUserPopedomModel {
            get;
        }
        string ProjectPropertyView_CheckBoxUserSubsequent {
            get;
        }
        string ProjectPropertyView_LabelAboutImg {
            get;
        }
        string ProjectPropertyView_LabelBackImg {
            get;
        }
        string ProjectPropertyView_LabelCode {
            get;
        }
        string ProjectPropertyView_LabelCompany {
            get;
        }
        string ProjectPropertyView_LabelCopyright {
            get;
        }
        string ProjectPropertyView_LabelSummary {
            get;
        }
        string ProjectPropertyView_LabelUseDataBase {
            get;
        }
        string ProjectPropertyView_LabelVersion {
            get;
        }
        string ProjectPropertyView_MessageResourcesExistConfirmOverWrite {
            get;
        }
        string ProjectPropertyView_ProjectName {
            get;
        }
        string ProjectPropertyView_SplashImg {
            get;
        }
        string ProjectPropertyView_TabPageAbout {
            get;
        }
        string ProjectPropertyView_TabPageGeneral {
            get;
        }
        string ProjectPropertyView_TabPageRemark {
            get;
        }
        string ProjectStartPage_Title {
            get;
        }
        bool IsDefault {
            get;
        }
    }
}
