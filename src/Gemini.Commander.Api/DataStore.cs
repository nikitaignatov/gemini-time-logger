using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gemini.Commander.Api
{
    public class DataStore
    {
        public Dictionary<Guid, TrackerSession> Data { get; set; } = new Dictionary<Guid, TrackerSession>();
        public ConcurrentDictionary<string, string> Users { get; set; } = new ConcurrentDictionary<string, string>();
        public Dictionary<string, User> UserStore { get; set; } = new Dictionary<string, User>();
        public Settings Settings { get; set; } = new Settings { gemini = new Gemini { ticket_url = "http://some.url" } };
        public Dictionary<string, User> CardUsers
            => UserStore
                .SelectMany(x => x.Value.Cards)
                .Distinct()
                .Select(card => new
                {
                    card,
                    user = UserStore.Values.SingleOrDefault(u => u.Cards.Contains(card))
                })
                .ToDictionary(x => x.card, x => x.user);

        public static DataStore Load(string key) => JsonConvert.DeserializeObject<DataStore>(File.ReadAllText(GetFilePathFromConfig(key)));
        public void Save() => Store(this, "nfc.datastore.path");
        public static void Store<T>(T input, string key) => File.WriteAllText(GetFilePathFromConfig(key), JsonConvert.SerializeObject(input, Formatting.Indented));

        private static string GetFilePathFromConfig(string key)
        {
            var path = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(path)) throw new Exception($"path [{path}] is not configured.");
            if (!File.Exists(path)) throw new Exception($"path [{path}] could not be found.");
            return path;
        }
    }
}
