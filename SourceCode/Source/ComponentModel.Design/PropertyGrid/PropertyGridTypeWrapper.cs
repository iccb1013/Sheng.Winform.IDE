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
namespace Sheng.SailingEase.ComponentModel.Design
{
   
   public class PropertyGridTypeWrapper
    {
        private Type _type;
        public Type Type
        {
            get { return _type; }
        }
        private bool _actOnSubClass = false;
        public bool ActOnSubClass
        {
            get { return _actOnSubClass; }
            set { _actOnSubClass = value; }
        }
        private bool _disableAll = false;
        public bool DisableAll
        {
            get { return _disableAll; }
            set { _disableAll = value; }
        }
        private List<string> _disableException = new List<string>();
        public List<string> DisableException
        {
            get { return _disableException; }
        }
        private bool _visibleAll = true;
        public bool VisibleAll
        {
            get { return _visibleAll; }
            set { _visibleAll = value; }
        }
        private List<string> _visibleException = new List<string>();
        public List<string> VisibleException
        {
            get { return _visibleException; }
        }
        public PropertyGridTypeWrapper(Type type)
        {
            _type = type;
        }
        public bool IsVisible(string property)
        {
            if (_visibleAll)
            {
                if (_visibleException.Contains(property))
                    return false;
                else
                    return true;
            }
            else
            {
                if (_visibleException.Contains(property))
                    return true;
                else
                    return false;
            }
        }
        public bool IsDisible(string property)
        {
            if (_disableAll)
            {
                if (_disableException.Contains(property))
                    return false;
                else
                    return true;
            }
            else
            {
                if (_disableException.Contains(property))
                    return true;
                else
                    return false;
            }
        }
    }
}
