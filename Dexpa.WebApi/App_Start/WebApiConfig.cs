using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Dexpa.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Report",
                routeTemplate: "api/report/{action}",
                defaults: new { Controller = "Report", Method = "GET" }
            );

            config.Routes.MapHttpRoute(
                name: "SystemEvents",
                routeTemplate: "api/events/{action}",
                defaults: new { Controller = "Events", Method = "GET" }
            );

            config.Routes.MapHttpRoute(
                name: "HelpDictionaries",
                routeTemplate: "api/helpdictionaries/{action}",
                defaults: new { Controller = "HelpDictionaries", Method = "GET" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
