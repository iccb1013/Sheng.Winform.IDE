/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Sheng.SailingEase.ComponentModel.Design
{
    public interface IPropertyGirdCell
    {
        PropertyGridPad Owner { get; set; }
        PropertyGridRow OwnerRow { get; set; }
        PropertyRelatorAttribute PropertyRelatorAttribute { get; set; }
        DefaultValueAttribute DefaultValueAttribute { get; set; }
        PropertyEditorAttribute PropertyEditorAttribute { get; set; }
        string GetPropertyXml(string xmlNodeName);
        string GetPropertyString();
        void SetPropertyValue(object value);
        object GetPropertyValue();
        object GetPropertyOldValue();
    }
}
