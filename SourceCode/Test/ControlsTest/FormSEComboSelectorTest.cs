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
    public partial class FormSEComboSelectorTest : SEForm
    {
        public FormSEComboSelectorTest()
        {
            InitializeComponent();
        }

        private void FormSEComboSelectorTest_Load(object sender, EventArgs e)
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

            seComboSelector21.DisplayMember = "Name";
            seComboSelector21.DescriptionMember = "Company";
            seComboSelector21.DataBind(users);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            seComboSelector21.AllowEmpty = false;
            this.DoValidate();
        }
    }
}
