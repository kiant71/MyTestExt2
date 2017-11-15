using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumiSoft.Net.AUTH;
using LumiSoft.Net.Mail;
using LumiSoft.Net.MIME;
using LumiSoft.Net.SMTP.Client;

namespace MyTestExt.ConsoleApp
{
    public class LumiSoftTest
    {
        string SmtpServer = "smtp.sina.com";
        string EMail = "sapmailtest@sina.com";
        string MailPwd = "test12345";
        string UserName = "许志坚";
        string ToAddress = "许志坚[1214850652@qq.com]";
        string Body = "<html>这是一份测试邮件，<img src=\"cid:{0}\">来自<font color=red><b>我的测试</b></font></html>";
        string cid = "";


        Mail_Message message = null;


        /// <summary>
        /// 发邮件常见场景是用户指定邮件写一封发一封。如果需要定义业务“群发（接收人拆分成多个独立发送）”，应该在外部调用多任务处理
        /// </summary>
        /// <param configName="email"></param>
        /// <param configName="user"></param>
        public void Send()
        {
            //try {

            //    MIME_h m_pOwner = new MIME_h();

            //    Dictionary<string, MIME_h_Parameter> m_pParameters = new Dictionary<string, MIME_h_Parameter>(StringComparer.CurrentCultureIgnoreCase);

            //    MIME_h_ParameterCollection m_pParameters = new MIME_h_ParameterCollection();


            //    MIME_h_ContentDisposition ContentDisposition = new MIME_h_ContentDisposition(MIME_DispositionTypes.Attachment);// {
            //            //Param_FileName = configName,
            //            //Param_Size = param_Size
            //        //};

            //    m_pParameters["filename"] = @"=?GB2312?B?1eK49s7EvP7D+7rcs6S63LOkutyzpLeiy825/bPM1tC74be1u9jS7LOjo7o1NTQgZHRzcG0gxNrI3c60sbvQ7b/JoaLTyrz+sbvN+NLXyrax8M6qwKy7+NPKvP6jrNfu0MLH6b/2ysdfYWJjv7TN6sHLw7QudHh0?=";


            //    get{ return this.Conditions["filename"]; }

            //set{  }
            //}
            //catch (Exception) {

            //    throw;
            //}



            CheckAndFillMessage();
            try
            {
                SMTP_Client smtp = new SMTP_Client();
                Connect(smtp);
                smtp.MailFrom(message.From[0].Address, 1);  //已知163smtp服务器需要MailFrom 命令放置在RcptTo前
                foreach (Mail_t_Mailbox toMailbox in message.To)
                {
                    smtp.RcptTo(toMailbox.Address);
                }
                Handler(message, smtp);
            }
            catch (Exception x)
            {

            }

            //等待后台线程处理完成
            //while (this.PendingCount > 0) {
            //    System.Threading.Thread.Sleep(200);
            //}
            #region DEBUG

            #endregion
        }

