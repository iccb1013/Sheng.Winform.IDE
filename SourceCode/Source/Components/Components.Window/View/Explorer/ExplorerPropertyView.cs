/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.Components.WindowComponent.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.WindowComponent.View
{
    partial class ExplorerPropertyView : PadViewBase
    {
        private WindowArchive _windowArchive = WindowArchive.Instance;
        public ExplorerPropertyView()
        {
            InitializeComponent();
            this.TabText = Language.Current.Explorer_PropertyView_TabText;
            PropertyGridValidator validator = new PropertyGridValidator(typeof(WindowEntity))
            {
                ActOnSub = true,
            };
            validator.Validator = (e) =>
            {
                bool success = true;
                string message = null;
                if (e.Property == EntityBase.Property_Code)
                {
                    if (Keywords.Container(e.Value.ToString()))
                    {
                        success = false;
                        message = CommonLanguage.Current.ValueInefficacyUseKeywords;
                    }
                    else
                    {
                        Debug.Assert(e.Objects.Length == 1, "验证 Code 时对象数目只能是一个");
                        object obj = e.Objects[0];
                        string value = e.Value.ToString();
                        if (_windowArchive.CheckExistByCode(value))
                        {
                            success = false;
                            message = Language.Current.Explorer_PropertyView_MessageCodeExist;
                        }
                    }
                }
                return new PropertyGridValidateResult(success, message);
            };
            propertyGrid.AddValidator(validator);
            propertyGrid.PropertyChanged += new PropertyGridPad.OnPropertyChangeHandler(propertyGrid_PropertyChanged);
        }
        void propertyGrid_PropertyChanged(object sender, PropertyChangeEventArgs e)
        {
            object obj = e.SelectedObjects[0];
            IPersistence persistence = obj as IPersistence;
            Debug.Assert(persistence != null, "没有实现 IPersistence");
            if (persistence != null)
                persistence.Save();
        }
        public void SetSelectedObject(object obj)
        {
            propertyGrid.SelectedObjects = new object[] { obj };
        }
    }
}
