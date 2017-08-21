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
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
   
    public static class EntityExtensions
    {
        private static IDataEntityComponentService _dataEntityComponentService;
        static EntityExtensions()
        {
            _dataEntityComponentService = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        }
        public static string GetDataItemCode(this UIElementDataListTextBoxColumnEntity entity)
        {
            if (entity.IsBind)
            {
                string[] ids = entity.DataItemId.Split('.');
                return _dataEntityComponentService.GetDataItemEntity(ids[1], ids[0]).Code;
            }
            else
            {
                return entity.DataPropertyName;
            }
        }
    }
}
