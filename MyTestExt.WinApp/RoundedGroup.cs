using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// 圆形头像组
    /// </summary>
    public class RoundedGroup
    {

        /// <summary>
        /// 图像生成
        /// </summary>
        public static Image Create(dynamic[] paramPic)
        {
            #region 参数初始化
            int destWH = 170;       //图片大小
            int spacing = 17;       //内部填充
            int iconWH;             //统一图标大小
            Rectangle[] iconRects;  //图标位置
            Rectangle interRect;    //首尾交集点的坐标区域
                        
            switch (paramPic.Length)
            {
                case 1:
                    iconWH = destWH;
                    iconRects = new Rectangle[] { new Rectangle(0, 0, iconWH, iconWH) };
                    interRect = new Rectangle(0, 0, 0, 0);
                    break;
                case 2:
                    iconWH = (int)(0.68 * destWH);
                    iconRects = new Rectangle[]{new Rectangle(0,    0, iconWH, iconWH)    /*左*/
                                             , new Rectangle(58,  58, iconWH, iconWH)};  /*右*/
                    interRect = new Rectangle(0, 0, 0, 0);
                    break;
                case 3:
                    iconWH = (int)(0.60 * destWH);
                    iconRects = new Rectangle[]{new Rectangle(0,   75, iconWH, iconWH)    /*左下*/
                                             , new Rectangle(75,  75, iconWH, iconWH)    /*右下*/
                                             , new Rectangle(35,  0,  iconWH, iconWH)};  /*上*/
                    interRect = new Rectangle(0, 70, 90, 30);
                    break;
                case 4:
                    iconWH = (int)(0.58 * destWH);
                    iconRects = new Rectangle[]{new Rectangle(0,   75, iconWH, iconWH)    /*左下*/
                                             , new Rectangle(75,  75, iconWH, iconWH)    /*右下*/
                                             , new Rectangle(75,   0, iconWH, iconWH)    /*右上*/
                                             , new Rectangle(0,    0, iconWH, iconWH)};  /*左上*/
                    interRect = new Rectangle(0, 65, 78, 35);
                    break;
                default:
                    iconWH = (int)(0.48 * destWH);
                    iconRects = new Rectangle[]{new Rectangle(17, 90, iconWH, iconWH)    /*左下*/
                                             , new Rectangle(82, 90, iconWH, iconWH)    /*右下*/
                                             , new Rectangle(90, 35, iconWH, iconWH)    /*右上*/                                            
                                             , new Rectangle(50,   0, iconWH, iconWH)    /*上*/    
                                             , new Rectangle(0,   35, iconWH, iconWH)};  /*左上*/
                    interRect = new Rectangle(0, 90, 70, 35);
                    break;
            }   
            #endregion

            Bitmap destImg = new Bitmap(destWH, destWH);
            using (Graphics g = Graphics.FromImage(destImg))
            {
                g.Clear(Color.WhiteSmoke);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var tmpImg = new Image[iconRects.Length];
                for (int i = 0; i < iconRects.Length; i++)
                {//"原始图片"处理后，生成一个"目标图像大小的笔刷"，且指定“原始图片坐标及大小”;
                    tmpImg[i] = CreateBrushImage(paramPic[i], iconRects[i], destWH, spacing);
                    using (var brush = new TextureBrush(tmpImg[i]))
                    {
                        g.FillEllipse(brush, iconRects[i]);
                    }                    
                }

                //如果数量大于2，形成首尾吞吃的效果（尾首两图生成临时图片，然后提取交集部分）
                if (iconRects.Length > 2)
                {
                    Image innerImg = CreateUnionImage(tmpImg[0], iconRects[0]
                        , tmpImg[tmpImg.Length-1], iconRects[iconRects.Length-1], destWH);
                    using (var bursh =new TextureBrush(innerImg))
                    {
                        g.FillRectangle(bursh, interRect); //截取交集部分，复制到主图像位置
                    }
                }
            }// end.of using g
                        
            
            return destImg;
        }

        
        /// <summary>
        /// 绘制目标大小“新图”，将“原始图片”比例缩放至“新图”指定的坐标及大小，供笔刷调用
        /// </summary>
        /// <param name="srcData">图像数据</param>
        /// <param name="rect">图像所处的坐标及大小</param>
        /// <returns></returns>
        private static Image CreateBrushImage(byte[] srcData, Rectangle rect, int destWH, int spacing)
        {
            using (MemoryStream ms = new MemoryStream(srcData))
            {
                ms.Seek(0, SeekOrigin.Begin);
                Image srcImg = Bitmap.FromStream(ms);
                return CreateBrushImage(srcImg, rect, destWH, spacing);
            }
        }

        /// <summary>
        /// 绘制目标大小“新图”，将“原始图片”比例缩放至“新图”指定的坐标及大小，供笔刷调用
        /// </summary>
        /// <param name="srcData">图像数据</param>
        /// <param name="rect">图像所处的坐标及大小</param>
        /// <returns></returns>
        private static Image CreateBrushImage(Image srcImage, Rectangle rect, int destWH, int spacing)
        {
            Bitmap destImage = new Bitmap(destWH, destWH);
            using (Graphics g = Graphics.FromImage(destImage))
            {
                g.Clear(Color.WhiteSmoke);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                //绘制图案，然后在图案上绘制圆框（图案会被覆盖一部分）
                g.DrawImage(srcImage
                    , rect
                    , new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                g.DrawEllipse(new Pen(Color.WhiteSmoke, spacing), rect);
            }
            return destImage;
        }

        /// <summary>
        /// 创建一个临时的尾首相交图案
        /// </summary>
        private static Image CreateUnionImage(Image firstImg, Rectangle firstRect, Image lastImg, Rectangle lastRect, int destWH)
        {
            Bitmap destMap = new Bitmap(destWH, destWH);
            using (Graphics g = Graphics.FromImage(destMap))
            {
                //g.Clear(Color.WhiteSmoke);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                using (var brush = new TextureBrush(lastImg))
                {//先尾
                    g.FillEllipse(brush, lastRect);
                }
                using (var brush = new TextureBrush(firstImg))
                {//后首，进行相交，覆盖重叠
                    g.FillEllipse(brush, firstRect);
                }
            }
            return destMap;
        }
                
    }
}
