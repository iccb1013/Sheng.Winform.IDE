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
using System.Diagnostics;
namespace Sheng.SailingEase.Core.Development
{
    public static class DialogUnity
    {
        public static bool ImageResourceChoose(out ImageResourceInfo imageResource)
        {
            bool result = false;
            imageResource = null;
            using (ImageResourceChooseView view = new ImageResourceChooseView())
            {
                if (view.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageResource = view.SelectedImageResource;
                    result = true;
                }
            }
            return result;
        }
        public static DataEntityTreeChooseResult DataEntityChoose(DataEntityTreeChooseArgs args)
        {
            DataEntityTreeChooseResult result = new DataEntityTreeChooseResult();
            using (DataEntityTreeChooseView view = new DataEntityTreeChooseView(args.ShowDataItem))
            {
                if (view.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    result.DialogResult = true;
                    result.SelectedId = view.SelectedId;
                    result.SelectedName = view.SelectedName;
                    result.SelectedDataEntity = view.SelectedDataEntity;
                    result.SelectedDataItemEntity = view.SelectedDataItemEntity;
                }
            }
            return result;
        }
        public static bool EnumChoose(out EnumEntity enumEntity)
        {
            bool result = false;
            enumEntity = null;
            using (FormEnumChoose view = new FormEnumChoose())
            {
                if (view.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    enumEntity = view.SelectedEnumEntity;
                    result = true;
                }
            }
            return result;
        }
    }
}
