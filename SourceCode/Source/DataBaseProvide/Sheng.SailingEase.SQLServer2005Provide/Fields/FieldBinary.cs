/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 binary [ ( n ) ] 
长度为 n 字节的固定长度二进制数据，其中 n 是从 1 到 8,000 的值。存储大小为 n 字节。
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.IDataBaseProvide;
namespace Sheng.SailingEase.SQLServer2005Provide
{
    [FieldProvideAttribute("Binary", "固定长度的 Binary 数据类型。", 0x000064)]
    public class FieldBinary : IField
    {
        public string TypeName
        {
            get { return "Binary"; }
        }
        public bool LengthEnable
        {
            get { return true; }
        }
        public bool AllowMaxLength
        {
            get { return false; }
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
