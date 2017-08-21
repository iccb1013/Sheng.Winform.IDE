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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    /// <summary>
    /// UI for Category Filter Editor.
    /// </summary>
    public class CategoryFilterEditorUI : Form
    {
        private Button OKButton;

        private IList<string> availableCategoryNames;
        private ComboBox availableCategoryNamesBox;
        private ListBox selectedCategories;
        private Button removeCategoryButton;
        private Button addCategoryButton;
        private RadioButton allowCategoriesRadio;
        private RadioButton denyCategoriesRadio;
        private Label categoryFiltersDescriptionLabel;
        private GroupBox categoriesGroupBox;
        private Button cancelButton;
        private GroupBox filterModeGroupBox;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// Initializes the component.
        /// </summary>
        public CategoryFilterEditorUI()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            UpdateAddFilterButtonEnabled();

            this.removeCategoryButton.Enabled = false;
        }

        /// <summary>
        /// List of available category names.
        /// </summary>
        public ReadOnlyCollection<string> AvailableCategoryNames
        {
            get { return new ReadOnlyCollection<string>(availableCategoryNames); }			
        }

		/// <summary>
		/// Updates the available category names.
		/// </summary>
		/// <param name="categoryNames">The new category names</param>
		public void UpdateAvailableCategoryNames(IList<string> categoryNames)
		{			
			availableCategoryNamesBox.Items.Clear();
			foreach (string availableCategory in categoryNames)
			{
				availableCategoryNamesBox.Items.Add(availableCategory);
			}

			foreach (string selectedCategory in selectedCategories.Items)
			{
				availableCategoryNamesBox.Items.Remove(selectedCategory);
			}
			availableCategoryNamesBox.Text = String.Empty;
			this.availableCategoryNames = categoryNames;
		}

        /// <summary>
        /// Sets the selected category names.
        /// </summary>
        /// <param name="names">The category names.</param>
        public void SetSelectedCategoryNames(ArrayList names)
        {
			if (null == names) throw new ArgumentNullException("names");

            foreach (string name in names)
            {
                selectedCategories.Items.Add(name);
            }
        }

        /// <summary>
        /// Returns the currently selected category names.
        /// </summary>
        /// <returns>See summary.</returns>
        public ArrayList SelectedCategoryNames
        {
			get { return new ArrayList(selectedCategories.Items); }
        }

        /// <summary>
        /// Determines the filter mode.
        /// </summary>
        public CategoryFilterMode FilterMode
        {
            get
            {
                if (allowCategoriesRadio.Checked)
                {
                    return CategoryFilterMode.AllowAllExceptDenied;
                }
                else
                {
                    return CategoryFilterMode.DenyAllExceptAllowed;
                }
            }
            set
            {
                allowCategoriesRadio.Checked = (value == CategoryFilterMode.AllowAllExceptDenied);
                denyCategoriesRadio.Checked = (value == CategoryFilterMode.DenyAllExceptAllowed);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
		/// <param name="disposing">True if method should dispose of unmanaged resources.</param>
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryFilterEditorUI));
			this.selectedCategories = new System.Windows.Forms.ListBox();
			this.addCategoryButton = new System.Windows.Forms.Button();
			this.removeCategoryButton = new System.Windows.Forms.Button();
			this.categoryFiltersDescriptionLabel = new System.Windows.Forms.Label();
			this.allowCategoriesRadio = new System.Windows.Forms.RadioButton();
			this.denyCategoriesRadio = new System.Windows.Forms.RadioButton();
			this.OKButton = new System.Windows.Forms.Button();
			this.categoriesGroupBox = new System.Windows.Forms.GroupBox();
			this.availableCategoryNamesBox = new System.Windows.Forms.ComboBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.filterModeGroupBox = new System.Windows.Forms.GroupBox();
			this.categoriesGroupBox.SuspendLayout();
			this.filterModeGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// selectedCategories
			// 
			resources.ApplyResources(this.selectedCategories, "selectedCategories");
			this.selectedCategories.Name = "selectedCategories";
			this.selectedCategories.SelectedIndexChanged += new System.EventHandler(this.SelectedCategories_SelectedIndexChanged);
			// 
			// addCategoryButton
			// 
			resources.ApplyResources(this.addCategoryButton, "addCategoryButton");
			this.addCategoryButton.Name = "addCategoryButton";
			this.addCategoryButton.Click += new System.EventHandler(this.AddCategoryButton_Click);
			// 
			// removeCategoryButton
			// 
			resources.ApplyResources(this.removeCategoryButton, "removeCategoryButton");
			this.removeCategoryButton.Name = "removeCategoryButton";
			this.removeCategoryButton.Click += new System.EventHandler(this.RemoveCategoryButton_Click);
			// 
			// categoryFiltersDescriptionLabel
			// 
			resources.ApplyResources(this.categoryFiltersDescriptionLabel, "categoryFiltersDescriptionLabel");
			this.categoryFiltersDescriptionLabel.Name = "categoryFiltersDescriptionLabel";
			// 
			// allowCategoriesRadio
			// 
			resources.ApplyResources(this.allowCategoriesRadio, "allowCategoriesRadio");
			this.allowCategoriesRadio.Name = "allowCategoriesRadio";
			// 
			// denyCategoriesRadio
			// 
			resources.ApplyResources(this.denyCategoriesRadio, "denyCategoriesRadio");
			this.denyCategoriesRadio.Name = "denyCategoriesRadio";
			// 
			// OKButton
			// 
			resources.ApplyResources(this.OKButton, "OKButton");
			this.OKButton.Name = "OKButton";
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// categoriesGroupBox
			// 
			resources.ApplyResources(this.categoriesGroupBox, "categoriesGroupBox");
			this.categoriesGroupBox.Controls.Add(this.removeCategoryButton);
			this.categoriesGroupBox.Controls.Add(this.addCategoryButton);
			this.categoriesGroupBox.Controls.Add(this.selectedCategories);
			this.categoriesGroupBox.Controls.Add(this.availableCategoryNamesBox);
			this.categoriesGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.categoriesGroupBox.Name = "categoriesGroupBox";
			this.categoriesGroupBox.TabStop = false;
			// 
			// availableCategoryNamesBox
			// 
			resources.ApplyResources(this.availableCategoryNamesBox, "availableCategoryNamesBox");
			this.availableCategoryNamesBox.DropDownWidth = 250;
			this.availableCategoryNamesBox.Name = "availableCategoryNamesBox";
			this.availableCategoryNamesBox.TextChanged += new System.EventHandler(this.availableCategoryNamesBox_TextChanged);
			this.availableCategoryNamesBox.SelectedValueChanged += new System.EventHandler(this.availableCategoryNamesBox_SelectedValueChanged);
			this.availableCategoryNamesBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.availableCategoryNamesBox_KeyDown);
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// filterModeGroupBox
			// 
			this.filterModeGroupBox.Controls.Add(this.allowCategoriesRadio);
			this.filterModeGroupBox.Controls.Add(this.denyCategoriesRadio);
			this.filterModeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			resources.ApplyResources(this.filterModeGroupBox, "filterModeGroupBox");
			this.filterModeGroupBox.Name = "filterModeGroupBox";
			this.filterModeGroupBox.TabStop = false;
			// 
			// CategoryFilterEditorUI
			// 
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.cancelButton;
			this.Controls.Add(this.filterModeGroupBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.categoriesGroupBox);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.categoryFiltersDescriptionLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CategoryFilterEditorUI";
			this.ShowInTaskbar = false;
			this.categoriesGroupBox.ResumeLayout(false);
			this.filterModeGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private void AddCategoryButton_Click(object sender, EventArgs e)
        {
            if (availableCategoryNamesBox.SelectedIndex > 0)
            {
                selectedCategories.Items.Add(availableCategoryNamesBox.Text);
            }
            else if (availableCategoryNamesBox.Text.Length > 0
                && selectedCategories.Items.IndexOf(availableCategoryNamesBox.Text) == -1)
            {
                selectedCategories.Items.Add(availableCategoryNamesBox.Text);
            }
            UpdateAvailableCategoryNames(this.availableCategoryNames);
        }

        private void RemoveCategoryButton_Click(object sender, EventArgs e)
        {
            if (selectedCategories.SelectedIndex > -1)
            {
                selectedCategories.Items.RemoveAt(selectedCategories.SelectedIndex);
            }
            UpdateAvailableCategoryNames(this.availableCategoryNames);
        }

        private void SelectedCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeCategoryButton.Enabled = selectedCategories.SelectedIndex > -1;
        }        

        private void availableCategoryNamesBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addCategoryButton.PerformClick();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UpdateAddFilterButtonEnabled() 
        {
            addCategoryButton.Enabled = availableCategoryNamesBox.Text != null && availableCategoryNamesBox.Text.Trim().Length > 0;
        }

        private void availableCategoryNamesBox_SelectedValueChanged(object sender, System.EventArgs e)
        {
            UpdateAddFilterButtonEnabled();
        }

        private void availableCategoryNamesBox_TextChanged(object sender, System.EventArgs e)
        {
            UpdateAddFilterButtonEnabled();
        }
    }
}
