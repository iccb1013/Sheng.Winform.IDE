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
    public class SQLServer2005 : IDataBase
    {
        internal static string GetSql(IField field)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(String.Format("[{0}] {1} ", field.Name, field.TypeName));
            if (field.LengthEnable)
            {
                sql.Append(String.Format("({0}{1})", field.Length, field.DecimalDigitsEnable ? "," + field.Length.DecimalDigits : ""));
            }
            sql.Append(field.AllowEmpty ? "NULL " : "NOT NULL ");
            sql.Append(String.IsNullOrEmpty(field.DefaultValue) ? "" : String.Format("DEFAULT ({0}) ", field.DefaultValue));
            return sql.ToString();
        }
        private IFieldFactory _fieldFactory = new FieldFactory();
        public IFieldFactory FieldFactory
        {
            get { return _fieldFactory; }
        }
        public string IdFieldDefaultValue
        {
            get { return "newid()"; }
        }
        public IField CreateIdField()
        {
            return new FieldUniqueidentifier();
        }
        public IField CreateIntField()
        {
            return new FieldInt();
        }
        public IField CreateVarCharField(int length)
        {
            return new FieldVarchar(length);
        }
        public IField CreateSmallDatetimeField()
        {
            return new FieldSmallDatetime();
        }
        public string CreateSql(string table, List<IField> fields)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(String.Format("CREATE TABLE [{0}]", table));
            sql.Append(Environment.NewLine);
            sql.Append("(");
            sql.Append(Environment.NewLine);
            for (int i = 0; i < fields.Count; i++)
            {
                sql.Append(fields[i].ToSql());
                if (i < fields.Count - 1)
                {
                    sql.Append(",");
                    sql.Append(Environment.NewLine);
                }
            }
            sql.Append(Environment.NewLine);
            sql.Append(")");
            return sql.ToString();
        }
        public string CreateSql(string table)
        {
            return String.Format("CREATE DATABASE [{0}]", table);
        }
    }
}
