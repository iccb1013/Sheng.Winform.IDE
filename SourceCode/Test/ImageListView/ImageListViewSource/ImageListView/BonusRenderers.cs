// ImageListView - A listview control for image files
// Copyright (C) 2009 Ozgur Ozcitak
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Ozgur Ozcitak (ozcitak@yahoo.com)

#if BONUSPACK
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents the built-in renderers.
    /// </summary>
    public static partial class ImageListViewRenderers
    {
        #region NewYear2010Renderer
        /// <summary>
        /// A renderer to celebrate the new year of 2010.
        /// </summary>
        /// <remarks>Compile with conditional compilation symbol BONUSPACK to
        /// include this renderer in the assembly.</remarks>
        public class NewYear2010Renderer : ImageListView.ImageListViewRenderer
        {
            /// <summary>
            /// Represents a snow flake
            /// </summary>
            private class SnowFlake
            {
                /// <summary>
                /// Gets or sets the client coordinates of the snow flake.
                /// </summary>
                public Point Location { get; set; }
                /// <summary>
                /// Gets or sets the rotation angle of the snow flake in degrees.
                /// </summary>
                public double Rotation { get; set; }
                /// <summary>
                /// Gets or sets the size of the snow flake.
                /// </summary>
                public int Size { get; set; }

                /// <summary>
                /// Initializes a new instance of the SnowFlake class.
                /// </summary>
                /// <param name="newSize">The size of the snow flake.</param>
                public SnowFlake(int newSize)
                {
                    Size = newSize;
                    Location = new Point(0, 0);
                    Rotation = 0.0;
                }
            }

            private readonly object lockObject = new object();

            private int maxFlakeCount = 100;
            private int minFlakeSize = 4;
            private int maxFlakeSize = 12;
            private int flakeGeneration = 3;
            private int currentGeneration = 0;
            private long refreshPeriod = 50;
            private int fallSpeed = 3;
            private DateTime lastRedraw = DateTime.Now;
            private volatile bool inTimer = false;

            private List<SnowFlake> flakes = null;
            private System.Threading.Timer timer;
            private Random random = new Random();

            private Rectangle displayBounds = Rectangle.Empty;

            private GraphicsPath flake;
            private GraphicsPath terrain;

            /// <summary>
            /// Initializes a new instance of the NewYear2010Renderer class.
            /// </summary>
            public NewYear2010Renderer()
            {
                flake = CreateFlake(10, 3);
                terrain = CreateTerrain();
                timer = new System.Threading.Timer(UpdateTimerCallback, null, 0, refreshPeriod);
            }

            /// <summary>
            /// Generates a snowflake from a Koch curve.
            /// http://en.wikipedia.org/wiki/Koch_snowflake
            /// </summary>
            /// <param name="size">The size of the snow flake.</param>
            /// <param name="iterations">Number of iterations. Higher values 
            /// produce more complex curves.</param>
            private GraphicsPath CreateFlake(int size, int iterations)
            {
                Queue<PointF> segments = new Queue<PointF>();
                float h = (float)Math.Sin(Math.PI / 3.0) * (float)size;
                PointF v1 = new PointF(-1.0f * (float)size / 2.0f, -h / 3.0f);
                PointF v2 = new PointF((float)size / 2f, -h / 3.0f);
                PointF v3 = new PointF(0.0f, h * 2.0f / 3.0f);
                segments.Enqueue(v1); segments.Enqueue(v2);
                segments.Enqueue(v2); segments.Enqueue(v3);
                segments.Enqueue(v3); segments.Enqueue(v1);

                for (int k = 0; k < iterations - 1; k++)
                {
                    int todivide = segments.Count / 2;
                    for (int i = 0; i < todivide; i++)
                    {
                        PointF p1 = segments.Dequeue();
                        PointF p2 = segments.Dequeue();

                        // Trisect the segment
                        PointF pi1 = new PointF((p2.X - p1.X) / 3.0f + p1.X,
                            (p2.Y - p1.Y) / 3.0f + p1.Y);
                        PointF pi2 = new PointF((p2.X - p1.X) * 2.0f / 3.0f + p1.X,
                            (p2.Y - p1.Y) * 2.0f / 3.0f + p1.Y);
                        double dist = Math.Sqrt((pi1.X - pi2.X) * (pi1.X - pi2.X) + (pi1.Y - pi2.Y) * (pi1.Y - pi2.Y));
                        double angle = Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) - Math.PI / 3.0;
                        PointF pn = new PointF(pi1.X + (float)(Math.Cos(angle) * dist),
                            pi1.Y + (float)(Math.Sin(angle) * dist));

                        segments.Enqueue(p1); segments.Enqueue(pi1);
                        segments.Enqueue(pi1); segments.Enqueue(pn);
                        segments.Enqueue(pn); segments.Enqueue(pi2);
                        segments.Enqueue(pi2); segments.Enqueue(p2);
                    }
                }

                GraphicsPath path = new GraphicsPath();
                while (segments.Count != 0)
                {
                    PointF p1 = segments.Dequeue();
                    PointF p2 = segments.Dequeue();
                    path.AddLine(p1, p2);
                }

                path.CloseFigure();
                return path;
            }

            /// <summary>
            /// Generates a random snowy terrain.
            /// </summary>
            private GraphicsPath CreateTerrain()
            {
                Random rnd = new Random();
                GraphicsPath path = new GraphicsPath();
                int width = 100;
                int height = 10;

                int count = 20;
                int step = width / count;
                int lastx = 0, lasty = 0;
                Point[] points = new Point[count];
                for (int i = 0; i < count; i++)
                {
                    int x = i * (width + 2 * step) / count - step;
                    int y = rnd.Next(-height / 2, height / 2);
                    points[i] = new Point(x, y);
                    lastx = x;
                    lasty = y;
                }
                path.AddCurve(points);

                path.AddLine(lastx, lasty, width + step, 0);
                path.AddLine(width + step, 0, width + step, 200);
                path.AddLine(width + step, 200, -step, 200);
                path.CloseFigure();

                return path;
            }

            /// <summary>
            /// Updates snow flakes at each timer tick.
            /// </summary>
            /// <param name="state">Not used, null.</param>
            private void UpdateTimerCallback(object state)
            {
                if (inTimer) return;
                inTimer = true;

                bool redraw = false;

                lock (lockObject)
                {
                    if (displayBounds.IsEmpty)
                    {
                        inTimer = false;
                        return;
                    }

                    if (flakes == null)
                        flakes = new List<SnowFlake>();

                    // Add new snow flakes
                    currentGeneration++;
                    if (currentGeneration == flakeGeneration)
                    {
                        currentGeneration = 0;
                        if (flakes.Count < maxFlakeCount)
                        {
                            SnowFlake snowFlake = new SnowFlake(random.Next(minFlakeSize, maxFlakeSize));
                            snowFlake.Rotation = 360.0 * random.NextDouble();
                            snowFlake.Location = new Point(random.Next(displayBounds.Left, displayBounds.Right), -20);
                            flakes.Add(snowFlake);
                        }
                    }

                    // Move snow flakes
                    for (int i = flakes.Count - 1; i >= 0; i--)
                    {
                        SnowFlake snowFlake = flakes[i];
                        if (snowFlake.Location.Y > displayBounds.Height)
                            flakes.Remove(snowFlake);
                        else
                        {
                            snowFlake.Location = new Point(snowFlake.Location.X, snowFlake.Location.Y + snowFlake.Size * fallSpeed / 10);
                            snowFlake.Rotation += 360.0 / 40.0;
                            if (snowFlake.Rotation > 360.0) snowFlake.Rotation -= 360.0;
                        }
                    }

                    // Do we need a refresh?
                    if ((DateTime.Now - lastRedraw).Milliseconds > refreshPeriod)
                        redraw = true;
                }

                if (redraw)
                {
                    try
                    {
                        if (ImageListView.InvokeRequired)
                            ImageListView.Invoke((MethodInvoker)delegate { ImageListView.Refresh(); });
                        else
                            ImageListView.Refresh();
                    }
                    catch
                    {
                        ;
                    }
                }

                inTimer = false;
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
            /// Sets the layout of the control.
            /// </summary>
            /// <param name="e">A LayoutEventArgs that contains event data.</param>
            public override void OnLayout(LayoutEventArgs e)
            {
                base.OnLayout(e);
                lock (lockObject)
                {
                    displayBounds = ImageListView.DisplayRectangle;
                }
            }

            /// <summary>
            /// Draws an overlay image over the client area.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="bounds">The bounding rectangle of the client area.</param>
            public override void DrawOverlay(Graphics g, Rectangle bounds)
            {
                lock (lockObject)
                {
                    lastRedraw = DateTime.Now;
                }

                // Draw the terrain
                DrawTerrain(g);

                lock (lockObject)
                {
                    // Draw the snow flakes
                    if (flakes != null)
                    {
                        foreach (SnowFlake snowFlake in flakes)
                            DrawSnowFlake(g, snowFlake);
                    }
                }

                // Redraw some of the terrain over slow flakes
                DrawTerrainOutline(g);
            }

            /// <summary>
            /// Draws a snow flake.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="snowFlake">The snowflake to draw.</param>
            private void DrawSnowFlake(Graphics g, SnowFlake snowFlake)
            {
                g.ResetTransform();
                // Tranform to upper left corner before rotating.
                // This produces a nice wobbling effect.
                g.TranslateTransform(-snowFlake.Size / 2, -snowFlake.Size / 2, MatrixOrder.Append);
                g.ScaleTransform((float)snowFlake.Size / 6.0f, (float)snowFlake.Size / 6.0f);
                g.RotateTransform((float)snowFlake.Rotation, MatrixOrder.Append);
                g.TranslateTransform(snowFlake.Location.X, snowFlake.Location.Y, MatrixOrder.Append);
                using (SolidBrush brush = new SolidBrush(Color.White))
                using (Pen pen = new Pen(Color.Gray))
                using (Pen glowPen = new Pen(Color.FromArgb(96, Color.White), 2.0f))
                {
                    g.DrawPath(glowPen, flake);
                    g.FillPath(brush, flake);
                    g.DrawPath(pen, flake);
                }

                g.ResetTransform();
            }

            /// <summary>
            /// Draws the terrain.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            private void DrawTerrain(Graphics g)
            {
                g.ResetTransform();
                using (SolidBrush brush = new SolidBrush(Color.White))
                using (Pen pen = new Pen(Color.Gray))
                {
                    Rectangle rec = ImageListView.DisplayRectangle;
                    g.ScaleTransform((float)rec.Width / 100.0f, 3.0f, MatrixOrder.Append);
                    g.TranslateTransform(0, rec.Height - 30, MatrixOrder.Append);
                    g.FillPath(brush, terrain);
                    g.DrawPath(pen, terrain);
                }
                g.ResetTransform();
            }

            /// <summary>
            /// Draws the terrain outline.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            private void DrawTerrainOutline(Graphics g)
            {
                g.ResetTransform();
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    Rectangle rec = ImageListView.DisplayRectangle;
                    g.ScaleTransform((float)rec.Width / 100.0f, 3.0f, MatrixOrder.Append);
                    g.TranslateTransform(0, rec.Height - 20, MatrixOrder.Append);
                    g.FillPath(brush, terrain);
                }
                g.ResetTransform();
            }

            /// <summary>
            /// Releases managed resources.
            /// </summary>
            public override void Dispose()
            {
                base.Dispose();

                flake.Dispose();
                terrain.Dispose();
                timer.Dispose();
            }
        }
        #endregion
    }
}
#endif