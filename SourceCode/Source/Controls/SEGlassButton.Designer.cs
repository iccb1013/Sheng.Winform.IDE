/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls
{
    partial class SEGlassButton
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_imageButton != null)
                    {
                        _imageButton.Parent.Dispose();
                        _imageButton.Parent = null;
                        _imageButton.Dispose();
                        _imageButton = null;
                    }
                    DestroyFrames();
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Timer timer;
    }
}
