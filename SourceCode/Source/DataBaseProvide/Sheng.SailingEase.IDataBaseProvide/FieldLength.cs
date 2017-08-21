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
    public class FieldLength
    {
        private bool _max = false;
        public bool Max
        {
            get { return _max; }
            set
            {
                _max = value;
            }
        }
        private int _length = 1;
        public int Length
        {
            get { return _length; }
            set
            {
                _length = value;
                if (_length <= 0)
                    Max = true;
            }
        }
        private byte _decimalDigits = 0;
        public byte DecimalDigits
        {
            get { return _decimalDigits; }
            set { _decimalDigits = value; }
        }
        public FieldLength()
        {
        }
        public FieldLength(int length)
        {
            Length = length;
        }
        public override string ToString()
        {
            if (this.Max)
                return "Max";
            else
                return this.Length.ToString();
        }
        public static explicit operator FieldLength(bool value)
        {
            FieldLength length = new FieldLength();
            length.Max = Convert.ToBoolean(value);
            return length;
        }
        public static explicit operator FieldLength(int value)
        {
            FieldLength length = new FieldLength();
            length.Length = Convert.ToInt32(value);
            return length;
        }
        public static explicit operator FieldLength(string value)
        {
            FieldLength length = new FieldLength();
            if (value.ToUpper() == "MAX")
            {
                length.Max = true;
            }
            else
            {
                try
                {
                    length.Length = (int)Convert.ChangeType(value, typeof(int));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
            return length;
        }
        public static bool operator >(FieldLength c1, int c2)
        {
            if (c1.Max)
                return true;
            return c1.Length > c2;
        }
        public static bool operator <(FieldLength c1, int c2)
        {
            if (c1.Max)
                return false;
            return c1.Length < c2;
        }
        public static bool operator >=(FieldLength c1, int c2)
        {
            if (c1.Max)
                return true;
            return c1.Length >= c2;
        }
        public static bool operator <=(FieldLength c1, int c2)
        {
            if (c1.Max)
                return false;
            return c1.Length <= c2;
        }
    }
}
