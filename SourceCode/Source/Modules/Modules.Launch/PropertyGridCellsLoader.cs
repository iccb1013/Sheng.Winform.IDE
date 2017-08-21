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
using System.Reflection;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Modules.LaunchModule
{
    class PropertyGridCellsLoader
    {
        private Assembly _assembly;
        public PropertyGridCellsLoader(Assembly assembly)
        {
            _assembly = assembly;
        }
        public void Load()
        {
            List<AttributeAndTypeRelation> avaliableCells =
                ReflectionAttributeHelper.GetAttributeAndTypeRelation<PropertyGridCellProvideAttribute>(_assembly, false);
            IPropertyGirdCellsContainer propertyGirdCellsContainer = ServiceUnity.Container.Resolve<IPropertyGirdCellsContainer>();
            foreach (var item in avaliableCells)
            {
                PropertyGridCellProvideAttribute cellProvideAttribute = (PropertyGridCellProvideAttribute)item.Attribute;
                propertyGirdCellsContainer.Register(cellProvideAttribute.EditorAttribut, item.Type);
            }
        }
    }
}
