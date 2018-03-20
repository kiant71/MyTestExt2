using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            Image tempImg1 = Image.FromFile(@".\Resource\01.png");
            Image tempImg2 = Image.FromFile(@".\Resource\02.png");
            Image tempImg3 = Image.FromFile(@".\Resource\03.png");
            Image tempImg4 = Image.FromFile(@".\Resource\04.png");
            Image tempImg5 = Image.FromFile(@".\Resource\05.png");

            var listUserImage = new List<byte[]>();
            MemoryStream stream = new MemoryStream();

            #region 方形平整摆放
            int width1 = 80;
            int height1 = 80;

            stream = new MemoryStream();
            tempImg1.Save(stream, ImageFormat.Png);
            listUserImage.Add(stream.ToArray());
            Image map = RectangleGroup.Create(listUserImage.ToArray(), width1, height1);
            map.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\fang1.jpg");
            pictureBox1.Image = map;
            // mProg
            byte[] byte11 = map.GetBytes();
            MemoryStream ms11 = new MemoryStream(byte11);
            ms11.Seek(0, SeekOrigin.Begin);
            Image image11 = Image.FromStream(ms11);
            image11.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\i.fang11_byte.jpg");
            pictureBox11.Image = image11;


            stream = new MemoryStream();
            tempImg2.Save(stream, ImageFormat.Png);
            listUserImage.Add(stream.ToArray());
            Image map2 = RectangleGroup.Create(listUserImage.ToArray(), width1, height1);
            map2.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\fang2.jpg");
            pictureBox2.Image = map2;
            // mProg
            byte[] byte12 = map2.GetBytes();
            MemoryStream ms12 = new MemoryStream(byte12);
            ms12.Seek(0, SeekOrigin.Begin);
            Image image12 = Image.FromStream(ms12);
            image12.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\i.fang12_byte.jpg");
            pictureBox12.Image = image12;


            stream = new MemoryStream();
            tempImg3.Save(stream, ImageFormat.Png);
            listUserImage.Add(stream.ToArray());
            Image map3 = RectangleGroup.Create(listUserImage.ToArray(), width1, height1);
            map3.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\fang3.jpg");
            pictureBox3.Image = map3;
            // mProg
            byte[] byte13 = map3.GetBytes();
            MemoryStream ms13 = new MemoryStream(byte13);
            ms13.Seek(0, SeekOrigin.Begin);
            Image image13 = Image.FromStream(ms13);
            image13.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\i.fang13_byte.jpg");
            pictureBox13.Image = image13;


            stream = new MemoryStream();
            tempImg4.Save(stream, ImageFormat.Png);
            listUserImage.Add(stream.ToArray());
            Image map4 = RectangleGroup.Create(listUserImage.ToArray(), width1, height1);
            map4.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\fang4.jpg");
            pictureBox4.Image = map4;
            // mProg
            byte[] byte14 = map4.GetBytes();
            MemoryStream ms14 = new MemoryStream(byte14);
            ms14.Seek(0, SeekOrigin.Begin);
            Image image14 = Image.FromStream(ms14);
            image14.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\i.fang14_byte.jpg");
            pictureBox14.Image = image14;
            #endregion


            listUserImage = new List<byte[]>();


            #region 圆形平摆
            stream = new MemoryStream();
            tempImg1.Save(stream, ImageFormat.Png);
            listUserImage.Add(stream.ToArray());
            Image map21 = RoundedGroup.Create(listUserImage.ToArray());
            map21.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\o.yuan1.jpg");
            pictureBox21.Image = map21;
            // mProg
            byte[] byte31 = map21.GetBytes();
            MemoryStream ms31 = new MemoryStream(byte31);
            ms31.Seek(0, SeekOrigin.Begin);
            Image image31 = Image.FromStream(ms31);
            image31.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\v.byteYuan1.jpg");
            pictureBox31.Image = image31;

            //// mSimple
            //string str = @"C:\Users\Administrator\Desktop\\map21.jpg";
            //FileStream fs = new FileStream(str, FileMode.Open, FileAccess.Read);
            //BinaryReader by = new BinaryReader(fs);
            //int length = (int)fs.Length;
            //byte[] imgbyte = by.ReadBytes(length);
            //MemoryStream ms = new MemoryStream(imgbyte);
            //ms.Seek(0, SeekOrigin.Begin);
            //Image image = Image.FromStream(ms);         



            stream = new MemoryStream();
            tempImg2.Save(stream, ImageFormat.Png);
            listUserImage.Add(stream.ToArray());
            Image map22 = RoundedGroup.Create(listUserImage.ToArray());
            map22.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\o.yuan2.jpg");
            pictureBox22.Image = map22;
            // mProg
            byte[] byte32 = map22.GetBytes();
            MemoryStream ms32 = new MemoryStream(byte32);
            ms32.Seek(0, SeekOrigin.Begin);
            Image img32 = Image.FromStream(ms32);
            img32.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\v.byteYuan2.jpg");
            pictureBox32.Image = img32;


            stream = new MemoryStream();
            tempImg3.Save(stream, ImageFormat.Png);
            listUserImage.Add(stream.ToArray());
            Image map23 = RoundedGroup.Create(listUserImage.ToArray());
            map23.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\o.yuan3.jpg");
            pictureBox23.Image = map23;
            // mProg
            byte[] byte33 = map23.GetBytes();
            MemoryStream ms33 = new MemoryStream(byte33);
            ms33.Seek(0, SeekOrigin.Begin);
            Image img33 = Image.FromStream(ms33);
            img33.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\v.byteYuan3.jpg");
            pictureBox33.Image = img33;


            stream = new MemoryStream();
            tempImg4.Save(stream, ImageFormat.Png);
            listUserImage.Add(stream.ToArray());
            Image map24 = RoundedGroup.Create(listUserImage.ToArray());
            map24.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\o.yuan4.jpg");
            pictureBox24.Image = map24;
            // mProg
            byte[] byte34 = map24.GetBytes();
            MemoryStream ms34 = new MemoryStream(byte34);
            ms34.Seek(0, SeekOrigin.Begin);
            Image img34 = Image.FromStream(ms34);
            img34.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\v.byteYuan4.jpg");
            pictureBox34.Image = img34;


            stream = new MemoryStream();
            tempImg5.Save(stream, ImageFormat.Png);
            listUserImage.Add(stream.ToArray());
            Image map25 = RoundedGroup.Create(listUserImage.ToArray());
            map25.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\o.yuan5.jpg");
            pictureBox25.Image = map25;
            // mProg
            byte[] byte35 = map25.GetBytes();
            MemoryStream ms35 = new MemoryStream(byte35);
            ms35.Seek(0, SeekOrigin.Begin);
            Image img35 = Image.FromStream(ms35);
            img35.Save(@"C:\Users\Administrator\Desktop\\ImageGroup\\v.byteYuan5.jpg");
            pictureBox35.Image = img35; 
            #endregion


            stream.Close();
            stream.Dispose();
        }
    }
}
