/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 varbinary [ ( n | max ) ] 
可变长度二进制数据。n 可以取从 1 到 8,000 的值。max 指示最大的存储大小为 2^31-1 字节。存储大小为所输入数据的实际长度 + 2 个字节。
所输入数据的长度可以是 0 字节。varbinary 的 SQL-2003 同义词为 binary varying。
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.IDataBaseProvide;
namespace Sheng.SailingEase.SQLServer2005Provide
{
    [FieldProvideAttribute("Varbinary", "可变长度的 Binary 数据类型。", 0x00006C)]
    public class FieldVarbinary : IField
    {
        public FieldVarbinary()
        {
        }
        public FieldVarbinary(int length)
        {
            Length = new FieldLength(length);
        }
        public string TypeName
        {
            get { return "Varbinary"; }
        }
        public bool LengthEnable
        {
            get { return true; }
        }
        public bool AllowMaxLength
        {
            get { return true; }
        }
        public int LengthMax
        {
            get { return 8000; }
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
        private FieldLength _length = new FieldLength(50);
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
