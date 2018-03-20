using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace MyTestExt.ConsoleApp
{

    public class SmtpTest
    {

        public void Send()
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Host = "smtp.sina.com";
                ////smtp.UseDefaultCredentials = true;    @163.com
                //smtp.Credentials = new System.Net.NetworkCredential("sapmailtest", "test12345");   //new NetworkCredential("renzhijie1111", "**");

                smtp.Host = "smtp.163.com";
                //smtp.UseDefaultCredentials = true; 
                //smtp.
                smtp.Credentials = new System.Net.NetworkCredential("sap360mailtest@163.com", "test123--");   //new NetworkCredential("renzhijie1111", "**");


                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("sapmailtest@sina.com", "许志坚");
                mm.To.Add("1214850652@qq.com");

                mm.Subject = "测试smtpclient 给自己的信";

                string plainTextBody = "如果你邮件客户端不支持HTML格式，或者你切换到“普通文本”视图，将看到此内容";
                mm.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainTextBody, null, "text/plain"));

                ////HTML格式邮件的内容   
                string htmlBodyContent = "如果你的看到<b>这个</b>， 说明你是在以 <span style=\"color:red\">HTML</span> 格式查看邮件<br><br>"
                    + "看到图片 <a href=\"http://www.fenbi360.net\">粉笔编程网</a> <img src=\"cid:weblogo\"> 了么"
                ;//注意此处嵌入的图片资源   
                AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(htmlBodyContent, null, "text/html");


                LinkedResource lrImage = new LinkedResource(@"F:\Email\img2.png", "image/gif");
                lrImage.ContentId = "weblogo"; //此处的ContentId 对应 htmlBodyContent 内容中的 cid: ，如果设置不正确，请不会显示图片   
                htmlBody.LinkedResources.Add(lrImage);



                FileStream fs1 = new FileStream(@"F:\Email\这个文件名很长很长很长发送过程中会返回异常：554 dtspm 内容未被许可、邮件被网易识别为垃圾邮件，最新情况是_abc看完了么.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                Attachment att1 = new Attachment(fs1, @"这个文件名很长很长很长发送过程中会返回异常：554 dtspm 内容未被许可、邮件被网易识别为垃圾邮件，最新情况是_abc看完了么.txt");
                mm.Attachments.Add(att1);

                FileStream fs2 = new FileStream(@"F:\Email\abcdefghil012345678901234567890123456789012345678901234567890123456789.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                Attachment att2 = new Attachment(fs2, @"abcdefghil012345678901234567890123456789012345678901234567890123456789.txt");
                mm.Attachments.Add(att2);







                mm.AlternateViews.Add(htmlBody);




                ////要求回执的标志   
                //mm.Headers.Add("Disposition-Notification-To", "sap360mailtest@163.com");

                ////自定义邮件头   
                mm.Headers.Add("X-Website", "http://www.fenbi360.net");

                ////针对 LOTUS DOMINO SERVER，插入回执头   
                mm.Headers.Add("ReturnReceipt", "1");

                mm.Priority = MailPriority.Normal; //优先级   
                //mm.ReplyTo = new MailAddress("sap360mailtest@163.com", "我自己");

                ////如果发送失败，SMTP 服务器将发送 失败邮件告诉我   
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                ////异步发送完成时的处理事件   
                smtp.SendCompleted += new SendCompletedEventHandler(smtp_SendCompleted);

                ////开始异步发送   
                smtp.SendAsync(mm, null);

                //smtp.Send(mm);
            }
            catch (Exception ex)
            {

            }
        }

        void smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var aa = e.Error.Message;
        }

    }
}
