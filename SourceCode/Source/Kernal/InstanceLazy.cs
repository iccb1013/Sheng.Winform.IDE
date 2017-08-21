/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 *  private static InstanceLazy<FormOption> m_lazyInstance = new InstanceLazy<FormOption>(() => new FormOption());
 *  see:http:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Sheng.SailingEase.Kernal
{
    public class InstanceLazy<T>
    {
        private Func<T> _initFunc;
        private Action _disposeFunc;
        private bool _initialized;
        private object _mutex;
        private T _value;
        public InstanceLazy(Func<T> initFunc)
            : this(initFunc, null)
        {
        }
        public InstanceLazy(Func<T> initFunc, Action disposeFunc)
        {
            this._initialized = false;
            this._initFunc = initFunc;
            this._disposeFunc = disposeFunc;
            this._mutex = new object();
        }
        public T Value
        {
            get
            {
                if (!this._initialized)
                {
                    lock (this._mutex)
                    {
                        if (!this._initialized)
                        {
                            this._value = this._initFunc();
                            this._initialized = true;
                        }
                    }
                }
                return this._value;
            }
        }
        public void DisposeValue()
        {
            if (this._disposeFunc != null)
            {
                this._disposeFunc();
            }
            
            _value = default(T);
            _initialized = false;
        }
    }
}
