using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.WebHost;
using System.Web.SessionState;
using System.Web.Routing;
using System.Web;

namespace UserWebApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance",
       System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            }
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "GetAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "user", action = "GetAllUsers", id = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
                name: "AddAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "user", action = "AddUser", id = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
                name: "GetSingleAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "user", action = "GetUser", id = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
                name: "UpdatetAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "user", action = "UpdateUser", id = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
                name: "DeleteAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "user", action = "DeleteUser", id = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
               name: "LoginAPI",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { controller = "user", action = "Login", id = RouteParameter.Optional }
               );
            config.Routes.MapHttpRoute(
              name: "SendEmailAPI",
              routeTemplate: "api/{controller}/{action}/{id}",
              defaults: new { controller = "user", action = "SendEmail", id = RouteParameter.Optional }
              );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

    }
    public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }

    public class SessionHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionControllerHandler(requestContext.RouteData);
        }
    }
}
