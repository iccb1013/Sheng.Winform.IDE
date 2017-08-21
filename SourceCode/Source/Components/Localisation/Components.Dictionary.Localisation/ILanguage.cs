/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DictionaryComponent.Localisation {
    public interface ILanguage {
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Resources.ResourceManager ResourceManager {
            get;
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Globalization.CultureInfo Culture {
            get;
        }
        string DictionaryView_TabText {
            get;
        }
        string EnumEditView {
            get;
        }
        string EnumEditView_ButtonCancel {
            get;
        }
        string EnumEditView_ButtonOK {
            get;
        }
        string EnumEditView_LabelCode {
            get;
        }
        string EnumEditView_LabelName {
            get;
        }
        string EnumEditView_MessageEnumCodeExist {
            get;
        }
        string EnumItemEditView {
            get;
        }
        string EnumItemEditView_ButtonCancel {
            get;
        }
        string EnumItemEditView_ButtonOK {
            get;
        }
        string EnumItemEditView_LabelText {
            get;
        }
        string EnumItemEditView_LabelValue {
            get;
        }
        string EnumItemEditView_MessageEnumItemValueExist {
            get;
        }
        string Explorer_GridColumns_EnumColumns_Name {
            get;
        }
        string Explorer_GridColumns_EnumItemColumns_Text {
            get;
        }
        string Explorer_GridColumns_EnumItemColumns_Value {
            get;
        }
        string Explorer_GridViewMenuEnumEntity_Add {
            get;
        }
        string Explorer_GridViewMenuEnumEntity_AddItem {
            get;
        }
        string Explorer_GridViewMenuEnumEntity_Delete {
            get;
        }
        string Explorer_GridViewMenuEnumEntity_Edit {
            get;
        }
        string Explorer_GridViewMenuEnumItemEntity_Add {
            get;
        }
        string Explorer_GridViewMenuEnumItemEntity_Delete {
            get;
        }
        string Explorer_GridViewMenuEnumItemEntity_Edit {
            get;
        }
        string Navigation_Menu_Dictionary {
            get;
        }
        string Navigation_ToolStrip_Dictionary {
            get;
        }
        bool IsDefault {
            get;
        }
    }
}
