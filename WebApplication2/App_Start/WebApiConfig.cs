using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplication2.Areas.HelpPage;
using WebApplication2.Utils;

namespace WebApplication2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //跨域配置
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Web API 配置和服务
            config.SetDocumentationProvider(new MultiXmlDocumentationProvider(
                HttpContext.Current.Server.MapPath("~/App_Data/WebApplication2.xml"),
                HttpContext.Current.Server.MapPath("~/App_Data/Mzh.Public.Model.xml")));
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
