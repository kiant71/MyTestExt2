using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// 矩形头像组（图标不相交）
    /// </summary>
    public class RectangleGroup
    {

        /// <summary>
        /// 图像生成
        /// </summary>
        public static Image Create(dynamic[] paramPic, int destWidth = 80, int destHeight = 80)
        {
            #region 参数初始化
            int spacing = (int)Math.Round(destWidth / 50.0, 0);     //内部填充
            Size iconSize = new Size(destWidth / 2 - spacing, destHeight / 2 - spacing);    //统一图标大小
            Rectangle[] iconRects;      //图标坐标区域            
            switch (paramPic.Length)
            {
                case 1:
                    iconRects = new Rectangle[] { new Rectangle(new Point(0, 0), new Size(destWidth, destHeight)) };
                    break;
                case 2:
                    iconRects = new Rectangle[]{new Rectangle(new Point(0                    , destHeight/4), iconSize)
                                             , new Rectangle(new Point(destWidth/2 +spacing , destHeight/4), iconSize)};
                    break;
                case 3:
                    iconRects = new Rectangle[]{new Rectangle(new Point(destWidth/4          , 0), iconSize)
                                             , new Rectangle(new Point(0                    , destHeight/2 +spacing), iconSize)
                                             , new Rectangle(new Point(destWidth/2 +spacing , destHeight/2 +spacing), iconSize)};
                    break;
                default:
                    iconRects = new Rectangle[]{new Rectangle(new Point(0                    , 0), iconSize)
                                             , new Rectangle(new Point(destWidth/2 +spacing , 0), iconSize)
                                             , new Rectangle(new Point(0                    , destHeight/2 +spacing), iconSize)
                                             , new Rectangle(new Point(destWidth/2 +spacing , destHeight/2 +spacing), iconSize)};
                    break;
            } 
            #endregion

            Bitmap destImg = new Bitmap(destWidth, destHeight);
            using (Graphics g = Graphics.FromImage(destImg))
            {
                g.Clear(Color.WhiteSmoke);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                for (int i = 0; i < iconRects.Length; i++)
                {
                    g.DrawImage(Create(paramPic[i]), iconRects[i]); //在指定位置以指定大小（缩放） 来放置组内图标
                }
                g.Save();
            }

            //矩形图像的圆角化，待后续处理，待提供圆角填充位置的背景色

            return destImg;
        }
        
        /// <summary>
        /// 按比例缩放图像
        /// </summary>
        public static Image Create(byte[] srcData)
        {
            using (MemoryStream ms = new MemoryStream(srcData))
            {
                ms.Seek(0, SeekOrigin.Begin);
                Image srcImg = Bitmap.FromStream(ms);
                return Create(srcImg);
            }
        }

        /// <summary>
        /// 按比例缩放图像
        /// </summary>
        public static Image Create(Image srcImage)
        {
            return srcImage;
        }
        
    }

    public static class Extensions
    {       
        /// <summary>
        /// 图像转成字节数组
        /// </summary>
        public static byte[] GetBytes(this System.Drawing.Image image)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.Drawing.Imaging.ImageFormat format = image.RawFormat;
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] buffer = new byte[ms.Length];
                ms.Seek(0, System.IO.SeekOrigin.Begin);   //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
        
    }
}
