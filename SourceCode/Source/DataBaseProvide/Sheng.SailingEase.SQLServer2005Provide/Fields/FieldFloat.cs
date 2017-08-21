/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 float [ ( n ) ] 
其中 n 为用于存储 float 数值尾数的位数，以科学记数法表示，因此可以确定精度和存储大小。如果指定了 n，则它必须是介于 1 和 53 之间的某个值。n 的默认值为 53。
n value  精度  存储大小  
1-24
 7 位数
 4 字节
25-53
 15 位数
 8 字节
注意：  
SQL Server 2005 将 n 视为下列两个可能值之一。如果 1<=n<=24，则将 n 视为 24。如果 25<=n<=53，则将 n 视为 53。 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.IDataBaseProvide;
namespace Sheng.SailingEase.SQLServer2005Provide
{
    [FieldProvideAttribute("Float",
        "用于表示浮点数值数据的大致数值数据类型。浮点数据为近似值；因此，并非数据类型范围内的所有值都能精确地表示。", 0x000068)]
    public class FieldFloat : IField
    {
        public FieldFloat()
        {
        }
        public FieldFloat(int length)
        {
            Length = new FieldLength(length);
        }
        public string TypeName
        {
            get { return "Float"; }
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
            get { return 53; }
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
        private FieldLength _length = new FieldLength(24);
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
