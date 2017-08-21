/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.RegexTool
{
    public partial class FormMain : Form
    {
        FormRegexLib formRegexLib = new FormRegexLib();
        public string Regex
        {
            get
            {
                return this.txtRegex.Text;
            }
            set
            {
                this.txtRegex.Text = value;
            }
        }
        public FormMain()
        {
            InitializeComponent();
        }
        private RegexOptions SelectedRegexOptions
        {
            get
            {
                RegexOptions selectedRegexOptions = RegexOptions.None;
                if (this.chkIgnoreCase.Checked) selectedRegexOptions |= RegexOptions.IgnoreCase;
                if (this.chkMultiLine.Checked) selectedRegexOptions |= RegexOptions.Multiline;
                if (this.chkSingleLine.Checked) selectedRegexOptions |= RegexOptions.Singleline;
                if (this.chkExplicitCapture.Checked) selectedRegexOptions |= RegexOptions.ExplicitCapture;
                return selectedRegexOptions;
            }
        }
        private void btnIsMatch_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = String.Empty;
            string error;
            this.txtResult.Text =
                RegexHelper.IsMatch(this.txtInput.Text, this.txtRegex.Text, SelectedRegexOptions, out error).ToString();
            if (error != null)
                this.txtResult.Text += Environment.NewLine + error;
        }
        private void btnReplace_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = String.Empty;
            string error;
            this.txtResult.Text =
                RegexHelper.Replace(this.txtInput.Text, this.txtRegex.Text,this.txtReplace.Text, SelectedRegexOptions, out error).ToString();
            if (error != null)
                this.txtResult.Text += Environment.NewLine + error;
        }
        private void btnSplit_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = String.Empty;
            string error;
            string [] result = 
                RegexHelper.Split(this.txtInput.Text, this.txtRegex.Text, SelectedRegexOptions, out error);
            foreach (string str in result)
                this.txtResult.Text += str + Environment.NewLine;
            if (error != null)
                this.txtResult.Text += Environment.NewLine + error;
        }
        private void btnMatches_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = String.Empty;
            string error;
            string[] result =
                RegexHelper.Matches(this.txtInput.Text, this.txtRegex.Text, SelectedRegexOptions, out error);
            foreach (string str in result)
                this.txtResult.Text += str + Environment.NewLine;
            if (error != null)
                this.txtResult.Text += Environment.NewLine + error;
        }
        private void btnGroups_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = String.Empty;
            string error;
            string[] result =
                RegexHelper.Groups(this.txtInput.Text, this.txtRegex.Text, SelectedRegexOptions, out error);
            foreach (string str in result)
                this.txtResult.Text += str + Environment.NewLine;
            if (error != null)
                this.txtResult.Text += Environment.NewLine + error;
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (formRegexLib.ShowDialog() == DialogResult.OK)
            {
                this.txtRegex.Text = formRegexLib.Regex;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
