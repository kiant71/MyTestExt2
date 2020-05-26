using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace MyTestExt.ConsoleAppCore
{
    public class ThumbnailTest
    {
        public void Do()
        {
            //Test();
            Draw_40();
        }

        public void Test()
        {
            var fullUrl = @"D:\0.Work\Thumb\Holding.on.jpg";
            var targetFullUrl = @"D:\0.Work\Thumb\Holding.on_thu200x200.jpg";
            using var image = Image.FromFile(fullUrl);
            using var thumb = image.GetThumbnailImage(200, 200, null, IntPtr.Zero);
            thumb.Save(targetFullUrl);
        }

        // 10: 生成图像指定宽高，不保持比例（可能会产生图片变形）
        public void Draw_40()
        {
            var srcFullName = @"D:\0.Work\Thumb\以宽优先.jpg";
            var dstFullUrl = @"D:\0.Work\Thumb\以宽优先_thu800x600_40.jpg";
            var srcImage = Image.FromFile(srcFullName);
            var rectangle = GetThumbSize4(srcImage.Width, srcImage.Height, 800, 600);
            
            // 判断是否需要裁切
            if (rectangle.X > 0 || rectangle.Y > 0)
            {
                var fromR = new Rectangle(rectangle.X, rectangle.Y, srcImage.Width-rectangle.X-rectangle.X
                    , srcImage.Height-rectangle.Y-rectangle.Y);
                var toR = new Rectangle(0, 0, srcImage.Width, srcImage.Height);
                var pickedImage = new Bitmap(srcImage.Width, srcImage.Height);
                var pickedG = Graphics.FromImage(pickedImage);
                pickedG.DrawImage(srcImage, toR, fromR, GraphicsUnit.Pixel);
                srcImage = (Image) pickedImage.Clone();

                pickedG.Dispose();
                pickedImage.Dispose();
            }
            
            var dstImage = new Bitmap(rectangle.Width, rectangle.Height);
            using var gr = Graphics.FromImage(dstImage);
            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gr.DrawImage(srcImage, 0, 0, rectangle.Width, rectangle.Height);

            dstImage.Save(dstFullUrl, ImageFormat.Jpeg);
        }

        /// <summary>
        /// 40: 生成图像指定宽高，图像裁剪后保持比例（裁剪了图片的一部分，微博、朋友圈常用）
        /// </summary>
        private Rectangle GetThumbSize4(int srcWidth, int srcHeight, int dstWidth, int dstHeight)
        {
            if (srcWidth < dstHeight) dstWidth = srcWidth;
            if (srcHeight < dstHeight) dstHeight = srcHeight;

            var point = new Point(0, 0);
            var size = new Size(dstWidth, dstHeight);
            var heightScale = (double) srcHeight / dstHeight; // 旧新高比例
            var widthScale = (double) srcWidth / dstWidth; // 旧新宽比例

            // 判断原图需要裁切标记
            // 如果都相等，则不用变动
            // 否则选择比例最小(高比/宽比)的为基准，然后照着这个比例裁剪另外(宽/高)。
            if (heightScale < widthScale)
            {
                point.X = (int) ((srcWidth - dstWidth * heightScale) / 2); 
            }
            else if (widthScale < heightScale)
            {
                point.Y = (int) ((srcHeight - dstHeight * widthScale) / 2);
            }

            return new Rectangle(point, size);
        }



    }
}
