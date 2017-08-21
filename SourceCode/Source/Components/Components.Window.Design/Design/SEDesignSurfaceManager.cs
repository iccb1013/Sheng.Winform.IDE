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
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class SEDesignSurfaceManager : DesignSurfaceManager
    {
        public SEDesignSurfaceManager()
            : base()
        {
            this.AddService(typeof(INameCreationService), new SENameCreationService());
        }
        protected override DesignSurface CreateDesignSurfaceCore(IServiceProvider parentProvider)
        {
            return new SEDesignSurface(parentProvider);
        }
        public SEDesignSurface NewDesignSurface()
        {
            SEDesignSurface designSurface = (SEDesignSurface)this.CreateDesignSurface(this.ServiceContainer);
            this.ActiveDesignSurface = designSurface;
            return designSurface;
        }
        public void AddService(Type type, object serviceInstance)
        {
            this.ServiceContainer.AddService(type, serviceInstance);
        }
    }
}
