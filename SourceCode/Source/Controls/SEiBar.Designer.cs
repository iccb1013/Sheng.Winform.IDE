/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls
{
	partial class SEiBar
	{
		private System.ComponentModel.IContainer components = null;
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}
		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "UserControl1";
			this.Size = new System.Drawing.Size( 422, 50 );
			this.ResumeLayout( false );
		}
	}
}