        /// <summary>
        /// 检查并填充邮件
        /// </summary>
        /// <param configName="email"></param>
        /// <param configName="message"></param>
        /// <returns></returns>
        private bool CheckAndFillMessage()
        {
            try
            {
                #region 填充邮件头
                message = new Mail_Message()
                {
                    Subject = "测试 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    MessageID = new Random().Next().ToString(),
                    Date = DateTime.Now
                };
                //发件人，必须有值
                message.From = new Mail_t_MailboxList();
                message.From.Add(new Mail_t_Mailbox(this.UserName, this.EMail));
                //收件人 ex.张三[xxx@163.com];李四[xis@gmail.com]  //后期改回正则
                var query = from e in this.ToAddress.Split(';')
                            select new Mail_t_Mailbox(e.Split('[')[0], e.Replace("]", "").Split('[')[1]);
                message.To = new Mail_t_AddressList();
                foreach (var item in query)
                {
                    message.To.Add(item);
                }

                #endregion

                #region 填充邮件内容文本
                //--- multipart/mixed -----------------------------------
                MIME_h_ContentType contentType_multipartMixed = new MIME_h_ContentType(MIME_MediaTypes.Multipart.mixed);
                contentType_multipartMixed.Param_Boundary = Guid.NewGuid().ToString().Replace('-', '.');
                MIME_b_MultipartMixed multipartMixed = new MIME_b_MultipartMixed(contentType_multipartMixed);
                message.Body = multipartMixed;

                //--- multipart/alternative -----------------------------
                MIME_Entity entity_multipartAlternative = new MIME_Entity();
                MIME_h_ContentType contentType_multipartAlternative = new MIME_h_ContentType(MIME_MediaTypes.Multipart.alternative);
                contentType_multipartAlternative.Param_Boundary = Guid.NewGuid().ToString().Replace('-', '.');
                MIME_b_MultipartAlternative multipartAlternative = new MIME_b_MultipartAlternative(contentType_multipartAlternative);
                entity_multipartAlternative.Body = multipartAlternative;
                multipartMixed.BodyParts.Add(entity_multipartAlternative);

                //--- text/plain ----------------------------------------
                MIME_Entity entity_text_plain = new MIME_Entity();
                MIME_b_Text text_plain = new MIME_b_Text(MIME_MediaTypes.Text.plain);
                entity_text_plain.Body = text_plain;

                //普通文本邮件内容，如果对方的收件客户端不支持HTML，这是必需的
                string plainTextBody = "如果你邮件客户端不支持HTML格式，或者你切换到“普通文本”视图，将看到此内容";
                text_plain.SetText(MIME_TransferEncodings.QuotedPrintable, Encoding.UTF8, plainTextBody);
                multipartAlternative.BodyParts.Add(entity_text_plain);

                //--- text/html -----------------------------------------
                string htmlText = string.Format(Body, cid);
                MIME_Entity entity_text_html = new MIME_Entity();
                MIME_b_Text text_html = new MIME_b_Text(MIME_MediaTypes.Text.html);
                entity_text_html.Body = text_html;
                text_html.SetText(MIME_TransferEncodings.QuotedPrintable, Encoding.UTF8, htmlText);
                multipartAlternative.BodyParts.Add(entity_text_html);

                #endregion

                #region 填充邮件附件或内容实体
                //--- application/octet-stream ----------------------------
                //MIME_Entity entity = MIME_Entity.CreateEntity_Attachment(atta.SourceFileName, new MemoryStream(atta.Data));
                //string configName = GetAttachmentNameStr(atta.SourceFileName);   //中文名的小转换
                //entity.ContentDisposition.Param_FileName = configName;       
                //multipartMixed.BodyParts.Add(entity);



                //修正为原生的代码
                FileStream stream = new FileStream(@"F:\Email\这个文件名很长很长很长发送过程中会返回异常：554 dtspm 内容未被许可、邮件被网易识别为垃圾邮件，最新情况是_abc看完了么.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                string attachmentName = @"这个文件名很长很长很长发送过程中会返回异常：554 dtspm 内容未被许可、邮件被网易识别为垃圾邮件，最新情况是_abc看完了么.txt";


                //FileStream stream = new FileStream(@"F:\Email\文件1.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                //string attachmentName = @"文件1.txt";


                MIME_Entity mIME_Entity = new MIME_Entity();
                MIME_b_Application mIME_b_Application = new MIME_b_Application(MIME_MediaTypes.Application.octet_stream);
                mIME_Entity.Body = mIME_b_Application;
                mIME_b_Application.SetData(stream, MIME_TransferEncodings.Base64);
                long param_Size = stream.CanSeek ? stream.Length : -1L;
                string name = attachmentName;


                string encodingStr = "utf-8".ToUpper();
                Encoding encoding = Encoding.GetEncoding(encodingStr);
                if (encoding.GetByteCount(name) > name.Length)
                {  //非英文字符
                    string ext = name.Substring(name.LastIndexOf("."));     // .xxx
                    string name2 = name.Substring(0, name.Length - ext.Length);    //aaaa

                    //截串
                    //while (encoding.GetByteCount(name2 + ext) > 44) {//TODO.附件名最多支持转化后长达44个字节（gb2312）的文本
                    //    name2 = name2.Substring(0, name2.Length - 1);
                    //}

                    string newName = name2 + ext;

                    //方式一、截串 gb2312+B、utf-8+B 正常
                    byte[] barray = encoding.GetBytes(newName);
                    name = "=?" + encodingStr + "?B?" + Convert.ToBase64String(barray) + "?=";


                    //Q 的方式
                    //string aa11 = Encode(newName, encoding); ;
                    //string aa12 = Decode(aa11, encoding);
                    //configName = "=?" + encodingStr + "?Q?" + aa11 + "?=";


                    //mIME_Entity.ContentType.Param_Name = configName;    //文件名乱码和ContentType 无关


                    //小转换
                    MIME_h_ContentDisposition a1 = new MIME_h_ContentDisposition(MIME_DispositionTypes.Attachment)
                    {
                        Param_FileName = name,
                        Param_Size = param_Size
                    };
                    var aa = a1.ToString();
                    //var aa1 = a1.ValueToString()

                    MIME_h_ContentDisposition a2 = MIME_h_ContentDisposition.Parse(aa);

                    //string format = "Content-Disposition: attachment;\r\n\tfilename=\"=?UTF-8?B?5paH5Lu2MS50eHQ=?=\";\r\n\tsize=\"68\"\r\n";
                    string format = "Content-Disposition: attachment;\r\n\tfilename=\"{0}\";\r\n\tsize=\"{1}\"\r\n";
                    string str3 = string.Format(format, name, param_Size);
                    MIME_h_ContentDisposition a3 = MIME_h_ContentDisposition.Parse(str3);


                    mIME_Entity.ContentDisposition = a3;
                }
                else
                {
                    //纯英文的处理
                    mIME_Entity.ContentDisposition = new MIME_h_ContentDisposition(MIME_DispositionTypes.Attachment)
                    {
                        Param_FileName = name,
                        Param_Size = param_Size
                    };
                }


                multipartMixed.BodyParts.Add(mIME_Entity);


                #region 内容附件
                //内容附件
                //MIME_Entity entity_image = new MIME_Entity();
                //entity_image.ContentID = atta.CID;
                //entity_image.ContentDisposition = new MIME_h_ContentDisposition(MIME_DispositionTypes.Inline);
                //MIME_b_Image body_image = new MIME_b_Image(MIME_MediaTypes.Image.jpeg);
                //entity_image.Body = body_image;
                //body_image.SetData(new MemoryStream(atta.Data), "base64");
                //multipartMixed.BodyParts.Add(entity_image); 
                #endregion


                #endregion
            }
            catch (Exception x)
            {

            }

            return true;
        }

        /// <summary>
        /// 连接SMTP 服务器
        /// </summary>
        /// <param configName="smtp"></param>
        /// <param configName="company"></param>
        /// <param configName="user"></param>
        /// <exception cref="外抛与服务器连接时异常"></exception>
        private void Connect(SMTP_Client smtp)
        {
            try
            {
                switch (SmtpServer.ToLower())
                {
                    case "smtp.exmail.qq.com":
                        smtp.Connect(SmtpServer, 465, true);  //调整，后期 可能强制要求SSL 
                        break;
                    default:
                        smtp.Connect(SmtpServer, 25, false);
                        break;
                }
                smtp.EhloHelo(SmtpServer);    //send helo first
                AUTH_SASL_Client auth = null;
                switch (SmtpServer.ToLower())
                {
                    case "smtp-mail.outlook.com":
                    case "smtp.mail.yahoo.com":
                        smtp.StartTLS();    //TLS 加密连接
                        auth = new AUTH_SASL_Client_Login(this.EMail, MailPwd);
                        break;
                    case "smtp.tom.com":
                        auth = new AUTH_SASL_Client_Login(this.EMail, MailPwd);
                        break;
                    default:
                        auth = smtp.AuthGetStrongestMethod(this.EMail, MailPwd);
                        break;
                }
                smtp.Auth(auth);
            }
            catch (Exception x)
            {

            }
        }

        /// <summary>
        /// 发送处理进程
        /// </summary>
        /// <param configName="message"></param>
        /// <param configName="smtp"></param>
        /// <exception cref="外抛与服务器连接时异常"></exception>
        private void Handler(Mail_Message message, SMTP_Client smtp)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                message.ToStream(stream, new MIME_Encoding_EncodedWord(MIME_EncodedWordEncoding.Q, Encoding.UTF8), Encoding.UTF8);
                stream.Position = 0;

                //采用同步的方式发送，当出现异常时容易捕获,而且如果同一时间向服务器发送大量邮件会被当做垃圾站点上黑名单
                smtp.SendMessage(stream, true);
                HandlerAsyncComplated(null, null);

                //异步方式
                //SMTP_Client.SendMessageAsyncOP op = new SMTP_Client.SendMessageAsyncOP(stream, true);
                //op.CompletedAsync += (sender, eventArgs) => { HandlerAsyncComplated(sender, eventArgs); };
                //smtp.SendMessageAsync(op);
            }
            catch (Exception x)
            {

            }
        }

