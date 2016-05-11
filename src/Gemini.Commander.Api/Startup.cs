using System.Web.Http;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Owin;

namespace Gemini.Commander.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            app.UseWebApi(config);

            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();

        }
    }
}
