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
using System.Windows.Forms;
namespace Sheng.SailingEase.Kernal
{
    public class DialogUnity
    {
        public static bool OpenFile(string filter, out string[] fileNames)
        {
            DialogResult result;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Filter = filter;
                result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                    fileNames = dialog.FileNames;
                else
                    fileNames = null;
            }
            return result == DialogResult.OK;
        }
        public static bool OpenFile(string filter, out string fileName)
        {
            DialogResult result;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = filter;
                result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                    fileName = dialog.FileName;
                else
                    fileName = null;
            }
            return result == DialogResult.OK;
        }
        public static bool FolderBrowser(string description, out string folder)
        {
            DialogResult result;
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = description;
                result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                    folder = dialog.SelectedPath;
                else
                    folder = null;
            }
            return result == DialogResult.OK;
        }
    }
}
