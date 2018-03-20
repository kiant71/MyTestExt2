﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace MyWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute(name: "DefaultApi",
                routeTemplate: "{api}/{controller}/{id}",
                defaults: new { api = "api", controller = "default", id = "" });
        }

    }
}
