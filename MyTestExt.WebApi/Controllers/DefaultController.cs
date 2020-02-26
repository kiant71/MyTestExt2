using System.Web.Http;

namespace MyTestExt.WebApi.Controllers
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
