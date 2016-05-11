using System;
using System.Net.Http;
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
                new TimeTracker().Run();
            }
        }
    }
}
