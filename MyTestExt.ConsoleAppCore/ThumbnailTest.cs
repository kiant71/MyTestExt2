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
            Draw();
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
        public void Draw()
        {
            //var srcFullName = @"D:\0.Work\Thumb\以高优先.jpg";
            var srcFullName = @"D:\0.Work\Thumb\以宽优先.jpg";
            //var srcFullName = @"D:\0.Work\Thumb\基本正方.jpg";
            var dstWidth = 800;
            var dstHeight = 600;
            var type = 40;
            var dstFullUrl = string.Format("{0}._thu{1}x{2}_{3}.jpg", srcFullName, dstWidth, dstHeight, type);
            
            var imgFormat = GetImageFormat(srcFullName);
            
            using var srcImage = Image.FromFile(srcFullName);
            var rectangle = GetClipSize(srcImage.Width, srcImage.Height, dstWidth, dstHeight);
            var x = rectangle.X; // 需要裁切的偏移量
            var y = rectangle.Y; 
            var dstWidth0 = rectangle.Width; // 计算过后的分辨率（可能存在原图小于目标图的情况下）
            var dstHeight0 = rectangle.Height;

            // 判断是否需要裁切
            if (x > 0 || y > 0)
            {
                var destRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);
                var srcRect = new Rectangle(x, y, srcImage.Width - x * 2, srcImage.Height - y * 2);
                using var clipImage = new Bitmap(srcImage.Width, srcImage.Height);
                using var clipGr = Graphics.FromImage(clipImage);
                //clipGr.SmoothingMode = SmoothingMode.HighQuality;
                //clipGr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //clipGr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                clipGr.DrawImage(srcImage, destRect, srcRect, GraphicsUnit.Pixel);
                Draw(clipImage, dstFullUrl, dstWidth0, dstHeight0, imgFormat);
                return;


            //    var srcFullName = @"D:\0.Work\Thumb\以宽优先.jpg";
            //    var dstFullUrl = @"D:\0.Work\Thumb\以宽优先_thu800x600_40.jpg";
            //    var srcImage = Image.FromFile(srcFullName);
            //    var rectangle = GetThumbSize4(srcImage.Width, srcImage.Height, 800, 600);
            
            //    // 判断是否需要裁切
            //    if (rectangle.X > 0 || rectangle.Y > 0)
            //    {
            //        var fromR = new Rectangle(rectangle.X, rectangle.Y, srcImage.Width-rectangle.X-rectangle.X
            //            , srcImage.Height-rectangle.Y-rectangle.Y);
            //        var toR = new Rectangle(0, 0, srcImage.Width, srcImage.Height);
            //        var pickedImage = new Bitmap(srcImage.Width, srcImage.Height);
            //        var pickedG = Graphics.FromImage(pickedImage);
            //        pickedG.DrawImage(srcImage, toR, fromR, GraphicsUnit.Pixel);
            //        srcImage = (Image) pickedImage.Clone();

            //        pickedG.Dispose();
            //        pickedImage.Dispose();
            //    }
            
            //    var dstImage = new Bitmap(rectangle.Width, rectangle.Height);
            //    using var gr = Graphics.FromImage(dstImage);
            //    gr.SmoothingMode = SmoothingMode.HighQuality;
            //    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //    gr.DrawImage(srcImage, 0, 0, rectangle.Width, rectangle.Height);

            //    dstImage.Save(dstFullUrl, ImageFormat.Jpeg);
            //}
            }

            Draw(srcImage, dstFullUrl, dstWidth0, dstHeight0, imgFormat);
        }

        private static ImageFormat GetImageFormat(string srcFullName)
        {
            var extName = Path.GetExtension(srcFullName).ToLower();
            switch (extName)
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".tiff":
                    return ImageFormat.Tiff;
                default:
                    return ImageFormat.Icon;
            }
        }

        private static void Draw(Image srcImage, string dstFullUrl, int dstWidth, int dstHeight
            , ImageFormat imgFormat)
        {
            var dstImage = new Bitmap(dstWidth, dstHeight);
            using var gr = Graphics.FromImage(dstImage);
            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gr.DrawImage(srcImage, 0, 0, dstWidth, dstWidth);
            dstImage.Save(dstFullUrl, imgFormat);
        }

        /// <summary>
        /// 40: 生成图像指定宽高，图像裁剪后保持比例（裁剪了图片的一部分，微博、朋友圈常用）
        /// </summary>
        private Rectangle GetClipSize(int srcWidth, int srcHeight, int dstWidth, int dstHeight)
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
