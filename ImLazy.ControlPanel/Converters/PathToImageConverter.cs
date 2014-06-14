using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Etier.IconHelper;

namespace ImLazy.ControlPanel.Converters
{
    public class PathToImageConverter : StringConverterBase
    {

        private static readonly Dictionary<string, BitmapSource> Cahce =
            new Dictionary<string, BitmapSource>();


        public static BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            return Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        protected override object Convert(string value)
        {
            BitmapSource image;
            if (!Cahce.TryGetValue(value, out image))
            {
                var icon = File.Exists(value) ?
                    IconReader.GetFileIcon(value, IconReader.IconSize.Large, false) :
                    IconReader.GetFolderIcon(IconReader.IconSize.Large, IconReader.FolderType.Closed);

                if (icon == null) return null;
                image = CreateBitmapSourceFromBitmap(icon.ToBitmap());
                Cahce[value] = image;
            }
            return image;
        }
    }
}