        /// <summary>
        /// 异步发送处理完成后
        /// </summary>
        /// <param configName="sender"></param>
        /// <param configName="eventArgs"></param>
        /// <param configName="email"></param>
        private void HandlerAsyncComplated(object sender, LumiSoft.Net.EventArgs<SMTP_Client.SendMessageAsyncOP> eventArgs)
        {

        }


        /// <summary>
        /// 转化中文格式名称
        /// </summary>
        /// <param configName="str"></param>
        /// <returns></returns>
        private string GetAttachmentNameStr(string str)
        {
            Encoding encoding = Encoding.GetEncoding("gb2312");
            if (encoding.GetByteCount(str) > str.Length)
            {  //非英文字符
                string ext = str.Substring(str.LastIndexOf("."));     // .xxx
                string name = str.Substring(0, str.Length - ext.Length);    //aaaa
                while (encoding.GetByteCount(name + ext) > 44)
                {//TODO.附件名最多支持转化后长达44个字节的文本
                    name = name.Substring(0, name.Length - 1);
                }
                //byte[] barray = encoding.GetBytes(configName + ext);
                //return "=?GB2312?B?" + Convert.ToBase64String(barray) + "?=";

                //全中文名转换
                byte[] barray = encoding.GetBytes(str);
                return "=?GB2312?B?" + Convert.ToBase64String(barray) + "?=";


                //byte[] barray = encoding.GetBytes(str);
                //string objValue = "=?GB2312?B?" + Convert.ToBase64String(barray, Base64FormattingOptions.None) + "?=";
                //return objValue;  //长度是过长

                //return "=?GB2312?B?1eK49s7EvP7D+7rcs6S63LOkutyzpLeiy825/bPM1tC74be1u9jS7LOjo7o1NTQgZHRzcG0gxNrI3c60sbvQ7b/JoaLTyrz+sbvN+NLXyrax8M6qwKy7+NPKvP6jrNfu0MLH6b/2ysdfYWJjv7TN6sHLw7QudHh0?=";

                //return "=?utf-8?B?" + Convert.ToBase64String(barray) + "?=";

                //string retString = string.Empty;
                //retString = Encode(str, encoding);
                //string objValue = Decode(retString, encoding);



                //return "=?GB2312?Q?" + retString.Replace("=\r\n", "").Replace("\r", "").Replace("\n", "") + "?=";
            }
            else
            {
                return str;
            }
        }


