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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing.Imaging;
using System.Text;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Contains utility functions.
    /// </summary>
    public static class Utility
    {
        #region Platform Invoke
        // GetFileAttributesEx
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetFileAttributesEx(string lpFileName,
            GET_FILEEX_INFO_LEVELS fInfoLevelId,
            out WIN32_FILE_ATTRIBUTE_DATA fileData);

        private enum GET_FILEEX_INFO_LEVELS
        {
            GetFileExInfoStandard,
            GetFileExMaxInfoLevel
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct WIN32_FILE_ATTRIBUTE_DATA
        {
            public FileAttributes dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;

            public DateTime Value
            {
                get
                {
                    long longTime = (((long)dwHighDateTime) << 32) | ((uint)dwLowDateTime);
                    return DateTime.FromFileTimeUtc(longTime);
                }
            }
        }
        // SHGetFileInfo
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, FileAttributes dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_TYPE)]
            public string szTypeName;
        };
        private const int MAX_PATH = 260;
        private const int MAX_TYPE = 80;
        [Flags]
        private enum SHGFI : uint
        {
            Icon = 0x000000100,
            DisplayName = 0x000000200,
            TypeName = 0x000000400,
            Attributes = 0x000000800,
            IconLocation = 0x000001000,
            ExeType = 0x000002000,
            SysIconIndex = 0x000004000,
            LinkOverlay = 0x000008000,
            Selected = 0x000010000,
            Attr_Specified = 0x000020000,
            LargeIcon = 0x000000000,
            SmallIcon = 0x000000001,
            OpenIcon = 0x000000002,
            ShellIconSize = 0x000000004,
            PIDL = 0x000000008,
            UseFileAttributes = 0x000000010,
            AddOverlays = 0x000000020,
            OverlayIndex = 0x000000040,
        }
        #endregion

        #region Text Utilities
        /// <summary>
        /// Formats the given file size as a human readable string.
        /// </summary>
        /// <param name="size">File size in bytes.</param>
        public static string FormatSize(long size)
        {
            double mod = 1024;
            double sized = size;

            // string[] units = new string[] { "B", "KiB", "MiB", "GiB", "TiB", "PiB" };
            string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            int i;
            for (i = 0; sized > mod; i++)
            {
                sized /= mod;
            }

            return string.Format("{0} {1}", System.Math.Round(sized, 2), units[i]);
        }
        #endregion

        #region Exif Tag IDs
        /// <summary>
        /// Represents the Exif tag for thumbnail data.
        /// </summary>
        private const int PropertyTagThumbnailData = 0x501B;
        /// <summary>
        /// Represents the Exif tag for thumbnail image width.
        /// </summary>
        private const int PropertyTagThumbnailImageWidth = 0x5020;
        /// <summary>
        /// Represents the Exif tag for thumbnail image height.
        /// </summary>
        private const int PropertyTagThumbnailImageHeight = 0x5021;
        /// <summary>
        /// Represents the Exif tag for  inage description.
        /// </summary>
        private const int PropertyTagImageDescription = 0x010E;
        /// <summary>
        /// Represents the Exif tag for the equipment model.
        /// </summary>
        private const int PropertyTagEquipmentModel = 0x0110;
        /// <summary>
        /// Represents the Exif tag for date and time the picture 
        /// was taken.
        /// </summary>        
        private const int PropertyTagDateTime = 0x0132;
        /// <summary>
        /// Represents the Exif tag for the artist.
        /// </summary>
        private const int PropertyTagArtist = 0x013B;
        /// <summary>
        /// Represents the Exif tag for copyright information.
        /// </summary>
        private const int PropertyTagCopyright = 0x8298;
        /// <summary>
        /// Represents the Exif tag for exposure time.
        /// </summary>
        private const int PropertyTagExposureTime = 0x829A;
        /// <summary>
        /// Represents the Exif tag for F-Number.
        /// </summary>
        private const int PropertyTagFNumber = 0x829D;
        /// <summary>
        /// Represents the Exif tag for ISO speed.
        /// </summary>
        private const int PropertyTagISOSpeed = 0x8827;
        /// <summary>
        /// Represents the Exif tag for shutter speed.
        /// </summary>
        private const int PropertyTagShutterSpeed = 0x9201;
        /// <summary>
        /// Represents the Exif tag for aperture value.
        /// </summary>
        private const int PropertyTagAperture = 0x9202;
        /// <summary>
        /// Represents the Exif tag for user comments.
        /// </summary>
        private const int PropertyTagUserComment = 0x9286;
        #endregion

        #region Shell Utilities
        /// <summary>
        /// A utility class combining FileInfo with SHGetFileInfo for image files.
        /// </summary>
        internal class ShellImageFileInfo
        {
            private static Dictionary<string, string> cachedFileTypes;
            private uint structSize = 0;

            public bool Error { get; private set; }
            public FileAttributes FileAttributes { get; private set; }
            public DateTime CreationTime { get; private set; }
            public DateTime LastAccessTime { get; private set; }
            public DateTime LastWriteTime { get; private set; }
            public string Extension { get; private set; }
            public string DirectoryName { get; private set; }
            public string DisplayName { get; private set; }
            public long Size { get; private set; }
            public string TypeName { get; private set; }
            public Size Dimensions { get; private set; }
            public SizeF Resolution { get; private set; }
            // Exif tags
            public string ImageDescription { get; private set; }
            public string EquipmentModel { get; private set; }
            public DateTime DateTaken { get; private set; }
            public string Artist { get; private set; }
            public string Copyright { get; private set; }
            public string ExposureTime { get; private set; }
            public float FNumber { get; private set; }
            public ushort ISOSpeed { get; private set; }
            public string ShutterSpeed { get; private set; }
            public string ApertureValue { get; private set; }
            public string UserComment { get; private set; }

            public ShellImageFileInfo(string path)
            {
                if (cachedFileTypes == null)
                    cachedFileTypes = new Dictionary<string, string>();

                try
                {
                    FileInfo info = new FileInfo(path);
                    FileAttributes = info.Attributes;
                    CreationTime = info.CreationTime;
                    LastAccessTime = info.LastAccessTime;
                    LastWriteTime = info.LastWriteTime;
                    Size = info.Length;
                    DirectoryName = info.DirectoryName;
                    DisplayName = info.Name;
                    Extension = info.Extension;

                    string typeName;
                    if (!cachedFileTypes.TryGetValue(Extension, out typeName))
                    {
                        SHFILEINFO shinfo = new SHFILEINFO();
                        if (structSize == 0) structSize = (uint)Marshal.SizeOf(shinfo);
                        SHGetFileInfo(path, (FileAttributes)0, out shinfo, structSize, SHGFI.TypeName);
                        typeName = shinfo.szTypeName;
                        cachedFileTypes.Add(Extension, typeName);
                    }
                    TypeName = typeName;
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (Image img = Image.FromStream(stream, false, false))
                        {
                            Dimensions = img.Size;
                            Resolution = new SizeF(img.HorizontalResolution, img.VerticalResolution);
                            // Read exif properties
                            foreach (PropertyItem prop in img.PropertyItems)
                            {
                                switch (prop.Id)
                                {
                                    case PropertyTagImageDescription:
                                        ImageDescription = ReadExifAscii(prop.Value);
                                        break;
                                    case PropertyTagEquipmentModel:
                                        EquipmentModel = ReadExifAscii(prop.Value);
                                        break;
                                    case PropertyTagDateTime:
                                        DateTaken = ReadExifDateTime(prop.Value);
                                        break;
                                    case PropertyTagArtist:
                                        Artist = ReadExifAscii(prop.Value);
                                        break;
                                    case PropertyTagCopyright:
                                        Copyright = ReadExifAscii(prop.Value);
                                        break;
                                    case PropertyTagExposureTime:
                                        ExposureTime = ReadExifURational(prop.Value);
                                        break;
                                    case PropertyTagFNumber:
                                        FNumber = ReadExifFloat(prop.Value);
                                        break;
                                    case PropertyTagISOSpeed:
                                        ISOSpeed = ReadExifUShort(prop.Value);
                                        break;
                                    case PropertyTagShutterSpeed:
                                        ShutterSpeed = ReadExifRational(prop.Value);
                                        break;
                                    case PropertyTagAperture:
                                        ApertureValue = ReadExifURational(prop.Value);
                                        break;
                                    case PropertyTagUserComment:
                                        UserComment = ReadExifAscii(prop.Value);
                                        break;
                                }
                            }
                        }
                    }
                    Error = false;
                }
                catch
                {
                    Error = true;
                }
            }
        }
        // Convert Exif types
        private static byte ReadExifByte(byte[] value)
        {
            return value[0];
        }
        private static string ReadExifAscii(byte[] value)
        {
            int len = Array.IndexOf(value, (byte)0);
            if (len == -1) len = value.Length;
            return Encoding.ASCII.GetString(value, 0, len);
        }
        private static DateTime ReadExifDateTime(byte[] value)
        {
            return DateTime.ParseExact(ReadExifAscii(value),
                "yyyy:MM:dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture);
        }
        private static ushort ReadExifUShort(byte[] value)
        {
            return BitConverter.ToUInt16(value, 0);
        }
        private static uint ReadExifUInt(byte[] value)
        {
            return BitConverter.ToUInt32(value, 0);
        }
        private static int ReadExifInt(byte[] value)
        {
            return BitConverter.ToInt32(value, 0);
        }
        private static string ReadExifURational(byte[] value)
        {
            return BitConverter.ToUInt32(value, 0).ToString() + "/" +
                    BitConverter.ToUInt32(value, 4).ToString();
        }
        private static string ReadExifRational(byte[] value)
        {
            return BitConverter.ToInt32(value, 0).ToString() + "/" +
                    BitConverter.ToInt32(value, 4).ToString();
        }
        private static float ReadExifFloat(byte[] value)
        {
            uint num = BitConverter.ToUInt32(value, 0);
            uint den = BitConverter.ToUInt32(value, 4);
            if (den == 0)
                return 0.0f;
            else
                return (float)num / (float)den;
        }
        #endregion

        #region Graphics Utilities
        /// <summary>
        /// Draws the given caption and text inside the given rectangle.
        /// </summary>
        internal static int DrawStringPair(Graphics g, Rectangle r, string caption, string text, Font font, Brush captionBrush, Brush textBrush)
        {
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.NoWrap;

                SizeF szc = g.MeasureString(caption, font, r.Size, sf);
                int y = (int)szc.Height;
                if (szc.Width > r.Width) szc.Width = r.Width;
                Rectangle txrect = new Rectangle(r.Location, Size.Ceiling(szc));
                g.DrawString(caption, font, captionBrush, txrect, sf);
                txrect.X += txrect.Width;
                txrect.Width = r.Width;
                if (txrect.X < r.Right)
                {
                    SizeF szt = g.MeasureString(text, font, r.Size, sf);
                    y = Math.Max(y, (int)szt.Height);
                    txrect = Rectangle.Intersect(r, txrect);
                    g.DrawString(text, font, textBrush, txrect, sf);
                }

                return y;
            }
        }
        /// <summary>
        /// Creates a thumbnail from the given image.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="size">Requested image size.</param>
        /// <param name="backColor">Background color of returned thumbnail.</param>
        /// <returns>The image from the given file or null if an error occurs.</returns>
        internal static Image ThumbnailFromImage(Image image, Size size, Color backColor)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentException();

            Image thumb = null;
            try
            {
                Size scaled = GetSizedImageBounds(image, size);
                thumb = new Bitmap(scaled.Width, scaled.Height);
                using (Graphics g = Graphics.FromImage(thumb))
                {
                    g.PixelOffsetMode = PixelOffsetMode.None;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    using (Brush brush = new SolidBrush(backColor))
                    {
                        g.FillRectangle(Brushes.White, 0, 0, scaled.Width, scaled.Height);
                    }

                    g.DrawImage(image, 0, 0, scaled.Width, scaled.Height);
                }
            }
            catch
            {
                if (thumb != null)
                    thumb.Dispose();
                thumb = null;
            }

            return thumb;
        }
        /// <summary>
        /// Creates a thumbnail from the given image file.
        /// </summary>
        /// <param name="filename">The filename pointing to an image.</param>
        /// <param name="size">Requested image size.</param>
        /// <param name="useEmbeddedThumbnails">Embedded thumbnail usage.</param>
        /// <param name="backColor">Background color of returned thumbnail.</param>
        /// <returns>The image from the given file or null if an error occurs.</returns>
        internal static Image ThumbnailFromFile(string filename, Size size, UseEmbeddedThumbnails useEmbeddedThumbnails, Color backColor)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentException();

            Image source = null;
            Image thumb = null;

            // Try to read the exif thumbnail
            if (useEmbeddedThumbnails != UseEmbeddedThumbnails.Never)
            {
                try
                {
                    using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        using (Image img = Image.FromStream(stream, false, false))
                        {
                            foreach (int index in img.PropertyIdList)
                            {
                                if (index == PropertyTagThumbnailData)
                                {
                                    // Fetch the embedded thumbnail
                                    byte[] rawImage = img.GetPropertyItem(PropertyTagThumbnailData).Value;
                                    using (MemoryStream memStream = new MemoryStream(rawImage))
                                    {
                                        source = Image.FromStream(memStream);
                                    }
                                    if (useEmbeddedThumbnails == UseEmbeddedThumbnails.Auto)
                                    {
                                        // Check that the embedded thumbnail is large enough.
                                        if (Math.Max((float)source.Width / (float)size.Width,
                                            (float)source.Height / (float)size.Height) < 1.0f)
                                        {
                                            source.Dispose();
                                            source = null;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    if (source != null)
                        source.Dispose();
                    source = null;
                }
            }

            // Fix for the missing semicolon in GIF files
            MemoryStream streamCopy = null;
            try
            {
                if (source == null)
                {
                    using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        byte[] gifSignature = new byte[6];
                        stream.Read(gifSignature, 0, 6);
                        if (Encoding.ASCII.GetString(gifSignature) == "GIF89a")
                        {
                            stream.Seek(0, SeekOrigin.Begin);
                            streamCopy = new MemoryStream();
                            byte[] buffer = new byte[32768];
                            int read = 0;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                streamCopy.Write(buffer, 0, read);
                            }
                            // Append the mising semicolon
                            streamCopy.Seek(-1, SeekOrigin.End);
                            if (streamCopy.ReadByte() != 0x3b)
                                streamCopy.WriteByte(0x3b);
                            source = Image.FromStream(streamCopy);
                        }
                    }
                }
            }
            catch
            {
                if (source != null)
                    source.Dispose();
                source = null;
                if (streamCopy != null)
                    streamCopy.Dispose();
                streamCopy = null;
            }

            // Revert to source image if an embedded thumbnail of required size
            // was not found.
            if (source == null)
            {
                try
                {
                    source = Image.FromFile(filename);
                }
                catch
                {
                    if (source != null)
                        source.Dispose();
                    source = null;
                }
            }

            // If all failed, return null.
            if (source == null) return null;

            // Create the thumbnail
            try
            {
                Size scaled = GetSizedImageBounds(source, size);
                thumb = new Bitmap(source, scaled.Width, scaled.Height);
                using (Graphics g = Graphics.FromImage(thumb))
                {
                    g.PixelOffsetMode = PixelOffsetMode.None;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.Clear(backColor);
                    g.DrawImage(source, 0, 0, scaled.Width, scaled.Height);
                }
            }
            catch
            {
                if (thumb != null)
                    thumb.Dispose();
                thumb = null;
            }
            finally
            {
                if (source != null)
                    source.Dispose();
                source = null;
                if (streamCopy != null)
                    streamCopy.Dispose();
                streamCopy = null;
            }

            return thumb;
        }
        /// <summary>
        /// Gets the scaled size of an image required to fit
        /// in to the given size keeping the image aspect ratio.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="fit">The size to fit in to.</param>
        /// <returns></returns>
        internal static Size GetSizedImageBounds(Image image, Size fit)
        {
            float f = System.Math.Max((float)image.Width / (float)fit.Width, (float)image.Height / (float)fit.Height);
            if (f < 1.0f) f = 1.0f; // Do not upsize small images
            int width = (int)System.Math.Round((float)image.Width / f);
            int height = (int)System.Math.Round((float)image.Height / f);
            return new Size(width, height);
        }
        /// <summary>
        /// Gets the bounding rectangle of an image required to fit
        /// in to the given rectangle keeping the image aspect ratio.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="fit">The rectangle to fit in to.</param>
        /// <param name="hAlign">Horizontal image aligment in percent.</param>
        /// <param name="vAlign">Vertical image aligment in percent.</param>
        /// <returns></returns>
        internal static Rectangle GetSizedImageBounds(Image image, Rectangle fit, float hAlign, float vAlign)
        {
            Size scaled = GetSizedImageBounds(image, fit.Size);
            int x = fit.Left + (int)(hAlign / 100.0f * (float)(fit.Width - scaled.Width));
            int y = fit.Top + (int)(vAlign / 100.0f * (float)(fit.Height - scaled.Height));

            return new Rectangle(x, y, scaled.Width, scaled.Height);
        }
        /// <summary>
        /// Gets the bounding rectangle of an image required to fit
        /// in to the given rectangle keeping the image aspect ratio.
        /// The image will be centered in the fit box.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="fit">The rectangle to fit in to.</param>
        /// <returns></returns>
        internal static Rectangle GetSizedImageBounds(Image image, Rectangle fit)
        {
            return GetSizedImageBounds(image, fit, 50.0f, 50.0f);
        }
        /// <summary>
        /// Gets a path representing a rounded rectangle.
        /// </summary>
        private static GraphicsPath GetRoundedRectanglePath(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(x + radius, y, x + width - radius, y);
            if (radius > 0)
                path.AddArc(x + width - 2 * radius, y, 2 * radius, 2 * radius, 270.0f, 90.0f);
            path.AddLine(x + width, y + radius, x + width, y + height - radius);
            if (radius > 0)
                path.AddArc(x + width - 2 * radius, y + height - 2 * radius, 2 * radius, 2 * radius, 0.0f, 90.0f);
            path.AddLine(x + width - radius, y + height, x + radius, y + height);
            if (radius > 0)
                path.AddArc(x, y + height - 2 * radius, 2 * radius, 2 * radius, 90.0f, 90.0f);
            path.AddLine(x, y + height - radius, x, y + radius);
            if (radius > 0)
                path.AddArc(x, y, 2 * radius, 2 * radius, 180.0f, 90.0f);
            return path;
        }
        /// <summary>
        /// Fills the interior of a rounded rectangle.
        /// </summary>
        public static void FillRoundedRectangle(System.Drawing.Graphics graphics, Brush brush, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = GetRoundedRectanglePath(x, y, width, height, radius))
            {
                graphics.FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle.
        /// </summary>
        public static void FillRoundedRectangle(System.Drawing.Graphics graphics, Brush brush, float x, float y, float width, float height, float radius)
        {
            FillRoundedRectangle(graphics, brush, (int)x, (int)y, (int)width, (int)height, (int)radius);
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle.
        /// </summary>
        public static void FillRoundedRectangle(System.Drawing.Graphics graphics, Brush brush, Rectangle rect, int radius)
        {
            FillRoundedRectangle(graphics, brush, rect.Left, rect.Top, rect.Width, rect.Height, radius);
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle.
        /// </summary>
        public static void FillRoundedRectangle(System.Drawing.Graphics graphics, Brush brush, RectangleF rect, float radius)
        {
            FillRoundedRectangle(graphics, brush, (int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height, (int)radius);
        }

        /// <summary>
        /// Draws the outline of a rounded rectangle.
        /// </summary>
        public static void DrawRoundedRectangle(System.Drawing.Graphics graphics, Pen pen, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = GetRoundedRectanglePath(x, y, width, height, radius))
            {
                graphics.DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws the outline of a rounded rectangle.
        /// </summary>
        public static void DrawRoundedRectangle(System.Drawing.Graphics graphics, Pen pen, float x, float y, float width, float height, float radius)
        {
            DrawRoundedRectangle(graphics, pen, (int)x, (int)y, (int)width, (int)height, (int)radius);
        }

        /// <summary>
        /// Draws the outline of a rounded rectangle.
        /// </summary>
        public static void DrawRoundedRectangle(System.Drawing.Graphics graphics, Pen pen, Rectangle rect, int radius)
        {
            DrawRoundedRectangle(graphics, pen, rect.Left, rect.Top, rect.Width, rect.Height, radius);
        }

        /// <summary>
        /// Draws the outline of a rounded rectangle.
        /// </summary>
        public static void DrawRoundedRectangle(System.Drawing.Graphics graphics, Pen pen, RectangleF rect, float radius)
        {
            DrawRoundedRectangle(graphics, pen, (int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height, (int)radius);
        }
        #endregion
    }
}
