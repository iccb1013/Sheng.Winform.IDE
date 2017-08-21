/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
小数点右边可以存储的十进制数字的最大位数。小数位数必须是从 0 到 p 之间的值。仅在指定精度后才可以指定小数位数。默认的小数位数为 0；因此，0 <= s <= p。最大存储大小基于精度而变化。
精度  存储字节数  
1 - 9
10-19
20-28
 13
29-38
 17
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.IDataBaseProvide;
namespace Sheng.SailingEase.SQLServer2005Provide
{
    [FieldProvideAttribute("Decimal", "带固定精度和小数位数的数值数据类型。", 0x000067)]
    public class FieldDecimal : IField
    {
        public FieldDecimal()
        {
        }
        public FieldDecimal(int length)
            : this(length, 0)
        {
        }
        public FieldDecimal(int length, byte decimalDigits)
        {
            Length = new FieldLength(length);
            Length.DecimalDigits = decimalDigits;
        }
        public string TypeName
        {
            get { return "Decimal"; }
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
            get { return 38; }
        }
        public int LengthMin
        {
            get { return 1; }
        }
        public bool DecimalDigitsEnable
        {
            get { return true; }
        }
        public int DecimalDigitsMax
        {
            get
            {
                if (_length >= 1 && _length <= 9)
                    return 5;
                else if (_length >= 10 && _length <= 19)
                    return 9;
                else if (_length >= 20 && _length <= 28)
                    return 13;
                else if (_length >= 29 && _length <= 38)
                    return 17;
                else
                    throw new ArgumentOutOfRangeException();
            }
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
        private FieldLength _length = new FieldLength(18);
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
