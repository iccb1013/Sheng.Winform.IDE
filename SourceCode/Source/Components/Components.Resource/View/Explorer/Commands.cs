/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Components.ResourceComponent.Localisation;
namespace Sheng.SailingEase.Components.ResourceComponent.View
{
    class AddImageResourceCommand : AbstactCommand
    {
        private ResourceComponentService _resourceService = ResourceComponentService.Instance;
        public AddImageResourceCommand()
        {
            ExcuteHandler = () =>
            {
                _resourceService.ImportImageResource();
            };
        }
    }
    class RemoveImageResourceCommand : AbstactCommand<List<ImageResourceInfo>>
    {
        private ResourceArchive _resourceArchive = ResourceArchive.Instance;
        public RemoveImageResourceCommand()
        {
            ExcuteHandler = () =>
            {
                List<ImageResourceInfo> imageResourceInfoList = GetArgument();
                if (imageResourceInfoList == null || imageResourceInfoList.Count == 0)
                    return;
                string strConfirmDelete = String.Format(
                    Language.Current.RemoveImageResourceCommand_ConfirmDelete, imageResourceInfoList.Count);
                if (MessageBox.Show(strConfirmDelete,
                     CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }
                foreach (ImageResourceInfo imageResource in imageResourceInfoList)
                {
                    _resourceArchive.Remove(imageResource.Name);
                }
            };
        }
    }
}
