using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using Countersoft.Gemini.Api;
using Gemini.Commander.Commands;
using Gemini.Commander.Core;
using Microsoft.Owin.Hosting;

namespace Gemini.Commander.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                HttpClient client = new HttpClient();
                var response = client.GetAsync(baseAddress + "api/show/words").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }
        }
    }

    public class CommandsController : ApiController
    {
        [Route("api/show/words")]
        [HttpGet]
        public dynamic Words()
        {
            return new ShowProfileQuery(LoadService()).Execute(new MainArgs(new string[] { "show", "words", "everyone", "--take=10" , "--stemmed" }));
        }

        [Route("api/show/hours/{user}")]
        [HttpPost]
        public IHttpActionResult Hours(string user)
        {
            return Ok();
        }

        private static ServiceManager LoadService()
        {
            var settings = ConfigurationManager.AppSettings;
            return new ServiceManager(settings["endpoint"], settings["username"], "", settings["apikey"]);
        }
    }
}
