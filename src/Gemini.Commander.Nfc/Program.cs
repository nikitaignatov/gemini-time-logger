using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Gemini.Commander.Nfc.Dialogs;
using Newtonsoft.Json;

namespace Gemini.Commander.Nfc
{
    public class Program
    {
        public static void Main()
        {
            var map = LoadCardMap();
            var history = LoadHistory();

            var reader = new CardReader<int>();

            reader.CreateLog = x =>
            {
                x.Id = new Random().Next();
                var dialog = new TimeLogMessageForm();
                dialog.Message = message =>
                {
                    x.Message = message;
                    Console.WriteLine($"Update:{message}");
                };

                Application.Run(dialog);
                Console.WriteLine($"Begin: { x.CardId} {Extract(map, x.CardId)}");
                return x;
            };

            reader.UpdateLog = x =>
            {
                Console.WriteLine($"End  : {x.CardId} {Extract(map, x.CardId)} id:{x.Id}  {x.Message}");
                if (x.CardId != "NONE")
                    history.Add(x);
                StoreHistory(history);
            };

            reader.Initialize();
            Console.ReadKey();
        }

        private static Dictionary<string, string> LoadCardMap()
        {
            var path = GetFilePath("nfc.conf.path");

            var text = File.ReadAllText(path);
            Console.WriteLine(text);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
        }

        private static List<CardTransaction<int>> LoadHistory()
        {
            var path = GetFilePath("nfc.history.path");

            var text = File.ReadAllText(path);
            Console.WriteLine(text);
            return JsonConvert.DeserializeObject<List<CardTransaction<int>>>(text);
        }

        private static void StoreHistory(IEnumerable<CardTransaction<int>> data)
        {
            var path = GetFilePath("nfc.history.path");
            var text = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, text);
        }

        private static string GetFilePath(string key)
        {
            var path = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(path)) throw new Exception($"path [{path}] is not configured.");
            if (!File.Exists(path)) throw new Exception($"path [{path}] could not be found.");
            return path;
        }

        public static string Extract(Dictionary<string, string> map, string key) => map.ContainsKey(key) ? map[key] : null;
    }
}