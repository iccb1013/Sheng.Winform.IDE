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
using System.ComponentModel.Design.Serialization;
using System.ComponentModel;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class SENameCreationService : INameCreationService
    {
        public SENameCreationService()
        {
        }
        public string CreateName(IContainer container, Type type)
        {
            if (null == container)
                return string.Empty;
            ComponentCollection cc = container.Components;
            int min = Int32.MaxValue;
            int max = Int32.MinValue;
            int count = 0;
            int i = 0;
            while (i < cc.Count)
            {
                Component comp = cc[i] as Component;
                if (comp.GetType() == type)
                {
                    count++;
                    string name = comp.Site.Name;
                    if (name.StartsWith(type.Name))
                    {
                        try
                        {
                            int value = Int32.Parse(name.Substring(type.Name.Length));
                            if (value < min) min = value;
                            if (value > max) max = value;
                        }
                        catch (Exception) { }
                    }
                }
                i++;
            } 
            if (0 == count)
            {
                return type.Name + "1";
            }
            else if (min > 1)
            {
                int j = min - 1;
                return type.Name + j.ToString();
            }
            else
            {
                int j = max + 1;
                return type.Name + j.ToString();
            }
        }
        public bool IsValidName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;
            if (!(char.IsLetter(name, 0)))
                return false;
            if (name.StartsWith("_"))
                return false;
            return true;
        }
        public void ValidateName(string name)
        {
            if (!(IsValidName(name)))
                throw new ArgumentException("Invalid name: " + name);
        }
    }
}
