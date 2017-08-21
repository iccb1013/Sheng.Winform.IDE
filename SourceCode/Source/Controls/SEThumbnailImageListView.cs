/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using Sheng.SailingEase.Drawing;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEThumbnailImageListView : ListView
    {
        private BackgroundWorker imageLoadBackgroundWork = new BackgroundWorker();
        private int thumbNailSize = 95;
        public int ThumbNailSize
        {
            get { return thumbNailSize; }
            set { thumbNailSize = value; }
        }
        private Color thumbBorderColor = Color.White;
        public Color ThumbBorderColor
        {
            get { return thumbBorderColor; }
            set { thumbBorderColor = value; }
        }
        public bool IsLoading
        {
            get { return imageLoadBackgroundWork.IsBusy; }
        }
        private string folder;
        public string Folder
        {
            get { return folder; }
            set
            {
                if (value == null || value == String.Empty)
                {
                    return;
                }
                if (!Directory.Exists(value))
                    throw new DirectoryNotFoundException();
                folder = value;
                ReLoadItems();
            }
        }
        private string _filter = "*.jpg|*.png|*.gif|*.bmp";
        public string Filter
        {
            get
            {
                return this._filter;
            }
            set
            {
                this._filter = value;
            }
        }
        public SEThumbnailImageListView()
        {
            LicenseManager.Validate(typeof(SEThumbnailImageListView)); 
            ImageList il = new ImageList();
            il.ImageSize = new Size(thumbNailSize, thumbNailSize);
            il.ColorDepth = ColorDepth.Depth32Bit;
            LargeImageList = il;
            imageLoadBackgroundWork.WorkerSupportsCancellation = true;
            imageLoadBackgroundWork.DoWork += new DoWorkEventHandler(bwLoadImages_DoWork);
            imageLoadBackgroundWork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myWorker_RunWorkerCompleted);
        }
        private delegate void SetThumbnailDelegate(Image image);
        private void SetThumbnail(Image image)
        {
            if (Disposing) return;
            if (this.InvokeRequired)
            {
                SetThumbnailDelegate d = new SetThumbnailDelegate(SetThumbnail);
                this.Invoke(d, new object[] { image });
            }
            else
            {
                LargeImageList.Images.Add(image); 
                int index = LargeImageList.Images.Count - 1;
                Items[index - 1].ImageIndex = index;
            }
        }
        public Image GetThumbNail(string fileName)
        {
            Bitmap bmp;
            try
            {
                bmp = (Bitmap)DrawingTool.GetImage(fileName);
            }
            catch
            {
                bmp = new Bitmap(ThumbNailSize, ThumbNailSize); 
            }
            bmp = (Bitmap)DrawingTool.GetScaleImage(bmp, ThumbNailSize, ThumbNailSize);
            if (bmp.Width < ThumbNailSize || bmp.Height < ThumbNailSize)
            {
                Bitmap bitmap2 = new Bitmap(ThumbNailSize, ThumbNailSize);
                Graphics g = Graphics.FromImage(bitmap2);
                Point point = new Point();
                point.X = (ThumbNailSize - bmp.Width) / 2;
                point.Y = (ThumbNailSize - bmp.Height) / 2;
                g.DrawImage(bmp, point);
                g.Dispose();
                bmp.Dispose();
                return bitmap2;
            }
            return bmp;
        }
        private void AddDefaultThumb()
        {
            Bitmap bmp = new Bitmap(LargeImageList.ImageSize.Width, LargeImageList.ImageSize.Height, 
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Graphics grp = Graphics.FromImage(bmp);
            grp.Clear(Color.White);
            LargeImageList.Images.Add(bmp);
        }
        public void LoadItems(string[] fileList)
        {
            if ((imageLoadBackgroundWork != null) && (imageLoadBackgroundWork.IsBusy))
                imageLoadBackgroundWork.CancelAsync();
            BeginUpdate();
            Items.Clear();
            LargeImageList.Images.Clear();
            AddDefaultThumb();
            foreach (string fileName in fileList)
            {
                ListViewItem liTemp = Items.Add(System.IO.Path.GetFileName(fileName));
                liTemp.ImageIndex = 0;
                liTemp.Tag = fileName;
            }
            EndUpdate();
            if (imageLoadBackgroundWork != null)
            {
                if (!imageLoadBackgroundWork.CancellationPending)
                {
                    if (OnLoadStart != null)
                        OnLoadStart(this, new EventArgs());
                    imageLoadBackgroundWork.RunWorkerAsync(fileList);
                }
            }
        }
        private void ReLoadItems()
        {
            List<string> fileList = new List<string>();
            string[] arExtensions = Filter.Split('|');
            foreach (string filter in arExtensions)
            {
                string[] strFiles = Directory.GetFiles(folder, filter);
                fileList.AddRange(strFiles);
            }
            fileList.Sort();
            LoadItems(fileList.ToArray());
        }
        private void bwLoadImages_DoWork(object sender, DoWorkEventArgs e)
        {
            if (imageLoadBackgroundWork.CancellationPending) return;
            string[] fileList = (string[])e.Argument;
            foreach (string fileName in fileList)
                SetThumbnail(GetThumbNail(fileName));
        }
        void myWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (OnLoadComplete != null)
                OnLoadComplete(this, new EventArgs());
        }
        public event EventHandler OnLoadComplete;
        public event EventHandler OnLoadStart;
        public void Add(string imageFile)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Tag.ToString().Equals(imageFile))
                {
                    this.LargeImageList.Images[i + 1] = GetThumbNail(imageFile);
                    this.Invalidate(this.Items[i].GetBounds(ItemBoundsPortion.Entire));
                    return;
                }
            }
            ListViewItem liTemp = Items.Add(System.IO.Path.GetFileName(imageFile));
            liTemp.ImageIndex = 0;
            liTemp.Tag = imageFile;
            SetThumbnail(GetThumbNail(imageFile));
        }
    }
}
