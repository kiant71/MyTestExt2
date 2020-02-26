using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;

namespace MyTestExt.WebApi.Controllers
{
    [Route("api/down")]
    public class DownController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string zipName = "测试压缩123.zip";

            var files = new Dictionary<string, string>();
            files[@"_7zip.cs"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBHmEbaEOAAAAAAHd2qY7660.cs";
            files[@"piao.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBO-EPc_dAAAAAG8yk98307.jpg";
            files[@"piao3.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBSmENMRvAAAAAPrulhM298.jpg";
            files[@"piao5.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBTyET87DAAAAACnsJuc074.jpg";
            files[@"piao6.jpg"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBUuEBwWjAAAAAK_nr34555.jpg";
            //files[@"SAP培训教材和帮助文档.rar"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBVyEU33wAAAAADPl4j8969.rar";
            files[@"新保理资产包打包上传功能开发方案.docx"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBXiERBKIAAAAAL5QcqE09.docx";
            //files[@"Change\A1\新建文本文档.txt"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBZOEDBnNAAAAACmN-r8421.txt";
            //files[@"txt\Result.xml"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBlKEcy6mAAAAAJ4JYRY426.xml";
            //files[@"txt\SAP360 服务端改进方案.docx"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztBmOEezNlAAAAALzsU2M46.docx";
            //files[@"txt\移动端API 迁移.xlsx"] = "http://in.sap360.com.cn:559/group1/M00/00/83/wKgBuFztBn-ENZokAAAAAGrgJjs86.xlsx";

            //files[@"Soft\SW_DVD5_Office_Professional_Plus_2013_64Bit_ChnSimp_MLF_X18-55285.ISO"] = "http://in.sap360.com.cn:559/group1/M00/00/82/wKgB0VztCFWECoDDAAAAAPh_DBA881.ISO";



            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create))
                {

                    //var request = new HttpRequestMessage(HttpMethod.Get, url);
                    //var client = new HttpClient();
                    //var response = client.SendAsync(request).Result;
                    //var result = response.Content.ReadAsStringAsync().Result ?? "";

                    foreach (var kv in files)
                    {
                        var entry = archive.CreateEntry(kv.Key);
                        using (var writer = new BufferedStream(entry.Open()))
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, kv.Value);
                            var client = new HttpClient();
                            var req0 = client.SendAsync(request).Result;
                            var res0 = req0.Content.ReadAsStreamAsync().Result;

                            byte[] bArr = new byte[1024000];
                            int size = res0.Read(bArr, 0, (int)bArr.Length);
                            while (size > 0)
                            {
                                writer.Write(bArr, 0, bArr.Length);

                                size = res0.Read(bArr, 0, (int)bArr.Length);
                            }
                        }
                    }
                    

                    InvokeWriteFile(archive);

                    // 下载输出
                    //var res = new byte[memoryStream.Length];
                    //memoryStream.Position = 0;
                    //memoryStream.Read(res, 0, res.Length);


                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    //response.Content = new StreamContent(new FileStream(@"D:\测试压缩123————.zip", FileMode.Open, FileAccess.Read));
                    /*response.Content = new StreamContent(memoryStream); */// new ByteArrayContent(res),
                    //response.Content = new PushStreamContent((stream, content, context) =>
                    //{
                    //    stream = memoryStream;

                    //}, "application/octet-stream");
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = zipName
                    };
                    return response;



                    //var result = new ZqsignAction(company).ContractPdfGet(company, no);
                    //var response = new HttpResponseMessage(HttpStatusCode.OK)
                    //{
                    //    Content = new StreamContent(result), //new ByteArrayContent(result)
                    //};
                    //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                    //{
                    //    FileName = no + ".pdf"
                    //};


                    //var response = new HttpResponseMessage(HttpStatusCode.OK)
                    //{
                    //    Content = new ByteArrayContent(res),
                    //};
                    //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream"); // 前端下载
                    //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") //new ContentDispositionHeaderValue("inline")
                    //{
                    //    FileName = zipName
                    //};
                    //return response;
                }
            }
        }


        // todo. 后期调整
        static void InvokeWriteFile(ZipArchive zipArchive)
        {
            foreach (var method in zipArchive.GetType().GetRuntimeMethods())
            {
                if (method.Name == "WriteFile")
                {
                    method.Invoke(zipArchive, new object[0]);
                }
            }
        }

    }
}
