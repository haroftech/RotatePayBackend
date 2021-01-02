using System.IO;
using System.Drawing;

namespace Backend.Helpers
{
    public class ResizeImage
    {
        public static Bitmap GetResizedImage(Stream stream, int standardWidth, int standardHeight, int minSizeW, int minSizeH)
        {

            Bitmap newBitmap = null;

            try
            {
                Bitmap oldBitmap = new Bitmap(stream);

                decimal ratio;
                decimal ratio1;

                int newWidth = 0;
                int newHeight = 0;

                if (oldBitmap.Width > oldBitmap.Height)
                {
                    ratio = (decimal)standardWidth / oldBitmap.Width;
                    newWidth = standardWidth;
                    decimal lnTemp = oldBitmap.Height * ratio;
                    newHeight = (int)lnTemp;                    
                    if (newWidth < minSizeW)
                    {
                        ratio1 = minSizeW / newWidth;
                        newWidth = minSizeW;
                        decimal lnTemp1 = newHeight * ratio1;
                        newHeight = (int)lnTemp1;
                    }
                    if (newHeight < minSizeH)
                    {
                        ratio1 = minSizeH / newHeight;
                        newHeight = minSizeH;
                        decimal lnTemp1 = newWidth * ratio1;
                        newWidth = (int)lnTemp1;
                    }
                }
                else
                {
                    ratio = (decimal)standardHeight / oldBitmap.Height;
                    newHeight = standardHeight;
                    decimal lnTemp = oldBitmap.Width * ratio;
                    newWidth = (int)lnTemp;
                    if (newWidth < minSizeW)
                    {
                        ratio1 = minSizeW / newWidth;
                        newWidth = minSizeW;
                        decimal lnTemp1 = newHeight * ratio1;
                        newHeight = (int)lnTemp1;
                    }
                    if (newHeight < minSizeH)
                    {
                        ratio1 = minSizeH / newHeight;
                        newHeight = minSizeH;
                        decimal lnTemp1 = newWidth * ratio1;
                        newWidth = (int)lnTemp1;
                    }
                }

                newBitmap = new Bitmap(newWidth, newHeight);
                Graphics graphics = Graphics.FromImage(newBitmap);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                graphics.DrawImage(oldBitmap, 0, 0, newWidth, newHeight);

                oldBitmap.Dispose();
            }
            catch
            {
                return null;
            }

            return newBitmap;
        }
    }
}
