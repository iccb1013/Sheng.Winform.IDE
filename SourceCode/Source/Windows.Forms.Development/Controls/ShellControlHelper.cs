using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Core;

namespace Sheng.SailingEase.Windows.Forms.Development
{
    static class ShellControlHelper
    {
        #region PropertyDescriptorUpdate

        /// <summary>
        /// 根据给定的实体对象更新控件的属性
        /// 对于一般窗体组件此方法只更新共有属性部分
        /// 为窗体或一般控件更新通用属性
        /// </summary>
        /// <param name="control"></param>
        /// <param name="entity"></param>
        public static void PropertyDescriptorUpdate(Control control, EntityBase entity)
        {
            if (entity is WindowEntity)
            {
                PropertyDescriptorUpdate(control, (WindowEntity)entity);
            }
            else
            {
                PropertyDescriptorUpdate(control, (UIElement)entity);
            }

        }

        /// <summary>
        /// 根据给定的实体对象更新控件的属性
        /// </summary>
        /// <param name="control"></param>
        /// <param name="formEntity"></param>
        public static void PropertyDescriptorUpdate(Control control, WindowEntity formEntity)
        {
            SetProperty(control, "Text", formEntity.Text);
            SetProperty(control, "Size", formEntity.Size);
            SetProperty(control, "BackColor", formEntity.BackColor);
            SetProperty(control, "ForeColor", formEntity.ForeColor);

            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(control);
            PropertyDescriptor pds;

            //font还是这样手动设置,因为在不设置字体时,既不能设一个过去,也不能设null
            //只有不设字体,才能使其使用系统默认并与父容器同步
            pds = pdc.Find("Font", false);
            if (pds != null)
            {
                if (formEntity.Font != null)
                {
                    if (!pds.GetValue(control).Equals(formEntity.Font.Font))
                    {
                        pds.SetValue(control, formEntity.Font.Font);
                    }
                }
                else
                {
                    pds.SetValue(control, SystemFonts.DefaultFont);
                }
            }

        }

        /// <summary>
        /// 根据给定的实体对象更新控件的属性
        /// 此方法只更新一般组件的共有属性部分
        /// </summary>
        /// <param name="control"></param>
        /// <param name="formElement"></param>
        public static void PropertyDescriptorUpdate(Control control, UIElement formElement)
        {

            SetProperty(control, "Text", formElement.Text);
            SetProperty(control, "Size", formElement.Size);
            SetProperty(control, "Location", formElement.Location);
            SetProperty(control, "BackColor", formElement.BackColor);
            SetProperty(control, "ForeColor", formElement.ForeColor);
            SetProperty(control, "Visible", formElement.Visible);
            SetProperty(control, "Enabled", formElement.Enabled);
            SetProperty(control, "TabIndex", formElement.TabIndex);

            AnchorStyles anchorStyles = new AnchorStyles();

            //处理边缘锚定
            //先清除所有方向描定，.net默认左上角锚定
            anchorStyles = anchorStyles & (AnchorStyles)AnchorStyles.Top;
            anchorStyles = anchorStyles & (AnchorStyles)AnchorStyles.Right;
            anchorStyles = anchorStyles & (AnchorStyles)AnchorStyles.Bottom;
            anchorStyles = anchorStyles & (AnchorStyles)AnchorStyles.Left;

            if (formElement.Anchor != null)
            {
                if (formElement.Anchor.Top)
                    anchorStyles = anchorStyles | (AnchorStyles)AnchorStyles.Top;
                if (formElement.Anchor.Right)
                    anchorStyles = anchorStyles | (AnchorStyles)AnchorStyles.Right;
                if (formElement.Anchor.Bottom)
                    anchorStyles = anchorStyles | (AnchorStyles)AnchorStyles.Bottom;
                if (formElement.Anchor.Left)
                    anchorStyles = anchorStyles | (AnchorStyles)AnchorStyles.Left;
            }

            SetProperty(control, "Anchor", anchorStyles);


            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(control);
            PropertyDescriptor pds;

            pds = pdc.Find("Font", false);
            if (pds != null)
            {
                if (formElement.Font != null)
                {
                    if (!pds.GetValue(control).Equals(formElement.Font.Font))
                    {
                        pds.SetValue(control, formElement.Font.Font);
                    }
                }
                else
                {
                    pds.SetValue(control, SystemFonts.DefaultFont);
                }
            }
        }

        #endregion

        //否决，转到WindowEntity中了
        /// <summary>
        /// 根据指定的实体类型,获取一个新的code
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="formEntity"></param>
        /// <returns></returns>
        public static string EntityCodeCreation(string prefix, WindowEntity formEntity)
        {
           
            int value = 0;
            //当前编号数组
            List<int> codes = new List<int>();
            //这里需要连同InnerElement一起获取
            foreach (UIElement element in formEntity.GetFormElement(true))
            {
                string name = element.Code;
                if (name.StartsWith(prefix))
                {
                    if (Int32.TryParse(name.Substring(prefix.Length), out value))
                    {
                        codes.Add(value);
                    }
                }
            }

            codes.Sort();

            int lastValue = 0; //上一个值，用于判断是否中间有空缺要补

            foreach (int code in codes)
            {
                if (code - lastValue > 1)
                {
                    break;
                }
                else
                {
                    lastValue = code;
                }
            }

            return prefix + (lastValue + 1);
        }

        /// <summary>
        /// 更新Control更新FormElementEntity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="control"></param>
        public static void UpdateEntity(UIElement entity, Control control)
        {
            //尺寸
            entity.Size = control.Size;

            //大小 
            entity.Location = control.Location;

            //tabindex
            entity.TabIndex = control.TabIndex;

            //ZOrder
            //if (control.Parent != null)
            //    entity.ZOrder = control.Parent.Controls.GetChildIndex(control);
            Helper.ZOrderSync(entity);

            //特别为兼容字体问题,见ElementFont中的说明
            if (entity.Font != null)
                entity.Font.Font = control.Font;
        }

        public static void SetProperty(object obj, string name, object value)
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(obj);
            PropertyDescriptor pds = pdc.Find(name, false);
            if (pds != null)
            {
                if (pds.GetValue(obj) == null)
                {
                    pds.SetValue(obj, value);
                }
                else if (!pds.GetValue(obj).Equals(value))
                {
                    pds.SetValue(obj, value);
                }
            }
        }

        /// <summary>
        /// 为 newEntity 替换 oldEntity 做准备 
        /// 主要判断 newEntity  的 Id,Code,Name 是否需要更新
        /// </summary>
        /// <param name="oldEntity"></param>
        /// <param name="newEntity"></param>
        public static void ReplaceControlEntity(UIElement oldEntity, UIElement newEntity)
        {
            

            if (oldEntity == null || oldEntity.HostFormEntity == null)
            {
                return;
            }

            
            if (newEntity.HostFormEntity == null)  
            {
                newEntity.Id = Guid.NewGuid().ToString();
            }

            if (oldEntity.HostFormEntity.FindFormElementByCode(newEntity.Code) != null)
            {
                newEntity.Code = oldEntity.Code;
                newEntity.Name = oldEntity.Code;
            }

            newEntity.HostFormEntity = oldEntity.HostFormEntity;

            oldEntity.HostFormEntity.Elements.Remove(oldEntity);
            newEntity.HostFormEntity.Elements.Add(newEntity);
        }
    }
}
