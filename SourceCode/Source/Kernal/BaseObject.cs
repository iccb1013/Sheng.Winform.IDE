using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

namespace Sheng.SailingEase.Kernal
{
    /// <summary>
    /// BaseObject类是一个用来继承的抽象类
    /// 每一个由此类继承而来的类将自动支持克隆方法。
    /// 该类实现了Icloneable接口，并且每个从该对象继承而来的对象都将同样地 
    /// Core 中所有的对象实体类,事件实体类都应直接或间接继承此类
    /// </summary> 
    [Serializable]
    public abstract class BaseObject : ICloneable
    {
        /// <summary>    
        /// 克隆对象，并返回一个已克隆对象的引用  
        /// 该方法使用 ToXml和 FromXml 方法来实现
        /// </summary>        
        public virtual object Clone()
        {
            object newObject = Activator.CreateInstance(this.GetType());

            Type xmlableThis = this.GetType().GetInterface("IXmlable", true) ;
            Type xmlableNewObject = this.GetType().GetInterface("IXmlable", true);
            if (xmlableThis == null || xmlableNewObject == null)
            {
                Debug.Assert(false, "调用对象的 Clone()方法,但是没有实现 IXmlable");
                return null;
            }

            IXmlable iXmlableThis = (IXmlable)this;
            IXmlable iXmlableNewObject = (IXmlable)newObject;

            iXmlableNewObject.FromXml(iXmlableThis.ToXml());

            return newObject;
        }
   }
}


