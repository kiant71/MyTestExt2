using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleApplication1.Util;

namespace MyTestExt.ConsoleApp
{
    public class RegularTest
    {
        public void MatchRetr()
        {
            string strRETR = "X-Mda-Received: from <m0.mail.sina.com.cn>([<172.16.201.48>])\n by <mda01.fmail.tg.sinanode.com> with LMTP id <109919>\n Jul 21 2015 14:57:28 +0800 (CST)\nX-Sina-MID:04AF2E0E859777BB0BA040EE9FF619AC1D00000000000006\nX-Sina-Attnum:0\nDate: Tue, 21 Jul 2015 14:57:28 +0800 \nFrom: =?UTF-8?B?5paw5rWq6YKu566x5Zui6Zif?= <sinamail@sina.com>\nSubject: =?UTF-8?B?5LiN5Zyo55S16ISR5YmN77yM6L+Z5qC355So6YKu566x44CC44CC44CC?=\nTo: sapmailtest@sina.com\nMime-Version: 1.0\nContent-type: text/html; charset=utf-8\nContent-transfer-encoding: 8BIT\nX-Mailer: SinaMail 3.0\n\n<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n<title>不在电脑前，这样用邮箱...</title>\n</head>\n\n<body>\n<table width=\"600\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family:'微软雅黑',Arial, Helvetica, sans-serif;\">\n  <tr>\n    <td align=\"left\" valign=\"top\"><img src=\"http://www.sinaimg.cn/rny/webface/emailPicture/zhongqiuMailLogo.jpg\" alt=\"新浪免费邮箱\"/></td>\n  </tr>\n  <tr>\n    <td height=\"15\" align=\"left\" valign=\"top\" bgcolor=\"#EFEFEF\"></td>\n  </tr>\n  <tr>\n    <td align=\"center\" valign=\"top\" bgcolor=\"#EFEFEF\"><table width=\"572\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"font-size:14px;line-height:22px;color:#333333;\">\n      <tr>\n        <td width=\"15\" bgcolor=\"#ffffff\">&nbsp;</td>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n        <td width=\"15\" bgcolor=\"#ffffff\">&nbsp;</td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n        <td bgcolor=\"#ffffff\"><h2 style=\"padding:0px;margin:0px;font-size:14px;text-align:left;\">Hi，新同学： </h2></td>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n        <td bgcolor=\"#ffffff\"><p style=\"padding:3px 5px 3px 0px;margin:0px;text-align:left;text-indent:2em;\">不在电脑前，需要收封邮件、发个照片，不要着急，新浪邮箱有手机版，随时随地访问mail.sina.cn，就可以登录邮箱。当然还有高端玩法，这里只介绍两个，其他等您慢慢发掘吧。</p></td>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"10\" bgcolor=\"#ffffff\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n        <td bgcolor=\"#ffffff\"><h2 style=\"padding:0px;margin:0px;font-size:14px;text-align:left;\">1、安装手机客户端，收发邮件So easy</h2></td>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"5\" bgcolor=\"#ffffff\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n        <td bgcolor=\"#ffffff\">\n        \t<p style=\"padding:3px 24px;margin:0px;text-align:left;\">\n            \t不用每次输入邮箱账号和密码，随时点一下就进邮箱了，收发随你。<br />\n新邮件还有免费的消息推送，没事儿的时候看看订阅的邮件。\n\n            </p>\n        </td>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"20\" bgcolor=\"#ffffff\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"10\" align=\"center\" bgcolor=\"#ffffff\"><a href=\"http://mail.sina.com.cn/client/mobile/index.php?suda-key=mail_app&amp;suda-value=zhucexin\" title=\"\" target=\"_blank\"><img src=\"http://www.sinaimg.cn/rny/webface/emailPicture/zczdxIco.jpg\" alt=\"立即下载\" border=\"0\" /></a></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"30\" bgcolor=\"#ffffff\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"10\" bgcolor=\"#ffffff\"><h2 style=\"padding:0px;margin:0px;font-size:14px;text-align:left;\">2、免费短信提醒，手机号码就是邮箱账号</h2></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"10\" bgcolor=\"#ffffff\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"10\" bgcolor=\"#ffffff\">\n        \t<p style=\"padding:3px 24px;margin:0px;text-align:left;\">\n            \t新邮箱地址记不牢、地址英文字母告知别人时候是在考验记忆力？<br />\n不用这么麻烦，立即激活手机号码邮箱，只需告诉别人：\"我的邮箱地址就是我的手机号@sina.cn\"。新邮件到了，还有免费的短信提醒哦。\n\n            </p>\n        </td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"20\" bgcolor=\"#ffffff\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"10\" align=\"center\" bgcolor=\"#ffffff\"><a href=\"#\" _act=\"open_tele_mail\"><img src=\"http://www.sinaimg.cn/rny/webface/emailPicture/zczdxJh.jpg\" alt=\"立即激活\" border=\"0\" /></a></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"30\" bgcolor=\"#ffffff\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"1\" bgcolor=\"#EFEFEF\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"10\" bgcolor=\"#ffffff\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n        <td bgcolor=\"#ffffff\">\n          <p style=\"padding:0px;margin:0px;text-align:right;\">\n            新浪邮箱团队<br />\n            2015年07月21日\n            </p>\n          </td>\n        <td bgcolor=\"#ffffff\">&nbsp;</td>\n      </tr>\n      <tr>\n        <td bgcolor=\"#ffffff\"></td>\n        <td height=\"10\" bgcolor=\"#ffffff\"></td>\n        <td bgcolor=\"#ffffff\"></td>\n      </tr>\n    </table></td>\n  </tr>\n  <tr>\n    <td align=\"left\" valign=\"top\" bgcolor=\"#EFEFEF\">\n    \t<p style=\"padding:15px 0px;margin:0px;font-size:12px;color:#929292;text-align:center;\">\n        \t如有任何疑问，可发送邮件至：webcn@staff.sina.com.cn<br />\n或者拨打全国统一客服热线：4006-900-000，我们的客服人员将会在第一时间为您解答\n        </p>\n    </td>\n  </tr>\n</table>\n</body>\n</html>\r\n\r\n";
            string strAttnum, strTransferEncoding, strMailer, strHtml;
            Regex regex = null;
            Match match = null;

            regex = new Regex(@"Attnum:\s*(\d+)?", RegexOptions.IgnoreCase);
            match = regex.Match(strRETR);
            strAttnum = match.Groups[1].Value;  //"0"

            regex = new Regex(@"Content-transfer-encoding:\s*(.*?)\n", RegexOptions.IgnoreCase);
            match = regex.Match(strRETR);
            strTransferEncoding = match.Groups[1].Value;    //"8BIT"

            regex = new Regex(@"Mailer:\s*(.*?)\n", RegexOptions.IgnoreCase);
            match = regex.Match(strRETR);
            strMailer = match.Groups[1].Value;  //"SinaMail 3.0"

            regex = new Regex(@"<html.*?</html>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            match = regex.Match(strRETR);
            strHtml = match.Groups[0].Value;  //"<html .... </html>"


            Console.WriteLine();
        }

        public void Test()
        {
            //using System.Text.RegularExpressions;
            //string strInput = "asdfasdf{sdf_xa}?oksdof={123_3}";
            //Regex regex = new Regex(@"{(?<field>\w+)}");
            //MatchCollection matches = regex.Matches(strInput);
            //foreach (Match match in matches)
            //{
            //    Console.WriteLine(match.Groups["field"].Value);
            //}

            //var str = "sdfsd @陈伟 @陈永芳 @王云 @温雪明 sdfsdf";
            //var regex = new Regex(@"@(\w+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //var matches = regex.Matches(str);
            //foreach (Match match in matches)
            //{
            //    Console.WriteLine(match.Groups[1]);
            //}

            var ccStr = "[    {      \"CompanyID\": 2181,      \"AtKey\": \"{2181_1_100761}\",      \"AtType\": 1,      \"Name\": \"001测试\",      \"AtValue\": \"100761\"    },    {      \"CompanyID\": 2181,      \"AtKey\": \"{2181_1_117228}\",      \"AtType\": 1,      \"Name\": \"02\",      \"AtValue\": \"117228\"    },    {      \"CompanyID\": 2181,      \"AtKey\": \"{2181_1_103284}\",      \"AtType\": 1,      \"Name\": \"Kkkk\",      \"AtValue\": \"103284\"    },    {      \"CompanyID\": 2181,      \"AtKey\": \"{2181_1_103356}\",      \"AtType\": 1,      \"Name\": \"T1001\",      \"AtValue\": \"103356\"    },    {      \"CompanyID\": 2181,      \"AtKey\": \"{2181_1_103287}\",      \"AtType\": 1,      \"Name\": \"test00001\",      \"AtValue\": \"103287\"    }  ]";
            var cCs = JsonParse.Deserialize<List<dynamic>>(ccStr);

            var str = @"分享[不屑][哼哼][哼哼][哼哼]{2181_1_100761} {2181_1_117228} {2181_1_103284} {2181_1_103356} {2181_1_103287} 啦啦啦啦[不屑][不屑][不屑][不屑]

[不屑][不屑]
{2181_1_103284} 
{2181_1_103356} 
[不屑][不屑]";
            var regex = new Regex(@"\{[\w.]+\}", RegexOptions.IgnoreCase );
            var matches = regex.Matches(str);

            var sb = new StringBuilder("");
            int index = 0;
            foreach (Match match in matches)
            {
                //Console.WriteLine(match.Groups[0]);
                sb.Append(str.Substring(index, match.Index- index));  // 上一个的结尾，到当前的开头 的正常内容

                var find = cCs.FirstOrDefault(c => match.Groups[0].Value.Equals(c?.AtKey?.ToString(), StringComparison.OrdinalIgnoreCase));
                if (find != null && find.Name != null)
                    sb.Append("@" + find.Name.ToString()); // 替换名字
                else
                    sb.Append(" ");

                index = match.Index + match.Length; // 记录这一个的结尾
            }

            var ab = sb.ToString();



            //string strInput = "张三[xxx@163.com],李四[yyy@suhu.com],王五[zzz@gmai.com]";
            //Regex regex = new Regex(@"[\$]*(?<grp1>[\w.]+)\[(?<grp2>[^\]]*)");
            //MatchCollection matches = regex.Matches(strInput);
            //foreach (Match match in matches)
            //{
            //    Console.WriteLine(match.Groups["grp1"].Value + " " + match.Groups["grp2"].Value);
            //}
            /*分别输出：
             张三  xxx@163.com
             李四  yyy@suhu.com
             王五  zzz@gmai.com 
             */
        }

        private bool VerifyTelephone(string input)
        {
            string pattern = @"^(\d{3,4}-)?\d{6,8}$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            return regex.IsMatch(input);
        }

        private bool VerifyMobilephone(string input)
        {
            string pattern = @"^1\d{10}$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            return regex.IsMatch(input);
        }

        private bool VerifyEmail(string input)
        {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            return regex.IsMatch(input);
        }

        
    }
}
