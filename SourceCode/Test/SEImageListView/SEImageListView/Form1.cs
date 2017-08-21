using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SEImageListView
{
    public partial class Form1 : Form
    {
        ImageListViewItem.GetImageHandler _getImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            seImageListView1.AllowMultiSelection = true;

             _getImage =
                new ImageListViewItem.GetImageHandler((key) => {
                    return Image.FromFile(key.ToString());
                });


            DirectoryInfo path = new DirectoryInfo(@"D:\Users\sheng\Pictures");
            foreach (FileInfo p in path.GetFiles("*.*"))
            {
                if (p.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".cur", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".emf", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".wmf", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".tif", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))

                    seImageListView1.Items.Add(new ImageListViewItem(p.FullName, p.Name, _getImage));
            }


            //for (int i = 0; i < 300; i++)
            //{
            //    seImageListView1.Items.Add(new ImageListViewItem());
            //}
        }

        private void btnShowDebugInfo_Click(object sender, EventArgs e)
        {
            lblFirstPartiallyVisible.Text = "FirstPartiallyVisible:" + seImageListView1.LayoutManager.FirstPartiallyVisible.ToString();
            lblLastPartiallyVisible.Text = "LastPartiallyVisible:" + seImageListView1.LayoutManager.LastPartiallyVisible.ToString();

            lblFirstVisible.Text = "FirstVisible:" +  seImageListView1.LayoutManager.FirstVisible.ToString();
            lblLastVisible.Text = "LastVisible" + seImageListView1.LayoutManager.LastVisible.ToString();
        }

        private void seImageListView1_MouseDown(object sender, MouseEventArgs e)
        {
            lblLastMouseDownLocation.Text = "LastMouseDownLocation:" + seImageListView1.LastMouseDownLocation.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo p = new FileInfo(dialog.FileName);
                seImageListView1.Items.Add(new ImageListViewItem(p.FullName, p.Name, _getImage));
            }
            dialog.Dispose();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            List<ImageListViewItem> itmes = seImageListView1.GetSelectedItems();
            seImageListView1.Items.Remove(itmes);
        }
    }
}
