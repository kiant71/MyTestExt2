using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace MyTestExt.ConsoleApp
{
    public class Barcode
    {
        public void Do()
        {



        }


        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="BarString">条码模式字符串</param>
        /// <param name="Height">生成的条码高度</param>
        /// <returns>条码图形</returns>
        public Bitmap KiCode128C(string BarString, int Height)
        {
            Bitmap b = new Bitmap(BarString.Length, Height, PixelFormat.Format24bppRgb);

            try
            {

                char[] cs = BarString.ToCharArray();

                for (int i = 0; i < cs.Length; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        if (cs[i] == 'b')
                        {
                            b.SetPixel(i, j, Color.Black);
                        }
                        else
                        {
                            b.SetPixel(i, j, Color.White);
                        }
                    }
                }

                return b;
            }
            catch
            {
                return null;
            }
        }

    }
}
