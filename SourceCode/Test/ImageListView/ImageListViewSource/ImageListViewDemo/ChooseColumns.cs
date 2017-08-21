using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Manina.Windows.Forms;

namespace ImageListViewDemo
{
    public partial class ChooseColumns : Form
    {
        public ImageListView imageListView;

        public ChooseColumns()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ImageListView.ImageListViewColumnHeader column = imageListView.Columns[e.Index];
            column.Visible = (e.NewValue == CheckState.Checked);
        }

        private void ChooseColumns_Load(object sender, EventArgs e)
        {
            foreach (ImageListView.ImageListViewColumnHeader column in imageListView.Columns)
            {
                checkedListBox.Items.Add(column.Text, column.Visible);
            }
        }
    }
}
