/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 数据类型  范围  精确度  
datetime
 1753 年 1 月 1 日到 9999 年 12 月 31 日
 3.33 毫秒
smalldatetime
 1900 年 1 月 1 日到 2079 年 6 月 6 日
 1 分钟
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.IDataBaseProvide;
namespace Sheng.SailingEase.SQLServer2005Provide
{
    [FieldProvideAttribute("SmallDatetime", "用于表示某天的日期和时间的数据类型。", 0x00006A)]
    public class FieldSmallDatetime : IField
    {
        public string TypeName
        {
            get { return "SmallDatetime"; }
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
            get { return 2; }
        }
        public int LengthMin
        {
            get { return 2; }
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
        private FieldLength _length = new FieldLength(2);
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
