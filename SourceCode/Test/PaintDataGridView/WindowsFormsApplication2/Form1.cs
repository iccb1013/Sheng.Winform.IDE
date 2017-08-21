using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        RichToolStripMenuItem item = new RichToolStripMenuItem();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            item.Text = "ToolStripItem2";
            
            fILEToolStripMenuItem.DropDownItems.Add(item);


            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Columns.Add("Class");
            dt.Columns.Add("Bool",typeof(bool));

            DataRow dr = dt.NewRow();
            dr[0] = "查了一下，多是用EventHandler完成的，看起来觉";
            dr[1] = "18";
            dr[2] = "A";
            dr[3] = "True";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Lily";
            dr[1] = "17";
            dr[2] = "B";
            dr[3] = "True";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Lily";
            dr[1] = "17";
            dr[2] = "http://ihower.tw/training/ Ruby on Rails";
            dr[3] = "True";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Lily";
            dr[1] = "17";
            dr[2] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "d的Select All功能。查了一下，多是用EventHandler完成的，看起来觉";
            dr[1] = "17";
            dr[2] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Lily";
            dr[1] = "17";
            dr[2] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Lily";
            dr[1] = "17";
            dr[2] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Lily";
            dr[1] = "17";
            dr[2] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Lily";
            dr[1] = "17";
            dr[2] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Lily";
            dr[1] = "17";
            dr[2] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Lily";
            dr[1] = "17";
            dr[2] = "B";
            dt.Rows.Add(dr);

            dataGridView.DataSource = dt;
        }

        private void fILEToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            item.Size = new System.Drawing.Size(200, 200);
        }
    }
}
