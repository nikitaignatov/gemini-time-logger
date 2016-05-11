using System.Configuration;
using System.Web.Http;
using Countersoft.Gemini.Api;
using Gemini.Commander.Commands;
using Gemini.Commander.Core;

namespace Gemini.Commander.Api
{
    public class CommandsController : ApiController
    {
        [Route("api/show/words")]
        [HttpGet]
        public dynamic Words()
        {
            return new ShowProfileQuery(LoadService()).Execute(new MainArgs(new string[] { "show", "words", "everyone", "--take=10", "--stemmed" }));
        }

        [Route("api/show/hours/{user}")]
        [HttpPost]
        public IHttpActionResult Hours(string user)
        {
            return Ok();
        }

        public static ServiceManager LoadService()
        {
            var settings = ConfigurationManager.AppSettings;
            return new ServiceManager(settings["endpoint"], settings["username"], "", settings["apikey"]);
        }
    }
}