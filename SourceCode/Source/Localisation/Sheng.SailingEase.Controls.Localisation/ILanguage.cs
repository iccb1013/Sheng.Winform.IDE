/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls.Localisation {
    public interface ILanguage {
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Resources.ResourceManager ResourceManager {
            get;
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Globalization.CultureInfo Culture {
            get;
        }
        string MessageBoxCaptiton_Message {
            get;
        }
        string SEPaginationDataGridView_EnumNavigationLocation_Bottom {
            get;
        }
        string SEPaginationDataGridView_EnumNavigationLocation_Top {
            get;
        }
        string SEToolStripImageSize_ExtraLarge {
            get;
        }
        string SEToolStripImageSize_Large {
            get;
        }
        string SEToolStripImageSize_Medium {
            get;
        }
        string SEToolStripImageSize_Small {
            get;
        }
        bool IsDefault {
            get;
        }
    }
}
