using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;

namespace ControlsTest
{
    public partial class FormListViewTest : Form
    {
        public FormListViewTest()
        {
            InitializeComponent();
        }

        private void FormListViewTest_Load(object sender, EventArgs e)
        {
            List<User> users = new List<User>()
            {
                new User(){Name="张三",Company="微软"},
                new User(){Name="李四",Company="Google"},
                new User(){Name="王五",Company="Yahoo"},
                new User(){Name="赵六",Company="Intel"},
                new User(){Name="张三",Company="微软"},
                new User(){Name="张三",Company="微软"},
                new User(){Name="张三",Company="微软"},
                new User(){Name="张三",Company="微软"},
                new User(){Name="张三",Company="微软"},
            };

            seListView1.DisplayMember = "Name";
            SEListViewDescriptiveMembers descriptiveMembers = new SEListViewDescriptiveMembers();
            descriptiveMembers.Descriptioin = "Company";
            seListView1.AddExtendMember(descriptiveMembers);
            seListView1.LayoutMode = ListViewLayoutMode.Descriptive;
            seListView1.DataBind(users);
        }
    }

    class User
    {
        public string Name { get; set; }

        public string Company { get; set; }
    }
}
