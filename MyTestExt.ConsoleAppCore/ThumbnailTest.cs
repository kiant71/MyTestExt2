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
            // todo.都小于则原样输出

            var sourceFullName = @"D:\0.Work\Thumb\kai.jpg";
            var targetFullUrl = @"D:\0.Work\Thumb\kai_thu600x800_40.jpg";
            var souceImage = Image.FromFile(sourceFullName);
            var rectangle = GetThumbSize4(souceImage.Width, souceImage.Height, 600, 800);
            
            var targetImage = new Bitmap(600, 800);
            using var gr = Graphics.FromImage(targetImage);
            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gr.DrawImage(souceImage, rectangle);

            targetImage.Save(targetFullUrl, ImageFormat.Jpeg);
        }

        /// <summary>
        /// 40: 生成图像指定宽高，图像裁剪后保持比例（裁剪了图片的一部分，微博、朋友圈常用）
        /// </summary>
        public Rectangle GetThumbSize4(int srcWidth, int srcHeight, int tgtWidth, int tgtHeight)
        {
            var size = new Size(tgtWidth, tgtHeight);
            var srcScale = (double) srcHeight / srcWidth; // 原始高宽比
            var dstScale = (double) tgtHeight / tgtWidth; // 目标高宽比

            var x = 0;
            var y = 0;
            

            // 过高 || 过宽
            return srcScale >= dstScale
                ? new Rectangle(new Point(0, (srcHeight - (int) (srcWidth * dstScale)) / 2), size)
                : new Rectangle(new Point((srcWidth - (int) (srcHeight * dstScale)) / 2, 0), size);
        }



    }
}
