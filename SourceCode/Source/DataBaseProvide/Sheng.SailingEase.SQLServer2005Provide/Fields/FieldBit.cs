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
using Sheng.SailingEase.IDataBaseProvide;
namespace Sheng.SailingEase.SQLServer2005Provide
{
    [FieldProvideAttribute("Bit",
        "可以取值为 1、0 或 NULL 的整数数据类型。字符串值 TRUE 和 FALSE 可以转换为以下 bit 值：TRUE 转换为 1，FALSE 转换为 0。",0x000065)]
    public class FieldBit : IField
    {
        public string TypeName
        {
            get { return "Bit"; }
        }
        public bool LengthEnable
        {
            get { return false; }
        }
        public bool AllowMaxLength
        {
            get { return false; }
        }
        public int LengthMax
        {
            get { return 1; }
        }
        public int LengthMin
        {
            get { return 1; }
        }
        public bool DecimalDigitsEnable
        {
            get { return false; }
        }
        public int DecimalDigitsMax
        {
            get { return 0; }
        }
        public int DecimalDigitsMin
        {
            get { return 0; }
        }
        private string _name = String.Empty;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private FieldLength _length = new FieldLength(1);
        public FieldLength Length
        {
            get { return _length; }
            set { _length = value; }
        }
        private bool _allowEmpty = true;
        public bool AllowEmpty
        {
            get { return _allowEmpty; }
            set { _allowEmpty = value; }
        }
        private string _defaultValue = String.Empty;
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
        public string ToSql()
        {
            return SQLServer2005.GetSql(this);
        }
    }
}
