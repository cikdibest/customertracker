using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CustomerTracker.Web.Utilities.Helpers
{
    public static class HelperImage
    {
         
        public static void SaveImage(Bitmap img, string imgName, string path, int lnWidth, ImageFormat imageFormat)
        {
            CreateThumbnail(img, lnWidth).Save(path + imgName, imageFormat);
        }

        public static Bitmap GetImage(Stream imageStream)
        {
            return new Bitmap(imageStream);
        }

        public static ImageFormat GetImageFormat(string extension)
        { 
            switch (extension.ToLower())
            {
                case @".bmp":
                    return ImageFormat.Bmp;

                case @".gif":
                    return ImageFormat.Gif;

                case @".ico":
                    return ImageFormat.Icon;

                case @".jpg":
                case @".jpeg":
                    return ImageFormat.Jpeg;

                case @".png":
                    return ImageFormat.Png;

                case @".tif":
                case @".tiff":
                    return ImageFormat.Tiff;

                case @".wmf":
                    return ImageFormat.Wmf;

                default:
                    throw new NotImplementedException();
            }
        }
           
        private static Bitmap CreateThumbnail(Bitmap img, int lnWidth)
        {
            Bitmap bmpOut = null;

            ImageFormat loFormat = img.RawFormat;

            decimal lnRatio;

            int lnNewWidth = 0;

            int lnNewHeight = 0;

            if (img.Width < lnWidth)
                return img;

            lnRatio = (decimal)lnWidth / img.Width;

            lnNewWidth = lnWidth;

            decimal lnTemp = img.Height * lnRatio;

            lnNewHeight = (int)lnTemp;


            bmpOut = new Bitmap(lnNewWidth, lnNewHeight);

            Graphics g = Graphics.FromImage(bmpOut);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);

            g.DrawImage(img, 0, 0, lnNewWidth, lnNewHeight);

            return bmpOut;
        }
       
    }
}
