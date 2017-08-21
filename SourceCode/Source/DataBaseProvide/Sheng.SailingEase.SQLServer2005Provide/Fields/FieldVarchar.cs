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
    [FieldProvideAttribute("Varchar", "可变长度的字符数据类型。", 0x00006D)]
    public class FieldVarchar : IField
    {
        public FieldVarchar()
        {
        }
        public FieldVarchar(int length)
        {
            Length = new FieldLength(length);
        }
        public string TypeName
        {
            get { return "Varchar"; }
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
