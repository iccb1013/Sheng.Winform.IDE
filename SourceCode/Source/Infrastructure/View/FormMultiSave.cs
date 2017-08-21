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
namespace Sheng.SailingEase.Infrastructure
{
    public partial class FormMultiSave : FormViewBase 
    {
      
        private List<ICanSaveForm> _saveForms = new List<ICanSaveForm>();
        public List<ICanSaveForm> SaveForms
        {
            get { return _saveForms; }
        }
        public FormMultiSave()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
        }
        private void FormMultiSave_Load(object sender, EventArgs e)
        {
            listBoxSaveForms.Items.Clear();
            listBoxSaveForms.Items.AddRange(_saveForms.ToArray());
            listBoxSaveForms.SelectedIndex = -1;
        }
        private void listBoxSaveForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            ICanSaveForm form = listBoxSaveForms.SelectedItem as ICanSaveForm;
            if (form == null)
                return;
            form.Activate();
        }
        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
