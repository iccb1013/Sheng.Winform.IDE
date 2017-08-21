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
using Sheng.SailingEase.Win32;
namespace Sheng.SailingEase.Infrastructure
{
    public partial class BackgroundWorkerView : FormViewBase
    {
        private BackgroundWorker _worker;
        public bool Cancelled { get; private set; }
        public Exception Error { get; private set; }
        public object Result { get; private set; }
        public BackgroundWorkerView(BackgroundWorker worker)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            _worker = worker;
        }
        private void BackgroundWorkerView_Load(object sender, EventArgs e)
        {
            User32.EnableMenuItem(User32.GetSystemMenu(this.Handle, false), User32.SC_CLOSE, User32.MF_DISABLE);
            if (_worker.WorkerReportsProgress)
            {
                this.progressBar1.Style = ProgressBarStyle.Blocks;
                this.progressBar1.Minimum = 0;
                this.progressBar1.Maximum = 100;
                _worker.ProgressChanged += (workerSender, workerE) =>
                {
                    this.progressBar1.Value = workerE.ProgressPercentage;
                };
            }
            else
            {
                this.progressBar1.Style = ProgressBarStyle.Marquee;
            }
            _worker.RunWorkerCompleted += (workerSender, workerE) =>
            {
                this.Cancelled = workerE.Cancelled;
                this.Error = workerE.Error;
                this.Result = workerE.Result;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            };
            _worker.RunWorkerAsync();
        }
    }
}
