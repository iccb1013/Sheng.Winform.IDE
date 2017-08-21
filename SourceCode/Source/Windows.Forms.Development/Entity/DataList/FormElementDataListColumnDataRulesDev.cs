/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    class FormElementDataListColumnDataRulesDev
    {
        [Serializable]
        public class NormalDev : UIElementDataListColumnDataRules.Normal, IFormElementDataListColumnDataRule
        {
            public string Name
            {
                get { return FormElementDataListColumnDataRuleDevTypes.Instance.GetName(this); }
            }
            [NonSerialized]
            private Control _parameterSetPanel;
            public Control ParameterSetPanel
            {
                get
                {
                    if (_parameterSetPanel == null)
                    {
                        _parameterSetPanel = new UserControlDataColumnDevDataRule_Normal();
                    }
                    return _parameterSetPanel;
                }
            }
        }
        [Serializable]
        public class RelationEnumDev : UIElementDataListColumnDataRules.RelationEnum, IFormElementDataListColumnDataRule
        {
            public string Name
            {
                get { return FormElementDataListColumnDataRuleDevTypes.Instance.GetName(this); }
            }
            [NonSerialized]
            private Control _parameterSetPanel;
            public Control ParameterSetPanel
            {
                get
                {
                    if (_parameterSetPanel == null)
                    {
                        _parameterSetPanel = new UserControlDataColumnDevDataRule_RelationEnum();
                    }
                    return _parameterSetPanel;
                }
            }
        }
        [Serializable]
        public class RelationDataEntityDev : UIElementDataListColumnDataRules.RelationDataEntity, IFormElementDataListColumnDataRule
        {
            public string Name
            {
                get { return FormElementDataListColumnDataRuleDevTypes.Instance.GetName(this); }
            }
            [NonSerialized]
            private Control _parameterSetPanel;
            public Control ParameterSetPanel
            {
                get
                {
                    if (_parameterSetPanel == null)
                    {
                        _parameterSetPanel = new UserControlDataColumnDevDataRule_RelationDataEntity();
                    }
                    return _parameterSetPanel;
                }
            }
        }
    }
    static class FormElementDataColumnDataRuleExtensions
    {
        public static UIElementDataListColumnDataRuleAbstract GetParameter(this IFormElementDataListColumnDataRule dataColumnDataRule)
        {
            IFormElementDataListColumnDataRuleParameterSetControl control = dataColumnDataRule.ParameterSetPanel as IFormElementDataListColumnDataRuleParameterSetControl;
            if (control == null)
            {
                Debug.Assert(false, "没有实现 IFormElementDataColumnDataRuleParameterSetControl");
                return null;
            }
            return control.GetParameter();
        }
        public static void SetParameter(this IFormElementDataListColumnDataRule dataColumnDataRule, UIElementDataListColumnDataRuleAbstract dataRule)
        {
            IFormElementDataListColumnDataRuleParameterSetControl control = dataColumnDataRule.ParameterSetPanel as IFormElementDataListColumnDataRuleParameterSetControl;
            if (control == null)
            {
                Debug.Assert(false, "没有实现 IFormElementDataColumnDataRuleParameterSetControl");
                return;
            }
            control.SetParameter(dataRule);
        }
    }
}
