using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Gemini.Commander.Nfc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Gemini.Commander.Api
{
    public class TimeTracker
    {
        public static ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();
        public static Dictionary<Guid, TrackerSession> data = new Dictionary<Guid, TrackerSession>();
        private readonly ICardReader reader;

        public TimeTracker(ICardReader reader)
        {
            this.reader = reader;
        }

        public void Run()
        {
            data = LoadHistory();
            users = LoadCardMap();
            reader.CreateLog = (x) =>
            {
                var session = new TrackerSession
                {
                    Transaction = x,
                };
                var context = GlobalHost.ConnectionManager.GetHubContext<NfcHub>();
                if (!users.ContainsKey(x.Card))
                    context.Clients.All.register(x.Card);
                else
                {
                    session.Name = users[x.Card];
                    data.Add(x.TransactionId, session);
                    context.Clients.All.update(Convert(data));
                }
                StoreHistory(data);
                StoreCardMap(users);
                return x;
            };

            reader.UpdateLog = (x) =>
            {
                if (users.ContainsKey(x.Card) && data.ContainsKey(x.TransactionId))
                {
                    data[x.TransactionId].Transaction = x;
                    var context = GlobalHost.ConnectionManager.GetHubContext<NfcHub>();
                    context.Clients.All.update(Convert(data));
                    StoreHistory(data);
                }
                StoreCardMap(users);
            };
            reader.Initialize();
        }

        public static dynamic Convert(Dictionary<Guid, TrackerSession> data)
        {
            var m = data.OrderByDescending(v => v.Value.Transaction.Started);
            return new
            {
                new_sessions = m.Where(x => !x.Value.IsValid),
                ready_to_submit = m.Where(x => x.Value.IsValid && !x.Value.IsSubmitted),
                complete = m.Where(x => x.Value.IsSubmitted),
                total_minutes = m.Where(x => x.Value.Transaction.IsEnded).Sum(x => x.Value.Transaction.Duration.TotalMinutes),
                total_sessions = m.Count(),
                total_questions = m.Count(x => x.Value.Type == TimeType.Question)
            };
        }

        private static ConcurrentDictionary<string, string> LoadCardMap() => Load<ConcurrentDictionary<string, string>>("nfc.conf.path");
        private static Dictionary<Guid, TrackerSession> LoadHistory() => Load<Dictionary<Guid, TrackerSession>>("nfc.history.path");
        private static T Load<T>(string key) => JsonConvert.DeserializeObject<T>(File.ReadAllText(GetFilePathFromConfig(key)));

        public static void StoreHistory(Dictionary<Guid, TrackerSession> data) => Store(data, "nfc.history.path");
        public static void StoreCardMap(ConcurrentDictionary<string, string> data) => Store(data, "nfc.conf.path");
        public static void Store<T>(T input, string key) => File.WriteAllText(GetFilePathFromConfig(key), JsonConvert.SerializeObject(input, Formatting.Indented));

        private static string GetFilePathFromConfig(string key)
        {
            var path = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(path)) throw new Exception($"path [{path}] is not configured.");
            if (!File.Exists(path)) throw new Exception($"path [{path}] could not be found.");
            return path;
        }

        public static string Extract(Dictionary<string, string> map, string key) => map.ContainsKey(key) ? map[key] : null;
    }
}