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
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Windows.Forms;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class SEDesignerSerializationService : IDesignerSerializationService
    {
        private IServiceProvider _serviceProvider;
        public SEDesignerSerializationService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
        public System.Collections.ICollection Deserialize(object serializationData)
        {
            SerializationStore serializationStore = serializationData as SerializationStore;
            if (serializationStore != null)
            {
                ComponentSerializationService componentSerializationService =
                    _serviceProvider.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService;
                ICollection collection = componentSerializationService.Deserialize(serializationStore);
                return collection;
            }
            return new object[] { };
        }
        public object Serialize(System.Collections.ICollection objects)
        {
            ComponentSerializationService componentSerializationService =
                _serviceProvider.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService;
            SerializationStore returnObject = null;
            using (SerializationStore serializationStore = componentSerializationService.CreateStore())
            {
                foreach (object obj in objects)
                {
                    if (obj is Control)
                        componentSerializationService.Serialize(serializationStore, obj);
                }
                returnObject = serializationStore;
            }
            return returnObject;
        }
    }
}
