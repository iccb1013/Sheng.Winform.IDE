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
namespace Sheng.SailingEase.IDataBaseProvide
{
    public interface IFieldFactory
    {
        IField CreateInstance(int code);
        IField CreateInstance(FieldProvideAttribute attribute);
        FieldProvideAttribute GetProvideAttribute(int code);
        FieldProvideAttribute GetProvideAttribute(IField field);
        FieldProvideAttribute GetProvideAttribute(Type type);
        List<FieldProvideAttribute> GetProvideAttributeList();
        string GetName(IField field);
        List<Type> GetAvailableDataItems();
    }
}
