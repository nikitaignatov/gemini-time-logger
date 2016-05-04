using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AttributeRouting.Web.Http.SelfHost;
using Newtonsoft.Json;
using Owin;

namespace Gemini.Commander.Api
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SerializerSettings=new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            appBuilder.UseWebApi(config);
        }
    }
}
