using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Manina.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace ImageListViewDemo
{
    public partial class DemoForm : Form
    {
        #region A Custom Renderer for Demonstrating the Control
        /// <summary>
        /// A renderer that displays useful information
        /// when the control is empty.
        /// </summary>
        private class DemoRenderer : ImageListView.ImageListViewRenderer
        {
            string[] infoTexts = new string[] { 
                "Start by adding some image files.",
                "You can switch between Thumbnails, Gallery, Pane and Details view modes.",
                "In Details mode, ImageListView displays image properties in columns.",
                "The appearance of ImageListView can be customized to a great extent.\r\nTry selecting a different renderer from the drop down.",
                "Size of generated thumbnails can be customized.\r\nImageListView will try to extract embedded Exif thumbnails if possible.",
            };

            int[] infoLocations = new int[]{
                23,
                190,
                244,
                360,
                490,
            };

            int current;
            private Timer infoTimer;

            public DemoRenderer()
            {
                current = 0;
                infoTimer = new Timer();
                infoTimer.Interval = 5000;
                infoTimer.Tick += new EventHandler(infoTimer_Tick);
                infoTimer.Enabled = true;
            }

            void infoTimer_Tick(object sender, EventArgs e)
            {
                current++;
                if (current == infoTexts.Length)
                    current = 0;

                ImageListView.Refresh();
            }

            public override void Dispose()
            {
                infoTimer.Dispose();
                base.Dispose();
            }

            /// <summary>
            /// Initializes the System.Drawing.Graphics used to draw
            /// control elements.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            public override void InitializeGraphics(Graphics g)
            {
                base.InitializeGraphics(g);
                g.SmoothingMode = SmoothingMode.HighQuality;
            }

            /// <summary>
            /// Draws an overlay image over the client area.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="bounds">The bounding rectangle of the client area.</param>
            public override void DrawOverlay(Graphics g, Rectangle bounds)
            {
                if (ImageListView.Items.Count != 0)
                {
                    infoTimer.Enabled = false;
                    return;
                }

                if (!infoTimer.Enabled)
                {
                    current = 0;
                    infoTimer.Enabled = true;
                }

                DrawToolTip(g, infoLocations[current], infoTexts[current]);
            }

            /// <summary>
            /// Draws a tooltip.
            /// </summary>
            private void DrawToolTip(Graphics g, int x, string s)
            {
                bool onLeft = (x < ImageListView.ClientRectangle.Width / 2);
                int width = 200;
                Size sz = Size.Round(g.MeasureString(s, ImageListView.Font, width));
                sz.Width += 20;
                sz.Height += 10;
                int y = 12;

                int arrowOffset = 15;
                if (!onLeft) arrowOffset = sz.Width - 30;
                if (!onLeft) x -= sz.Width - 45;

                int fillet = 10;
                if (fillet > sz.Height / 2) fillet = sz.Height / 2;
                int shadow = 3;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddLine(x - 20 + arrowOffset, y, x - 15 + arrowOffset, y - 10);
                    path.AddLine(x - 15 + arrowOffset, y - 10, x - 10 + arrowOffset, y);
                    path.AddLine(x - 10 + arrowOffset, y, x + sz.Width - 20 - fillet, y);
                    path.AddArc(x + sz.Width - 20 - 2 * fillet, y, 2 * fillet, 2 * fillet, 270.0f, 90.0f);
                    path.AddLine(x + sz.Width - 20, y + fillet, x + sz.Width - 20, y + sz.Height - fillet);
                    path.AddArc(x + sz.Width - 20 - 2 * fillet, y + sz.Height - 2 * fillet, 2 * fillet, 2 * fillet, 0.0f, 90.0f);
                    path.AddLine(x + sz.Width - 20 - fillet, y + sz.Height, x - 20 + fillet, y + sz.Height);
                    path.AddArc(x - 20, y + sz.Height - 2 * fillet, 2 * fillet, 2 * fillet, 90.0f, 90.0f);
                    path.AddLine(x - 20, y + sz.Height - fillet, x - 20, y + fillet);
                    path.AddArc(x - 20, y, 2 * fillet, 2 * fillet, 180.0f, 90.0f);
                    path.AddLine(x - 20 + fillet, y, x - 20 + arrowOffset, y);
                    path.CloseFigure();

                    path.Transform(new Matrix(1, 0, 0, 1, shadow, shadow));
                    using (Brush b = new SolidBrush(Color.FromArgb(128, Color.Gray)))
                    {
                        g.FillPath(b, path);
                    }
                    path.Transform(new Matrix(1, 0, 0, 1, -shadow, -shadow));

                    using (Brush b = new LinearGradientBrush(path.GetBounds(), Color.BlanchedAlmond, Color.White, LinearGradientMode.ForwardDiagonal))
                    {
                        g.FillPath(b, path);
                    }
                    using (Pen p = new Pen(SystemColors.InfoText))
                    {
                        g.DrawPath(p, path);
                    }
                    using (Brush b = new SolidBrush(SystemColors.InfoText))
                    {
                        g.DrawString(infoTexts[current], ImageListView.Font, b, new Rectangle(x - 20 + 10, y + 5, sz.Width - 16, sz.Height - 10));
                    }
                }
            }
        }
        #endregion

        #region Constructor
        public DemoForm()
        {
            InitializeComponent();

            Application.Idle += new EventHandler(Application_Idle);

            // Populate renderer dropdown
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            int i = 0;
            foreach (Type t in assembly.GetTypes())
            {
                if (t.BaseType == typeof(Manina.Windows.Forms.ImageListView.ImageListViewRenderer))
                {
                    renderertoolStripComboBox.Items.Add(new RendererItem(t));
                    if (t.Name == "DefaultRenderer")
                        renderertoolStripComboBox.SelectedIndex = i;
                    i++;
                }
            }
            imageListView1.SetRenderer(new DemoRenderer());
        }
        #endregion

        #region Refresh UI Cues
        void Application_Idle(object sender, EventArgs e)
        {
            // Refresh UI cues
            removeToolStripButton.Enabled = (imageListView1.SelectedItems.Count > 0);
            removeToolStripButton.Enabled = (imageListView1.SelectedItems.Count > 0);
            removeAllToolStripButton.Enabled = (imageListView1.Items.Count > 0);

            thumbnailsToolStripButton.Checked = (imageListView1.View == Manina.Windows.Forms.View.Thumbnails);
            detailsToolStripButton.Checked = (imageListView1.View == Manina.Windows.Forms.View.Details);
            galleryToolStripButton.Checked = (imageListView1.View == Manina.Windows.Forms.View.Gallery);
            paneToolStripButton.Checked = (imageListView1.View == Manina.Windows.Forms.View.Pane);

            clearCacheToolStripButton.Enabled = (imageListView1.Items.Count > 0);

            deleteToolStripMenuItem.Enabled = (imageListView1.SelectedItems.Count > 0);

            x48ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(48, 48));
            x96ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(96, 96));
            x120ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(120, 120));
            x150ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(150, 150));
            x200ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(200, 200));

            rotateCCWToolStripButton.Enabled = (imageListView1.SelectedItems.Count > 0);
            rotateCWToolStripButton.Enabled = (imageListView1.SelectedItems.Count > 0);
        }
        #endregion

        #region Add/Remove Items
        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            string folder = Properties.Settings.Default.LastFolder;
            if (Directory.Exists(folder))
                openFileDialog.InitialDirectory = folder;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                folder = Path.GetDirectoryName(openFileDialog.FileName);
                Properties.Settings.Default.LastFolder = folder;
                Properties.Settings.Default.Save();
                imageListView1.Items.AddRange(openFileDialog.FileNames);
            }
        }

        private void removeToolStripButton_Click(object sender, EventArgs e)
        {
            // Suspend the layout logic while we are removing items.
            // Otherwise the control will be refreshed after each item
            // is removed.
            imageListView1.SuspendLayout();

            // Remove selected items
            foreach (var item in imageListView1.SelectedItems)
                imageListView1.Items.Remove(item);

            // Resume layout logic.
            imageListView1.ResumeLayout(true);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeToolStripButton_Click(sender, e);
        }

        private void removeAllToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.Items.Clear();
        }
        #endregion

        #region Switch Renderers
        private struct RendererItem
        {
            public Type Type;

            public override string ToString()
            {
                return Type.Name;
            }

            public RendererItem(Type type)
            {
                Type = type;
            }
        }

        private void renderertoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change the renderer
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            RendererItem item = (RendererItem)renderertoolStripComboBox.SelectedItem;
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(item.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }
        #endregion

        #region Switch View Modes
        private void detailsToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Details;
        }

        private void thumbnailsToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Thumbnails;
        }

        private void galleryToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Gallery;
        }

        private void paneToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Pane;
        }
        #endregion

        #region Update Status Bar
        private void imageListView1_ThumbnailCached(object sender, ThumbnailCachedEventArgs e)
        {
            // This event is fired after a new thumbnail is cached.
            UpdateStatus(string.Format("Cached image: {0}", e.Item.Text));
            timerStatus.Enabled = true;
        }

        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
            timerStatus.Enabled = false;
        }

        private void UpdateStatus(string text)
        {
            toolStripStatusLabel.Text = text;
        }

        private void UpdateStatus()
        {
            if (imageListView1.Items.Count == 0)
                UpdateStatus("Ready");
            else if (imageListView1.SelectedItems.Count == 0)
                UpdateStatus(string.Format("{0} images", imageListView1.Items.Count));
            else
                UpdateStatus(string.Format("{0} images ({1} selected)", imageListView1.Items.Count, imageListView1.SelectedItems.Count));
        }
        #endregion

        #region Modify Column Headers
        private void columnsToolStripButton_Click(object sender, EventArgs e)
        {
            ChooseColumns form = new ChooseColumns();
            form.imageListView = imageListView1;
            form.ShowDialog();
        }
        #endregion

        #region Change Thumbnail Size
        private void x48ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(48, 48);
        }

        private void x96ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(96, 96);
        }

        private void x120ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(120, 120);
        }

        private void x150ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(150, 150);
        }

        private void x200ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(200, 200);
        }

        private void clearCacheToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.ClearThumbnailCache();
        }
        #endregion

        #region Rotate Selected Images
        private void rotateCCWToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
                "ImageListViewDemo", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    item.BeginEdit();
                    using (Image img = Image.FromFile(item.FileName))
                    {
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        img.Save(item.FileName);
                    }
                    item.Update();
                    item.EndEdit();
                }
            }
        }

        private void rotateCWToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
                "ImageListViewDemo", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    item.BeginEdit();
                    using (Image img = Image.FromFile(item.FileName))
                    {
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        img.Save(item.FileName);
                    }
                    item.Update();
                    item.EndEdit();
                }
            }
        }
        #endregion
    }
}
