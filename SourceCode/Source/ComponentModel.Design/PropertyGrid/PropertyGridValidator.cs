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
    public delegate PropertyGridValidateResult PropertyGridValidateHandler(PropertyGridValidateArgs args);
    public class PropertyGridValidator
    {
        private Type _type;
        public Type Type
        {
            get { return _type; }
        }
        private bool _actOnSub= false;
        public bool ActOnSub
        {
            get { return _actOnSub; }
            set { _actOnSub = value; }
        }
        public PropertyGridValidateHandler Validator
        {
            get;
            set;
        }
        public PropertyGridValidator(Type type)
        {
            _type = type;
        }
    }
    public class PropertyGridValidateArgs
    {
        private object _value;
        public object Value
        {
            get { return _value; }
            internal set { _value = value; }
        }
        private string _property;
        public string Property
        {
            get { return _property; }
            internal set { _property = value; }
        }
        private object[] _objects;
        public object[] Objects
        {
            get { return _objects; }
            internal set { _objects = value; }
        }
        public PropertyGridValidateArgs(string property, object value, object[] objects)
        {
            _property = property;
            _value = value;
            _objects = objects;
        }
    }
    public class PropertyGridValidateResult
    {
        private bool _success = true;
        public bool Success
        {
            get { return _success; }
        }
        private string _message;
        public string Message
        {
            get { return _message; }
        }
        public PropertyGridValidateResult()
        {
        }
        public PropertyGridValidateResult(bool success, string message)
        {
            _success = success;
            _message = message;
        }
    }
}
