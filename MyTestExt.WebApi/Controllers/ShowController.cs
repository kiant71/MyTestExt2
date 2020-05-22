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
    [Route("api/show2")]
    public class ShowController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var fullName = @"D:\0.Work\FtpTest\1\2020\05\MTU4OTAyMzgzMDQ5MS0zNTc0NTE1LTE5Mi4xNjguMS4yMy0w.jpg";

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(fullName, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            /*response.Content = new StreamContent(memoryStream); */// new ByteArrayContent(res),
            

            //response.Content = new PushStreamContent((stream, content, context) =>
            //{
            //    stream = memoryStream;

            //}, "application/octet-stream");


            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = "aaa.jpg"
            //};

            return response;
            
        }


       
    }
}
