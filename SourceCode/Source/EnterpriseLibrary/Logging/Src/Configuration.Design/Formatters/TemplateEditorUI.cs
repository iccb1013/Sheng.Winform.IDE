//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    /// <summary>
    /// Represents an editor for a template.
    /// </summary>
    public class TemplateEditorUI : Form
    {
        private string[] tokens = new string[]
            {
                "{message}",
                "{category}",
                "{priority}",
                "{eventid}",
                "{severity}",
                "{title}",
                "{timestamp}",
                "{machine}",
                "{appDomain}",
                "{processId}",
                "{processName}",
                "{threadName}",
                "{win32ThreadId}",
                "{dictionary()}",
                "{keyvalue()}",
                "{newline}",
                "{tab}",
                "{property()}"
            };

        private TextBox templateTextBox;
        private ComboBox tokenDropdown;
        private Button okButton;
        private Button insertTokenButton;
        private Button cancelButton;

        /// <summary>
        /// The text of the template as defined by the user at designtime.
        /// </summary>
        public string UserText
        {
            get { return templateTextBox.Text; }
            set
            {
                templateTextBox.Text = value;
                templateTextBox.Select(templateTextBox.Text.Length, 0);
            }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// Initializes the components.
        /// </summary>
        public TemplateEditorUI()
        {
            InitializeComponent();

            tokenDropdown.DataSource = tokens;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TemplateEditorUI));
            this.templateTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.tokenDropdown = new System.Windows.Forms.ComboBox();
            this.insertTokenButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // templateTextBox
            // 
            this.templateTextBox.AcceptsReturn = true;
            this.templateTextBox.AcceptsTab = true;
            this.templateTextBox.AccessibleDescription = resources.GetString("templateTextBox.AccessibleDescription");
            this.templateTextBox.AccessibleName = resources.GetString("templateTextBox.AccessibleName");
            this.templateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("templateTextBox.Anchor")));
            this.templateTextBox.AutoSize = ((bool)(resources.GetObject("templateTextBox.AutoSize")));
            this.templateTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("templateTextBox.BackgroundImage")));
            this.templateTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("templateTextBox.Dock")));
            this.templateTextBox.Enabled = ((bool)(resources.GetObject("templateTextBox.Enabled")));
            this.templateTextBox.Font = ((System.Drawing.Font)(resources.GetObject("templateTextBox.Font")));
            this.templateTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("templateTextBox.ImeMode")));
            this.templateTextBox.Location = ((System.Drawing.Point)(resources.GetObject("templateTextBox.Location")));
            this.templateTextBox.MaxLength = ((int)(resources.GetObject("templateTextBox.MaxLength")));
            this.templateTextBox.Multiline = ((bool)(resources.GetObject("templateTextBox.Multiline")));
            this.templateTextBox.Name = "templateTextBox";
            this.templateTextBox.PasswordChar = ((char)(resources.GetObject("templateTextBox.PasswordChar")));
            this.templateTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("templateTextBox.RightToLeft")));
            this.templateTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("templateTextBox.ScrollBars")));
            this.templateTextBox.Size = ((System.Drawing.Size)(resources.GetObject("templateTextBox.Size")));
            this.templateTextBox.TabIndex = ((int)(resources.GetObject("templateTextBox.TabIndex")));
            this.templateTextBox.Text = resources.GetString("templateTextBox.Text");
            this.templateTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("templateTextBox.TextAlign")));
            this.templateTextBox.Visible = ((bool)(resources.GetObject("templateTextBox.Visible")));
            this.templateTextBox.WordWrap = ((bool)(resources.GetObject("templateTextBox.WordWrap")));
            // 
            // okButton
            // 
            this.okButton.AccessibleDescription = resources.GetString("okButton.AccessibleDescription");
            this.okButton.AccessibleName = resources.GetString("okButton.AccessibleName");
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("okButton.Anchor")));
            this.okButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("okButton.BackgroundImage")));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("okButton.Dock")));
            this.okButton.Enabled = ((bool)(resources.GetObject("okButton.Enabled")));
            this.okButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("okButton.FlatStyle")));
            this.okButton.Font = ((System.Drawing.Font)(resources.GetObject("okButton.Font")));
            this.okButton.Image = ((System.Drawing.Image)(resources.GetObject("okButton.Image")));
            this.okButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.ImageAlign")));
            this.okButton.ImageIndex = ((int)(resources.GetObject("okButton.ImageIndex")));
            this.okButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("okButton.ImeMode")));
            this.okButton.Location = ((System.Drawing.Point)(resources.GetObject("okButton.Location")));
            this.okButton.Name = "okButton";
            this.okButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("okButton.RightToLeft")));
            this.okButton.Size = ((System.Drawing.Size)(resources.GetObject("okButton.Size")));
            this.okButton.TabIndex = ((int)(resources.GetObject("okButton.TabIndex")));
            this.okButton.Text = resources.GetString("okButton.Text");
            this.okButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.TextAlign")));
            this.okButton.Visible = ((bool)(resources.GetObject("okButton.Visible")));
            // 
            // tokenDropdown
            // 
            this.tokenDropdown.AccessibleDescription = resources.GetString("tokenDropdown.AccessibleDescription");
            this.tokenDropdown.AccessibleName = resources.GetString("tokenDropdown.AccessibleName");
            this.tokenDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tokenDropdown.Anchor")));
            this.tokenDropdown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tokenDropdown.BackgroundImage")));
            this.tokenDropdown.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tokenDropdown.Dock")));
            this.tokenDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tokenDropdown.Enabled = ((bool)(resources.GetObject("tokenDropdown.Enabled")));
            this.tokenDropdown.Font = ((System.Drawing.Font)(resources.GetObject("tokenDropdown.Font")));
            this.tokenDropdown.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tokenDropdown.ImeMode")));
            this.tokenDropdown.IntegralHeight = ((bool)(resources.GetObject("tokenDropdown.IntegralHeight")));
            this.tokenDropdown.ItemHeight = ((int)(resources.GetObject("tokenDropdown.ItemHeight")));
            this.tokenDropdown.Location = ((System.Drawing.Point)(resources.GetObject("tokenDropdown.Location")));
            this.tokenDropdown.MaxDropDownItems = ((int)(resources.GetObject("tokenDropdown.MaxDropDownItems")));
            this.tokenDropdown.MaxLength = ((int)(resources.GetObject("tokenDropdown.MaxLength")));
            this.tokenDropdown.Name = "tokenDropdown";
            this.tokenDropdown.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tokenDropdown.RightToLeft")));
            this.tokenDropdown.Size = ((System.Drawing.Size)(resources.GetObject("tokenDropdown.Size")));
            this.tokenDropdown.TabIndex = ((int)(resources.GetObject("tokenDropdown.TabIndex")));
            this.tokenDropdown.Text = resources.GetString("tokenDropdown.Text");
            this.tokenDropdown.Visible = ((bool)(resources.GetObject("tokenDropdown.Visible")));
            // 
            // insertTokenButton
            // 
            this.insertTokenButton.AccessibleDescription = resources.GetString("insertTokenButton.AccessibleDescription");
            this.insertTokenButton.AccessibleName = resources.GetString("insertTokenButton.AccessibleName");
            this.insertTokenButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("insertTokenButton.Anchor")));
            this.insertTokenButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("insertTokenButton.BackgroundImage")));
            this.insertTokenButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("insertTokenButton.Dock")));
            this.insertTokenButton.Enabled = ((bool)(resources.GetObject("insertTokenButton.Enabled")));
            this.insertTokenButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("insertTokenButton.FlatStyle")));
            this.insertTokenButton.Font = ((System.Drawing.Font)(resources.GetObject("insertTokenButton.Font")));
            this.insertTokenButton.Image = ((System.Drawing.Image)(resources.GetObject("insertTokenButton.Image")));
            this.insertTokenButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("insertTokenButton.ImageAlign")));
            this.insertTokenButton.ImageIndex = ((int)(resources.GetObject("insertTokenButton.ImageIndex")));
            this.insertTokenButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("insertTokenButton.ImeMode")));
            this.insertTokenButton.Location = ((System.Drawing.Point)(resources.GetObject("insertTokenButton.Location")));
            this.insertTokenButton.Name = "insertTokenButton";
            this.insertTokenButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("insertTokenButton.RightToLeft")));
            this.insertTokenButton.Size = ((System.Drawing.Size)(resources.GetObject("insertTokenButton.Size")));
            this.insertTokenButton.TabIndex = ((int)(resources.GetObject("insertTokenButton.TabIndex")));
            this.insertTokenButton.Text = resources.GetString("insertTokenButton.Text");
            this.insertTokenButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("insertTokenButton.TextAlign")));
            this.insertTokenButton.Visible = ((bool)(resources.GetObject("insertTokenButton.Visible")));
            this.insertTokenButton.Click += new System.EventHandler(this.insertTokenButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = resources.GetString("cancelButton.AccessibleDescription");
            this.cancelButton.AccessibleName = resources.GetString("cancelButton.AccessibleName");
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cancelButton.Anchor")));
            this.cancelButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cancelButton.BackgroundImage")));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cancelButton.Dock")));
            this.cancelButton.Enabled = ((bool)(resources.GetObject("cancelButton.Enabled")));
            this.cancelButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("cancelButton.FlatStyle")));
            this.cancelButton.Font = ((System.Drawing.Font)(resources.GetObject("cancelButton.Font")));
            this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.ImageAlign")));
            this.cancelButton.ImageIndex = ((int)(resources.GetObject("cancelButton.ImageIndex")));
            this.cancelButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cancelButton.ImeMode")));
            this.cancelButton.Location = ((System.Drawing.Point)(resources.GetObject("cancelButton.Location")));
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cancelButton.RightToLeft")));
            this.cancelButton.Size = ((System.Drawing.Size)(resources.GetObject("cancelButton.Size")));
            this.cancelButton.TabIndex = ((int)(resources.GetObject("cancelButton.TabIndex")));
            this.cancelButton.Text = resources.GetString("cancelButton.Text");
            this.cancelButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.TextAlign")));
            this.cancelButton.Visible = ((bool)(resources.GetObject("cancelButton.Visible")));
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // TemplateEditorUI
            // 
            this.AcceptButton = this.okButton;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.insertTokenButton);
            this.Controls.Add(this.tokenDropdown);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.templateTextBox);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "TemplateEditorUI";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.ResumeLayout(false);

        }

        #endregion


        private void insertTokenButton_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(templateTextBox.Text);
            if (templateTextBox.SelectionLength > 0)
            {
                sb.Remove(templateTextBox.SelectionStart, templateTextBox.SelectionLength);
            }

            sb.Insert(templateTextBox.SelectionStart, tokenDropdown.SelectedValue);

            int parentIdx = tokenDropdown.SelectedValue.ToString().IndexOf("()");
            int selectionStart = 0;

            selectionStart = templateTextBox.SelectionStart + tokenDropdown.SelectedValue.ToString().Length;
            if (parentIdx > 0)
            {
                selectionStart -= 2;
            }

            templateTextBox.Text = sb.ToString();
            tokenDropdown.SelectedIndex = 0;

            templateTextBox.Focus();
            templateTextBox.Select(selectionStart, 0);
            templateTextBox.ScrollToCaret();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
