using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace Gemini.Commander.Nfc
{
    public class Program
    {
        public static void Main()
        {
            var map = LoadCardMap();

            var reader = new CardReader();

            reader.CreateLog = x =>
            {
                x.Id = new Random().Next();
                Console.WriteLine($"Begin: { x.CardId} {Extract(map, x.CardId)}");
                return x;
            };

            reader.UpdateLog = x => Console.WriteLine($"End  : {x.CardId} {Extract(map, x.CardId)} id:{x.Id}  ");
            reader.Initialize();
            Console.ReadKey();
        }

        private static Dictionary<string, string> LoadCardMap()
        {
            var path = ConfigurationManager.AppSettings["nfc.conf.path"];
            if (string.IsNullOrWhiteSpace(path)) throw new Exception("path for nfc map is not configured.");
            if (!File.Exists(path)) throw new Exception("path for nfc map could not be found.");

            var text = File.ReadAllText(path);
            Console.WriteLine(text);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
        }

        public static string Extract(Dictionary<string, string> map, string key) => map.ContainsKey(key) ? map[key] : null;
    }
}