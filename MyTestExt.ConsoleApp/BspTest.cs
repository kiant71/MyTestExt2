using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class BspTest
    {
        public void Do()
        {
            #region

            var checkword = "";

            var postXml = "<?xml version='1.0' encoding='UTF-8'?> <Request service=\"OrderService\" lang=\"zh-CN\"> <Head>BSPdevelop</Head> <Body> <Order orderid='TE20150104' j_company='罗湖火车站' j_contact='小雷' j_tel='13810744' j_mobile='13111744' j_province='广东省' j_city='深圳' j_county='福田区' j_address='罗湖火车站东区调度室' d_company='顺丰速运' d_contact='小邱' d_tel='15819050' d_mobile='15539050' d_address='北京市海淀区中关村' express_type='1' pay_method='1' parcel_quantity='1' cargo_length='33' cargo_width='33' cargo_height='33' url_flag='1' remark=''> <Cargo name='LV1' count='3' unit='a' weight='' amount='' currency='' source_area=''></Cargo> <Cargo name='LV2' count='3' unit='a' weight='' amount='' currency='' source_area=''> </Cargo> <AddedService name='COD' value='3000' value1='0123456789'> </AddedService> <AddedService name='INSURE' value='2304.23'> </AddedService> <AddedService name='URGENT'> </AddedService> <Extra e1='abc' e2='abc'/></Order></Body></Request>";

            //postXml =
            //    "<?xml version='1.0' encoding='UTF-8'?> <Request service='CheckWorkDayService' lang='zh-CN'><Head>BSPdevelop</Head><Body><CheckWorkDayReq source_code=\"755\" dest_code=\"755\" cargo_type_code=\"T4,T6\" lang_code=\"sc\" media_code=\"baidu\" system_code=\"club\" day=\"2016-04-21\" /></Body></Request>";

            //postXml = XmlTest.Do2();


            #endregion

            var str1 = postXml + checkword;

            //var md5 = MD5.Create();
            //byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str1));
            //StringBuilder result = new StringBuilder();
            //string formatString = "x";
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    string hex = bytes[i].ToString(formatString);
            //    if (hex.Length == 1)
            //    {
            //        result.Append("0");
            //    }
            //    result.Append(hex);
            //}
            //var strMd5 = result.ToString();

            //MD5 m = new MD5CryptoServiceProvider();
            //byte[] s = m.ComputeHash(Encoding.UTF8.GetBytes(str1));
            //var strMd5 = BitConverter.ToString(s);
            //strMd5 = strMd5.ToLower();
            //strMd5 = strMd5.Replace("-", "");
            //var strBase64= Convert.ToBase64String(Encoding.UTF8.GetBytes(strMd5));


            var result = Encoding.UTF8.GetBytes(str1);
            MD5 md5 = new MD5CryptoServiceProvider();
            var output = md5.ComputeHash(result);
            var strBase64 = Convert.ToBase64String(output);


            try
            {
                var fullUrl = "http://bsp-ois.sit.sf-express.com:9080/bsp-ois/sfexpressService";


                //设置HttpClientHandler的AutomaticDecompression
                var handler = new HttpClientHandler() {  };  // AutomaticDecompression = DecompressionMethods.GZip

                //创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    //使用FormUrlEncodedContent做HttpContent
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                    {
                        {"xml", postXml},
                        {"verifyCode", strBase64}
                    });

                    //await异步等待回应
                    var response = http.PostAsync(fullUrl, content).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var rep = response.Content.ReadAsStringAsync().Result ?? "";
                    }
                    else
                    {
                        var intt = (int)response.StatusCode;
                        var aa = response;
                    }

                    //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                    ////Console.WriteLine(await response.Content.ReadAsStringAsync());
                }


                //var request = new HttpRequestMessage( HttpMethod.Post, fullUrl);
                //request.Headers.Add("xml", postXml);
                //request.Headers.Add("verifyCode", strBase64);

                ////var content = new StringContent(postJson);
                ////content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                ////request.Content = content;
                ////var handle = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                ////var client = new HttpClient(handle);

                //var client = new HttpClient();
                //var response = client.SendAsync(request).Result;
                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    var rep = response.Content.ReadAsStringAsync().Result ?? "";
                //}
                //else
                //{
                //    var intt = (int) response.StatusCode;
                //    var aa = response;
                //}


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }

}
