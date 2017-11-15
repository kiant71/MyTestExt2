using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// 测试案例
    /// </summary>
    public class RoundedImage2
    {
        public static Bitmap Create(params Image[] argsImages)
        {
            Size tgtSize = new Size(170, 170);
            Size iconSize = new Size(60, 60);

            Bitmap dstImage = new Bitmap(tgtSize.Width, tgtSize.Width);
            Graphics g = Graphics.FromImage(dstImage);
            //5张
            //Graphics g = pictureBox1.CreateGraphics();
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var arrRect = new Rectangle[]{new Rectangle(new Point(0,  35), iconSize)
                                        , new Rectangle(new Point(45, 0 ), iconSize)
                                        , new Rectangle(new Point(90, 35), iconSize)
                                        , new Rectangle(new Point(70, 90), iconSize)
                                        , new Rectangle(new Point(20, 90), iconSize) 
                                        };
            Pen spacingPen = new Pen(Color.Red, 15);            
            for (int i = 0; i < arrRect.Length; i++)
            {
                
            }



            //界定点
            var r1 = new Rectangle(new Point(0, 35), iconSize);
            var p1 = new Pen(Color.Red, 10);     
            g.DrawEllipse(p1, r1);
            var r2 = new Rectangle(new Point(45, 0), iconSize);
            var p2 = new Pen(Color.DarkRed, 10);
            g.DrawEllipse(p2, r2);
            var r3 = new Rectangle(new Point(90, 35), iconSize);
            var p3 = new Pen(Color.Black, 10);
            g.DrawEllipse(p3, r3);
            var r4 = new Rectangle(new Point(70, 90), iconSize);
            var p4 = new Pen(Color.Green, 10);
            g.DrawEllipse(p4, r4);
            var r5 = new Rectangle(new Point(20, 90), iconSize);
            var p5 = new Pen(Color.Yellow, 10);
            g.DrawEllipse(p5, r5);


            var ob5 = new Bitmap(tgtSize.Width, tgtSize.Width);
            var image5 = argsImages[4];// Image.FromFile(@"D:\\Image5.jpg");
            Graphics image5g = Graphics.FromImage(ob5);
            image5g.Clear(Color.WhiteSmoke);
            image5g.CompositingQuality = CompositingQuality.HighQuality;
            image5g.SmoothingMode = SmoothingMode.HighQuality;
            image5g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            image5g.DrawImage(image5, r5, 0, 0, image5.Width, image5.Height, GraphicsUnit.Pixel);
            TextureBrush tb5 = new TextureBrush(ob5, WrapMode.Clamp);
            g.FillEllipse(tb5, r5);

            var ob4 = new Bitmap(tgtSize.Width, tgtSize.Width);
            var image4 = argsImages[3];//Image.FromFile(@"D:\\Image4.jpg");
            Graphics image4g = Graphics.FromImage(ob4);
            image4g.Clear(Color.WhiteSmoke);
            image4g.CompositingQuality = CompositingQuality.HighQuality;
            image4g.SmoothingMode = SmoothingMode.HighQuality;
            image4g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            image4g.DrawImage(image4, r4, 0, 0, image4.Width, image4.Height, GraphicsUnit.Pixel);
            TextureBrush tb4 = new TextureBrush(ob4, WrapMode.Clamp);
            g.FillEllipse(tb4, r4);


            var ob3 = new Bitmap(tgtSize.Width, tgtSize.Width);
            var image3 = argsImages[2];//Image.FromFile(@"D:\\Image3.jpg");
            Graphics image3g = Graphics.FromImage(ob3);
            image3g.Clear(Color.WhiteSmoke);
            image3g.CompositingQuality = CompositingQuality.HighQuality;
            image3g.SmoothingMode = SmoothingMode.HighQuality;
            image3g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            image3g.DrawImage(image3, r3, 0, 0, image3.Width, image3.Height, GraphicsUnit.Pixel);
            TextureBrush tb3 = new TextureBrush(ob3, WrapMode.Clamp);
            g.FillEllipse(tb3, r3);

            var ob2 = new Bitmap(tgtSize.Width, tgtSize.Width);
            var image2 = argsImages[1];// Image.FromFile(@"D:\\Image2.jpg");
            Graphics image2g = Graphics.FromImage(ob2);
            image2g.Clear(Color.WhiteSmoke);
            image2g.CompositingQuality = CompositingQuality.HighQuality;
            image2g.SmoothingMode = SmoothingMode.HighQuality;
            image2g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            image2g.DrawImage(image2, r2, 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel);
            TextureBrush tb2 = new TextureBrush(ob2, WrapMode.Clamp);
            g.FillEllipse(tb2, r2);


            var ob1 = new Bitmap(tgtSize.Width, tgtSize.Width);
            var image1 = argsImages[0];//Image.FromFile(@"D:\\Image1.jpg");
            Graphics image1g = Graphics.FromImage(ob1);
            image1g.Clear(Color.WhiteSmoke);
            image1g.CompositingQuality = CompositingQuality.HighQuality;
            image1g.SmoothingMode = SmoothingMode.HighQuality;
            image1g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            image1g.DrawImage(image1, r1, 0, 0, image1.Width, image1.Height, GraphicsUnit.Pixel);
            TextureBrush tb1 = new TextureBrush(ob1, WrapMode.TileFlipX);
            g.FillEllipse(tb1, r1);


            //重绘起始的一半
            Bitmap dstImage2 = new Bitmap(tgtSize.Width, tgtSize.Width);
            Graphics g2 = Graphics.FromImage(dstImage2);
            g2.CompositingQuality = CompositingQuality.HighQuality;
            g2.SmoothingMode = SmoothingMode.HighQuality;
            g2.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g2.DrawEllipse(p1, r1);
            g2.FillEllipse(tb1, r1);
            g2.DrawEllipse(p5, r5);
            g2.FillEllipse(tb5, r5);


            #region 重复代码
            //Bitmap ob0 = new Bitmap(70, 30);
            //Graphics image0g = Graphics.FromImage(ob0);
            //image0g.Clear(Color.WhiteSmoke);
            //image0g.CompositingQuality = CompositingQuality.HighQuality;
            //image0g.SmoothingMode = SmoothingMode.HighQuality;
            //image0g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //image0g.DrawImage(uMap
            //    , new Rectangle(new Point(0, 0 ), new Size(70, 30))
            //    , new Rectangle(new Point(0, 80), new Size(70, 30)), GraphicsUnit.Pixel);
            //TextureBrush tb0 = new TextureBrush(ob0, WrapMode.Clamp);            
            //image0g.FillRectangle(tb0, new Rectangle(new Point(0, 0), new Size(60, 30)));

            ////return ob0;

            //var ob9 = new Bitmap(iconWH.Width, iconWH.Width);
            //Graphics image9g = Graphics.FromImage(ob9);
            //image9g.Clear(Color.WhiteSmoke);
            //image9g.CompositingQuality = CompositingQuality.HighQuality;
            //image9g.SmoothingMode = SmoothingMode.HighQuality;
            //image9g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //image9g.DrawImage(ob0
            //    , new Rectangle(new Point(0, 80), new Size(70, 30))
            //    , new Rectangle(new Point(0, 0 ), new Size(70, 30)), GraphicsUnit.Pixel);
            //TextureBrush tb9 = new TextureBrush(ob9, WrapMode.TileFlipX); 
            #endregion
                      

            //输出截图的部分
            return dstImage2;

            //g.FillEllipse(tb9, new Rectangle(new Point(0, 80), new Size(65, 25)));
            //return destPic;
        }
    }
}
