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
using System.Reflection;
using System.Diagnostics;
namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    public class SEUndoMember
    {
        public SEUndoMember()
        {
        }
        public SEUndoMember(string memberName,object oldValue,object newValue)
        {
            this.MemberName = memberName;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
        private string[] _memberNames;
        private string _memberName;
        public string MemberName
        {
            get { return this._memberName; }
            set
            {
                this._memberName = value;
                _memberNames = value.Split('/');
            }
        }
        private object _oldValue;
        public object OldValue
        {
            get { return this._oldValue; }
            set { this._oldValue = value; }
        }
        private object _newValue;
        public object NewValue
        {
            get { return this._newValue; }
            set { this._newValue = value; }
        }
        public void SetMember(object obj, EnumMemberValue enumMemberValue)
        {
            PropertyInfo propertyInfo = null;
            object currentObj = obj; 
            for (int i = 0; i < this._memberNames.Length; i++)
            {
                if (i == this._memberNames.Length - 1)
                {
                    propertyInfo = currentObj.GetType().GetProperty(this._memberNames[i]);
                }
                else
                {
                    currentObj = currentObj.GetType().GetProperty(this._memberNames[i]).GetValue(currentObj, null);
                    if (currentObj == null)
                        return;
                }
            }
            if (propertyInfo == null)
                return;
            object value;
            if (enumMemberValue == EnumMemberValue.NewValue)
                value = this.NewValue;
            else
                value = this.OldValue;
            try
            {
                if (propertyInfo.CanWrite)
                {
                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(currentObj, Enum.Parse(propertyInfo.PropertyType, value.ToString()), null);
                    }
                    else
                    {
                        propertyInfo.SetValue(currentObj, value, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }
        public override string ToString()
        {
            if (this.MemberName == null)
            {
                return String.Empty;
            }
            StringBuilder str = new StringBuilder(this.MemberName.Length);
            str.Append(this.MemberName);
            str.Append(":");
            str.Append("NewValue:");
            if (this.NewValue != null)
                str.Append(this.NewValue.ToString());
            else
                str.Append("Null");
            str.Append(",");
            str.Append("OldValue:");
            if (this.OldValue != null)
                str.Append(this.OldValue.ToString());
            else
                str.Append("Null");
            return str.ToString();
        }
        public enum EnumMemberValue
        {
            OldValue,
            NewValue
        }
    }
}