        private const byte EQUALS = 61;
        private const byte CR = 13;
        private const byte LF = 10;
        private const byte SPACE = 32;
        private const byte TAB = 9;

        /// <summary>
        /// Encodes a string to QuotedPrintable
        /// </summary>
        /// <param configName="_ToEncode">String to encode</param>
        /// <returns>QuotedPrintable encoded string</returns>
        public static string Encode(string _ToEncode, Encoding encoding)
        {
            StringBuilder Encoded = new StringBuilder();
            string hex = string.Empty;
            byte[] bytes = encoding.GetBytes(_ToEncode);
            int count = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                //these characters must be encoded
                if ((bytes[i] < 33 || bytes[i] > 126 || bytes[i] == EQUALS) && bytes[i] != CR && bytes[i] != LF && bytes[i] != SPACE)
                {
                    if (bytes[i].ToString("X").Length < 2)
                    {
                        hex = "0" + bytes[i].ToString("X");
                        Encoded.Append("=" + hex);
                    }
                    else
                    {
                        hex = bytes[i].ToString("X");
                        Encoded.Append("=" + hex);
                    }
                }
                else
                {
                    //check if index out of range
                    if ((i + 1) < bytes.Length)
                    {
                        //if TAB is at the end of the line - encode it!
                        if ((bytes[i] == TAB && bytes[i + 1] == LF) || (bytes[i] == TAB && bytes[i + 1] == CR))
                        {
                            Encoded.Append("=0" + bytes[i].ToString("X"));
                        }
                        //if SPACE is at the end of the line - encode it!
                        else if ((bytes[i] == SPACE && bytes[i + 1] == LF) || (bytes[i] == SPACE && bytes[i + 1] == CR))
                        {
                            Encoded.Append("=" + bytes[i].ToString("X"));
                        }
                        else
                        {
                            Encoded.Append(System.Convert.ToChar(bytes[i]));
                        }
                    }
                    else
                    {
                        Encoded.Append(System.Convert.ToChar(bytes[i]));
                    }
                }
                if (count == 75)
                {
                    Encoded.Append("=\r\n"); //insert soft-linebreak
                    count = 0;
                }
                count++;
            }

            return Encoded.ToString();
        }

        /// <summary>
        /// Decodes a QuotedPrintable encoded string
        /// </summary>
        /// <param configName="_ToDecode">The encoded string to decode</param>
        /// <returns>Decoded string</returns>
        public static string Decode(string _ToDecode, Encoding encoding)
        {
            try
            {
                //remove soft-linebreaks first
                _ToDecode = _ToDecode.Replace("=\r\n", "");
                char[] chars = _ToDecode.ToCharArray();
                byte[] bytes = new byte[chars.Length];
                int bytesCount = 0;
                for (int i = 0; i < chars.Length; i++)
                {
                    // if encoded character found decode it
                    if (chars[i] == '=')
                    {
                        bytes[bytesCount++] = System.Convert.ToByte(int.Parse(chars[i + 1].ToString() + chars[i + 2].ToString(), System.Globalization.NumberStyles.HexNumber));
                        i += 2;
                    }
                    else
                    {
                        bytes[bytesCount++] = System.Convert.ToByte(chars[i]);
                    }
                }
                return encoding.GetString(bytes, 0, bytesCount);
            }
            catch (Exception)
            {

                return _ToDecode;
            }
        }

    }
}
