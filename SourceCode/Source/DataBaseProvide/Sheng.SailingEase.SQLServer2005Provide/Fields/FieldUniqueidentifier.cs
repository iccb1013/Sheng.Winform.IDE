/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 uniqueidentifier 数据类型的列或局部变量可通过以下方式初始化为一个值： 
使用 NEWID 函数。
从 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx 形式的字符串常量转换，其中，每个 x 是一个在 0-9 或 a-f 范围内的十六进制数字。例如，6F9619FF-8B86-D011-B42D-00C04FC964FF 为有效 uniqueidentifier 值。
比较运算符可与 uniqueidentifier 值一起使用。不过，排序不是通过比较两个值的位模式来实现的。可针对 uniqueidentifier 值执行的运算只有比较运算（=、<>、<、>、<=、>=）以及检查是否为 NULL（IS NULL 和 IS NOT NULL）。不能使用其他算术运算符。除 IDENTITY 之外的所有列约束和属性均可对 uniqueidentifier 数据类型使用。
具有更新订阅的合并复制和事务复制使用 uniqueidentifier 列来确保在表的多个副本中唯一地标识行。
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.IDataBaseProvide;
namespace Sheng.SailingEase.SQLServer2005Provide
{
    [FieldProvideAttribute("Uniqueidentifier", "16 字节 GUID。", 0x00006B)]
    public class FieldUniqueidentifier : IField
    {
        public string TypeName
        {
            get { return "Uniqueidentifier"; }
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
            get { return 16; }
        }
        public int LengthMin
        {
            get { return 16; }
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
        private FieldLength _length = new FieldLength(16);
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
