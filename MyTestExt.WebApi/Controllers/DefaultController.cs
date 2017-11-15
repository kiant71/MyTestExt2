using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebApi.Controllers
{
    [Route("api/default")]
    public class DefaultController : ApiController
    {
        public string Get()
        {
            return "ok";
        }
    }
}
