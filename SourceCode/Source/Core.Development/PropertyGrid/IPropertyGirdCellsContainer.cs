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
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design;
namespace Sheng.SailingEase.Core.Development
{
    public interface IPropertyGirdCellsContainer
    {
        IPropertyGirdCell Create(PropertyEditorAttribute attribute);
        void Register(Type editorAttribute, Type cellType);
    }
    public class PropertyGirdCellsContainer : IPropertyGirdCellsContainer
    {
        private CellFactory _factory = CellFactory.Instance;
        public IPropertyGirdCell Create(PropertyEditorAttribute attribute)
        {
            return _factory.Create(attribute);
        }
        public void Register(Type editorAttribute, Type cellType)
        {
            _factory.Register(editorAttribute, cellType);
        }
    }
}
